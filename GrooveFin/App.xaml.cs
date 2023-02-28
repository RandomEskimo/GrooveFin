using GrooveFin.DataClasses;
using GrooveFin.Services;
using JellyFinAPI;
using JellyFinAPI.Interfaces;

namespace GrooveFin;

public partial class App : Application
{
	public static UserProfile? LoggedInUser { get; set; }
	public static AppAndDeviceDetails DeviceDetails =>
		new AppAndDeviceDetails("Groove Fin", DeviceInfo.Platform.ToString(), DeviceID.ToString(), AppInfo.VersionString);
	public static IJellyfinAccess? Jellyfin { get; set; }

	public static Guid DeviceID
	{
		get
		{
			if (CacheAccess.GetSettingsObject<Guid?>("DeviceID") is Guid deviceID)
				return deviceID;
			Guid newDeviceID = Guid.NewGuid();
			CacheAccess.SetSettingsObject("DeviceID", newDeviceID);
			return newDeviceID;
		}
	}

	public App()
	{
		InitializeComponent();

		MainPage = new MainPage();//new AppShell();

		AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
	}

	private void CurrentDomain_FirstChanceException(object? sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
	{
		ReportException(e.Exception);
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();

		if (Handler?.MauiContext?.Services.GetService<INativeAudioService>() is INativeAudioService audioService)
			PlaybackManager.Init(audioService);
	}

	public static void ReportException(Exception Exception, string Where = "maui app")
	{
		string message = $"Caught an {Exception.GetType().Name} in {Where}: {Exception}";
		System.Diagnostics.Debug.WriteLine(message);
		ReportMessage(message);
	}

	public static void ReportMessage(string Message)
	{
		File.AppendAllText($"{FileSystem.Current.AppDataDirectory}/debug.log", Message + "\n\n");
	}
}
