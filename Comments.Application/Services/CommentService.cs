using Comments.Application.Repositories;
using Comments.Domain.DTOs;
using Comments.Domain.Entities;

namespace Comments.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly IUserService userService;

        public CommentService(ICommentRepository commentRepository, IUserService userService)
        {
            this.commentRepository = commentRepository;
            this.userService = userService;
        }
        public async Task<Comment> AddComment(CommentDto commentDto)
        {
            UserDto userDto = new UserDto() 
            {
                Email = commentDto.UserDto.Email,
                UserName = commentDto.UserDto.UserName
            };

            User user = await userService.GetUser(userDto);
            if (user == null) 
            {
                user = await userService.AddUser(userDto);
            }

            Comment comment = new Comment()
            {
                Text = commentDto.Text,
                User = user
            };

            return await commentRepository.AddComment(comment);
        }

        public async Task<List<Comment>> GetCommentsDescended()
        {
            return await commentRepository.GetCommentsDescended();
        }
    }
}
