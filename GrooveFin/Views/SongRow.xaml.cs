using JellyFinAPI.CommunicationClasses;

namespace GrooveFin.Views;

public partial class SongRow : ContentView
{
	public event EventHandler? PlayClicked;
	private Song Song { get; set; }

	public SongRow(Song Song)
	{
		InitializeComponent();
		this.Song = Song;

		lblSongName.Text = Song.Name;
		if(Song.UserData?.PlayCount is int playCount)
			lblPlayCount.Text = playCount.ToString();
		if(Song.RunTimeTicks != 0)
		{
			var duration = TimeSpan.FromTicks(Song.RunTimeTicks);
			lblDuration.Text = NowPlayingPage.FormatDuration(duration);
		}	
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		PlayClicked?.Invoke(this, EventArgs.Empty);
	}
}