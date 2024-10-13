using Comments.Application.Features;
using Comments.Application.Services;
using Comments.Domain.DTOs;
using Comments.Domain.Entities;
using Comments.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IMemoryCache cache;
    private readonly CommentQueue commentQueue;
    private readonly ICommentService commentService;

    public CommentsController(
        IMemoryCache memoryCache, 
        CommentQueue commentQueue,
        ICommentService commentService)
    {

        this.cache = memoryCache;
        this.commentQueue = commentQueue;
        this.commentService = commentService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!cache.TryGetValue("commentsCache", out List<Comment> comments))
        { 
            comments = await commentService.GetCommentsDescended();
            cache.Set("commentsCache", comments, TimeSpan.FromMinutes(5));
        }
        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CommentViewModel commentVM)
    {
        if (ModelState.IsValid)
        {
            UserDto userDto = new UserDto() 
            {
                Email = commentVM.User.Email,
                UserName = commentVM.User.UserName
            };
            CommentDto commentDto = new CommentDto() 
            {
                Text = commentVM.Text,
                UserDto = userDto
            };

            var comment = await commentService.AddComment(commentDto);

            // Добавляем комментарий в очередь
            commentQueue.Enqueue(comment);

            // Отправляем сообщение через WebSocket
            var message = JsonConvert.SerializeObject(comment, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore// possible bug of cycle reference
            });
            await WebSocketHandler.SendMessageToAll(message);

            return Ok(comment);
        }
        return BadRequest(ModelState);
    }
}
