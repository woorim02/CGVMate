namespace CgvMate.Services.DTOs.Cgv;

internal class GiveawayEventModelResDTO
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
}
