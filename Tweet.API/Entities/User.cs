namespace Tweet.API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public int FollowersCount { get; set; } = 0;
        public virtual ICollection<Tweet>? Tweets { get; set; }
        public virtual ICollection<User>? Following { get; set; }

        // public ICollection<UserLikedTweets>? UserLikedTweets { get; set; }
        //public ICollection<UserRetweetedTweets>? UserRetweetedTweets { get; set; }
    }
}
