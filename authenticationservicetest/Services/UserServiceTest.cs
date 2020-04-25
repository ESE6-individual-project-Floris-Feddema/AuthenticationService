using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using authenticationservice.Domain;
using authenticationservice.Exceptions;
using authenticationservice.Helpers;
using authenticationservice.Repositories;
using authenticationservice.Services;
using Moq;
using Xunit;

namespace authenticationservicetest.Services
{
    public class UserServiceTest
    {
        private readonly IUserService _userService;

        private readonly Mock<ITokenGenerator> _tokenGenerator;
        private readonly Mock<IHasher> _hasher;
        private readonly Mock<IUserRepository> _repository;
        private readonly Mock<IUserValidator> _validator;
        
        public UserServiceTest()
        {
            _tokenGenerator = new Mock<ITokenGenerator>();
            _hasher = new Mock<IHasher>();
            _repository = new Mock<IUserRepository>();
            _validator = new Mock<IUserValidator>();
            _userService = new UserService(_repository.Object, _hasher.Object, _tokenGenerator.Object, _validator.Object);
        }

        [Fact]
        public async void InsertTest()
        {
            //Arrange
            const string name = "test";
            const string email = "test@test.nl";
            const string password = "secure";
            var encryptedPassword = Encoding.ASCII.GetBytes(password);
            var salt = new byte[] { 0x20, 0x20, 0x20, 0x20};
            const string token = "afdsafdsafsda";
            
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Password = encryptedPassword,
                Salt = salt,
                Token = token
            };

            _repository.Setup(x => x.Get(email)).ReturnsAsync((User)null);
            _hasher.Setup(x => x.CreateSalt()).Returns(salt);
            _hasher.Setup(x => x.HashPassword(password, salt)).ReturnsAsync(encryptedPassword);
            _repository.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(user);

            _validator.Setup(x => x.ValidateEmail(It.IsAny<string>())).Returns(true);
            _validator.Setup(x => x.ValidatePassword(It.IsAny<string>())).Returns(true);
            
