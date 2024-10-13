using Comments.Application.Services;
using Comments.Application.Repositories;
using Comments.Domain.Entities;
using Comments.Domain.DTOs;
using NSubstitute;

namespace Comments.Tests.Services
{
    [TestClass]
    public class CommentServiceTests
    {
        private ICommentRepository _commentRepository;
        private IUserService _userService;
        private CommentService _commentService;

        [TestInitialize]
        public void SetUp()
        {
            // Create substitutes (mocks) for the dependencies
            _commentRepository = Substitute.For<ICommentRepository>();
            _userService = Substitute.For<IUserService>();

            // Inject mocks into the service
            _commentService = new CommentService(_commentRepository, _userService);
        }

        [TestMethod]
        public async Task AddComment_ExistingUser_AddsComment()
        {
            // Arrange
            var userDto = new UserDto { Email = "test@test.com", UserName = "testuser" };
            var user = new User { Id = 1, Email = "test@test.com", UserName = "testuser" };
            var commentDto = new CommentDto { Text = "Test comment", UserDto = userDto };

            var newComment = new Comment { Id = 1, Text = "Test comment", User = user };

            // Mock user retrieval to return an existing user
            _userService.GetUser(Arg.Any<UserDto>()).Returns(user);

            // Mock comment repository to return the created comment
            _commentRepository.AddComment(Arg.Any<Comment>()).Returns(newComment);

            // Act
            var result = await _commentService.AddComment(commentDto);

            // Assert
            Assert.IsNotNull(result);  // The result should not be null now
            Assert.AreEqual("Test comment", result.Text);  // Verify the comment text
            Assert.AreEqual(user, result.User);  // Verify the comment user

            await _commentRepository.Received(1).AddComment(Arg.Any<Comment>());
        }


        [TestMethod]
        public async Task AddComment_NewUser_AddsUserAndComment()
        {
            // Arrange
            var userDto = new UserDto { Email = "newuser@test.com", UserName = "newuser" };
            var newUser = new User { Id = 2, Email = "newuser@test.com", UserName = "newuser" };
            var commentDto = new CommentDto { Text = "New comment", UserDto = userDto };

            var newComment = new Comment { Id = 1, Text = "New comment", User = newUser };

            // Mock user retrieval to return null (user doesn't exist)
            _userService.GetUser(Arg.Any<UserDto>()).Returns((User)null);

            // Mock user creation for the new user
            _userService.AddUser(Arg.Any<UserDto>()).Returns(newUser);

            // Mock comment repository to return the created comment
            _commentRepository.AddComment(Arg.Any<Comment>()).Returns(newComment);

            // Act
            var result = await _commentService.AddComment(commentDto);

            // Assert
            Assert.IsNotNull(result);  // The result should not be null now
            Assert.AreEqual("New comment", result.Text);  // Verify the comment text
            Assert.AreEqual(newUser, result.User);  // Verify the comment user

            await _userService.Received(1).AddUser(Arg.Any<UserDto>());
            await _commentRepository.Received(1).AddComment(Arg.Any<Comment>());
        }


        [TestMethod]
        public async Task GetCommentsDescended_ReturnsCommentsList()
        {
            // Arrange
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Text = "First comment" },
                new Comment { Id = 2, Text = "Second comment" }
            };

            // Mock the repository to return a list of comments
            _commentRepository.GetCommentsDescended().Returns(comments);

            // Act
            var result = await _commentService.GetCommentsDescended();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("First comment", result[0].Text);
            Assert.AreEqual("Second comment", result[1].Text);
            await _commentRepository.Received(1).GetCommentsDescended();
        }
    }
}
