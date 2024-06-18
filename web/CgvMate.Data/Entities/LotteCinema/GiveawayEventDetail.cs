namespace CgvMate.Data.Entities.LotteCinema;

public class GiveawayEventDetail
{
    public string EventID { get; set; }
    public string GiftID { get; set; }
    public List<CinemaDivision> CinemaDivisions { get; set; }
    public List<CinemaDivisionGood> CinemaDivisionGoods { get; set; }
}