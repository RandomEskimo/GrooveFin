using GrooveFin.Services;
using JellyFinAPI.CommunicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GrooveFin.Services
{
    static class PlaybackManager
    {
        public static event EventHandler<SongPositionChangedEventArgs>? SongPositionChanged;
        public static event EventHandler? PlayStateChanged;
        public static event EventHandler? SongFinished;
        public static event EventHandler? SongStarted;

        private static INativeAudioService? _audioService;
        private static INativeAudioService AudioService => _audioService ?? throw new Exception("AudioManager is not initialised");

        public static bool IsPlaying => AudioService.IsPlaying;

        public static List<Song>? CurrentSongs { get; private set; }
        private static int CurrentSongIndex;
        public static Song? CurrentSong => CurrentSongs == null || CurrentSongIndex < 0 || CurrentSongIndex >= CurrentSongs.Count ? null : CurrentSongs[CurrentSongIndex];
        public static TimeSpan? CurrentSongDuration => CurrentSong is Song song ? TimeSpan.FromTicks(song.RunTimeTicks) : null;
        public static string? SongImagePath { get; private set; }

        private static bool Looping;
        private static TimeSpan? PrevPosition;

        public static void Init(INativeAudioService AudioService)
        {
            _audioService = AudioService;
			AudioService.IsPlayingChanged += AudioService_IsPlayingChanged;
            Looping = true;
            Task.Run(CheckPosition);
			AudioService.PreviousRequested += (o,e) => PlayNext();
			AudioService.NextRequested += (o,e) => PlayPrev();
        }

		private static void AudioService_IsPlayingChanged(object? sender, bool e)
		{
			if(CurrentSongDuration is TimeSpan duration && AudioService.CurrentPosition >= duration.Subtract(TimeSpan.FromSeconds(1)))
            {
                SongFinished?.Invoke(null, EventArgs.Empty);
                PlayNext();
            }
            else
            {
                PlayStateChanged?.Invoke(null, EventArgs.Empty);
            }
		}

		private static async void CheckPosition()
        {
            while(Looping)
            {
                TimeSpan? currentPos = AudioService.CurrentPosition;
				if (currentPos.HasValue)
                {
                    if(PrevPosition == null || currentPos.Value != PrevPosition.Value)
                    {
                        SongPositionChanged?.Invoke(null, new SongPositionChangedEventArgs() { NewPosition = currentPos.Value });
                    }
                    PrevPosition = currentPos.Value;
                }
                await Task.Delay(250);
            }
        }

        public static void PlaySongs(List<Song> SongsToPlay, int StartIndex)
        {
			if (StartIndex < 0 || StartIndex > SongsToPlay.Count) throw new IndexOutOfRangeException();
			CurrentSongs = SongsToPlay;
			CurrentSongIndex = StartIndex;
            _=CurrentSongChanged();
		}

        private static async Task CurrentSongChanged()
        {
            if (App.Jellyfin != null && CurrentSong != null && CurrentSong.Id != null)
            {
				string songUrl = App.Jellyfin.Songs.MakeSongUrl(CurrentSong.Id);
				await AudioService.InitializeAsync(songUrl);
                await AudioService.PlayAsync();
                if(CurrentSong.AlbumId != null)
                    SongImagePath = await CacheAccess.GetImagePath(CurrentSong.AlbumId);
                SongStarted?.Invoke(null, EventArgs.Empty);
            }
		}

        public static void TogglePlaying()
        {
            if (IsPlaying)
                AudioService.PauseAsync();
            else
                AudioService.PlayAsync();
        }

        public static void PlayPrev()
        {
			if (AudioService.CurrentPosition.HasValue && AudioService.CurrentPosition.Value.TotalSeconds > 3)
			{
				AudioService.SetCurrentTime(TimeSpan.Zero);
			}
			else if (CurrentSongIndex > 0)
			{
				CurrentSongIndex--;
				_=CurrentSongChanged();
			}
		}

        public static void PlayNext() 
        {
			if (CurrentSongs != null && CurrentSongIndex < CurrentSongs.Count - 1)
			{
				CurrentSongIndex++;
				_=CurrentSongChanged();
			}
		}

        public static void SeekTo(TimeSpan Position)
        {
            AudioService.SetCurrentTime(Position);
        }
    }

    public class SongPositionChangedEventArgs : EventArgs
    {
        public TimeSpan NewPosition { get; set; }
    }
}
