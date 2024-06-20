using CgvMate.Data.Enums;

namespace CgvMate.Services.DTOs.LotteCinema;

internal class EventsReqDTO : ReqDTOBase
{
    public string EventClassificationCode;
    public string SearchText = "";
    public string CinemaID = "";
    public string MemberNo = "0";
    public int PageNo;
    public int PageSize;

    public EventsReqDTO(LotteEventType type, int pageNo = 1, int pageSize = 50, string searchText = "")
    {
        MethodName = "GetEventLists";
        EventClassificationCode = $"{(int)type}";
        PageNo = pageNo;
        PageSize = pageSize;
        SearchText = searchText;
    }
}