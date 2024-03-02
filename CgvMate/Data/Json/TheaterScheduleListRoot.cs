namespace CgvMate.Data.Json;

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
public class TheaterScheduleListRoot
{
    public string ResultCode { get; set; }
    public string ResultMessage { get; set; }
    public ResultSchedule ResultSchedule { get; set; }
}

public class ResultSchedule
{
    public string BmResultCd { get; set; }
    public string BmResultMsg { get; set; }
    public ResultServerConditions ResultServerConditions { get; set; }
    public string ListPlayYmd { get; set; }
    public string TimeNotice { get; set; }
    public List<Schedule> ScheduleList { get; set; }
}

public class ResultServerConditions
{
    public string RealtimeSeatYn { get; set; }
    public string QueryMode { get; set; }
    public string MovieMaxCount { get; set; }
    public string TheaterMaxCount { get; set; }
    public string TodayDate { get; set; }
}

public class Schedule
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
    public object SelectYn { get; set; }
    public object SuggestType { get; set; }
    public object SuggestGuideText { get; set; }
    public string TheaterIconCd { get; set; }
    public string TheaterTagTitle { get; set; }
}

