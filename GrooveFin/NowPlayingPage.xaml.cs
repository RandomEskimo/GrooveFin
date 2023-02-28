using GrooveFin.Services;
using JellyFinAPI.CommunicationClasses;

namespace GrooveFin;

public partial class NowPlayingPage : ContentPage, IDisposable
{
	private bool IsProgrammaticChange;

	public NowPlayingPage()
	{
		InitializeComponent();
		PlaybackManager.PlayStateChanged += PlaybackManager_PlayStateChanged;
		PlaybackManager.SongPositionChanged += PlaybackManager_SongPositionChanged;
		PlaybackManager.SongFinished += PlaybackManager_SongFinished;
		PlaybackManager.SongStarted += PlaybackManager_SongStarted;
	}

	private void PlaybackManager_SongStarted(object? sender, EventArgs e)
	{
		LoadSongDetails();
	}

	private void PlaybackManager_SongFinished(object? sender, EventArgs e)
	{
		LoadSongDetails();
	}

	private void PlaybackManager_SongPositionChanged(object? sender, SongPositionChangedEventArgs e)
	{
		if (PlaybackManager.CurrentSongDuration is TimeSpan duration)
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				IsProgrammaticChange = true;
				sldProgress.Maximum = duration.TotalSeconds;
				sldProgress.Value = e.NewPosition.TotalSeconds;
				lblSongLength.Text = FormatDuration(duration);
				lblCurrentPosition.Text = FormatDuration(e.NewPosition);
				IsProgrammaticChange = false;
			});
		}
	}

	private void PlaybackManager_PlayStateChanged(object? sender, EventArgs e)
	{
		LoadSongDetails();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		LoadSongDetails();
	}

	private async void LoadSongDetails()
	{
		if (PlaybackManager.CurrentSong is Song currentSong && currentSong.Id != null && App.Jellyfin != null)
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				lblArtist.Text = currentSong.AlbumArtist;
				lblSongName.Text = currentSong.Name;
				if (PlaybackManager.SongImagePath is string path)
					imgAlbumCover.Source = ImageSource.FromFile(path);
			});
		}
		else
		{
			await Navigation.PopModalAsync();
		}
	}

	public static string FormatDuration(TimeSpan Duration) => $"{Duration.Minutes}:{(Duration.Seconds < 10 ? "0" : "")}{Duration.Seconds}";

	private void btnPause_Clicked(object sender, EventArgs e)
	{
		if(PlaybackManager.IsPlaying)
		{
			btnPause.Text = "Play";
		}
		else
		{
			btnPause.Text = "Pause";
		}
		PlaybackManager.TogglePlaying();
	}

	private void btnPrev_Clicked(object sender, EventArgs e)
	{
		PlaybackManager.PlayPrev();
	}

	private void btnNext_Clicked(object sender, EventArgs e)
	{
		PlaybackManager.PlayNext();
	}

	private void sldProgress_ValueChanged(object sender, ValueChangedEventArgs e)
	{
		if (!IsProgrammaticChange)
		{
			PlaybackManager.SeekTo(TimeSpan.FromSeconds(sldProgress.Value));
		}
	}

	public void Dispose()
	{
		PlaybackManager.PlayStateChanged -= PlaybackManager_PlayStateChanged;
		PlaybackManager.SongPositionChanged -= PlaybackManager_SongPositionChanged;
		PlaybackManager.SongFinished -= PlaybackManager_SongFinished;
		PlaybackManager.SongStarted -= PlaybackManager_SongStarted;
	}
}