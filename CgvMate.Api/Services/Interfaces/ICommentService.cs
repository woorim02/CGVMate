using CgvMate.Api.Entities;

namespace CgvMate.Api.Services.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<Comment>> GetAllCommentsAsync();
    Task<Comment?> GetCommentByIdAsync(int id);
    Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
    Task AddCommentAsync(Comment comment);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(int id);
}