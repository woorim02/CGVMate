namespace CgvMate.Data;

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
public class TheaterGiveawayInfo
{
    public string TheaterCode { get; init; }
    public string TheaterName { get; init; }
    public string CountTypeCode { get; init; }
    public string EncCount { get; init; }
    public string GiveawayName { get; init; }
    public string GiveawayRemainCount { get; set; }
    public string ReceiveTypeCode { get; init; }
}
