namespace LotteMate;

public class LotteEvent
{
    public string EventID { get; set; }
    public string EventName { get; set; }
    public string EventClassificationCode { get; set; }
    public string EventTypeCode { get; set; }
    public string EventTypeName { get; set; }
    public string ProgressStartDate { get; set; }
    public string ProgressEndDate { get; set; }
    public string ImageUrl { get; set; }
    public string ImageAlt { get; set; }
    public int ImageDivisionCode { get; set; }
    public string CinemaID { get; set; }
    public string CinemaName { get; set; }
    public string CinemaAreaCode { get; set; }
    public string CinemaAreaName { get; set; }
    public int DevTemplateYN { get; set; }
    public int CloseNearYN { get; set; }
    public int RemainsDayCount { get; set; }
    public int EventWinnerYN { get; set; }
    public int EventSeq { get; set; }
    public string EventCntnt { get; set; }
    public string EventNtc { get; set; }
    public int Views { get; set; }
}

public class LotteEventRoot
{
    public List<LotteEvent> Items { get; set; }
    public int TotalCount { get; set; }
    public string IsOK { get; set; }
    public string ResultMessage { get; set; }
    public object ResultCode { get; set; }
    public object EventResultYn { get; set; }
}