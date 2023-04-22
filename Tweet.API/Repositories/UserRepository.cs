using Microsoft.EntityFrameworkCore;
using Tweet.API.Data;
using Tweet.API.Interface;
using Tweet.API.Model;

namespace Tweet.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;
        
        private readonly ApplicationDbContext _dbContext; // Replace YourDbContext with the actual name of your database context

        public UserRepository(ApplicationDbContext dbContext) // Replace YourDbContext with the actual name of your database context
        {
            _dbContext = dbContext;
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

        public async Task<User> GetUserByEmail(string email)
        {
            // Your database query or ORM logic here to retrieve the user by email
            // Example using Entity Framework Core
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task CreateUserAsync(User user)
        {
            user.Id = _users.Count + 1;
            _users.Add(user);
            await _dbContext.SaveChangesAsync(); // Save changes to the database
            //await Task.CompletedTask;
        }
    }

}
