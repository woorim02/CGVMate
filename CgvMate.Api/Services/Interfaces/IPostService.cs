using CgvMate.Api.Entities;

namespace CgvMate.Api.Services.Interfaces;

public interface IPostService
{
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task<Post?> GetPostByIdAsync(int id);
    Task<IEnumerable<Post>> GetPostsByBoardIdAsync(int boardId, int pageNo, int pageSize);
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId, int pageNo, int pageSize);
    Task AddPostAsync(Post post);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(int id);
}