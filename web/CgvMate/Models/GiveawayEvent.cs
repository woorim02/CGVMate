namespace CgvMate.Models;

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
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
        get {
            if (startDate == DateOnly.MinValue)
                DateOnly.TryParse(Period.Split('~')[0], out startDate);
            return startDate;
        }
    }

    private DateOnly endDate = DateOnly.MinValue;
    public DateOnly EndDate
    {
        get {
            if (endDate == DateOnly.MinValue)
                DateOnly.TryParse(Period.Split('~')[0],out endDate);
            return endDate;
        }
    }

	/// <summary>
	/// 빈 생성자를 직접 호출하지 마세요. json 직렬화 또는 ef core에서 사용하는 클래스입니다.
	/// </summary>
	public GiveawayEvent()
    {
    }
    public GiveawayEvent(string title, string eventIndex, string period, string dDay)
	{
		Title = title;
		EventIndex = eventIndex;
		Period = period;
		DDay = dDay;
	}
}