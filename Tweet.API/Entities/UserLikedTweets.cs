namespace Tweet.API.Entities
{
    public class UserLikedTweets
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int TweetId { get; set; }
        public Tweet? Tweet { get; set; }
    }
}
