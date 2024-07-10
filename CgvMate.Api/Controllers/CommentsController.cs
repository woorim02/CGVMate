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
        public async Task<ActionResult<CommentResDTO>> AddComment(CommentAddReqDto commentDto)
        {
            var comment = commentDto.ToEntity();
            await _commentService.AddCommentAsync(comment);
            comment.WriterIP = HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR");

            var commentResDto = CommentResDTO.FromEntity(comment);

            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, commentResDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommentResDTO>> GetCommentById(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var commentResDto = CommentResDTO.FromEntity(comment);
            return Ok(commentResDto);
        }
    }
}
