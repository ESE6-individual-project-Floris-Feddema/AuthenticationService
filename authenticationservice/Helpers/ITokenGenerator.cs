using System;

namespace authenticationservice.Helpers
{
    public interface ITokenGenerator
    {
        /// <summary>
        /// Generates a new JWT based on the users Guid
        /// </summary>
        /// <param name="id">The Guid of the user</param>
        /// <returns>A JWT</returns>
        string CreateToken(Guid id);
    }
}
