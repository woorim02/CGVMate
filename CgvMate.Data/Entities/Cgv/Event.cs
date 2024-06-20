namespace CgvMate.Data.Entities.Cgv;

public class Event
{
    public string EventId { get; set; }
    public string EventName { get; set; }
    public string ImageSource { get; set; }
    public string Period { get; set; }

    private DateOnly? startDate;
    private DateOnly? endDate;

    public DateOnly StartDate
    {
        get
        {
            if (startDate == null)
            {
                if (Period == "상시진행")
                    return DateOnly.MinValue;
                var startDateText = Period.Split('~')[0].Split('(')[0];
                startDate = DateOnly.ParseExact(startDateText.Trim(), "yy.MM.dd");
            }
            return startDate.Value;
        }
    }
    public DateOnly EndDate
    {
        get
        {
            if (endDate == null)
            {
                if (Period == "상시진행")
                    return DateOnly.MaxValue;
                var endDateText = Period.Split('~')[1].Split('(')[0];
                endDate = DateOnly.ParseExact(endDateText.Trim(), "yy.MM.dd");
            }
            return endDate.Value;
        }
    }
}