            //Act
            var result = await _userService.Insert(name, email, password);

            
            //Assert
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Token, result.Token);
            Assert.Null(result.Password);
            Assert.Null(result.Salt);
            Assert.Null(result.OauthSubject);
            Assert.Null(result.OauthIssuer);
            Assert.NotNull(result);
        }

        [Fact]
        public async void InsertNotValidEmailTest()
        {
            //Arrange
            const string name = "test";
            const string email = "test@testnl";
            const string password = "secure";
            var encryptedPassword = Encoding.ASCII.GetBytes(password);
            var salt = new byte[] { 0x20, 0x20, 0x20, 0x20};
            const string token = "afdsafdsafsda";
            
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Password = encryptedPassword,
                Salt = salt,
                Token = token
            };

            _repository.Setup(x => x.Get(email)).ReturnsAsync((User)null);
            _hasher.Setup(x => x.CreateSalt()).Returns(salt);
            _hasher.Setup(x => x.HashPassword(password, salt)).ReturnsAsync(encryptedPassword);
            _repository.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(user);

            _validator.Setup(x => x.ValidateEmail(It.IsAny<string>())).Returns(false);
            _validator.Setup(x => x.ValidatePassword(It.IsAny<string>())).Returns(true);
            
            //Act
            var result = await Assert.ThrowsAsync<NotValidException>(() => _userService.Insert(name, email, password));

            
            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotValidException>(result);
        }

        [Fact]
        public async void InsertNotValidPasswordTest()
        {
            //Arrange
            const string name = "test";
            const string email = "test@testnl";
            const string password = "secure";
            var encryptedPassword = Encoding.ASCII.GetBytes(password);
            var salt = new byte[] { 0x20, 0x20, 0x20, 0x20};
            const string token = "afdsafdsafsda";
            
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Password = encryptedPassword,
                Salt = salt,
                Token = token
            };

            _repository.Setup(x => x.Get(email)).ReturnsAsync((User)null);
            _hasher.Setup(x => x.CreateSalt()).Returns(salt);
            _hasher.Setup(x => x.HashPassword(password, salt)).ReturnsAsync(encryptedPassword);
            _repository.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(user);

            _validator.Setup(x => x.ValidateEmail(It.IsAny<string>())).Returns(true);
            _validator.Setup(x => x.ValidatePassword(It.IsAny<string>())).Returns(false);
            
            //Act
            var result = await Assert.ThrowsAsync<NotValidException>(() => _userService.Insert(name, email, password));

            
            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotValidException>(result);
        }


        [Fact]
        public async void InsertAlreadyExistsTest()
        {
            //Arrange
            const string name = "test";
            const string email = "test@test.nl";
            const string password = "secure";
            var encryptedPassword = Encoding.ASCII.GetBytes(password);
            var salt = new byte[] { 0x20, 0x20, 0x20, 0x20};
            
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Password = encryptedPassword,
                Salt = salt
            };

            _repository.Setup(x => x.Get(email)).ReturnsAsync(user);
            _hasher.Setup(x => x.CreateSalt()).Returns(salt);
            _hasher.Setup(x => x.HashPassword(password, salt)).ReturnsAsync(encryptedPassword);
            _repository.Setup(x => x.Create(user)).ReturnsAsync(user);

            _validator.Setup(x => x.ValidateEmail(It.IsAny<string>())).Returns(true);
            _validator.Setup(x => x.ValidatePassword(It.IsAny<string>())).Returns(true);
            
            //Act
            var result = await Assert.ThrowsAsync<AlreadyExistsException>(() => _userService.Insert(name, email, password));

            
            //Assert
            Assert.NotNull(result);
            Assert.IsType<AlreadyExistsException>(result);
        }
        
        [Fact]
        public async Task LoginTest()
        {
            //Arrange
            var id = Guid.NewGuid();
            const string name = "test";
            const string email = "test@test.nl";
            const string password = "secure";
            var encryptedPassword = Encoding.ASCII.GetBytes(password);
            var salt = new byte[] { 0x20, 0x20, 0x20, 0x20};
            const string token = "zqawsexdctvbyunimo";

            var user = new User
            {
                Id = id,
                Name = name,
                Email = email,
                Password = encryptedPassword,
                Salt = salt
            };

            _repository.Setup(x => x.Get(email)).ReturnsAsync(user);
            _hasher.Setup(x => x.VerifyHash(password, salt, encryptedPassword)).ReturnsAsync(true);
            _tokenGenerator.Setup(x => x.CreateToken(user.Id)).Returns(token);
            
            //Act
            var result = await _userService.Login(email, password);

            
            //Assert
            Assert.NotNull(result);
            Assert.Equal(id, user.Id);
        }

        [Fact]
        public async Task LoginWrongPasswordTest()
        {
            //Arrange
            var id = Guid.NewGuid();
            const string name = "test";
            const string email = "test@test.nl";
            const string password = "secure";
            var encryptedPassword = Encoding.ASCII.GetBytes(password);

            var salt = new byte[] { 0x20, 0x20, 0x20, 0x20};
            const string token = "zqawsexdctvbyunimo";

            var user = new User
            {
                Id = id,
                Name = name,
                Email = email,
                Password = encryptedPassword,
                Salt = salt
            };

            _repository.Setup(x => x.Get(email)).ReturnsAsync(user);
            _hasher.Setup(x => x.VerifyHash(password, salt, encryptedPassword)).ReturnsAsync(false);
            _tokenGenerator.Setup(x => x.CreateToken(user.Id)).Returns(token);
            
            //Act
            var result = await Assert.ThrowsAsync<NotValidException>(() => _userService.Login(email, password));

            
            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotValidException>(result);
        }
        
        [Fact]
        public async Task LoginNotExistsTest()
        {
            //Arrange
            var id = Guid.NewGuid();
            const string name = "test";
            const string email = "test@test.nl";
            const string password = "secure";
            var encryptedPassword = Encoding.ASCII.GetBytes(password);
            var salt = new byte[] { 0x20, 0x20, 0x20, 0x20};
            const string token = "zqawsexdctvbyunimo";

            var user = new User
            {
                Id = id,
                Name = name,
                Email = email,
                Password = encryptedPassword,
                Salt = salt
            };

            _repository.Setup(x => x.Get(email)).ReturnsAsync((User)null);
            _hasher.Setup(x => x.VerifyHash(password, salt, encryptedPassword)).ReturnsAsync(true);
            _tokenGenerator.Setup(x => x.CreateToken(user.Id)).Returns(token);
            
            //Act
            var result = await Assert.ThrowsAsync<NotFoundException>(() => _userService.Login(email, password));

            
            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundException>(result);
        }
    }
}