namespace LotteMate;
public class CinemaDivision
{
    public int DivisionCode { get; set; }
    public string DetailDivisionCode { get; set; }
    public string GroupNameKR { get; set; }
    public string GroupNameUS { get; set; }
    public int SortSequence { get; set; }
    public int CinemaCount { get; set; }
}

public class CinemaDivisionGood
{
    public int DivisionCode { get; set; }
    public string DetailDivisionCode { get; set; }
    public string CinemaID { get; set; }
    public string CinemaNameKR { get; set; }
    public string CinemaNameUS { get; set; }
    public int SortSequence { get; set; }
    public int Cnt { get; set; }
    public string DetailDivisionNameKR { get; set; }
    public string DetailDivisionNameUS { get; set; }
}

public class LotteGiveawayInfo
{
    public List<CinemaDivision> CinemaDivisions { get; set; }
    public List<CinemaDivisionGood> CinemaDivisionGoods { get; set; }
    public string IsOK { get; set; }
    public string ResultMessage { get; set; }
    public object ResultCode { get; set; }
    public object EventResultYn { get; set; }
}