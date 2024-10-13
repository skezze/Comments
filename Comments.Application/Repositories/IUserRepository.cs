using Comments.Domain.Entities;

namespace Comments.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<List<User>> GetUsers();
        Task<User> GetById(int id);
        Task<User> GetAsync(User user);
    }
}
