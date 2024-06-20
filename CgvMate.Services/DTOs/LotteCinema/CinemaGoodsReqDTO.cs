namespace CgvMate.Services.DTOs.LotteCinema;

internal class CinemaGoodsReqDTO : ReqDTOBase
{
    public string EventID { get; set; }
    public string GiftID { get; set; }

    public CinemaGoodsReqDTO(string eventID, string giftID)
    {
        MethodName = "GetCinemaGoods";
        EventID = eventID;
        GiftID = giftID;
    }
}
