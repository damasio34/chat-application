using ChatApplication.Core.Domain;
using ChatApplication.Core.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ChatApplication.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users = new()
        {
            new User { Id = 1, Username = "batman", Password = "batman", Role = "manager" },
            new User { Id = 2, Username = "robin", Password = "robin", Role = "employee" }
        };

        public User Get(string username, string password)
            => this._users.FirstOrDefault(user => user.Username.ToLower() == username.ToLower() && user.Password == password);

        public bool IsValid(string username, string password)
            => this._users.Any(user => user.Username.ToLower() == username.ToLower() && user.Password == password);
    }
}
