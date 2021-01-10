using AuthenticationClientService.API.Interfaces;
using AuthenticationClientService.API.Models;
using AuthenticationClientService.API.Utils;
using AuthenticationClientService.Configurations;
using AuthenticationClientService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationClientService.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public IdentityService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationResult> ConfirmEmailAsync(ConfirmEmailModel confirmEmail)
        {
            var user = await _userManager
                .FindByEmailAsync(confirmEmail.Email);

            if (user is null)
            {
                _userManager.Logger.LogWarning(
                    "Nenhum usuário foi encontrado que corresponda ao email '{0}'",
                    confirmEmail.Email);

                return new AuthenticationResult
                {
                    Message = "Não existe usuário com email especificado",
                    Success = false
                };
            }

            var confirmedEmailResult = await _userManager
                .ConfirmEmailAsync(user, confirmEmail.Token);

            if (!confirmedEmailResult.Succeeded)
            {
                _userManager.Logger.LogWarning(
                    "Ocorreu uma falha ao confirmar email do usuário. Erros: {0}",
                    JsonSerializer.Serialize(confirmedEmailResult.Errors));

                return new AuthenticationResult
                {
                    Message = "Falha no confirmação de email do usuário",
                    Success = false,
                    Errors = confirmedEmailResult.Errors.Select(x => x.Description).ToArray()
                };
            }

            _userManager.Logger.LogInformation("Email {0} foi confirmado com sucesso", confirmEmail.Email);

            return new AuthenticationResult
            {
                Message = "Email confirmado com sucesso",
                Success = true
            };
        }

        public async Task<AuthenticationResult> LoginAsync(LoginModel login)
        {
            var user = string.IsNullOrEmpty(login.Email)
                ? await _userManager.FindByNameAsync(login.UserName)
                : await _userManager.FindByEmailAsync(login.Email);

            if (user is null)
            {
                _userManager.Logger.LogWarning(
                    "Nenhum usuário com foi encontrado que corresponda ao valor '{0}'",
                    string.IsNullOrEmpty(login.Email) ? login.UserName : login.Email);

                return new AuthenticationResult
                {
                    Message = "Não existe usuário com email/username especificado",
                    Success = false
                };
            }

            var confirmedEmail = await _userManager.IsEmailConfirmedAsync(user);

            if (!confirmedEmail)
            {
                _userManager.Logger.LogWarning(
                    "Usuário {0} não pode logar sem confirmar email", login.Email);

                return new AuthenticationResult
                {
                    Message = "Usuário não pode logar no sistema, pois não está com email confirmado",
                    Success = false
                };
            }

            var loginResult = await _signInManager
                .PasswordSignInAsync(user, login.Password, true, lockoutOnFailure: true);

            if (loginResult.IsLockedOut)
            {
                _userManager.Logger.LogWarning(
                    "Usuário com email/username {0} está bloqueado",
                    string.IsNullOrEmpty(login.Email) ? login.UserName : login.Email);

                return new AuthenticationResult
                {
                    Message = "Esse usuário encontra-se bloqueado",
                    Success = false
                };
            }

            if (!loginResult.Succeeded)
            {
                _userManager.Logger.LogWarning(
                    "Falha ao logar usuário {0} no sistema",
                    string.IsNullOrEmpty(login.Email) ? login.UserName : login.Email);

                return new AuthenticationResult
                {
                    Message = "Falha no login",
                    Success = false
                };
            }

            _userManager.Logger.LogInformation("Usuário foi autenticado com sucesso");

            return await TokenUtils
                .GenerateAuthenticationResultForUserAsync(
                user, _jwtSettings.Value, _userManager);
        }

        public async Task<AuthenticationResult> RegisterAsync(RegisterModel register)
        {
            var user = await _userManager.FindByEmailAsync(register.Email);

            if (!(user is null))
            {
                _userManager.Logger.LogWarning(
                    "Foi encontrado um usuário com o endereço de email {0} já cadastrado",
                    register.Email);

                return new AuthenticationResult
                {
                    Message = "Falha no registro de usuário",
                    Success = false,
                    Errors = new[] { "Já existe usuário com esse email" }
                };
            }


            var newUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = register.Name,
                LastName = register.LastName,
                Email = register.Email,
                NormalizedEmail = register.Email.ToUpper(),
                UserName = register.UserName,
                NormalizedUserName = register.UserName.ToUpper()
            };

            var createdUser = await _userManager
                .CreateAsync(newUser, register.Password);

            if (!createdUser.Succeeded)
            {
                _userManager.Logger.LogWarning(
                    "Ocorreu uma falha ao registrar usuário. Erros: {0}",
                    JsonSerializer.Serialize(createdUser.Errors));

                return new AuthenticationResult
                {
                    Message = "Falha no registro de usuário",
                    Success = false,
                    Errors = createdUser.Errors.Select(x => x.Description).ToArray()
                };
            }

            await _userManager.AddToRoleAsync(newUser, register.Role);
            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            _userManager.Logger.LogInformation("Token para confirmação do email {0} = {1}", newUser.Email, confirmEmailToken);
            
            _userManager.Logger.LogInformation("Usuário registrado com sucesso");

            return await TokenUtils
                .GenerateAuthenticationResultForUserAsync(
                newUser, _jwtSettings.Value, _userManager);
        }
    }
}
