using CgvMate.Api.Entities;

namespace CgvMate.Api.DTOs;

public class PostResDto
{
    public int Id { get; set; }
    public int BoardId { get; set; }
    public string? WriterIP { get; set; }
    public string? WriterName { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DateCreated { get; set; }
    public int ViewCount { get; set; }
    public int Upvote { get; set; }
    public int Downvote { get; set; }
    public IEnumerable<CommentResDto> Comments { get; set; }

    public static PostResDto FromEntity(Post post)
    {
        return new PostResDto
        {
            Id = post.Id,
            BoardId = post.BoardId,
            WriterIP = post.WriterIP,
            WriterName = post.WriterName,
            UserId = post.UserId,
            UserName = post.User?.UserName,
            Title = post.Title,
            Content = post.Content,
            DateCreated = post.DateCreated,
            ViewCount = post.ViewCount,
            Upvote = post.Upvote,
            Downvote = post.Downvote,
            Comments = post.Comments.Select(c => CommentResDto.FromEntity(c)).ToList()
        };
    }
}