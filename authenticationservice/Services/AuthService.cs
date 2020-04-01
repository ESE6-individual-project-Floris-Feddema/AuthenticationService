using System;
using System.Threading.Tasks;
using authenticationservice.Repositories;

namespace authenticationservice.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;

        public AuthService(IUserRepository repository)
        {
            this._repository = repository;
            Refresh();
        }
        
        public async Task<User> Authenticate(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload)
        {
            var u = await _repository.Get(payload.Email);
            if (u != null) return u;
            
            u = new User()
            {
                Id = Guid.NewGuid(),
                Name = payload.Name,
                Email = payload.Email,
                OauthSubject = payload.Subject,
                OauthIssuer = payload.Issuer
            };

            return await _repository.Create(u);

        }

        private async Task Refresh()
        {
            var list = await _repository.Get();
            if (list.Count != 0) return;

            await _repository.Create(new User()
                {
                    Id = Guid.NewGuid(), 
                    Name = "Test Person1", 
                    Email = "testperson1@gmail.com",
                });
            
            await _repository.Create(new User()
            {
                Id = Guid.NewGuid(), 
                Name = "Test Person2", 
                Email = "testperson2@gmail.com",
            });
            
            await _repository.Create(new User()
            {
                Id = Guid.NewGuid(), 
                Name = "Test Person3", 
                Email = "testperson3@gmail.com",
            }); 
        }
    }
}