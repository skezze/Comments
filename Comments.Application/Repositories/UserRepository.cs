using Comments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Comments.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CommentContext commentContext;

        public UserRepository(CommentContext commentContext)
        {
            this.commentContext = commentContext;
        }
        public async Task<User> CreateAsync(User user)
        {
            commentContext.Users.Add(user);
            await commentContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetAsync(User user)
        {
            return await commentContext.Users
                .FirstOrDefaultAsync(x=>x.UserName==user.UserName
                &&x.Email==user.Email);
        }

        public async Task<User> GetById(int id)
        {
            return await commentContext.Users.FirstOrDefaultAsync(x=>x.Id == id);
         
        }

        public async Task<List<User>> GetUsers()
        {
            return await commentContext.Users.ToListAsync();
        }
    }
}
