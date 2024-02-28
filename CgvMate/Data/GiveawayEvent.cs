namespace CgvMate.Data;

public class GiveawayEvent
{
    public string Title { get; init; }
    public string EventIndex { get; init; }
    public string Period { get; init; }
    public string DDay { get; init; }

    private DateTime startDate = DateTime.MinValue;
    public DateTime StartDate
    {
        get {
            if (startDate == DateTime.MinValue)
                DateTime.TryParse(Period.Split('~')[0], out startDate);
            return startDate;
        }
    }

    private DateTime endDate = DateTime.MinValue;
    public DateTime EndDate
    {
        get {
            if (endDate == DateTime.MinValue)
                DateTime.TryParse(Period.Split('~')[0],out endDate);
            return endDate;
        }
    }
}
