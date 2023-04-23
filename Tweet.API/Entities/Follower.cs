namespace Tweet.API.Entities
{
    public class Follower
    {
        public int Id { get; set; } // Primary key for the Followers entity
        public int UserId { get; set; } // Foreign key for UserTweet
        public int FollowerId { get; set; } // Foreign key for the follower user
        // Additional properties related to the follower relationship can also be added here
    }
}
