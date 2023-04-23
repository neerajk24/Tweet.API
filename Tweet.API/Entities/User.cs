namespace Tweet.API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string ProfilePicture { get; set; } // Profile picture field
        public string Bio { get; set; } // Bio field
        public int FollowersCount { get; set; } // Followers count field
        public List<Tweet> Tweets { get; set; } // Newly added field as a list of UserTweet
    }

}
