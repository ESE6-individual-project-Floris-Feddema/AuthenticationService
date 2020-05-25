using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using authenticationservice.Domain;
using authenticationservice.Helpers;
using Xunit;

namespace authenticationservicetest.Helpers
{
    public class ExtentionsMethodsTest
    {
        [Fact]
        public void WithoutPasswordTest()
        {
            //Arrange
            var user = new User()
            {
                Email = "xxx@xxx.com",
                Id = Guid.NewGuid(),
                Name = "xxx",
                OauthIssuer = "test",
                OauthSubject = "test",
                Password = new byte[] {0x20, 0x20, 0x20},
                Salt = new byte[] {0x20, 0x20, 0x20},
                Token = "",
            };

            //Act
            var result = user.WithoutPassword();

            //Assert
            Assert.NotNull(result);

            Assert.NotNull(result.Email);
            Assert.NotNull(result.Name);

            Assert.Null(result.OauthIssuer);
            Assert.Null(result.OauthSubject);
            Assert.Null(result.Password);
            Assert.Null(result.Salt);
        }

        [Fact]
        public void WithoutsPasswordTest()
        {
            //Arrange
            var users = new List<User>();

            for (var i = 0; i < 5; i++)
            {
                users.Add(new User()
                    {
                        Email = "xxx@xxx.com",
                        Id = Guid.NewGuid(),
                        Name = "xxx",
                        OauthIssuer = "test",
                        OauthSubject = "test",
                        Password = new byte[] {0x20, 0x20, 0x20},
                        Salt = new byte[] {0x20, 0x20, 0x20},
                        Token = "",
                    }
                    );
            }

            //Act
            var result = users.WithoutPasswords();

            //Assert
            Assert.NotNull(result);
            for (var i = 0; i < 5; i++)
            {
                Assert.NotNull(result[i]);

                Assert.NotNull(result[i].Email);
                Assert.NotNull(result[i].Name);
                               
                Assert.Null(result[i].OauthIssuer);
                Assert.Null(result[i].OauthSubject);
                Assert.Null(result[i].Password);
                Assert.Null(result[i].Salt);
            }
        }

    }
}