using GrooveFin.Views;
using JellyFinAPI.CommunicationClasses;
using System.Collections.ObjectModel;

namespace GrooveFin;

public partial class AlbumsPage : AbstractContentPage
{
    private List<Album>? Albums;

	public AlbumsPage()
	{
		InitializeComponent();
	}

    public override async void OnAppearing()
    {
        base.OnAppearing();

        if(Albums == null && await CacheAccess.GetAllAlbums() is AlbumsResponse albums && albums.Items != null)
        {
            Albums = albums.Items;
            PopulateAlbums();
        }
    }

	private void lstMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
        if(e.CurrentSelection.FirstOrDefault() is Album album && album.Id != null)
        {
            MainPage.Instance.PushView(new SongsView(album.Id, album));
            lstMain.SelectedItems.Clear();
        }
    }

    private void PopulateAlbums()
    {
        lstMain.ItemsSource = Albums;// !.GetRange(0, 5);
        //stkMain.Children.Clear();
        /*if (Albums != null)
        {
            foreach (var album in Albums)
            {
                var control = new AlbumRowItem() { Album = album };
                var tap = new TapGestureRecognizer();
                tap.Tapped += (o, e) =>
                {
                    MainPage.Instance.PushView(new SongsView(album.Id!, album));
                };
                control.GestureRecognizers.Add(tap);
                //stkMain.Children.Add(control);
            }
        }*/
    }
}