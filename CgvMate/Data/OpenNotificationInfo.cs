namespace CgvMate.Data;

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
