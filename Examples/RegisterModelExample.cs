using AuthenticationClientService.API.Models;
using Swashbuckle.AspNetCore.Filters;

namespace AuthenticationClientService.Examples
{
    public class RegisterModelExample : IExamplesProvider<RegisterModel>
    {
        public RegisterModel GetExamples()
        {
            return new RegisterModel
            {
                Name = "Example",
                LastName = "Swagger",
                Email = "example.swagger@email.com",
                UserName = "Example",
                Password = "Example@123",
                Role = "Operador"
            };
        }
    }
}
