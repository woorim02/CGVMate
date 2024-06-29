namespace CgvMate.Data.Entities.LotteCinema;

public class LotteGiveawayEventModel
{
    public string EventID { get; set; }
    public string FrGiftID { get; set; }
    public string FrGiftNm { get; set; }
    /// <summary>
    /// 기본값 = <see langword="false"/>
    /// </summary>
    public bool HasNext { get; set; } = false;
}
