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

        public Task<List<Entities.Tweet>> GetLikedTweetsByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Entities.Tweet>> GetRetweetedTweetsByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        // Fetch tweets authored by the user from the database
        public async Task<List<Entities.Tweet>> GetTweetsByUserIdAsync(int userId)
        {
            return await _dbContext.Tweets.Where(t => t.UserId == userId).ToListAsync();
        }

        // Fetch tweets liked by the user from the database
        //public async Task<List<Entities.Tweet>> GetLikedTweetsByUserIdAsync(int userId)
        //{
        //    //return await _dbContext.Tweets.Where(t => t.LikedByUsers.Any(u => u.Id == userId)).ToListAsync();
        //}

        //// Fetch tweets retweeted by the user from the database
        //public async Task<List<Entities.Tweet>> GetRetweetedTweetsByUserIdAsync(int userId)
        //{
        //    //return await _dbContext.Tweets.Where(t => t.RetweetedByUsers.Any(u => u.Id == userId)).ToListAsync();
        //}
    }
}
