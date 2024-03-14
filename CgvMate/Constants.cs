namespace CgvMate;

public static class Constants
{
#if ANDROID
    public const string FOREGROUND_CHANNEL_ID = "openNotification.foreground";
    public const string FOREGROUND_CHANNEL_NAME = "오픈알림 활성화 알림";
    public const string OPEN_CHANNEL_ID = "openNotification.channel";
    public const string OPEN_CHANNEL_NAME = "오픈알림";
    public const string OPEN_GROUP_KEY = "openNotification.group";
    public const string OPEN_GROUP_NAME = "OpenNotification NotificationGroup";
#endif

    #region DataBase
    public static string APP_DB_FILENAME => "app_database_v0.1.sqlite";
    public static string APP_DB_PATH => Path.Combine(FileSystem.AppDataDirectory, APP_DB_FILENAME);
    #endregion

    #region Routes
    public const string root = "/";
    public const string event_ = "/event";
    public const string event_cupon_speed = "/event/cupon/speed";
    public const string event_giveaway = "/event/giveaway";
    public const string event_giveaway_detail = "/event/giveaway/detail";
    public const string reservation_notification = "/reservation/notification";
    public const string reservation_notification_add = "/reservation/notification/add";
    #endregion
}
