namespace CgvMate.Services.DTOs.LotteCinema;

internal class InfomationDeliveryEventDetailReqDTO : ReqDTOBase
{
    public string EventID;

    public InfomationDeliveryEventDetailReqDTO(string eventID)
    {
        MethodName = "GetInfomationDeliveryEventDetail";
        EventID = eventID;
    }
}
