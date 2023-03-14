using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrooveFin.Services
{
	public interface INativeAudioService
	{
		Task InitializeAsync(string AudioURI, string? SongName = null, string? Artist = null);

		Task PlayAsync(TimeSpan? Position);
		Task PlayAsync();

		Task PauseAsync();

		Task SetMuted(bool Value);

		Task SetVolume(int Value);

		Task SetCurrentTime(TimeSpan Position);

		ValueTask DisposeAsync();

		bool IsPlaying { get; }

		TimeSpan? CurrentPosition { get; }

		event EventHandler<bool>? IsPlayingChanged;
		event EventHandler? SongFinished;
		event EventHandler? NextRequested;
		event EventHandler? PreviousRequested;
	}
}
