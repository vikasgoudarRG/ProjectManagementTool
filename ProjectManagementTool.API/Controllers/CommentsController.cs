using Microsoft.AspNetCore.Mvc;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.DTOs.Comment;

[ApiController]
[Route("api/comments")]
public class CommentController : ControllerBase
{
    private readonly ITaskItemCommentService _commentService;

    public CommentController(ITaskItemCommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateCommentDTO dto)
    {
        await _commentService.AddAsync(dto);
        return NoContent();
    }

    [HttpPut("{commentId}/author/{authorId}")]
    public async Task<IActionResult> Update(Guid commentId, Guid authorId, [FromBody] string newContent)
    {
        await _commentService.UpdateAsync(commentId, authorId, newContent);
        return NoContent();
    }

    [HttpDelete("{commentId}/author/{authorId}")]
    public async Task<IActionResult> Delete(Guid commentId, Guid authorId)
    {
        await _commentService.DeleteAsync(commentId, authorId);
        return NoContent();
    }

    [HttpGet("task/{taskId}")]
    public async Task<IActionResult> GetAll(Guid taskId)
    {
        var comments = await _commentService.GetAllForTaskAsync(taskId);
        return Ok(comments);
    }
}