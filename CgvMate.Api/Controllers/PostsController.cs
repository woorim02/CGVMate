using CgvMate.Api.DTOs;
using CgvMate.Api.Entities;
using CgvMate.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CgvMate.Api.Controllers;

[ApiController]
[Route("post")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostResDto>> GetPostById(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(PostResDto.FromEntity(post));
    }

    [HttpGet("list/{boardId}")]
    public async Task<IActionResult> GetPostSummarysByBoardIdAsync(int boardId, [FromQuery] int pageNo = 1, [FromQuery] int pageSize = 10)
    {
        var posts = await _postService.GetPostSummarysByBoardIdAsync(boardId, pageNo, pageSize);
        return Ok(posts.Select(p => PostSummaryResDTO.FromEntity(p)));
    }

    [HttpPost]
    public async Task<ActionResult<PostAddReqDto>> AddPost(PostAddReqDto postDto)
    {
        var post = postDto.ToEntity();
        post.WriterIP = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        await _postService.AddPostAsync(post);
        return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, postDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id,[FromBody] string password)
    {
        var post = await _postService.GetPostByIdAsync(id);

        if (post == null || PasswordHasher.VerifyPassword(password, post.WriterPasswordHash))
            return BadRequest();

        await _postService.DeletePostAsync(id);
        return NoContent();
    }
}
