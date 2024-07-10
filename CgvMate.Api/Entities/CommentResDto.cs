namespace CgvMate.Api.Entities;

public class CommentResDto
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int ParentCommentId { get; set; }
    public string? WriterIP { get; set; }
    public string? WriterName { get; set; }
    public int UserId { get; set; } = -1;
    public string? UserName { get; set; }
    public string Content { get; set; }
    public DateTime DateCreated { get; set; }

    public static CommentResDto FromEntity(Comment comment)
    {
        return new CommentResDto
        {
            Id = comment.Id,
            PostId = comment.PostId,
            ParentCommentId = comment.ParentCommentId,
            WriterIP = comment.WriterIP,
            WriterName = comment.WriterName,
            UserId = comment.UserId,
            UserName = comment.User?.UserName,
            Content = comment.Content,
            DateCreated = comment.DateCreated
        };
    }
}
