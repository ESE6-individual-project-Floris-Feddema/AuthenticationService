using System.Text.RegularExpressions;

namespace authenticationservice.Helpers
{
    public class UserValidator : IUserValidator
    {
        public bool ValidatePassword(string input)
        {
            var regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,})");
            return regex.IsMatch(input);
        }

        public bool ValidateEmail(string input)
        {
            var regex = new Regex("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$");
            return regex.IsMatch(input);
        }
    }
}