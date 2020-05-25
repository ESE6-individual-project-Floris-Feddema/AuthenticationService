using System.Threading.Tasks;

namespace authenticationservice.Helpers
{
    public interface IHasher
    {
        /// <summary>
        /// Hashes a password with a given salt
        /// </summary>
        /// <param name="password">Password to be hashed</param>
        /// <param name="salt">the salt used in the hash</param>
        /// <returns>return the hashed password as an byte array</returns>
        Task<byte[]> HashPassword(string password, byte[] salt);
        
        /// <summary>
        /// Verifies a given password and hash
        /// </summary>
        /// <param name="password">The password to be verified</param>
        /// <param name="salt">The salt used in the hashing process</param>
        /// <param name="hash">The hash to compare to</param>
        /// <returns>returns true if the password and salt matches the given hash</returns>
        Task<bool> VerifyHash(string password, byte[] salt, byte[] hash);
        
        /// <summary>
        /// Generates a pseudorandom salt as a byte array
        /// </summary>
        /// <returns>16 bit pseudorandom byte array</returns>
        byte[] CreateSalt();
    }
}