using AuthenticationClientService.API.Interfaces;
using AuthenticationClientService.API.Models;
using AuthenticationClientService.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthenticationClientService.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        /// <summary>
        /// Obter perfil de usuário do sistema
        /// </summary>
        /// <response code="200">Perfil do usuário retornado</response>
        /// <response code="404">Nenhum perfil de usuário encontrado</response>
        [HttpGet(ApiRoutes.Profile.Get)]
        [ProducesDefaultResponseType(typeof(UserModel))]
        public async Task<IActionResult> GetProfileData([FromServices] IProfileService profileService, [FromRoute] Guid id)
        {
            var userProfile = await profileService
                .GetProfileDataAsync(id);

            if (userProfile is null)
                return NotFound();

            return Ok(userProfile);
        }

        /// <summary>
        /// Atualizar perfil de usuário do sistema
        /// </summary>
        /// <response code="200">Perfil do usuário atualizado</response>
        /// <response code="404">Nenhum perfil de usuário encontrado</response>
        /// <response code="400">Um erro ocorreu ao atualizar perfil de usuário</response>
        [HttpPut(ApiRoutes.Profile.Update)]
        [ProducesDefaultResponseType(typeof(UserModel))]
        public async Task<IActionResult> UpdateProfileData([FromServices] IProfileService profileService, 
            [FromRoute] Guid id, [FromBody] UserModel user)
        {
            var userProfile = await profileService
                .UpdateProfileDataAsync(id, user);

            if (userProfile is null)
                return NotFound();

            return Ok(userProfile);
        }
    }
}
