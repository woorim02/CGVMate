using System.ComponentModel.DataAnnotations;

namespace CgvMate.Api.Entities;

public class PostSummary
{
    public int Id { get; set; }

    // Board
    public int BoardId { get; set; }
    public Board Board { get; set; }

    // Writer
    public string? WriterIP { get; set; }
    public string? WriterName { get; set; }
    public int UserId { get; set; } = 0;
    public User? User { get; set; }

    //Content
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.Now;
    public int ViewCount { get; set; } = 0;
    public int CommentCount { get; set; } = 0;
    public int Upvote { get; set; } = 0;
    public int Downvote { get; set; } = 0;
}
