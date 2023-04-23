using Microsoft.EntityFrameworkCore;
using Tweet.API.Data;
using Tweet.API.Interface;
using Tweet.API.Entities;

namespace Tweet.API.Repositories
{
    public class TweetRepository : ITweetRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TweetRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

    }

}
