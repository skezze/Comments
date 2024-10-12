using Comments.Application.Features;
using Comments.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly CommentContext _context;
    private readonly IMemoryCache _cache;
    private readonly CommentQueue _commentQueue;

    public CommentsController(CommentContext context, IMemoryCache memoryCache, CommentQueue commentQueue)
    {
        _context = context;
        _cache = memoryCache;
        _commentQueue = commentQueue;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!_cache.TryGetValue("commentsCache", out List<Comment> comments))
        {
            comments = await _context.Comments.OrderByDescending(c => c.DateAdded).ToListAsync();
            _cache.Set("commentsCache", comments, TimeSpan.FromMinutes(5));
        }
        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Comment comment)
    {
        if (ModelState.IsValid)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Добавляем комментарий в очередь
            _commentQueue.Enqueue(comment);

            // Отправляем сообщение через WebSocket
            var message = JsonConvert.SerializeObject(comment);
            await WebSocketHandler.SendMessageToAll(message);

            return Ok(comment);
        }
        return BadRequest(ModelState);
    }
}
