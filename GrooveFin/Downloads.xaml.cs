using GrooveFin.Services;
using GrooveFin.Helpers;

namespace GrooveFin;

public partial class Downloads : AbstractContentPage, IDisposable
{
	public Downloads()
	{
		InitializeComponent();

		SongDownloadService.DownloadStarted += SongDownloadService_Update;
		SongDownloadService.DownloadFailed += SongDownloadService_Update;
		SongDownloadService.DownloadCompleted += SongDownloadService_Update;

		Populate();
	}

	private void SongDownloadService_Update(object? sender, EventArgs e)
	{
		Populate();
	}

	private Task Populate()
	{
		return MainThread.InvokeOnMainThreadAsync(() =>
		{
			stkForDownloads.UpdateChildren(
				SongDownloadService.CurrentQueue,
				v => v is DownloadsRow dr ? dr.Download : null,
				(v, d) => { if (v is DownloadsRow dr) dr.Download = d; },
				d => new DownloadsRow(d),
				(d1, d2) => d1 == d2
			);
		});
	}

	public void Dispose()
	{
		SongDownloadService.DownloadStarted -= SongDownloadService_Update;
		SongDownloadService.DownloadFailed -= SongDownloadService_Update;
		SongDownloadService.DownloadCompleted -= SongDownloadService_Update;
	}

	private class DownloadsRow : Grid
	{
		private SongDownload _Download;
		public SongDownload Download { get => _Download; set { _Download = value; Update(); } }

		private Label lblSongName;
		private Label lblArtist;
		private Label lblStatus;

		public DownloadsRow(SongDownload Download)
		{
			_Download = Download;
			Margin = new Thickness(0, 0, 0, 5);
			ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
			ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
			ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
			RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			RowDefinitions.Add(new RowDefinition(GridLength.Auto));

			new BoxView()
			{
				WidthRequest = 40,
				HeightRequest = 40,
				BackgroundColor = Color.FromRgb(255, 0, 0)
			}.Also(Children.Add).Also(it =>
			{
				Grid.SetColumn(it, 0);
				Grid.SetRow(it, 0);
				Grid.SetRowSpan(it, 2);
			});

			lblSongName = new Label().Also(Children.Add).Also(it =>
			{
				Grid.SetColumn(it, 1);
				Grid.SetRow(it, 0);
			});

			lblArtist = new Label().Also(Children.Add).Also(it =>
			{
				Grid.SetColumn(it, 1);
				Grid.SetRow(it, 1);
			});

			lblStatus = new Label() 
			{ 
				VerticalTextAlignment = TextAlignment.Center 
			}.Also(Children.Add).Also(it =>
			{
				Grid.SetColumn(it, 2);
				Grid.SetRow(it, 0);
				Grid.SetRowSpan(it, 2);
			});

			Update();
		}

		private void Update()
		{
			lblSongName.Text = Download.SongName;
			lblArtist.Text = Download.Artist;
			switch(Download.State)
			{
				case SongDownloadState.Downloading:
					lblStatus.Text = "Downloading";
					lblStatus.TextColor = Color.FromRgb(255, 255, 0);
					break;
				case SongDownloadState.Waiting:
					lblStatus.Text = "Waiting";
					lblStatus.TextColor = Color.FromRgb(255, 255, 255);
					break;
				case SongDownloadState.Failed:
					lblStatus.Text = "Failed";
					lblStatus.TextColor = Color.FromRgb(255, 0, 0);
					break;
				case SongDownloadState.Completed:
					lblStatus.Text = "Done";
					lblStatus.TextColor = Color.FromRgb(0, 255, 0);
					break;
			}
		}
	}
}

