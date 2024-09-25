using Microsoft.AspNetCore.Mvc;
using CgvMate.Api.Entities;
using CgvMate.Api.Services.Interfaces;
using System.Threading.Tasks;
using CgvMate.Api.DTOs;
using Microsoft.Extensions.Hosting;

namespace CgvMate.Api.Controllers
{
    [Route("comment")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<ActionResult<CommentResDTO>> AddComment(CommentAddReqDto dto)
        {
            dto.WriterIP = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            await _commentService.AddCommentAsync(dto);

            return Created();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id, [FromBody] string password)
        {
            await _commentService.DeleteCommentAsync(id, password);
            return Ok();
        }
    }
}
