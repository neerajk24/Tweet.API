using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Tweet.API.Model
{
    public class CommentModel
    {
        [Required]
        public int TweetId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Content { get; set; }
    }

}
