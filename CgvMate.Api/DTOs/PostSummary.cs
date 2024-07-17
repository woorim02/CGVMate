using System.ComponentModel.DataAnnotations;
using CgvMate.Api.Entities;

namespace CgvMate.Api.DTOs;

public class PostSummary
{
    public int Id { get; set; }
    public int No { get; set; }

    // Board
    public string BoardId { get; set; }
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
