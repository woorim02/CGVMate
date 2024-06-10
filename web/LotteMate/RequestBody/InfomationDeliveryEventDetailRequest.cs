namespace LotteMate;

public class InfomationDeliveryEventDetailRequest :RequestBodyBase
{
    public string EventID;

    public InfomationDeliveryEventDetailRequest(string eventID)
    {
        MethodName = "GetInfomationDeliveryEventDetail";
        EventID = eventID;
    }
}
