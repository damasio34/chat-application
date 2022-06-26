using ChatApplication.Core.Domain.DTOs;

namespace ChatApplication.Core.Domain.Services
{
    public interface IUserAppService
    {
        LoginResultDTO Authenticate(LoginDTO login);
    }
}
