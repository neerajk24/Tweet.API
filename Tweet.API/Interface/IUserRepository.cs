using Tweet.API.Model;

namespace Tweet.API.Interface
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(int id);
        Task CreateUserAsync(User user);
    }

}
