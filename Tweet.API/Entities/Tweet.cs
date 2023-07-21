using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tweet.API.Entities
{
    public class Tweet
    {
        [Key]
        public int Id { get; set; } // Primary key property
        public string? Content { get; set; }

        public DateTime Timestamp { get; set; }
        public int Likes { get; set; } = 0;

        public int Retweets { get; set; } = 0;
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }

        // Navigation properties for related entities
        //public List<Comment> Comments { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public int Shares { get; set; }
        public List<Image> Images { get; set; }
        public List<Video> Videos { get; set; }

        // Additional properties for a tweet such as timestamp, author, etc. can also be added here

        // Collection of users who liked the tweet
        //public ICollection<UserLikedTweets>? UserLikedTweets { get; set; } // Newly added field as a list of tweets liked by the user

        // Collection of users who retweeted the tweet
        //public ICollection<UserRetweetedTweets>? UserRetweetedTweets { get; set; }
    }

    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string? Content { get; set; }

        [ForeignKey("Tweet")]
        public int TweetId { get; set; }
        public virtual Tweet? Tweet { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }

    public class Share
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }

        // Foreign key property for Tweet
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }
    }

    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }

        // Foreign key property for Tweet
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }
    }

    public class Video
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }

        // Foreign key property for Tweet
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }
    }
}
