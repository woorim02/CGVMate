namespace CgvMate.Data.Json;

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
public class MiniMapDataRoot
{
    public string ResultCode { get; set; }
    public string ResultMessage { get; set; }
    public string Notice { get; set; }
    public string NonTheaterNotice { get; set; }
    public string ScreenNotice { get; set; }
    public string UserLevelInfo { get; set; }
    public ResultNotice ResultNotice { get; set; }
    public ResultSeatingChart ResultSeatingChart { get; set; }
    public SeatBanner SeatBanner { get; set; }
}

public class ResultNotice
{
    public List<object> CommonNotice { get; set; }
}

public class ResultSeatingChart
{
    public string BmSeatResultCd { get; set; }
    public string BmSeatResultMsg { get; set; }
    public string BmScheduleResultCd { get; set; }
    public string BmScheduleResultMsg { get; set; }
    public ResultMiniSuggestScheduleInfos ResultMiniSuggestScheduleInfos { get; set; }
    public ResultMinimapNotice ResultMinimapNotice { get; set; }
    public ResultSeatInfos ResultSeatInfos { get; set; }
}

public class ResultMiniSuggestScheduleInfos
{
    public List<MinimapScheduleInfo> MinimapScheduleInfo { get; set; }
    public List<object> SuggestedScheduleInfo { get; set; }
}

public class MinimapScheduleInfo
{
    public string TheaterCd { get; set; }
    public string TheaterNm { get; set; }
    public string FrequentTheaterYn { get; set; }
    public string MovieIdx { get; set; }
    public string MovieCd { get; set; }
    public string MovieGroupCd { get; set; }
    public string MovieNmKor { get; set; }
    public string MovieNmEng { get; set; }
    public string WishMovieYn { get; set; }
    public string MovieRatingCd { get; set; }
    public string MovieRatingNm { get; set; }
    public string ScreenCd { get; set; }
    public string ScreenNm { get; set; }
    public string PlayYmd { get; set; }
    public string PlayStartTm { get; set; }
    public string PlayEndTm { get; set; }
    public string RunningTime { get; set; }
    public string PlayNum { get; set; }
    public string SeatRemainCnt { get; set; }
    public string SeatCapacity { get; set; }
    public string ScreenRatingCd { get; set; }
    public string ScreenRatingNm { get; set; }
    public string PlayTimeCd { get; set; }
    public string PlayTimeNm { get; set; }
    public string AllowSaleYn { get; set; }
    public string PlatformCd { get; set; }
    public string PlatformNm { get; set; }
    public string KidsScreenType { get; set; }
    public string MovieEventCd { get; set; }
    public string MovieEventNm { get; set; }
    public string MovieAttrCd { get; set; }
    public string MovieAttrNm { get; set; }
    public string MoviePkgYn { get; set; }
    public string MovieNoshowYn { get; set; }
    public string SeatRate { get; set; }
    public string PosterImageUrl { get; set; }
    public string SelectYn { get; set; }
    public object SuggestType { get; set; }
    public object SuggestGuideText { get; set; }
    public string TheaterIconCd { get; set; }
    public string TheaterTagTitle { get; set; }
}

public class ResultMinimapNotice
{
    public List<string> NoticeTop { get; set; }
    public List<string> NoticeBottom { get; set; }
}

public class ResultSeatInfos
{
    public string SkyboxYn { get; set; }
    public string PressingNotice { get; set; }
    public ResultScreenInfo ResultScreenInfo { get; set; }
    public List<ResultTicketInfo> ResultTicketInfo { get; set; }
    public List<ResultPriceInfo> ResultPriceInfo { get; set; }
    public List<ResultSeatInfoList> ResultSeatInfoList { get; set; }
    public ResultScreenGateInfo ResultScreenGateInfo { get; set; }
    public ResultDistanceSeatingInfo ResultDistanceSeatingInfo { get; set; }
}

public class ResultScreenInfo
{
    public string TheaterCd { get; set; }
    public string TheaterNm { get; set; }
    public string ScreenCd { get; set; }
    public string ScreenNm { get; set; }
    public string ScreenImage { get; set; }
    public string FloorNo { get; set; }
    public string YCnt { get; set; }
    public string XCnt { get; set; }
}

public class ResultTicketInfo
{
    public string TicketType { get; set; }
    public string TicketTypeNm { get; set; }
    public string TicketTypeMsg { get; set; }
    public string TicketTypeOrder { get; set; }
    public string TicketTypeCss { get; set; }
}

public class ResultPriceInfo
{
    public string RatingCd { get; set; }
    public string TicketType { get; set; }
    public string Price { get; set; }
}

public class ResultSeatInfoList
{
    public string SeatLocNo { get; set; }
    public string FloorNo { get; set; }
    public string LocY { get; set; }
    public string LocYNm { get; set; }
    public string LocX { get; set; }
    public string KindCd { get; set; }
    public string KindNm { get; set; }
    public string RatingCd { get; set; }
    public string RatingNm { get; set; }
    public string SeatNo { get; set; }
    public string SeatOrderCd { get; set; }
    public string SeatState { get; set; }
    public string SeatAreaCd { get; set; }
    public string SeatRelationCode { get; set; }
    public string SeatPreperenceNm { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
}

public class ResultScreenGateInfo
{
    public string GateInfo1 { get; set; }
    public string GateInfo2 { get; set; }
    public string GateInfo3 { get; set; }
}

public class ResultDistanceSeatingInfo
{
    public string ResrPeople { get; set; }
    public string DistanceCls { get; set; }
    public string DistanceMsg { get; set; }
}

public class SeatBanner
{
    public string DisplayYN { get; set; }
    public string ImageUrl { get; set; }
    public string EventUrl { get; set; }
    public string BackgroundColor { get; set; }
}