using System;
using System.Threading.Tasks;
using authenticationservice.Domain;
using authenticationservice.Exceptions;
using authenticationservice.Helpers;
using authenticationservice.Repositories;
using authenticationservice.Settings;
using Google.Apis.Auth;
using MessageBroker;
using Microsoft.Extensions.Options;

namespace authenticationservice.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IHasher _hasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUserValidator _userValidator;
        private readonly MessageQueueSettings _messageQueueSettings;
        private readonly IMessageQueuePublisher _messageQueuePublisher;
        
        public UserService(IUserRepository repository, IHasher hasher, 
            ITokenGenerator tokenGenerator, IUserValidator userValidator,
            IOptions<MessageQueueSettings> messageQueueSettings, IMessageQueuePublisher messageQueuePublisher)
        {
            _repository = repository;
            _hasher = hasher;
            _tokenGenerator = tokenGenerator;
            _userValidator = userValidator;
            _messageQueuePublisher = messageQueuePublisher;
            _messageQueueSettings = messageQueueSettings.Value;
        }

        public async Task<User> Insert(string viewName, string viewEmail, string viewPassword)
        {
            var emailuser = await _repository.Get(viewEmail);
            if (emailuser != null) throw new AlreadyExistsException("There already exists an account with this email");

            if (!_userValidator.ValidateEmail(viewEmail)) throw new NotValidException("The email is not valid");

            if (!_userValidator.ValidatePassword(viewPassword)) throw new NotValidException("The password does not meet the criteria");

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

            if (!_userValidator.ValidateEmail(payload.Email)) throw new NotValidException("The email is not valid");


            var user = await _repository.Get(payload.Email);
            if (user != null)
            {
                if (user.OauthIssuer == "Google")
                {
                    throw new AlreadyExistsException("There already exists an account with this Google account");
                }

                throw new AlreadyExistsException("There already exists an account with this email");
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
            if (user == null) throw new NotFoundException("There is no user with this email");

            if (!await _hasher.VerifyHash(viewPassword, user.Salt, user.Password)) 
                throw new NotValidException("The password is not correct");

            user.Token = _tokenGenerator.CreateToken(user.Id);
            
            return user.WithoutPassword();
        }
        
        public async Task<User> LoginGoogle(string viewTokenId)
        {  
            var payload = await GoogleJsonWebSignature
                .ValidateAsync(viewTokenId, new GoogleJsonWebSignature.ValidationSettings());
            if (payload == null) throw new NotFoundException("This Google account is not registered");

            var user = await _repository.Get(payload.Email);
            if (user == null) throw new NotValidException("The given information is not correct");

            user.Token = _tokenGenerator.CreateToken(user.Id);
            return user;
        }

        public async Task<User> Update(Guid id, string name)
        {
            var user = await _repository.Get(id);
            if (user.Name == name )
            {
                return user;
            }

            user.Name = name;
            await _repository.Update(id, user);

            await _messageQueuePublisher.PublishMessageAsync(_messageQueueSettings.Exchange,
                "CompanyService", "ChangeUser", new {userId = user.Id, name = user.Name});
            
            return user;
        }

        public async Task<User> Get(string email)
        {
            var user = await _repository.Get(email);
            if (user == null)
            {
                throw new NotFoundException("There is no user with this email");
            }

            return user;
        }
    }
}