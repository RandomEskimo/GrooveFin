using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace GrooveFin;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkitMediaElement()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

#if WINDOWS
        builder.Services.TryAddSingleton<Services.INativeAudioService, GrooveFin.Platforms.Windows.NativeAudioService>();
#elif ANDROID
		builder.Services.TryAddSingleton<Services.INativeAudioService, GrooveFin.Platforms.Android.Services.NativeAudioService>();
#endif

		return builder.Build();
	}
}
