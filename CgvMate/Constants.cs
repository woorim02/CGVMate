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
}
