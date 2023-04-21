using System.ComponentModel.DataAnnotations;

namespace Tweet.API.Model
{
    public class CreateUserRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)] // Example of minimum password length requirement
        public string Password { get; set; }
    }

}
