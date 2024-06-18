namespace CgvMate.Client;

public static class Constants
{
    #region Routes
    public const string root = "/";

    public const string event_ = "/cgv/event";
    public const string event_cupon_speed = "/cgv/event/cupon/speed";
    public const string event_cupon_surprise = "/cgv/event/cupon/surprise";
    public const string event_giveaway = "/cgv/event/giveaway";
    public const string event_giveaway_detail = "/cgv/event/giveaway/detail";
    public const string event_giveaway_autosignup = "/cgv/event/giveaway/autosignup";

    public const string lotte_event = "/lotte/event";
    public const string lotte_event_giveaway = "/lotte/event/giveaway";
    public const string lotte_event_giveaway_detail = "/lotte/event/giveaway/detail";
    #endregion

    #if DEBUG
    public const string API_HOST = "http://192.168.0.51:8080";
    #else
    public const string API_HOST = "https://api.cgvmate.com";
    #endif
}