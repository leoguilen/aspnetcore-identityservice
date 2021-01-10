using AuthenticationClientService.API.Interfaces;
using AuthenticationClientService.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationClientService.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public ProfileService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserModel> GetProfileDataAsync(Guid userId)
        {
            var userProfile = await _userManager
                .FindByIdAsync(userId.ToString());

            if (userProfile is null)
                return null;

            return _mapper.Map<UserModel>(userProfile);
        }

        public async Task<object> UpdateProfileDataAsync(Guid userid, UserModel userModel)
        {
            var user = await _userManager.FindByIdAsync(userid.ToString());

            if (user is null)
                return null;

            var updated = await _userManager
                .UpdateAsync(user);

            if (!updated.Succeeded)
            {
                return new
                {
                    Message = "Falha na atualização do usuário",
                    Success = false,
                    Errors = updated.Errors.Select(x => x.Description)
                };
            }

            return new
            {
                Message = "Usuário atualizado com sucesso",
                Success = true
            };
        }
    }
}
