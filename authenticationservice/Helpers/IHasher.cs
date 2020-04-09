using System.Threading.Tasks;

namespace authenticationservice.Helpers
{
    public interface IHasher
    {
        Task<byte[]> HashPassword(string password, byte[] salt);
        Task<bool> VerifyHash(string password, byte[] salt, byte[] hash);
        byte[] CreateSalt();
    }
}