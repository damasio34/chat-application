using ChatApplication.Core.Domain.DTOs;
using ChatApplication.Core.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAppService _userAppService;

        public AuthController(IUserAppService userAppService)
        {
            this._userAppService = userAppService;
        }

        /// <summary>
        /// User Authentication
        /// </summary>
        /// <param name="login">Provide user access data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public ActionResult<LoginResultDTO> Authenticate([FromBody] LoginDTO login)
        {
            var result = this._userAppService.Authenticate(login);
            if (!result.IsAuthenticated)
                return NotFound("Username or password is invalid");

            return result;
        }
    }
}
