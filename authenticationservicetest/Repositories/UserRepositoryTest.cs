using System;
using System.Text;
using authenticationservice.Domain;
using authenticationservice.Repositories;
using authenticationservice.Settings;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Mongo2Go;
using Xunit;

namespace authenticationservicetest.Repositories
{
    public class UserRepositoryTest : IDisposable
    {
        private readonly IUserRepository _userRepository;
        private readonly MongoDbRunner _mongoDbRunner;

        public UserRepositoryTest()
        {
            _mongoDbRunner = MongoDbRunner.Start();
            var settings = new UserstoreDatabaseSettings()
            {
                ConnectionString = _mongoDbRunner.ConnectionString, 
                DatabaseName = "IntegrationTest", 
                UserCollectionName = "TestCollection"
            };
            _userRepository = new UserRepository(settings);
        }

        public void Dispose()
        {
            _mongoDbRunner?.Dispose();
        }

        [Fact]
        public async void CreateTest()
        {
            //Arrange
            var user1 = new User
            {
                Name = "user1",
                Email = "user1@mail.com",
                Password = Encoding.ASCII.GetBytes("secure"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "Google",
                OauthSubject = "user1Google",
                Token = "yesyes"
            };
            var user2 = new User
            {
                Name = "user2",
                Email = "user2@mail.com",
                Password = Encoding.ASCII.GetBytes("yikes"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "None",
                OauthSubject = "",
                Token = "nono"
            };

            //Act
            var result1 = await _userRepository.Create(user1);
            var result2 = await _userRepository.Create(user2);

            //Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Equal(user1.Email, result1.Email);
            Assert.Equal(user2.Email, result2.Email);
        }

        [Fact]
        public async void GetAllTest()
        {
            //Arrange
            var user1 = new User
            {
                Name = "user1",
                Email = "user1@mail.com",
                Password = Encoding.ASCII.GetBytes("secure"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "Google",
                OauthSubject = "user1Google",
                Token = "yesyes"
            };
            var user2 = new User
            {
                Name = "user2",
                Email = "user2@mail.com",
                Password = Encoding.ASCII.GetBytes("yikes"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "None",
                OauthSubject = "",
                Token = "nono"
            };

            //Act
            await _userRepository.Create(user1);
            await _userRepository.Create(user2);

            var result = await _userRepository.Get();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(user1.Name, result[0].Name);
            Assert.Equal(user2.Name, result[1].Name);
        }

        [Fact]
        public async void GetAllEmptyTest()
        {
            //Arrange

            //Act
            var result = await _userRepository.Get();

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

       
        [Fact]
        public async void GetByEmailTest()
        {
            //Arrange
            const string email = "user1@mail.com";
            var user1 = new User
            {
                Name = "user1",
                Email = email,
                Password = Encoding.ASCII.GetBytes("secure"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "Google",
                OauthSubject = "user1Google",
                Token = "yesyes"
            };
            var user2 = new User
            {
                Name = "user2",
                Email = "user2@mail.com",
                Password = Encoding.ASCII.GetBytes("yikes"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "None",
                OauthSubject = "",
                Token = "nono"
            };

            //Act
            user1 = await _userRepository.Create(user1);
            user2 = await _userRepository.Create(user2);

            var result = await _userRepository.Get(email);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(user1.Id, result.Id);
            Assert.NotEqual(user2.Id, result.Id);
        }

        [Fact]
        public async void GetByEmailNotFoundTest()
        {
            //Arrange
            const string email = "user3@mail.com";
            var user1 = new User
            {
                Name = "user1",
                Email = "user1@mail.com",
                Password = Encoding.ASCII.GetBytes("secure"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "Google",
                OauthSubject = "user1Google",
                Token = "yesyes"
            };
            var user2 = new User
            {
                Name = "user2",
                Email = "user2@mail.com",
                Password = Encoding.ASCII.GetBytes("yikes"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "None",
                OauthSubject = "",
                Token = "nono"
            };

            //Act
            await _userRepository.Create(user1);
            await _userRepository.Create(user2);

            var result = await _userRepository.Get(email);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void GetByIdTest()
        {
            //Arrange
            var user1 = new User
            {
                Name = "user1",
                Email = "user1@mail.com",
                Password = Encoding.ASCII.GetBytes("secure"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "Google",
                OauthSubject = "user1Google",
                Token = "yesyes"
            };
            var user2 = new User
            {
                Name = "user2",
                Email = "user2@mail.com",
                Password = Encoding.ASCII.GetBytes("yikes"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "None",
                OauthSubject = "",
                Token = "nono"
            };

            //Act
            user1 = await _userRepository.Create(user1);
            user2 = await _userRepository.Create(user2);

            var result = await _userRepository.Get(user1.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(user1.Id, result.Id);
            Assert.Equal(user1.Name, result.Name);
            Assert.NotEqual(user2.Id, result.Id);
        }

        [Fact]
        public async void GetByGuidNotFoundTest()
        {
            //Arrange
            var guid = Guid.Empty;
            var user1 = new User
            {
                Name = "user1",
                Email = "user1@mail.com",
                Password = Encoding.ASCII.GetBytes("secure"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "Google",
                OauthSubject = "user1Google",
                Token = "yesyes"
            };
            var user2 = new User
            {
                Name = "user2",
                Email = "user2@mail.com",
                Password = Encoding.ASCII.GetBytes("yikes"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "None",
                OauthSubject = "",
                Token = "nono"
            };

            //Act
            await _userRepository.Create(user1);
            await _userRepository.Create(user2);

            var result = await _userRepository.Get(guid);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateTest()
        {
            //Arrange
            var user1 = new User
            {
                Name = "user1",
                Email = "user1@mail.com",
                Password = Encoding.ASCII.GetBytes("secure"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "Google",
                OauthSubject = "user1Google",
                Token = "yesyes"
            };
            var user2 = new User
            {
                Name = "user2",
                Email = "user2@mail.com",
                Password = Encoding.ASCII.GetBytes("yikes"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "None",
                OauthSubject = "",
                Token = "nono"
            };
            const string email = "new@mail.com";

            //Act
            await _userRepository.Create(user1);
            await _userRepository.Create(user2);

            user2.Email = email;

            await _userRepository.Update(user2.Id, user2);
            var result = await _userRepository.Get(user2.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async void UpdateNotFoundTest()
        {
            //Arrange
            var user1 = new User
            {
                Name = "user1",
                Email = "user1@mail.com",
                Password = Encoding.ASCII.GetBytes("secure"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "Google",
                OauthSubject = "user1Google",
                Token = "yesyes"
            };
            var user2 = new User
            {
                Name = "user2",
                Email = "user2@mail.com",
                Password = Encoding.ASCII.GetBytes("yikes"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "None",
                OauthSubject = "",
                Token = "nono"
            };
            const string email = "new@mail.com";

            //Act
            await _userRepository.Create(user1);
            await _userRepository.Create(user2);

            user2.Email = email;

            await _userRepository.Update(Guid.Empty, user2);
            var result = await _userRepository.Get(user2.Id);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(email, result.Email);
        }

        [Fact]
        public async void RemoveByUserTest()
        {
            //Arrange
            var user1 = new User
            {
                Name = "user1",
                Email = "user1@mail.com",
                Password = Encoding.ASCII.GetBytes("secure"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "Google",
                OauthSubject = "user1Google",
                Token = "yesyes"
            };
            var user2 = new User
            {
                Name = "user2",
                Email = "user2@mail.com",
                Password = Encoding.ASCII.GetBytes("yikes"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "None",
                OauthSubject = "",
                Token = "nono"
            };

            //Act
            await _userRepository.Create(user1);
            await _userRepository.Create(user2);

            await _userRepository.Remove(user2);
            var result = await _userRepository.Get(user2.Id);
            var resultAll = await _userRepository.Get();

            //Assert
            Assert.Null(result);
            Assert.NotNull(resultAll);
            Assert.NotEmpty(resultAll);
            Assert.Single(resultAll);
        }

        [Fact]
        public async void RemoveByUserNotFoundTest()
        {
            //Arrange
            var user1 = new User
            {
                Name = "user1",
                Email = "user1@mail.com",
                Password = Encoding.ASCII.GetBytes("secure"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "Google",
                OauthSubject = "user1Google",
                Token = "yesyes"
            };
            var user2 = new User
            {
                Name = "user2",
                Email = "user2@mail.com",
                Password = Encoding.ASCII.GetBytes("yikes"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "None",
                OauthSubject = "",
                Token = "nono"
            };

            //Act
            await _userRepository.Create(user1);
            await _userRepository.Create(user2);

            await _userRepository.Remove(new User());
            var resultAll = await _userRepository.Get();

            //Assert
            Assert.NotNull(resultAll);
            Assert.NotEmpty(resultAll);
            Assert.Equal(2, resultAll.Count);
        }

        [Fact]
        public async void RemoveByIdTest()
        {
            //Arrange
            var user1 = new User
            {
                Name = "user1",
                Email = "user1@mail.com",
                Password = Encoding.ASCII.GetBytes("secure"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "Google",
                OauthSubject = "user1Google",
                Token = "yesyes"
            };
            var user2 = new User
            {
                Name = "user2",
                Email = "user2@mail.com",
                Password = Encoding.ASCII.GetBytes("yikes"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "None",
                OauthSubject = "",
                Token = "nono"
            };

            //Act
            await _userRepository.Create(user1);
            await _userRepository.Create(user2);

            await _userRepository.Remove(user2.Id);
            var result = await _userRepository.Get(user2.Id);
            var resultAll = await _userRepository.Get();

            //Assert
            Assert.Null(result);
            Assert.NotNull(resultAll);
            Assert.NotEmpty(resultAll);
            Assert.Single(resultAll);
        }

        [Fact]
        public async void RemoveByIdNotFoundTest()
        {
            //Arrange
            var user1 = new User
            {
                Name = "user1",
                Email = "user1@mail.com",
                Password = Encoding.ASCII.GetBytes("secure"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "Google",
                OauthSubject = "user1Google",
                Token = "yesyes"
            };
            var user2 = new User
            {
                Name = "user2",
                Email = "user2@mail.com",
                Password = Encoding.ASCII.GetBytes("yikes"),
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20},
                OauthIssuer = "None",
                OauthSubject = "",
                Token = "nono"
            };

            //Act
            await _userRepository.Create(user1);
            await _userRepository.Create(user2);

            await _userRepository.Remove(Guid.Empty);
            var resultAll = await _userRepository.Get();

            //Assert
            Assert.NotNull(resultAll);
            Assert.NotEmpty(resultAll);
            Assert.Equal(2, resultAll.Count);
        }
    }
}