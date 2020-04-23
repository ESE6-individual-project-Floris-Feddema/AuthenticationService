using System;
using authenticationservice.Domain;
using authenticationservice.Helpers;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace authenticationservicetest.Helpers
{
    
    public class TokenGeneratorTest
    {

        private readonly ITokenGenerator _tokenGenerator;

        public TokenGeneratorTest()
        {
            var appSettings = Mock.Of<IOptions<AppSettings>>(
                p => p.Value == new AppSettings
                {
                    GoogleClientId = "xxxx", 
                    GoogleClientSecret = "yyyy", 
                    JwtSecret = "abcdefghijklmnopqrstuvwxyz"
                });

            _tokenGenerator = new TokenGenerator(appSettings);
        }

        [Fact]
        public void CreateTokenTest()
        {
            //Arrange
            var id = Guid.NewGuid();
            
            //Act
            var token = _tokenGenerator.CreateToken(id);
            
            //Assert
            Assert.NotNull(token);
            Assert.Contains(".", token);
        }
        
        [Fact]
        public void CreateTokenDifferentIdTest()
        {
            //Arrange
            var id = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            
            //Act
            var token = _tokenGenerator.CreateToken(id);
            var token2 = _tokenGenerator.CreateToken(id2);
            
            //Assert
            Assert.NotNull(token);
            Assert.NotNull(token2);
            Assert.Contains(".", token);
            Assert.Contains(".", token2);
            Assert.NotEqual(token, token2);
        }
        
        [Fact]
        public void CreateTokenSameIdTest()
        {
            //Arrange
            var id = Guid.NewGuid();
            
            //Act
            var token = _tokenGenerator.CreateToken(id);
            var token2 = _tokenGenerator.CreateToken(id);
            
            //Assert
            Assert.NotNull(token);
            Assert.NotNull(token2);
            Assert.Contains(".", token);
            Assert.Contains(".", token2);
            Assert.Equal(token, token2);
        }
    }
}