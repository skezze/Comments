using NSubstitute;
using Comments.Application.Features;
using Comments.Application.Services;
using Comments.Domain.DTOs;
using Comments.Domain.Entities;
using Comments.Domain.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;

namespace Comments.Tests.Controllers
{
    [TestClass]
    public class CommentsControllerTests
    {
        private IMemoryCache _memoryCache;
        private CommentQueue _commentQueue;
        private ICommentService _commentService;
        private CommentsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _memoryCache = Substitute.For<IMemoryCache>();
            _commentQueue = new CommentQueue();
            _commentService = Substitute.For<ICommentService>();
            _controller = new CommentsController(_memoryCache, _commentQueue, _commentService);
        }

        [TestMethod]
        public async Task Get_ShouldReturnCommentsFromCache_WhenCacheIsSet()
        {
            // Arrange
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Text = "Test Comment", DateAdded = DateTime.Now }
            };


            _memoryCache.TryGetValue("commentsCache", out Arg.Any<object>())
                .Returns(x =>
                {
                    x[1] = comments;
                    return true;
                });

            // Act
            var result = await _controller.Get() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(comments, result.Value);
        }


        [TestMethod]
        public async Task Get_ShouldReturnCommentsFromService_WhenCacheIsNotSet()
        {
            // Arrange
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Text = "Test Comment", DateAdded = DateTime.Now }
            };
            object cachedComments = null;

            _memoryCache.TryGetValue("commentsCache", out cachedComments).Returns(false);
            _commentService.GetCommentsDescended().Returns(comments);

            // Act
            var result = await _controller.Get() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(comments, result.Value);

            _memoryCache.Received(1).Set("commentsCache", comments, TimeSpan.FromMinutes(5));
        }


        [TestMethod]
        public async Task Post_ShouldAddCommentToQueue_AndSendMessageViaWebSocket()
        {
            // Arrange
            var commentViewModel = new CommentViewModel
            {
                Text = "Test Comment",
                User = new UserViewModel { Email = "test@example.com", UserName = "testuser" }
            };
            var userDto = new UserDto { Email = "test@example.com", UserName = "testuser" };
            var commentDto = new CommentDto { Text = "Test Comment", UserDto = userDto };
            var comment = new Comment { Id = 1, Text = "Test Comment", DateAdded = DateTime.Now };

            _commentService.AddComment(Arg.Any<CommentDto>()).Returns(comment);

            // Act
            var result = await _controller.Post(commentViewModel) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(comment, result.Value);

            Assert.AreEqual(1, _commentQueue.DequeueAll().Count());
        }

    }
}
