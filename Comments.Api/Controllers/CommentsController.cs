using Comments.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]/[action]")]
public class CommentsController : ControllerBase
{
    private readonly CommentContext _context;

    public CommentsController(CommentContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetComments() =>
        Ok(await _context.Comments.Include(c => c.User).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetComments), new { id = comment.Id }, comment);
    }
}