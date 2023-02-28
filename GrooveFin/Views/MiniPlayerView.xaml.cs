using GrooveFin.Services;
using JellyFinAPI.CommunicationClasses;

namespace GrooveFin.Views;

public partial class MiniPlayerView : ContentView
{
	private string? PrevSongImagePath;

	public MiniPlayerView()
	{
		InitializeComponent();
		PlaybackManager.PlayStateChanged += PlaybackManager_PlayStateChanged;
		PlaybackManager.SongPositionChanged += PlaybackManager_SongPositionChanged;
		PlaybackManager.SongFinished += PlaybackManager_SongFinished;
		PlaybackManager.SongStarted += PlaybackManager_SongStarted;
		bvProgress.WidthRequest = 0;
		grdMain.IsVisible = false;
	}

	private void UpdateSongInfo()
	{
		if(PlaybackManager.CurrentSong is Song currentSong)
		{
			grdMain.IsVisible = true;
			lblArtist.Text = currentSong.AlbumArtist;
			lblSongTitle.Text = currentSong.Name;
			if(PlaybackManager.SongImagePath is string path && path != PrevSongImagePath)
			{
				PrevSongImagePath = path;
				imgAlbumArt.Source = ImageSource.FromFile(path);
			}
			if (PlaybackManager.IsPlaying)
			{
				btnPause.Text = "Pause";
			}
			else
			{
				btnPause.Text = "Play";
			}
		}
		else
		{
			grdMain.IsVisible = false;
		}
	}

	private void PlaybackManager_SongStarted(object? sender, EventArgs e)
	{
		UpdateSongInfo();
	}

	private void PlaybackManager_SongFinished(object? sender, EventArgs e)
	{
		UpdateSongInfo();
	}

	private void PlaybackManager_SongPositionChanged(object? sender, SongPositionChangedEventArgs e)
	{
		if(PlaybackManager.CurrentSongDuration is TimeSpan duration && Width > 0)
		{
			double perPixel = Width / duration.TotalSeconds;
			MainThread.BeginInvokeOnMainThread(() => {
				bvProgress.WidthRequest = perPixel * e.NewPosition.TotalSeconds;
				lblProgress.Text = NowPlayingPage.FormatDuration(e.NewPosition);
				lblDuration.Text = NowPlayingPage.FormatDuration(duration);
			});
		}
	}

	private void PlaybackManager_PlayStateChanged(object? sender, EventArgs e)
	{
		UpdateSongInfo();
	}

	private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
		Navigation.PushModalAsync(new NowPlayingPage());
	}

	private void btnPause_Clicked(object sender, EventArgs e)
	{
		if (PlaybackManager.IsPlaying)
		{
			btnPause.Text = "Play";
		}
		else
		{
			btnPause.Text = "Pause";
		}
		PlaybackManager.TogglePlaying();
	}
}