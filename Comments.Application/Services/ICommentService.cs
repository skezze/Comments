using Comments.Domain.Entities;

namespace Comments.Application.Services
{
    public interface ICommentService
    {
        Comment[] GetCommentsDescended();
        bool AddComment();
    }
}
