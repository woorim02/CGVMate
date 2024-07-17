using CgvMate.Api.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace CgvMate.Api.DTOs;

public class PostAddReqDto
{
    [Required]
    public string BoardId { get; set; }

    // Writer
    public string WriterIP { get; set; }
    public string WriterName { get; set; }
    public string WriterPassword { get; set; }

    // Content
    [Required]
    [StringLength(100)]
    public string Title { get; set; }
    [Required]
    public List<Content> Contents { get; set; }

    public Post ToEntity()
    {
        return new Post
        {
            BoardId = this.BoardId,
            WriterIP = this.WriterIP,
            WriterName = this.WriterName.Trim().Replace(" ", ""),
            WriterPasswordHash = PasswordHasher.HashPassword(this.WriterPassword), 
            Title = this.Title,
            Content = JsonConvert.SerializeObject(this.Contents),
            DateCreated = DateTime.Now,
            ViewCount = 0,
            Upvote = 0,
            Downvote = 0
        };
    }

    public class Content
    {
        public ContentType ContentType { get; set; }

        public string Body { get; set; }
    }

    public enum ContentType
    {
        Text = 1,
        Image = 2
    }
}
