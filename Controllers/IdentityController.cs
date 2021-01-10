using AuthenticationClientService.API.Interfaces;
using AuthenticationClientService.API.Models;
using AuthenticationClientService.Constants;
using AuthenticationClientService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationClientService.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Produces("application/json")]
    public class IdentityController : ControllerBase
    {
        /// <summary>
        /// Registrar usuário no sistema
        /// </summary>
        /// <response code="200">Novo usuário registrado no sistema</response>
        /// <response code="400">Um erro ocorreu ao registrar usuário no sistema</response>
        [HttpPost(ApiRoutes.Identity.Register)]
        [ProducesDefaultResponseType(typeof(AuthenticationResult))]
        public async Task<IActionResult> Register([FromServices] IIdentityService identityService,
            [FromBody] RegisterModel registerModel)
        {
            var authenticationResult = await identityService
                .RegisterAsync(registerModel);

            if (!authenticationResult.Success)
                return BadRequest(new
                {
                    authenticationResult.Message,
                    authenticationResult.Success,
                    authenticationResult.Errors
                });

            return Ok(new
            {
                authenticationResult.Message,
                authenticationResult.Success,
                authenticationResult.Token,
                authenticationResult.User
            });
        }

        /// <summary>
        /// Logar usuário no sistema
        /// </summary>
        /// <response code="200">Usuário logado no sistema</response>
        /// <response code="400">Um erro ocorreu no login do usuário no sistema</response>
        [HttpPost(ApiRoutes.Identity.Login)]
        [ProducesDefaultResponseType(typeof(AuthenticationResult))]
        public async Task<IActionResult> Login([FromServices] IIdentityService identityService,
            [FromBody] LoginModel loginModel)
        {
            var authenticationResult = await identityService
                .LoginAsync(loginModel);

            if (!authenticationResult.Success)
                return BadRequest(new
                {
                    authenticationResult.Message,
                    authenticationResult.Success,
                    authenticationResult.Errors
                });

            return Ok(new
            {
                authenticationResult.Message,
                authenticationResult.Success,
                authenticationResult.Token,
                authenticationResult.User
            });
        }

        /// <summary>
        /// Confirmar email de usuário no sistema
        /// </summary>
        /// <response code="200">Email do usuário confirmado no sistema</response>
        /// <response code="400">Um erro ocorreu ao confirmar email do usuário no sistema</response>
        [HttpPost(ApiRoutes.Identity.ConfirmEmail)]
        [ProducesDefaultResponseType(typeof(AuthenticationResult))]
        public async Task<IActionResult> ConfirmEmail([FromServices] IIdentityService identityService,
            [FromBody] ConfirmEmailModel confirmEmailModel)
        {
            var authenticationResult = await identityService
                .ConfirmEmailAsync(confirmEmailModel);

            if (!authenticationResult.Success)
                return BadRequest(new
                {
                    authenticationResult.Message,
                    authenticationResult.Success,
                    authenticationResult.Errors
                });

            return Ok(new
            {
                authenticationResult.Message,
                authenticationResult.Success
            });
        }
    }
}
