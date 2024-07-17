using CgvMate.Api.DTOs;
using CgvMate.Api.Entities;

namespace CgvMate.Api.Services.Interfaces;

public interface ICommentService
{
    Task AddCommentAsync(CommentAddReqDto dto);
    Task DeleteCommentAsync(int id, string password);
}