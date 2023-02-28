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
		IAudioActivity? Instance;

		private MediaPlayer? mediaPlayer => Instance != null && Instance.Binder != null &&
			Instance.Binder.GetMediaPlayerService() != null ?
			Instance.Binder.GetMediaPlayerService().mediaPlayer : null;

		public bool IsPlaying => mediaPlayer?.IsPlaying ?? false;

		public TimeSpan? CurrentPosition => mediaPlayer?.CurrentPosition is int currentPos ? TimeSpan.FromMilliseconds(currentPos) : null;
		public event EventHandler<bool>? IsPlayingChanged;

		public Task InitializeAsync(string audioURI)
		{
			if (Instance == null)
			{
				var activity = CurrentActivity;
				Instance = activity as IAudioActivity;
			}
			else
			{
				Instance.Binder.GetMediaPlayerService().isCurrentEpisode = false;
				Instance.Binder.GetMediaPlayerService().UpdatePlaybackStateStopped();
			}

			this.Instance.Binder.GetMediaPlayerService().PlayingChanged += (object sender, bool e) =>
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

			Instance.Binder.GetMediaPlayerService().AudioUrl = audioURI;

			return Task.CompletedTask;
		}

		public Task PauseAsync()
		{
			if (IsPlaying)
			{
				return Instance?.Binder?.GetMediaPlayerService()?.Pause() ?? Task.CompletedTask;
			} 

			return Task.CompletedTask;
		}

		public async Task PlayAsync(TimeSpan? Position)
		{
			if (Instance?.Binder?.GetMediaPlayerService() is MediaPlayerService mediaPlayer)
			{
				await mediaPlayer.Play();
				if(Position.HasValue)
					await mediaPlayer.Seek((int)Position.Value.TotalMilliseconds);
			}
		}

		public Task PlayAsync() => PlayAsync(null);

		public Task SetMuted(bool Value)
		{
			Instance?.Binder?.GetMediaPlayerService()?.SetMuted(Value);

			return Task.CompletedTask;
		}

		public Task SetVolume(int Value)
		{
			Instance?.Binder?.GetMediaPlayerService()?.SetVolume(Value);

			return Task.CompletedTask;
		}

		public Task SetCurrentTime(TimeSpan Position)
		{
			return Instance?.Binder?.GetMediaPlayerService()?.Seek((int)Position.TotalMilliseconds) ?? Task.CompletedTask;
		}

		public ValueTask DisposeAsync()
		{
			Instance?.Binder?.Dispose();
			return ValueTask.CompletedTask;
		}
	}
}
