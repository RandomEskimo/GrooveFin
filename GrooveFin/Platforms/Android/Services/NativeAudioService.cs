using Android.Media;
using GrooveFin.Services;
using AndroidNet = Android.Net;
using AndroidApp = Android.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Platform;
using GrooveFin.Helpers;

namespace GrooveFin.Platforms.Android.Services
{
	public class NativeAudioService : INativeAudioService
	{
		private AudioPlayer? Player;

		public string? CurrentSongTitle;
		public string? CurrentSongArtist;
		public string? CurrentSongUri;

		public static NativeAudioService? Instance;

		public event EventHandler<bool>? IsPlayingChanged;
		public event EventHandler? NextRequested;
		public event EventHandler? PreviousRequested;
		public event EventHandler? SongFinished;

		public bool IsPlaying => Player?.IsPlaying ?? false;

		public TimeSpan? CurrentPosition => Player?.CurrentPosition;

		public NativeAudioService()
		{
			Instance = this;
		}

		public async Task InitializeAsync(string audioURI, string? SongName = null, string? Artist = null)
		{
			InitPlayer();
			if (await (Player?.Play(audioURI) ?? Task.FromResult(false)))
			{
				CurrentSongUri = audioURI;
				CurrentSongTitle = SongName;
				CurrentSongArtist = Artist;
			}
			else
			{
				CurrentSongUri = null;
				CurrentSongTitle = null;
				CurrentSongArtist = null;
			}
		}

		private void InitPlayer()
		{
			if (Player == null)
			{
				Player = new AudioPlayer();
				Player.FinishedPlaying += (o, e) => SongFinished?.Invoke(this, EventArgs.Empty);
				Player.PlayStateChanged += (o, e) => IsPlayingChanged?.Invoke(this, Player.IsPlaying);
			}
		}

		public Task PauseAsync()
		{
			Player?.Pause();
			return Task.CompletedTask;
		}

		public async Task PlayAsync(TimeSpan? Position)
		{
			if(Player != null && Position.HasValue)
				await Player.SeekAsync(Position.Value);
			Player?.Play();
		}

		public Task PlayAsync() => PlayAsync(null);

		public Task SetMuted(bool Value)
		{
			Player?.SetVolume(Value ? 1.0f : 0.0f);
			return Task.CompletedTask;
		}

		public Task SetVolume(int Value)
		{
			Player?.SetVolume(Value / 100.0f);
			return Task.CompletedTask;
		}

		public async Task SetCurrentTime(TimeSpan Position)
		{
			await (Player?.SeekAsync(Position) ?? Task.CompletedTask);
		}

		public ValueTask DisposeAsync()
		{
			return ValueTask.CompletedTask;
		}

		public void RequestNextSong() => NextRequested?.Invoke(this, EventArgs.Empty);
		public void RequestPreviousSong() => PreviousRequested?.Invoke(this, EventArgs.Empty);
	}
}
