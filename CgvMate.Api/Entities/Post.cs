﻿using System.ComponentModel.DataAnnotations;

namespace CgvMate.Api.Entities;

public class Post
{
    public int Id { get; set; }

    public int No { get; set; }

    // Board
    public string BoardId { get; set; }
    public Board Board { get; set; }

    // Writer
    public bool IsAnonymous => UserId == -1;
    public string? WriterIP { get; set; }
    public string? WriterName { get; set; }
    public string? WriterPasswordHash { get; set; }
    public string? WriterPasswordSalt { get; set; }
    public int UserId { get; set; } = -1;
    public User? User { get; set; }

    //Content
    [Required]
    [StringLength(100)]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public int ViewCount { get; set; } = 0;
    public int Upvote { get; set; } = 0;
    public int Downvote { get; set; } = 0;

    // Comments
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
