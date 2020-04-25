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
            var regex = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
            return regex.IsMatch(input);
        }
    }
}