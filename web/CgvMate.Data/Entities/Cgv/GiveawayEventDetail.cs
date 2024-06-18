using Newtonsoft.Json;

namespace CgvMate.Data.Entities.Cgv;

public class GiveawayEventDetail
{
    public string GivewayIndex { get; init; }
    [JsonProperty("AreaList")]
    public List<Area> Areas { get; init; }
    [JsonProperty("TheaterList")]
    public List<TheaterGiveawayInfo> TheaterGiveawayInfos { get; init; }
}
