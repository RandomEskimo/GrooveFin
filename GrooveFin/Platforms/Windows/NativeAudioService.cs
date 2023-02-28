using GrooveFin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace GrooveFin.Platforms.Windows;

public class NativeAudioService : INativeAudioService
{
	string? _uri;
	MediaPlayer? mediaPlayer;

	public bool IsPlaying => mediaPlayer != null
		&& mediaPlayer.CurrentState == MediaPlayerState.Playing;

	public TimeSpan? CurrentPosition => mediaPlayer?.Position ?? null;
	public event EventHandler<bool>? IsPlayingChanged;

	public async Task InitializeAsync(string AudioURI)
	{
		_uri = AudioURI;

		if (mediaPlayer == null)
		{
			mediaPlayer = new MediaPlayer
			{
				Source = MediaSource.CreateFromUri(new Uri(_uri)),
				AudioCategory = MediaPlayerAudioCategory.Media
			};
		}
		if (mediaPlayer != null)
		{
			await PauseAsync();
			mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(_uri));
		}

	}

	public Task PauseAsync()
	{
		mediaPlayer?.Pause();
		return Task.CompletedTask;
	}

	public Task PlayAsync(TimeSpan? Position)
	{
		if (mediaPlayer != null)
		{
			if(Position.HasValue)
				mediaPlayer.Position = Position.Value;
			mediaPlayer.Play();
		}

		return Task.CompletedTask;
	}

	public Task PlayAsync() => PlayAsync(null);

	public Task SetCurrentTime(TimeSpan Position)
	{
		if (mediaPlayer != null)
		{
			mediaPlayer.Position = Position;
		}

		return Task.CompletedTask;
	}

	public Task SetMuted(bool Value)
	{
		if (mediaPlayer != null)
		{
			mediaPlayer.IsMuted = Value;
		}

		return Task.CompletedTask;
	}

	public Task SetVolume(int Value)
	{
		if (mediaPlayer != null)
		{
			mediaPlayer.Volume = Value != 0
				? Value / 100d
				: 0;
		}

		return Task.CompletedTask;
	}

	public ValueTask DisposeAsync()
	{
		mediaPlayer?.Dispose();
		return ValueTask.CompletedTask;
	}
}
