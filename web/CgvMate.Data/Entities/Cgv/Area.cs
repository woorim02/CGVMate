namespace CgvMate.Data.Entities.Cgv;

public class Area
{
    public string AreaName { get; set; }

    public string AreaCode { get; set; }

    public bool IsGiveawayAreaCode { get; set; } = false;

    public string TheaterCount { get; set; }
}