using System.Threading.Tasks;

namespace authenticationservice.Services
{
    public interface IAuthService
    {
        Task<User> Authenticate(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload);
    }
}