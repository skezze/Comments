using Comments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Comments.Application.Repositories
{
    public class CommentsRepository : ICommentRepository
    {
        private readonly CommentContext commentContext;

        public CommentsRepository(CommentContext commentContext)
        {
            this.commentContext = commentContext;
        }
        public async Task<Comment> AddComment(Comment comment)
        {
            commentContext.Comments.Add(comment);
            await commentContext.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment> GetCommentById(int id)
        {
            return await commentContext.Comments
                .Include(x=>x.User)
                .FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<List<Comment>> GetComments()
        {
            return await commentContext.Comments.ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsDescended()
        {
            return await commentContext.Comments.OrderByDescending(x=>x.DateAdded).ToListAsync();
        }
    }
}
