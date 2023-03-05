using Android.Media;
using GrooveFin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Platform;

namespace GrooveFin.Platforms.Android.Services
{
	public class NativeAudioService : INativeAudioService
	{
		public static IAudioActivity? AudioActivityInstance;
		public static NativeAudioService? Instance;

		private MediaPlayer? mediaPlayer => AudioActivityInstance != null && AudioActivityInstance.Binder != null &&
			AudioActivityInstance.Binder.GetMediaPlayerService() != null ?
			AudioActivityInstance.Binder.GetMediaPlayerService().mediaPlayer : null;

		public bool IsPlaying => mediaPlayer?.IsPlaying ?? false;

		public TimeSpan? CurrentPosition => mediaPlayer?.CurrentPosition is int currentPos ? TimeSpan.FromMilliseconds(currentPos) : null;
		public event EventHandler<bool>? IsPlayingChanged;
		public event EventHandler? NextRequested;
		public event EventHandler? PreviousRequested;

		public NativeAudioService()
		{
			Instance = this;
		}

		public Task InitializeAsync(string audioURI)
		{
			if (AudioActivityInstance == null)
			{
				var activity = CurrentActivity;
				AudioActivityInstance = activity as IAudioActivity;
			}
			else
			{
				AudioActivityInstance.Binder.GetMediaPlayerService().UpdatePlaybackStateStopped();
			}

			AudioActivityInstance.Binder.GetMediaPlayerService().PlayingChanged += (object sender, bool e) =>
			{
				/*Task.Run(async () => {
					if (e)
					{
						await this.PlayAsync();
					}
					else
					{
						await this.PauseAsync();
					}
				});*/
				IsPlayingChanged?.Invoke(this, e);
			};

			AudioActivityInstance.Binder.GetMediaPlayerService().AudioUrl = audioURI;

			return Task.CompletedTask;
		}

		public Task PauseAsync()
		{
			if (IsPlaying)
			{
				return AudioActivityInstance?.Binder?.GetMediaPlayerService()?.Pause() ?? Task.CompletedTask;
			} 

			return Task.CompletedTask;
		}

		public async Task PlayAsync(TimeSpan? Position)
		{
			if (AudioActivityInstance?.Binder?.GetMediaPlayerService() is MediaPlayerService mediaPlayer)
			{
				await mediaPlayer.Play();
				if(Position.HasValue)
					await mediaPlayer.Seek((int)Position.Value.TotalMilliseconds);
			}
		}

		public Task PlayAsync() => PlayAsync(null);

		public Task SetMuted(bool Value)
		{
			AudioActivityInstance?.Binder?.GetMediaPlayerService()?.SetMuted(Value);

			return Task.CompletedTask;
		}

		public Task SetVolume(int Value)
		{
			AudioActivityInstance?.Binder?.GetMediaPlayerService()?.SetVolume(Value);

			return Task.CompletedTask;
		}

		public Task SetCurrentTime(TimeSpan Position)
		{
			return AudioActivityInstance?.Binder?.GetMediaPlayerService()?.Seek((int)Position.TotalMilliseconds) ?? Task.CompletedTask;
		}

		public ValueTask DisposeAsync()
		{
			AudioActivityInstance?.Binder?.Dispose();
			return ValueTask.CompletedTask;
		}

		public void RequestNextSong() => NextRequested?.Invoke(this, EventArgs.Empty);
		public void RequestPreviousSong() => PreviousRequested?.Invoke(this, EventArgs.Empty);
	}
}
