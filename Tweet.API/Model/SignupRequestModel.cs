using System.ComponentModel.DataAnnotations;
using Tweet.API.Validators;

namespace Tweet.API.Model
{
    public class SignupRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        [PasswordComplexity(ErrorMessage = "The Password field must contain at least one capital letter, one small letter, one number, and one symbol.")]
        public string Password { get; set; }
    }

}
