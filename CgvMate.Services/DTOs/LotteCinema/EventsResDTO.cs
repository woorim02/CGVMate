using CgvMate.Data.Entities.LotteCinema;

namespace CgvMate.Services.DTOs.LotteCinema;

internal class EventsResDTO
{
    public List<Event> Items { get; set; }
    public int TotalCount { get; set; }
    public string IsOK { get; set; }
    public string ResultMessage { get; set; }
    public object ResultCode { get; set; }
    public object EventResultYn { get; set; }
}
