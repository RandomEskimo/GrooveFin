using Android.Media;
using Android.OS;
using Android.Runtime;
using Java.Interop;
using AndroidNet = Android.Net;
using AndroidApp = Android.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrooveFin.Helpers;

namespace GrooveFin.Platforms.Android
{
	internal class AudioPlayer : Java.Lang.Object, AudioManager.IOnAudioFocusChangeListener,
   MediaPlayer.IOnBufferingUpdateListener,
   MediaPlayer.IOnCompletionListener,
   MediaPlayer.IOnErrorListener,
   MediaPlayer.IOnPreparedListener
	{
		public event EventHandler? FinishedPlaying;
		public event EventHandler? PlayStateChanged;

		private MediaPlayer Player;

		public bool IsPlaying => Player.IsPlaying;

		public TimeSpan CurrentPosition => TimeSpan.FromMilliseconds(Player.CurrentPosition);

		public AudioPlayer() 
		{
			Player = new MediaPlayer();
			Player.SetAudioAttributes(
				new AudioAttributes.Builder()
					.SetContentType(AudioContentType.Music)!
					.SetUsage(AudioUsageKind.Media)!
					.Build()
			);
			Player.Error += (o, e) => App.ReportMessage($"Error in media player: {e.What}");

			Player.SetWakeMode(AndroidApp.Application.Context, WakeLockFlags.Partial);

			Player.SetOnBufferingUpdateListener(this);
			Player.SetOnCompletionListener(this);
			Player.SetOnErrorListener(this);
			Player.SetOnPreparedListener(this);
		}

		public void OnAudioFocusChange([GeneratedEnum] AudioFocus focusChange)
		{
			switch (focusChange)
			{
				case AudioFocus.Gain:
					if (!Player.IsPlaying)
					{
						Play();
					}

					Player.SetVolume(1.0f, 1.0f);
					break;
				case AudioFocus.Loss:
					//We have lost focus stop!
					Stop();
					break;
				case AudioFocus.LossTransient:
					//We have lost focus for a short time, but likely to resume so pause
					Pause();
					break;
				case AudioFocus.LossTransientCanDuck:
					//We have lost focus but should still play at a muted 10% volume
					if (Player.IsPlaying)
						Player.SetVolume(.1f, .1f);
					break;
			}
		}

		public async Task<bool> Play(string Uri)
		{
			if (AndroidNet.Uri.Parse(Uri) is AndroidNet.Uri songUri)
			{
				Player.Reset();
				await (Player.SetDataSourceAsync(AndroidApp.Application.Context, songUri) ?? Task.CompletedTask);
				await Task.Run(() => Player.Prepare());
				return true;
			}
			Player.Stop();
			return false;
		}

		public void Play()
		{
			if (!Player.IsPlaying)
			{
				Player.Start();
				PlayStateChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void Stop()
		{
			Player.Stop();
			PlayStateChanged?.Invoke(this, EventArgs.Empty);
		}

		public void Pause()
		{
			Player.Pause();
		}

		public async Task SeekAsync(TimeSpan Position)
		{
			Task seekTask = new TaskFromEvent(h => Player.SeekComplete += h, h => Player.SeekComplete -= h).Task;
			Player.SeekTo((int)Position.TotalMilliseconds);
			await seekTask;
		}

		public void SetVolume(float Volume)
		{
			Player.SetVolume(Volume, Volume);
		}

		public void OnBufferingUpdate(MediaPlayer? mp, int percent)
		{
			
		}

		public void OnCompletion(MediaPlayer? mp)
		{
			FinishedPlaying?.Invoke(this, EventArgs.Empty);
			PlayStateChanged?.Invoke(this, EventArgs.Empty);
		}

		public bool OnError(MediaPlayer? mp, [GeneratedEnum] MediaError what, int extra)
		{
			return true;
		}

		public void OnPrepared(MediaPlayer? mp)
		{
			mp?.Start();
			PlayStateChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
