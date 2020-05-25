using authenticationservice.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Xunit;

namespace authenticationservicetest.Helpers
{
    public class UserValidatorTest
    {
        private readonly IUserValidator _validator;

        public UserValidatorTest()
        {
            _validator = new UserValidator();
        }

        [Fact]
        public void ValidatePasswordTest()
        {
            //Arrange
            var password = "Testtest1!";

            //Act
            var result = _validator.ValidatePassword(password);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidatePasswordNoCapitalTest()
        {
            //Arrange
            var password = "testtest1!";

            //Act
            var result = _validator.ValidatePassword(password);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidatePasswordNoSmallTest()
        {
            //Arrange
            var password = "testtest1!";

            //Act
            var result = _validator.ValidatePassword(password);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidatePasswordNoDigitTest()
        {
            //Arrange
            var password = "testtest!";

            //Act
            var result = _validator.ValidatePassword(password);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidatePasswordNoSpecialTest()
        {
            //Arrange
            var password = "testtest1";

            //Act
            var result = _validator.ValidatePassword(password);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidatePasswordToShortTest()
        {
            //Arrange
            var password = "Test1!";

            //Act
            var result = _validator.ValidatePassword(password);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateEmailTest()
        {
            //Arrange
            var email = "test@test.test";

            //Act
            var result = _validator.ValidateEmail(email);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateEmailNoAtSignTest()
        {
            //Arrange
            var email = "testtest.test";

            //Act
            var result = _validator.ValidateEmail(email);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateEmailNoDotTest()
        {
            //Arrange
            var email = "test@test";

            //Act
            var result = _validator.ValidateEmail(email);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateEmailNoTopDomainTest()
        {
            //Arrange
            var email = "test@test.";

            //Act
            var result = _validator.ValidateEmail(email);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateEmailNoDomainTest()
        {
            //Arrange
            var email = "test@.test";

            //Act
            var result = _validator.ValidateEmail(email);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateEmailToShortTest()
        {
            //Arrange
            var email = "test@test.n";

            //Act
            var result = _validator.ValidateEmail(email);

            //Assert
            Assert.False(result);
        }

    }
}