using System.ComponentModel.DataAnnotations;

namespace CgvMate.Api.Entities;

public class Comment
{
    public int Id { get; set; }

    // Post
    public int PostId { get; set; }
    public Post Post { get; set; }

    // Comment
    /// <summary>
    /// -1일 경우 부모 댓글이 존재하지 않음
    /// </summary>
    public int ParentCommentId { get; set; } = -1;
    public Comment? ParentComment { get; set; }
    public bool HasParent => ParentCommentId == -1;

    public ICollection<Comment> Children { get; set; } = new List<Comment>();

    // Writer
    public string? WriterIP { get; set; }
    public string? WriterName { get; set; }
    public int UserId { get; set; } = 0;
    public User? User { get; set; }

    // Content
    [Required]
    public string Content { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
