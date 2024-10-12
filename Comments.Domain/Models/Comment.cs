namespace Comments.Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateAdded { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
