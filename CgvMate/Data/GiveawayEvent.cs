namespace CgvMate.Data;

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
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
