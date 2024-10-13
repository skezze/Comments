using Comments.Domain.DTOs;
using Comments.Domain.Entities;

namespace Comments.Application.Services
{
    public interface ICommentService
    {
        Task<List<Comment>> GetCommentsDescended();
        Task<Comment> AddComment(CommentDto commentDto);
    }
}
