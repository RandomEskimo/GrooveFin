using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net.Sip;
using Android.OS;
using GrooveFin.Platforms.Android;
using GrooveFin.Platforms.Android.Services;

namespace GrooveFin;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{

	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);
		Platform.Init(this, savedInstanceState);

		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

		if (Intent != null)
		{
			
		}
	}

	protected override void OnNewIntent(Intent? Intent)
	{
		base.OnNewIntent(Intent);

		if (Intent != null)
		{
			
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
}
