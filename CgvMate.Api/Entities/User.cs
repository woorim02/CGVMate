using System.ComponentModel.DataAnnotations;

namespace CgvMate.Api.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string PasswordSalt { get; set; }

    public string[] Roles { get; set; }

    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public int PostCount { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public int CommentCount { get; set; }
}
