using GrooveFin.Services;
using GrooveFin.Views;
using JellyFinAPI.CommunicationClasses;
using System;

namespace GrooveFin;

public partial class SongsView : AbstractContentPage, IDisposable
{
    private string CollectionId;
    private Album? AlbumDetails;
    private bool IsAlbum => AlbumDetails != null;
    private List<Song>? Songs;

	public SongsView(string CollectionId, Album? AlbumDetails)
	{
		InitializeComponent();
        this.CollectionId = CollectionId;
        this.AlbumDetails = AlbumDetails;

        if(IsAlbum)
        {
            btnPlayAll.Text = "Play Album";
        }
	}

    public override async void OnAppearing()
    {
        base.OnAppearing();

        if (AlbumDetails != null)
        {
            lblCollectionName.Text = AlbumDetails.Name;
            lblArtist.Text = AlbumDetails.AlbumArtist;
        }

        if(Songs == null)
        {
            var songsTask = CacheAccess.GetSongs(CollectionId);
            var imageTask = CacheAccess.GetImagePath(CollectionId);
            if(await songsTask is SongsResponse songs && songs.Items != null)
            {
                Songs = songs.Items;
                ClearSongRows();
                songs.Items.ForEach(song => {
                    var songRow = new SongRow(song);
					songRow.PlayClicked += SongRow_PlayClicked;
                    stkSongs.Children.Add(songRow);
                });
            }
            if(await imageTask is string path)
            {
                imgAlbumCover.Source = ImageSource.FromFile(path);
                if(DeviceInfo.Idiom != DeviceIdiom.Phone)
                {
                    imgAlbumCover.MaximumWidthRequest = 500;
                }
            }
        }
    }

	private void SongRow_PlayClicked(object? sender, EventArgs e)
	{
        if (sender is SongRow songRow && Songs != null)
        {
            int index = stkSongs.Children.IndexOf(songRow);
            PlaybackManager.PlaySongs(Songs, index);
            Navigation.PushModalAsync(new NowPlayingPage());
        }
	}

	public void Dispose()
	{
        ClearSongRows();
	}

    private void ClearSongRows()
    {
		foreach (var row in stkSongs.Children)
		{
			if (row is SongRow songRow)
				songRow.PlayClicked -= SongRow_PlayClicked;
		}
        stkSongs.Children.Clear();
	}

    private void btnPlayAll_Clicked(object sender, EventArgs e)
    {
        if (Songs != null)
        {
            PlaybackManager.PlaySongs(Songs, 0);
			Navigation.PushModalAsync(new NowPlayingPage());
		}
	}
}