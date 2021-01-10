using AuthenticationClientService.API.Models;
using System;
using System.Threading.Tasks;

namespace AuthenticationClientService.API.Interfaces
{
    public interface IProfileService
    {
        Task<UserModel> GetProfileDataAsync(Guid userId);
        Task<object> UpdateProfileDataAsync(Guid userid, UserModel user);
    }
}
