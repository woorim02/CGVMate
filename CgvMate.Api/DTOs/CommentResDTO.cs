using CgvMate.Api.Entities;

namespace CgvMate.Api.DTOs
{
    public class CommentResDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int ParentCommentId { get; set; }
        public string? WriterIP { get; set; }
        public string? WriterName { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }

        public static CommentResDTO FromEntity(Comment comment)
        {
            return new CommentResDTO
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
}