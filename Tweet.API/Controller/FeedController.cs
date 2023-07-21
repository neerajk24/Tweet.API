using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Tweet.API.Interface;
using Tweet.API.Model;
using BCrypt; // Import the BCrypt namespace
using BCrypt.Net; // Import the BCrypt.Net namespace
using Tweet.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Tweet.API.Controller
{


    [ApiController]
    [Route("api/[controller]")]
    public class FeedController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITweetRepository _tweetRepository;
        private readonly IConfiguration _configuration;

        public FeedController(IUserRepository userRepository, ITweetRepository tweetRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tweetRepository = tweetRepository;
            _configuration = configuration;
        }


            // GET: api/feed/{userId}
            [HttpGet("{userId}")]
            public async Task<IActionResult> GetFeed(int userId)
            {
                try
                {
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user == null)
                    {
                        return NotFound("User not found");
                    }

                    // Fetch tweets from the user and the users they follow, sorted chronologically
                    var tweets = await _tweetRepository.GetTweetsByUserIdAsync(userId);

                var retweets = await _tweetRepository.GetRetweetedTweetsByUserIdAsync(userId);

                var tweetViewModels = tweets.Select(tweet =>
                {
                    var tweetModel = new TweetModel
                    {
                        Id = tweet.Id,
                        Content = tweet.Content.ToString(),
                        Likes = tweet.Likes,
                        Comments = tweet.Comments.Select(comment => comment.Content).ToList(),
                        Timestamp = tweet.Timestamp,
                        Shares = tweet.Shares,
                       
                        //ReTweetModel = new TweetModel() // Initialize the list to store retweets
                    };

                    // Find all the corresponding retweets for this tweet from the retweets list
                    var matchingRetweets = retweets.Where(retweet => retweet.Id == tweet.Id);

                    // If there are matching retweets, add them to the ReTweetModel list
                    //if (matchingRetweets.Any())
                    //{
                    //    foreach (var retweet in matchingRetweets)
                    //    {
                    //        tweetModel.ReTweetModel.Add(new TweetModel
                    //        {
                    //            Id = retweet.Id,
                    //            Content = retweet.Content.ToString(),
                    //            Likes = retweet.Likes,
                    //            Comments = retweet.Comments.Select(comment => comment.Content).ToList(),
                    //            Timestamp = retweet.Timestamp,
                    //            Shares = retweet.Shares
     
                    //        });
                    //    }
                    //}

                    return tweetModel;
                }).ToList();

               // Create a view model or transform the tweet entities as needed
                //var tweetViewModels = tweets.Select(tweet => new TweetModel
                //{
                //    Content = tweet.Content.ToString(),
                //    Likes = tweet.Likes,
                //    Comments = tweet.Comments.Select(comment => comment.Content).ToList(),
                //    Timestamp = tweet.Timestamp,
                //    Id = tweet.Id,
                //    ReTweetModel = new TweetModel()

                //}).ToList();


                return Ok(tweetViewModels);

            }
                catch (Exception ex)
                {
                    // Handle exception and return error response
                    return StatusCode(500, "An error occurred while retrieving the feed");
                }
            }

            // POST: api/feed/{userId}/tweets
            [HttpPost("{userId}/tweets")]
            public async Task<IActionResult> CreateTweet(int userId, [FromBody] TweetModel model)
            {
                // Validate input
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user == null)
                    {
                        return NotFound("User not found");
                    }

                    // Create a new tweet
                    var tweet = new Entities.Tweet
                    {
                        UserId = userId,
                        Content = model.Content,
                        Timestamp = DateTime.Now,
                        
                        
                        // Set other properties as needed
                    };

                    await _tweetRepository.CreateTweetAsync(tweet);

                    return Ok(true);
                }
                catch (Exception ex)
                {
                    // Handle exception and return error response
                    return StatusCode(500, "An error occurred while creating the tweet");
                }
            }

            // POST: api/feed/{userId}/tweets/{tweetId}/like
            [HttpPost("{userId}/tweets/{tweetId}/like")]
            public async Task<IActionResult> LikeTweet(int userId, int tweetId)
            {
                try
                {
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user == null)
                    {
                        return NotFound("User not found");
                    }

                    var tweet = await _tweetRepository.GetTweetByIdAsync(tweetId);
                    if (tweet == null)
                    {
                        return NotFound("Tweet not found");
                    }

                    // Increment the likes count for the tweet
                    tweet.Likes++;

                    await _tweetRepository.UpdateTweetAsync(tweet);

                    return Ok("Tweet liked");
                }
                catch (Exception ex)
                {
                    // Handle exception and return error response
                    return StatusCode(500, "An error occurred while liking the tweet");
                }
            }

            // POST: api/feed/{userId}/tweets/{tweetId}/comment
            [HttpPost("Tweet/comment")]
            public async Task<IActionResult> CommentOnTweet([FromBody] CommentModel model)
            {
                // Validate input
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    var user = await _userRepository.GetByIdAsync(model.UserId);
                    if (user == null)
                    {
                        return NotFound("User not found");
                    }

                    var tweet = await _tweetRepository.GetTweetByIdAsync(model.TweetId);
                    if (tweet == null)
                    {
                        return NotFound("Tweet not found");
                    }

                    // Create a new comment
                    var comment = new Comment
                    {
                        UserId = model.UserId,
                        TweetId = model.TweetId,
                        Content = model.Content,

                        // Set other properties as needed
                    };

                    await _tweetRepository.CreateCommentAsync(comment);

                    return Ok("Comment added");
                }
                catch (Exception ex)
                {
                    // Handle exception and return error response
                    return StatusCode(500, "An error occurred while commenting on the tweet");
                }
            }

            // POST: api/feed/{userId}/tweets/{tweetId}/retweet
            [HttpPost("{userId}/tweets/{tweetId}/retweet")]
            public async Task<IActionResult> Retweet(int userId, int tweetId)
            {
                try
                {
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user == null)
                    {
                        return NotFound("User not found");
                    }

                    var tweet = await _tweetRepository.GetTweetByIdAsync(tweetId);
                    if (tweet == null)
                    {
                        return NotFound("Tweet not found");
                    }

                    // Increment the retweets count for the tweet
                    tweet.Retweets++;

                    await _tweetRepository.UpdateTweetAsync(tweet);

                    return Ok("Tweet retweeted");
                }
                catch (Exception ex)
                {
                    // Handle exception and return error response
                    return StatusCode(500, "An error occurred while retweeting the tweet");
                }
            }
        }

    }



