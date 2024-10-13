using Comments.Application.Repositories;
using Comments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Comments.Tests.Repositories
{
    [TestClass]
    public class UserRepositoryTests
    {
        private DbContextOptions<CommentContext> _options;
        private CommentContext _context;
        private UserRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            // Настройка InMemory базы данных
            _options = new DbContextOptionsBuilder<CommentContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CommentContext(_options);
            _repository = new UserRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task CreateAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "testuser", Email = "test@example.com" };

            // Act
            var result = await _repository.CreateAsync(user);
            var userInDb = await _context.Users.FindAsync(user.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.UserName, result.UserName);
            Assert.AreEqual(1, await _context.Users.CountAsync());
            Assert.AreEqual(user.UserName, userInDb.UserName);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "testuser", Email = "test@example.com" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAsync(user);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.UserName, result.UserName);
            Assert.AreEqual(user.Email, result.Email);
        }

        [TestMethod]
        public async Task GetById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "testuser", Email = "test@example.com" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("testuser", result.UserName);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task GetUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, UserName = "testuser1", Email = "test1@example.com" },
                new User { Id = 2, UserName = "testuser2", Email = "test2@example.com" }
            };
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(u => u.UserName == "testuser1"));
            Assert.IsTrue(result.Any(u => u.UserName == "testuser2"));
        }
    }
}
