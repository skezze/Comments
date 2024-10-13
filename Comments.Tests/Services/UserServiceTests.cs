using NSubstitute;
using Comments.Application.Repositories;
using Comments.Application.Services;
using Comments.Domain.DTOs;
using Comments.Domain.Entities;

namespace Comments.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private IUserRepository _userRepository;
        private UserService _userService;

        [TestInitialize]
        public void SetUp()
        {
            // Create a mock for IUserRepository
            _userRepository = Substitute.For<IUserRepository>();

            // Initialize the UserService with the mock repository
            _userService = new UserService(_userRepository);
        }

        [TestMethod]
        public async Task AddUser_ShouldReturnCreatedUser()
        {
            // Arrange
            var userDto = new UserDto { UserName = "testuser", Email = "testuser@example.com" };
            var newUser = new User { Id = 1, UserName = "testuser", Email = "testuser@example.com" };

            // Mock the CreateAsync method to return the newly created user
            _userRepository.CreateAsync(Arg.Any<User>()).Returns(newUser);

            // Act
            var result = await _userService.AddUser(userDto);

            // Assert
            Assert.IsNotNull(result);  // Check that the result is not null
            Assert.AreEqual("testuser", result.UserName);  // Validate user name
            Assert.AreEqual("testuser@example.com", result.Email);  // Validate user email
            await _userRepository.Received(1).CreateAsync(Arg.Any<User>());  // Ensure CreateAsync was called once
        }

        [TestMethod]
        public async Task GetUser_ShouldReturnUserIfExists()
        {
            // Arrange
            var userDto = new UserDto { UserName = "testuser", Email = "testuser@example.com" };
            var existingUser = new User { Id = 1, UserName = "testuser", Email = "testuser@example.com" };

            // Mock the GetAsync method to return an existing user
            _userRepository.GetAsync(Arg.Any<User>()).Returns(existingUser);

            // Act
            var result = await _userService.GetUser(userDto);

            // Assert
            Assert.IsNotNull(result);  // Check that the result is not null
            Assert.AreEqual("testuser", result.UserName);  // Validate user name
            Assert.AreEqual("testuser@example.com", result.Email);  // Validate user email
            await _userRepository.Received(1).GetAsync(Arg.Any<User>());  // Ensure GetAsync was called once
        }

        [TestMethod]
        public async Task GetUser_ShouldReturnNullIfUserDoesNotExist()
        {
            // Arrange
            var userDto = new UserDto { UserName = "nonexistentuser", Email = "nonexistent@example.com" };

            // Mock the GetAsync method to return null (user does not exist)
            _userRepository.GetAsync(Arg.Any<User>()).Returns((User)null);

            // Act
            var result = await _userService.GetUser(userDto);

            // Assert
            Assert.IsNull(result);  // Check that the result is null since the user doesn't exist
            await _userRepository.Received(1).GetAsync(Arg.Any<User>());  // Ensure GetAsync was called once
        }
    }
}
