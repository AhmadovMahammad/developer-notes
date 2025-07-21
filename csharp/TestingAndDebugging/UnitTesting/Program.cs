internal class Program
{
    private static void Main(string[] args)
    {
        /* Introduction to Unit Testing
         
        --Purpose of Unit Testing: 
        
        Verify that individual pieces of code (typically methods) work as expected. 
        Unit tests isolate each part of the program and validate its correctness, 
        promoting code quality and enabling faster debugging.



        --Frameworks:
        
        1) xUnit: Favored for .NET Core projects, known for its extensibility and active development.
        2) NUnit: Popular, highly flexible, and integrates well with various tools.
        3) MSTest: Microsoft's built-in testing framework for Visual Studio.



        --Key Unit Testing Principles
        
        Each test should focus on a single unit of work without external dependencies.
        Tests should yield the same results each time, unaffected by outside factors.
        Tests should not depend on each other; this avoids cascading failures.
        Well-organized, readable, and maintainable test code is essential.



        --Setting up a Testing Project
        Create a test project separate from the main application (e.g., in a Tests folder).
        
        Install the necessary testing libraries:
            For xUnit: dotnet add package xunit
            For NUnit: dotnet add package NUnit
            For MSTest: dotnet add package MSTest.TestFramework



        --Naming Conventions:
        Use meaningful test names that reflect the behavior being tested. 
        
        A common pattern: MethodName_StateUnderTest_ExpectedBehavior



        --Arrange, Act, Assert (AAA) Pattern
        The AAA pattern is the foundational structure for writing unit tests:

        Arrange: Set up necessary preconditions and inputs.
        Act: Execute the method under test.
        Assert: Verify the result to check if it meets expectations.

        */

        /* xUnit Testing
         
        Writing tests using xUnit is essential for building robust, reliable applications. 
        xUnit, a widely-used testing framework for .NET, is known for its flexibility and effectiveness in 
        supporting modern testing practices like test-driven development (TDD) and continuous integration (CI).

        ---Key Theoretical Concepts in xUnit Testing

        xUnit provides an attribute-based approach to testing, enabling developers to structure and organize tests in a meaningful way.

        1) [Fact]: Represents a standalone test case. 
        It’s used when a single, specific scenario is tested, and no parameterization is required.

        2) [Theory]: Used for data-driven tests. When testing a function with multiple inputs, 
        [Theory] helps avoid repetition by using data sets to create multiple scenarios for a single test.

        3) [InlineData]: Defines the data sets for a [Theory]. 
        Each [InlineData] entry represents one test case, enabling parameterized testing.


        ---What are Testing Patterns in xUnit?

        1) Arrange-Act-Assert (AAA) Pattern: Organizes tests by dividing them into three distinct sections:

        Arrange: Set up the context or state.
        Act: Perform the action you’re testing.
        Assert: Verify that the action produced the expected outcome.

        2) Single Responsibility in Tests: A test should focus on verifying one specific behavior or outcome. 
        By adhering to this, each test failure will indicate a single, specific issue, making debugging easier.


        ---Assertions in xUnit

        Assertions verify that a particular condition is met. xUnit offers a range of assertions to handle various scenarios:

        1) Equality Assertions: 
        Assert.Equal checks if two values are equal, Assert.NotEqual checks if they aren’t.

        2) Null Assertions: 
        Assert.Null and Assert.NotNull are used to check for null values.
        
        3) Boolean Assertions: 
        Assert.True and Assert.False validate boolean conditions.
        
        4) Exception Assertions: 
        Assert.Throws verifies that an expected exception is thrown during a test, often used for error handling scenarios.

        public class CalculatorTests
        {
            [Fact]
            public void Divide_ShouldThrowDivideByZeroException_WhenDivisorIsZero()
            {
                // Arrange
                var calculator = new Calculator();
        
                // Act & Assert
                Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
            }
        }

        */

        /* Example 1: Testing with [Fact] for Fixed Scenarios

        public class UserService
        {
            public bool RegisterUser(string username, string password)
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException("username and password cannot be empty.");
                }

                if (password.Length < 8)
                {
                    throw new ArgumentException("Password must be at lest 8 characters.");
                }

                return true;
            }

            public int GetUserAge(DateTime birthdate)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - birthdate.Year;

                //today: 11/12/2024
                //input: 05/31/2003 

                //age: 21
                //if( 11/12/2003 < 05/31/2003 )
                //    age--

                // get exact age
                if (today.AddYears(-age) < birthdate)
                {
                    age--;
                }

                return age;
            }
        }
        
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
            }
        }
        
        */

        /* Example 2: Testing Multiple Scenarios with [Theory] and [InlineData]
         
        Using [Theory] and [InlineData] enables us to test multiple input combinations without duplicating code.

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

        Summary:
        
        [Fact] is used for single, straightforward tests that don’t vary with different inputs.
        [Theory] with [InlineData] is ideal for testing the same logic with multiple sets of data, 
        minimizing code repetition.

        */

        /* What is Mocking
         
        Mocking is a technique used in unit testing to simulate the behavior of external systems or components
        that your unit under test interacts with. 
        
        It allows you to isolate the code you are testing from external dependencies such as databases, file systems, or external APIs. 
        Mocking is vital because it helps you test the behavior of yozur unit in a controlled and predictable environment.

        When performing unit tests, we don't want to rely on external systems or services because:
        1) They may be slow.
        2) They might not be easily available.
        3) They could introduce variability that makes the test unreliable.

        Mocking solves this problem by creating controlled, lightweight objects that mimic the behavior of these external systems.

        --Why is Mocking Important?

        It ensures the unit being tested doesn’t depend on any external resources.
        External resources like databases and APIs can slow down tests. Mocks bypass these, speeding up tests.


        ---Creating a Mock Object

        Creating a mock object with Moq is simple. You use the Mock<T> class where T is the type you want to mock.
        Typically an interface or an abstract class.

        */
    }
}