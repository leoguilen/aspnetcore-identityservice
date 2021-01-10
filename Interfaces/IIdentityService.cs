using AuthenticationClientService.API.Models;
using AuthenticationClientService.Models;
using System.Threading.Tasks;

namespace AuthenticationClientService.API.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> LoginAsync(LoginModel login);
        Task<AuthenticationResult> RegisterAsync(RegisterModel register);
        Task<AuthenticationResult> ConfirmEmailAsync(ConfirmEmailModel confirmEmail);
    }
}
