namespace CgvMate.Data;

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
public class EventInfo
{
    public string EventId { get; set; }
    public string EventName { get; set; }
    public string ImageSource { get; set; }
    public string Period { get; set; }

    /// <summary>
    /// 빈 생성자를 직접 호출하지 마세요. json 직렬화 또는 ef core에서 사용하는 클래스입니다.
    /// </summary>
    public EventInfo() { }

    public EventInfo(string eventId, string eventName, string imageSource, string period)
    {
        EventId = eventId;
        EventName = eventName;
        ImageSource = imageSource;
        Period = period;
    }
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
