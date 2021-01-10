using AuthenticationClientService.API.Models;
using Swashbuckle.AspNetCore.Filters;

namespace AuthenticationClientService.Examples
{
    public class UpdateProfileDataExample : IExamplesProvider<UserModel>
    {
        public UserModel GetExamples()
        {
            return new UserModel
            {
                Name = "Example",
                LastName = "New",
                Email = "example.new@email.com",
                UserName = "ExampleNew"
            };
        }
    }
}
