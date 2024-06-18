namespace CgvMate.Data.Entities.Cgv;

public class TheaterGiveawayInfo
{
    public string GiveawayIndex { get; set; }
    public string TheaterCode { get; init; }
    public string TheaterName { get; init; }
    public string CountTypeCode { get; init; }
    public string EncCount { get; init; }
    public string GiveawayName { get; init; }
    public string GiveawayRemainCount { get; set; }
    public string ReceiveTypeCode { get; init; }
}