using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Tweet.API.Entities
{
    public class Tweet
    {
        [Key]
        public int Id { get; set; } // Primary key property
        public string Content { get; set; }
        public int Likes { get; set; }

        // Foreign key property for User
        public int UserId { get; set; }
        public User User { get; set; }

        // Navigation properties for related entities
        public List<Comment> Comments { get; set; }
        public List<Share> Shares { get; set; }
        public List<Image> Images { get; set; }
        public List<Video> Videos { get; set; }

        // Additional properties for a tweet such as timestamp, author, etc. can also be added here
    }

    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }

        // Foreign key property for Tweet
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }
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
