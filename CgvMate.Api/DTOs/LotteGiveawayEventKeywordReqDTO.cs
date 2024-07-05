using System.ComponentModel.DataAnnotations;

namespace CgvMate.Api.DTOs;

public class LotteGiveawayEventKeywordReqDTO
{
    [Required]
    public string Method { get; set; }

    [Required]
    public string Keyword { get; set; }
}
