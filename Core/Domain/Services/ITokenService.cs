namespace ChatApplication.Core.Domain.Services
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}
