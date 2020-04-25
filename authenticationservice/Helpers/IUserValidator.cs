using System;

namespace authenticationservice.Helpers
{
    public interface IUserValidator
    {
        /// <summary>
        /// Validates an string to match the password requirements
        /// </summary>
        /// <param name="input">The string to validate</param>
        /// <returns>True if is is valid</returns>
        bool ValidatePassword(string input);

        /// <summary>
        /// Validates an string to match the email requirements
        /// </summary>
        /// <param name="input">The string to validate</param>
        /// <returns>True if is is valid</returns>
        bool ValidateEmail(string input);
    }
}