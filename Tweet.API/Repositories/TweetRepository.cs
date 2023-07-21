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

        public async Task<List<Entities.Tweet>> GetTweetsByUserIdAsync(int userId)
        {
            var tweets = await _dbContext.Tweets
                .Include(t => t.User)
                .Include(t=> t.Comments)
                .Where(t => t.UserId == userId)
                .ToListAsync();

            return tweets;
        }

        public async Task<Entities.Tweet> GetTweetByIdAsync(int tweetId)
        {
            var tweet = await _dbContext.Tweets.FindAsync(tweetId);
            return tweet;
        }

        public async Task CreateTweetAsync(Entities.Tweet tweet)
        {
            _dbContext.Tweets.Add(tweet);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Entities.Tweet>> GetLikedTweetsByUserIdAsync(int userId)
        {
            var likedTweets = await _dbContext.Tweets
                .Where(t => t.Likes > 0)
                .ToListAsync();

            return likedTweets;
        }


        public Task<List<Entities.Tweet>> GetRetweetedTweetsByUserIdAsync(int userId)
        {
            var reTweets = _dbContext.Tweets
                .Where(t => t.Retweets == userId)
                .ToListAsync();

            return reTweets;
        }

        public async Task UpdateTweetAsync(Entities.Tweet tweet)
        {
            _dbContext.Tweets.Update(tweet);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateCommentAsync(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();
        }


        //public async Task<List<Entities.Tweet>> GetRetweetedTweetsByUserIdAsync(int userId)
        //{
        //    var retweetedTweets = await _dbContext.Tweets
        //        .Include(t => t.Retweets)
        //        .Where(t => t.Retweets.Any(r => r.UserId == userId))
        //        .ToListAsync();

        //    return retweetedTweets;
        //}
    }

}
