using ChatApplication.Core.Domain.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ChatApplication.API.SwaggerExamples
{
    public class LoginInputExample : IExamplesProvider<LoginDTO>
    {
        public LoginDTO GetExamples()
        {
            return new LoginDTO
            {
                Username = "batman",
                Password = "batman"
            };
        }
    }
}
