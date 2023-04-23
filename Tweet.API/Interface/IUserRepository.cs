using Tweet.API.Entities;

namespace Tweet.API.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(int id);
        Task CreateUserAsync(User user);
    }

}
