using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net.Sip;
using Android.OS;
using GrooveFin.Platforms.Android;
using GrooveFin.Platforms.Android.Services;

namespace GrooveFin;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity, IAudioActivity
{
	MediaPlayerServiceConnection? mediaPlayerServiceConnection;

	public MediaPlayerServiceBinder? Binder { get; set; }

	public event StatusChangedEventHandler? StatusChanged;
	public event CoverReloadedEventHandler? CoverReloaded;
	public event PlayingEventHandler? Playing;
	public event BufferingEventHandler? Buffering;

	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);
		Platform.Init(this, savedInstanceState);
		if (ApplicationContext != null)
		{
			NotificationHelper.CreateNotificationChannel(ApplicationContext);
			if (mediaPlayerServiceConnection == null)
				InitializeMedia();
		}

		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

		if (Intent != null)
		{
			NativeAudioService.AudioActivityInstance?.Binder?.GetMediaPlayerService()?.HandleIntent(Intent);
		}
	}

	protected override void OnNewIntent(Intent? Intent)
	{
		base.OnNewIntent(Intent);

		if (Intent != null)
		{
			NativeAudioService.AudioActivityInstance?.Binder?.GetMediaPlayerService()?.HandleIntent(Intent);
		}
	}

	private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
	{
		App.ReportException(e.Exception);
	}

	private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		string message = $"Caught {e.ExceptionObject.GetType().Name} in maui app: {e.ExceptionObject}\n\n";
		App.ReportMessage(message);
	}

	private void InitializeMedia()
	{
		if (ApplicationContext != null)
		{
			mediaPlayerServiceConnection = new MediaPlayerServiceConnection(this);
			var mediaPlayerServiceIntent = new Intent(ApplicationContext, typeof(MediaPlayerService));
			BindService(mediaPlayerServiceIntent, mediaPlayerServiceConnection, Bind.AutoCreate);
		}
	}
}
