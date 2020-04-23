using System;
using System.Threading.Tasks;
using authenticationservice.Domain;
using authenticationservice.Helpers;
using Xunit;

namespace authenticationservicetest.Helpers
{
    public class HasherTest
    {
        private readonly IHasher _hasher;

        public HasherTest()
        {
            _hasher = new Hasher();
        }

        [Fact]
        public void CreateSaltTest()
        {
            //Arrange
         
            //Act
            var result = _hasher.CreateSalt();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(16, result.Length);
        }

        [Fact]
        public async Task HashPasswordNotNullTest()
        {
            //Arrange
            var password = "test123";

            //Act
            var salt = _hasher.CreateSalt();
            var result = await _hasher.HashPassword(password, salt);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task HashPasswordNotEqualTest()
        {
            //Arrange
            var password = "test123";
            var password2 = "test123";

            //Act
            var salt = _hasher.CreateSalt();
            var result = await _hasher.HashPassword(password, salt);
            var result2 = await _hasher.HashPassword(password2, salt);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result2);
            Assert.Equal(result, result2);
        }

        [Fact]
        public async Task VerifyNotNullTest()
        {
            //Arrange
            var password = "test123";

            //Act
            var salt = _hasher.CreateSalt();
            var hash = await _hasher.HashPassword(password, salt);
            var result = await _hasher.VerifyHash(password, salt, hash);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task VerifyIncorrectTest()
        {
            //Arrange
            var password = "test123";
            var password2 = "test1234";


            //Act
            var salt = _hasher.CreateSalt();
            var hash = await _hasher.HashPassword(password, salt);
            var hash2 = await _hasher.HashPassword(password2, salt);
            var result = await _hasher.VerifyHash(password, salt, hash2);

            //Assert
            Assert.NotNull(hash);
            Assert.NotNull(hash2);
            Assert.NotEqual(hash, hash2);
            Assert.False(result);
        }

        [Fact]
        public async Task VerifyIncorrectSaltTest()
        {
            //Arrange
            var password = "test123";


            //Act
            var salt = _hasher.CreateSalt();
            var salt2 = _hasher.CreateSalt();
            var hash = await _hasher.HashPassword(password, salt);
            var hash2 = await _hasher.HashPassword(password, salt2);
            var result = await _hasher.VerifyHash(password, salt2, hash);

            //Assert
            Assert.NotNull(hash);
            Assert.NotNull(hash2);
            Assert.NotEqual(hash, hash2);
            Assert.False(result);
        }
    }
}