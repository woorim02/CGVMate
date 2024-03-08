using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;

namespace CgvMate
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			CreateForegroundNotificationChannel();
			CreateOpenNotificationChannel();
		}

		private void CreateForegroundNotificationChannel()
		{
			var notificationManager = NotificationManagerCompat.From(this);
			var channel = new NotificationChannel(Constants.FOREGROUND_CHANNEL_ID, Constants.FOREGROUND_CHANNEL_NAME, NotificationImportance.Low);
			notificationManager.CreateNotificationChannel(channel);
		}

		private void CreateOpenNotificationChannel()
		{
			var notificationManager = NotificationManagerCompat.From(this);
			var channel = new NotificationChannel(Constants.OPEN_CHANNEL_ID, Constants.OPEN_CHANNEL_NAME, NotificationImportance.High);
			notificationManager.CreateNotificationChannel(channel);
			notificationManager.CreateNotificationChannelGroup(new NotificationChannelGroup(Constants.OPEN_GROUP_KEY, Constants.OPEN_GROUP_NAME));
		}
	}
}
