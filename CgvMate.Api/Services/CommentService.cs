using CgvMate.Api.Data.Interfaces;
using CgvMate.Api.Entities;
using CgvMate.Api.Services.Interfaces;

namespace CgvMate.Api.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepo _commentRepo;

    public CommentService(ICommentRepo commentRepo)
    {
        _commentRepo = commentRepo;
    }

    public Task<IEnumerable<Comment>> GetAllCommentsAsync()
    {
        return _commentRepo.GetAllCommentsAsync();
    }

    public Task<Comment> GetCommentByIdAsync(int id)
    {
        return _commentRepo.GetCommentByIdAsync(id);
    }

    public Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        return _commentRepo.GetCommentsByPostIdAsync(postId);
    }

    public Task AddCommentAsync(Comment comment)
    {
        return _commentRepo.AddCommentAsync(comment);
    }

    public Task UpdateCommentAsync(Comment comment)
    {
        return _commentRepo.UpdateCommentAsync(comment);
    }

    public Task DeleteCommentAsync(int id)
    {
        return _commentRepo.DeleteCommentAsync(id);
    }
}
