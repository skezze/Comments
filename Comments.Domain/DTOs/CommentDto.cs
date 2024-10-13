using Comments.Domain.Entities;

namespace Comments.Domain.DTOs
{
    public class CommentDto
    {
        public string Text { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public UserDto UserDto { get; set; }
    }
}
