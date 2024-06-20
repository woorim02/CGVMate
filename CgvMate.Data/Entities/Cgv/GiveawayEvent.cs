namespace CgvMate.Data.Entities.Cgv;

public class GiveawayEvent
{
    public string Title { get; init; }
    public string EventIndex { get; init; }
    public string Period { get; init; }
    public string DDay { get; init; }
    public int Views { get; set; }

    private DateOnly startDate = DateOnly.MinValue;
    public DateOnly StartDate
    {
        get
        {
            if (startDate == DateOnly.MinValue)
                DateOnly.TryParse(Period.Split('~')[0], out startDate);
            return startDate;
        }
    }

    private DateOnly endDate = DateOnly.MaxValue;
    public DateOnly EndDate
    {
        get
        {
            if (endDate == DateOnly.MaxValue)
                DateOnly.TryParse(Period.Split('~')[0], out endDate);
            return endDate;
        }
    }
}