using Comments.Domain.Entities;

namespace Comments.Application.Repositories
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetComments();
        Task<List<Comment>> GetCommentsDescended();
        Task<Comment> GetCommentById(int id);
        Task<Comment> AddComment(Comment comment);
    }
}
