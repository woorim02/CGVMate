using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CgvMate.Api;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public bool IsAdmin { get; set; }
    [Required]
    public string PasswordHash { get; set; }
}
