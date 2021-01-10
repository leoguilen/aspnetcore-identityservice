using AuthenticationClientService.API.Models;
using Swashbuckle.AspNetCore.Filters;

namespace AuthenticationClientService.Examples
{
    public class LoginModelExample : IExamplesProvider<LoginModel>
    {
        public LoginModel GetExamples()
        {
            return new LoginModel
            {
                Email = "example.swagger@email.com",
                Password = "Example@123"
            };
        }
    }
}
