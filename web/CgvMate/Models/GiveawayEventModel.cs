namespace CgvMate.Models;

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
public class GiveawayEventModel
{
    public string Title { get => GiveawayItemList[0].GiveawayItemName; }
    public string EventIndex { get; init; }
    public string GiveawayIndex { get => GiveawayItemList[0].GiveawayItemCode; }
    public string Contents { get; init; }

    /// <summary>
    /// Please do not use it. This is a property for json serialization.
    /// </summary>
    public GiveawayItem[] GiveawayItemList { get; set; }

	/// <summary>
	/// Please do not use it. This is a property for json serialization.
	/// </summary>
	public class GiveawayItem
    {
        public string GiveawayItemCode { get; set; }
        public string GiveawayItemName { get; set; }
    }

	/// <summary>
	/// 빈 생성자를 직접 호출하지 마세요. json 직렬화 또는 ef core에서 사용하는 클래스입니다.
	/// </summary>
	public GiveawayEventModel()
	{

	}
}