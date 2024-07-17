using CgvMate.Api.DTOs;

namespace CgvMate.Api.Services.Interfaces;

public interface IPostService
{
    Task<PostResDto> GetPostAsync(string boardId, int postNo);
    Task<Tuple<string, int>> AddPostAsync(PostAddReqDto post);
    Task<Tuple<string, int>> UpdatePostAsync(PostAddReqDto post, int postId);
    Task DeletePostAsync(int id, string password);
    Task<IEnumerable<PostSummary>> GetPostSummarysAsync(string boardId, int pageNo = 1, int pageSize = 10);
}