using AuthenticationClientService.API.Models;
using AuthenticationClientService.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationClientService.API.Utils
{
    public class TokenUtils
    {
        public static ClaimsPrincipal GetPrincipalFromToken(string token,
            TokenValidationParameters tokenValidationParameters)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validadedToken);

                if (!IsJwtWithValidSecurityAlgorithm(validadedToken))
                    return null;

                return principal;
            }
            catch { return null; }
        }

        public static async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(
            ApplicationUser user, JwtSettings jwtSettings,
            UserManager<ApplicationUser> userManager)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            // Busca todas as claims do usuario para add no token
            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // Busca claims de cada role para add no token
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await userManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await userManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                        continue;

                    claims.Add(roleClaim);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Message = "Autenticação realizado com sucesso",
                Success = true,
                Token = tokenHandler.WriteToken(token),
                User = new UserModel
                {
                    Id = Guid.Parse(user.Id),
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName
                }
            };
        }

        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validadedToken)
        {
            return (validadedToken is JwtSecurityToken jwtSecurityToken) &&
                    jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
