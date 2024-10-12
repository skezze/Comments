using Comments.Domain.Models;
using System.Collections.Concurrent;

namespace Comments.Application.Features
{
    public class CommentQueue
    {
        private readonly ConcurrentQueue<Comment> _queue = new ConcurrentQueue<Comment>();

        public void Enqueue(Comment comment)
        {
            _queue.Enqueue(comment);
        }

        public IEnumerable<Comment> DequeueAll()
        {
            while (_queue.TryDequeue(out Comment comment))
            {
                yield return comment;
            }
        }
    }

}
