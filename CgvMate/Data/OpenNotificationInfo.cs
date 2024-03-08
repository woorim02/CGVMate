namespace CgvMate.Data;

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
public class OpenNotificationInfo
{
    public int Id { get; set; }

    private string? movieIndex = null;
    public string? MovieIndex { get => Movie?.Index ?? movieIndex; set => movieIndex = value; }

    public Movie? Movie { get; set; }
    public string ScreenType { get; set; }

    private string theaterCode;
    public string TheaterCode { get => TheaterGiveawayInfo?.TheaterCode ?? theaterCode; set => theaterCode = value; }

    public TheaterGiveawayInfo TheaterGiveawayInfo { get; set; }
    public DateOnly TargetDate { get; set; }
    public bool IsOpen { get; set; } = false;
    private bool canReservation = false;
    public bool CanReservation { get => IsOpen && canReservation; set => canReservation = value; }

    /// <summary>
    /// 빈 생성자를 직접 호출하지 마세요. json 직렬화 또는 ef core에서 사용하는 클래스입니다.
    /// </summary>
    public OpenNotificationInfo()
    {

    }
}
