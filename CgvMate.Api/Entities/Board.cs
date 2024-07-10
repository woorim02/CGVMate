using System.ComponentModel.DataAnnotations;

namespace CgvMate.Api.Entities;

public class Board
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    public string Description { get; set; }

    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
