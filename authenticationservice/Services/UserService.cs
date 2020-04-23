using System;
using System.Threading.Tasks;
using authenticationservice.Domain;
using authenticationservice.Helpers;
using authenticationservice.Repositories;
using Google.Apis.Auth;

namespace authenticationservice.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IHasher _hasher;
        private readonly ITokenGenerator _tokenGenerator;
        
        public UserService(IUserRepository repository, IHasher hasher, ITokenGenerator tokenGenerator)
        {
            _repository = repository;
            _hasher = hasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<User> Insert(string viewName, string viewEmail, string viewPassword)
        {
            var emailuser = await _repository.Get(viewEmail);
            if (emailuser != null) throw new ArgumentException("There already exists an account with this email");
            
            var salt = _hasher.CreateSalt();
            var password = await _hasher.HashPassword(viewPassword, salt);
            var user =  new User()
            {
                Email = viewEmail,
                Name = viewName,
                Salt = salt,
                Password = password,
                OauthIssuer = "none",
            };

            return (await _repository.Create(user)).WithoutPassword();
        }

        public async Task<User> InsertGoogle(string viewTokenId)
        {
            var payload = await GoogleJsonWebSignature
                .ValidateAsync(viewTokenId, new GoogleJsonWebSignature.ValidationSettings());

            var user = await _repository.Get(payload.Email);
            if (user != null)
            {
                if (user.OauthIssuer == "Google")
                {
                    throw new ArgumentException("There already exists an account with this Google account");
                }

                throw new ArgumentException("There already exists an account with this email");
            }

            user = new User()
            {
                Name = payload.Name,
                Email = payload.Email,
                OauthIssuer = "Google",
                OauthSubject = payload.Subject,
            };

            return await _repository.Create(user);

        }
        
        public async Task<User> Login(string viewEmail, string viewPassword)
        {
            var user = await _repository.Get(viewEmail);
            if (user == null) throw new ArgumentException("There is no user with this email");

            if (!await _hasher.VerifyHash(viewPassword, user.Salt, user.Password)) 
                throw new ArgumentException("The password is not correct");

            user.Token = _tokenGenerator.CreateToken(user.Id);
            
            return user.WithoutPassword();
        }
        
        public async Task<User> LoginGoogle(string viewTokenId)
        {  
            var payload = await GoogleJsonWebSignature
                .ValidateAsync(viewTokenId, new GoogleJsonWebSignature.ValidationSettings());
            if (payload == null) throw new ArgumentException("This Google account is not registered");

            var user = await _repository.Get(payload.Email);
            if (user == null) throw new ArgumentException("The given information is not correct");

            user.Token = _tokenGenerator.CreateToken(user.Id);
            return user;
        }
    }
}