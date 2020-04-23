using System;
using System.Text;
using System.Threading.Tasks;
using authenticationservice.Controllers;
using authenticationservice.Domain;
using authenticationservice.Services;
using authenticationservice.Views;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace authenticationservicetest.Controllers
{
    public class UserControllerTest
    {
        private readonly UserController _userController;
        private readonly Mock<IUserService> _userService;

        public UserControllerTest()
        {
            _userService = new Mock<IUserService>();
            _userController = new UserController(_userService.Object);
        }

        [Fact]
        public async Task InsertTest()
        {
            //Arrange
            const string name = "test";
            const string email = "test@test.nl";
            const string password = "secure";
            var encryptedPassword = Encoding.ASCII.GetBytes(password);
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
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

            var userView = new UserRegisterView
            {
                Name = name,
                Email = email,
                Password = password
            };

            _userService.Setup(x => x.Insert(userView.Name, userView.Email, userView.Password)).ReturnsAsync(user);

            //Act
            var result = await _userController.Insert(userView);
            var data = result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(data);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(user.Id, ((User) data.Value).Id);
        }

        [Fact]
        public async Task InsertBadRequestTest()
        {
            //Arrange
            const string name = "test";
            const string email = "test@test.nl";
            const string password = "secure";

            var userView = new UserRegisterView
            {
                Name = name,
                Email = email,
                Password = password
            };

            _userService.Setup(x => x.Insert(userView.Name, userView.Email, userView.Password))
                .Throws<ArgumentException>();

            //Act
            var result = await _userController.Insert(userView);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void InsertGoogle()
        {
            //Arrange
            const string name = "test";
            const string email = "test@test.nl";
            const string password = "secure";
            const string tokenId = "dfasfdafdasfdsa";
            var encryptedPassword = Encoding.ASCII.GetBytes(password);
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
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

            var userView = new GoogleUserView()
            {
                TokenId = tokenId
            };

            _userService.Setup(x => x.InsertGoogle(tokenId)).ReturnsAsync(user);

            //Act
            var result = await _userController.InsertGoogle(userView);
            var data = result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(data);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(user.Id, ((User) data.Value).Id);
        }

        [Fact]
        public async Task InsertGoogleBadRequestTest()
        {
            //Arrange
            const string tokenId = "dfasfdafdasfdsa";

            var userView = new GoogleUserView()
            {
                TokenId = tokenId
            };

            _userService.Setup(x => x.InsertGoogle(tokenId))
                .Throws<ArgumentException>();

            //Act
            var result = await _userController.InsertGoogle(userView);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task LoginTest()
        {
            //Arrange
            const string name = "test";
            const string email = "test@test.nl";
            const string password = "secure";
            var encryptedPassword = Encoding.ASCII.GetBytes(password);
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
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

            var userView = new UserLoginView
            {
                Email = email,
                Password = password
            };

            _userService.Setup(x => x.Login(userView.Email, userView.Password)).ReturnsAsync(user);

            //Act
            var result = await _userController.Login(userView);
            var data = result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(data);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(user.Id, ((User) data.Value).Id);
        }

        [Fact]
        public async Task LoginBadRequestTest()
        {
            //Arrange
            const string email = "test@test.nl";
            const string password = "secure";

            var userView = new UserLoginView()
            {
                Email = email,
                Password = password
            };

            _userService.Setup(x => x.Login(userView.Email, userView.Password))
                .Throws<ArgumentException>();

            //Act
            var result = await _userController.Login(userView);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task LoginGoogleTest()
        {
            //Arrange
            const string name = "test";
            const string email = "test@test.nl";
            const string password = "secure";
            var encryptedPassword = Encoding.ASCII.GetBytes(password);
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            const string token = "afdsafdsafsda";
            const string tokenId = "dfasfdafdasfdsa";

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Password = encryptedPassword,
                Salt = salt,
                Token = token
            };

            var userView = new GoogleUserView()
            {
                TokenId = tokenId
            };

            _userService.Setup(x => x.LoginGoogle(tokenId)).ReturnsAsync(user);

            //Act
            var result = await _userController.LoginGoogle(userView);
            var data = result as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(data);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(user.Id, ((User) data.Value).Id);
        }

        [Fact]
        public async Task LoginGoogleBadRequestTest()
        {
            //Arrange
            const string tokenId = "dfasfdafdasfdsa";

            var userView = new GoogleUserView()
            {
                TokenId = tokenId
            };

            _userService.Setup(x => x.LoginGoogle(tokenId))   .Throws<ArgumentException>();

            //Act
            var result = await _userController.LoginGoogle(userView);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}