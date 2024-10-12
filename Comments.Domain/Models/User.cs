namespace Comments.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string HomePage { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
