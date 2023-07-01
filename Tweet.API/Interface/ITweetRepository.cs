using Tweet.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tweet.API.Interface
{
    public interface ITweetRepository
    {
        Task<List<Tweet.API.Entities.Tweet>> GetTweetsByUserIdAsync(int userId); // Fetch tweets authored by the user

        Task<Tweet.API.Entities.Tweet> GetTweetByIdAsync(int tweetId);

        Task CreateTweetAsync(Tweet.API.Entities.Tweet tweet);

        Task<List<Entities.Tweet>> GetLikedTweetsByUserIdAsync(int userId); // Fetch tweets liked by the user

        Task<List<Entities.Tweet>> GetRetweetedTweetsByUserIdAsync(int userId); // Fetch tweets retweeted by the user
        Task UpdateTweetAsync(Entities.Tweet tweet);
        Task CreateCommentAsync(Comment comment);
    }
}
