using CgvMate.Api.Data.Interfaces;
using CgvMate.Api.Entities;
using CgvMate.Api.Services.Interfaces;

namespace CgvMate.Api.Services;

public class PostService : IPostService
{
    private readonly IPostRepo _postRepo;

    public PostService(IPostRepo postRepo)
    {
        _postRepo = postRepo;
    }

    public Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        return _postRepo.GetAllPostsAsync();
    }

    public Task<Post?> GetPostByIdAsync(int id)
    {
        return _postRepo.GetPostByIdAsync(id);
    }

    public Task<IEnumerable<Post>> GetPostsByBoardIdAsync(int boardId, int pageNo, int pageSize)
    {
        return _postRepo.GetPostsByBoardIdAsync(boardId, pageNo, pageSize);
    }

    public Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId, int pageNo, int pageSize)
    {
        return _postRepo.GetPostsByUserIdAsync(userId, pageNo, pageSize);
    }

    public Task AddPostAsync(Post post)
    {
        return _postRepo.AddPostAsync(post);
    }

    public Task UpdatePostAsync(Post post)
    {
        return _postRepo.UpdatePostAsync(post);
    }

    public Task DeletePostAsync(int id)
    {
        return _postRepo.DeletePostAsync(id);
    }

    public Task<IEnumerable<PostSummary>> GetPostSummarysByBoardIdAsync(int boardId, int pageNo, int pageSize)
    {
        return _postRepo.GetPostSummarysByBoardIdAsync(boardId, pageNo, pageSize);
    }

    public Task<IEnumerable<PostSummary>> GetPostSummarysByUserIdAsync(int userId, int pageNo, int pageSize)
    {
        return _postRepo.GetPostSummarysByUserIdAsync(userId, pageNo, pageSize);
    }
}
