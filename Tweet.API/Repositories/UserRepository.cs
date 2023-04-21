using Tweet.API.Interface;
using Tweet.API.Model;

namespace Tweet.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public UserRepository()
        {
            _users = new List<User>();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await Task.FromResult(_users.FirstOrDefault(u => u.Email == email));
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
        }

        public async Task CreateUserAsync(User user)
        {
            user.Id = _users.Count + 1;
            _users.Add(user);

            await Task.CompletedTask;
        }
    }

}
