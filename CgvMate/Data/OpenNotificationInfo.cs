namespace CgvMate.Data;

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
public class OpenNotificationInfo
{
    private string theaterCode;
    public int Id { get; set; }
    private string movieIndex;
    public string MovieIndex { get => Movie?.Index ?? movieIndex; set => movieIndex = value; }
    public Movie? Movie { get; set; }
    public string ScreenType { get; set; }
    public string TheaterCode { get => Theater?.TheaterCode ?? theaterCode; set => theaterCode = value; }
    public TheaterGiveawayInfo Theater { get; set; }
    public DateTime TargetDate { get; set; }
    public bool IsOpen { get; set; } = false;
    private bool canReservation = false;
    public bool CanReservation { get => IsOpen && canReservation; set => canReservation = value; }
}
