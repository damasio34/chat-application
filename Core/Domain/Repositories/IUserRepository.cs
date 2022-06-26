namespace ChatApplication.Core.Domain.Repositories
{
    public interface IUserRepository
    {
        User Get(string username, string password);
        bool IsValid(string username, string password);
    }
}
