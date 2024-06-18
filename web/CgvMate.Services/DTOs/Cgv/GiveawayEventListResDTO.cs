namespace CgvMate.Services.DTOs.Cgv;

internal class GiveawayEventListResDTO
{
    public D d { get; set; }
    public class D
    {
        public string __type { get; set; }
        public string TotalCount { get; set; }
        public string List { get; set; }
        public string ResultCode { get; set; }
    }
}
