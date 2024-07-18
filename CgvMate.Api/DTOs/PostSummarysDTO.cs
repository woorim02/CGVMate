namespace CgvMate.Api.DTOs;

public class PostSummarysDTO
{
    public int TotalCount {get; set;}
    public IEnumerable<PostSummary> PostSummaries {get; set;} = new List<PostSummary>();
}
