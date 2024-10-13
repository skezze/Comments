using System.Text.Json.Serialization;

namespace Comments.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
