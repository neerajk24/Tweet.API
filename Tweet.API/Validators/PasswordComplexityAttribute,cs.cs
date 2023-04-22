using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Tweet.API.Validators
{
    public class PasswordComplexityAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var password = value as string;

            // Check if password is null or empty
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            // Check if password contains at least one capital letter, one small letter, one number, and one symbol
            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=]).{6,}$"))
            {
                return false;
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The {name} field must contain at least one capital letter, one small letter, one number, and one symbol.";
        }
    }


}
