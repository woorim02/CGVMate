using System.ComponentModel.DataAnnotations;

namespace CgvMate.Api.Entites;

public class Comment
{
    public int Id { get; set; }

    // Post
    public int PostId { get; set; }
    public Post Post { get; set; }

    // Comment
    public int ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }
    public bool HasParent => ParentComment is not null;

    public ICollection<Comment> Children { get; set; }

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
