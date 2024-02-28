namespace CgvMate.Data;

public class GiveawayEventModel
{
    public string Title { get => GiveawayItemList?[0].GiveawayItemName; }
    public string EventIndex { get; init; }
    public string GiveawayIndex { get => GiveawayItemList?[0].GiveawayItemCode; }
    public string Contents { get; init; }

    /// <summary>
    /// Please do not use it. This is a property for json serialization.
    /// </summary>
    public GiveawayInfo[] GiveawayItemList { private get; set; }

    public class GiveawayInfo
    {
        public string GiveawayItemCode { get; set; }
        public string GiveawayItemName { get; set; }
    }
}
