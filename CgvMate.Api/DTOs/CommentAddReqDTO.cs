using CgvMate.Api.Entities;

namespace CgvMate.Api.DTOs;

public class CommentAddReqDto
{
    public int PostId { get; set; }
    public int ParentCommentId { get; set; } = -1;
    public string? WriterIP { get; set; }
    public string? WriterName { get; set; }
    public string? WriterPassword { get; set; }
    public int UserId { get; set; } = -1;
    public string Content { get; set; }

    public Comment ToEntity()
    {
        if (WriterIP == null || WriterName == null || WriterPassword == null)
        {
            throw new Exception("WriterIP == null || WriterName == null || WriterPassword == null");
        }
        if (string.IsNullOrEmpty(Content))
            throw new Exception("Contents.Count는 0 이상이어야 합니다.");
        return new Comment
        {
            PostId = PostId,
            ParentCommentId = ParentCommentId,
            WriterIP = WriterIP,
            WriterName = WriterName,
            WriterPasswordHash = PasswordHasher.HashPassword(WriterPassword),
            UserId = UserId,
            Content = Content,
            DateCreated = DateTime.Now
        };
    }
}
