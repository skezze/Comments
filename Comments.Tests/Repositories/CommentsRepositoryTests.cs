using Comments.Application.Repositories;
using Comments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Comments.Tests.Repositories
{
    [TestClass]
    public class CommentsRepositoryTests
    {
        private DbContextOptions<CommentContext> _options;
        private CommentContext _context;
        private CommentsRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            // Настройка InMemory базы данных
            _options = new DbContextOptionsBuilder<CommentContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CommentContext(_options);
            _repository = new CommentsRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task AddComment_ShouldAddCommentToDatabase()
        {
            // Arrange
            var newComment = new Comment { Id = 1, Text = "New Comment", DateAdded = System.DateTime.Now };

            // Act
            var result = await _repository.AddComment(newComment);
            var commentInDb = await _context.Comments.FindAsync(newComment.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newComment.Text, result.Text);
            Assert.AreEqual(1, await _context.Comments.CountAsync());
            Assert.AreEqual(newComment.Text, commentInDb.Text);
        }

        [TestMethod]
        public async Task GetCommentById_ShouldReturnCorrectComment()
        {
            // Arrange
            var comment = new Comment { Id = 1, Text = "First Comment", DateAdded = System.DateTime.Now };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetCommentById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("First Comment", result.Text);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task GetComments_ShouldReturnAllComments()
        {
            // Arrange
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Text = "First Comment", DateAdded = System.DateTime.Now },
                new Comment { Id = 2, Text = "Second Comment", DateAdded = System.DateTime.Now.AddMinutes(-10) }
            };
            await _context.Comments.AddRangeAsync(comments);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetComments();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetCommentsDescended_ShouldReturnCommentsInDescendingOrder()
        {
            // Arrange
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Text = "First Comment", DateAdded = System.DateTime.Now },
                new Comment { Id = 2, Text = "Second Comment", DateAdded = System.DateTime.Now.AddMinutes(-10) }
            };
            await _context.Comments.AddRangeAsync(comments);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetCommentsDescended();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("First Comment", result[0].Text); // Проверяем что "First Comment" добавлен последним
            Assert.AreEqual("Second Comment", result[1].Text); // Проверяем что "Second Comment" добавлен раньше
        }
    }
}
