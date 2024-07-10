using CgvMate.Api.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace CgvMate.Api.DTOs;

public class PostAddReqDto
{
    [Required]
    public int BoardId { get; set; }

    // Writer
    public string? WriterIP { get; set; }
    public string? WriterName { get; set; }
    public string? WriterPassword { get; set; }

    // Content
    [Required]
    [StringLength(100)]
    public string Title { get; set; }
    [Required]
    public List<Content> Contents { get; set; }

    public Post ToEntity()
    {
        if (WriterIP == null || WriterName == null || WriterPassword == null)
        {
            throw new Exception("WriterIP == null || WriterName == null || WriterPassword == null");
        }
        if (Contents.Count == 0)
            throw new Exception("Contents.Count는 0 이상이어야 합니다.");
        return new Post
        {
            BoardId = this.BoardId,
            WriterIP = this.WriterIP,
            WriterName = this.WriterName,
            WriterPasswordHash = this.WriterPassword is null ? null : PasswordHasher.HashPassword(this.WriterPassword), // Assume hashing happens elsewhere
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
