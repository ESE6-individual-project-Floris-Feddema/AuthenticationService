using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;

namespace authenticationservice.Helpers
{
    public class Hasher : IHasher
    {
        public Task<byte[]> HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt, 
                DegreeOfParallelism = 2, 
                Iterations = 4, 
                MemorySize = 1024
            };

            return argon2.GetBytesAsync(16);
        }

        public byte[] CreateSalt()
        {
            var buffer = new byte[16];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }

        public async Task<bool> VerifyHash(string password, byte[] salt, byte[] hash)
        {
            var newHash = await HashPassword(password, salt);
            return hash.SequenceEqual(newHash);
        }
    }
}