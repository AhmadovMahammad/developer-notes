using Moq;
using UnitTesting.Services;

namespace xUnitTestProject
{
    // A common pattern: MethodName_StateUnderTest_ExpectedBehavior
    public class UnitTest1
    {
        private readonly UserService _userService;

        public UnitTest1()
        {
            // setup the test
            _userService = new UserService();
        }

        [Fact]
        public void RegisterUser_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            string userName = "ghost_punisher001";
            string password = "StrongPassword_-_";

            // Act
            bool response = _userService.RegisterUser(userName, password);

            // Assert
            Assert.True(response);
        }

        [Fact]
        public void RegisterUser_WithEmptyUsername_ShouldThrowArgumentNullException()
        {
            // Arrange
            string userName = "";
            string password = "StrongPassword_-_";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _userService.RegisterUser(userName, password));
        }

        [Fact]
        public void RegisterUser_WithShortPassword_ShouldThrowArgumentException()
        {
            // Arrange
            string userName = "ghost_punisher001";
            string password = "short";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _userService.RegisterUser(userName, password));
        }

        [Theory]
        [InlineData("user1", "password123", true)]
        [InlineData("user2", "", false)]
        [InlineData("", "password123", false)]
        [InlineData("user3", "short", false)]
        public void RegisterUser_ShouldValidateUserInputs(string username, string password, bool expectedResult)
        {
            // Act
            bool result;
            try
            {
                result = _userService.RegisterUser(username, password);
            }
            catch (Exception)
            {
                result = false;
            }

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("2000-05-15", 24)]
        [InlineData("1990-10-05", 34)]
        [InlineData("2010-01-20", 14)]
        public void CalculateUserAge_ShouldReturnCorrectAge(string birthdate, int expectedAge)
        {
            // Arrange
            DateTime birthDateParsed = DateTime.Parse(birthdate);

            // Act
            int age = _userService.GetUserAge(birthDateParsed);

            // Assert
            Assert.Equal(expectedAge, age);
        }
    }
}