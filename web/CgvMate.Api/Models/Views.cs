using System.ComponentModel.DataAnnotations;

namespace CgvMate.Api;

public class Views
{
    [Key]
    public long EventIndex { get; set; }
    public int Count { get; set; }
}
