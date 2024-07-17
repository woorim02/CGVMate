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

    [HttpGet]
    public async Task<ActionResult<PostResDto>> GetPost([FromQuery] string boardId, [FromQuery] int postNo)
    {
        var post = await _postService.GetPostAsync(boardId, postNo);
        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> AddPost([FromBody] PostAddReqDto dto)
    {
        dto.WriterIP = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        var tuple = await _postService.AddPostAsync(dto);
        return Ok(tuple);
    }

    [HttpPut("{postId}")]
    public async Task<ActionResult> UpdatePost(int postId, [FromBody] PostAddReqDto dto)
    {
        var tuple = await _postService.UpdatePostAsync(dto, postId);
        return Ok(tuple);
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePost(int postId, [FromBody] string password)
    {
        await _postService.DeletePostAsync(postId, password);
        return Ok();
    }

    [HttpGet("{boardId}")]
    public async Task<IActionResult> GetPostSummarys(string boardId, [FromQuery] int pageNo = 1, [FromQuery] int pageSize = 10)
    {
        var posts = await _postService.GetPostSummarysAsync(boardId, pageNo, pageSize);
        return Ok(posts);
    }
}
