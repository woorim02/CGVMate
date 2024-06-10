namespace LotteMate;

public class CinemaGoodsRequestBody : RequestBodyBase
{
    public string EventID { get; set; }
    public string GiftID { get; set; }

    public CinemaGoodsRequestBody(string eventID, string giftID)
    {
        MethodName = "GetCinemaGoods";
        EventID = eventID;
        GiftID = giftID;
    }
}
