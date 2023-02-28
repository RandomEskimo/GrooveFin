using JellyFinAPI.CommunicationClasses;
using JellyFinAPI.Interfaces;

namespace GrooveFin;

public partial class ArtistsPage : ContentPage
{
    private List<Artist>? Artists;

	public ArtistsPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        LoadArtists();
    }

    private async void LoadArtists()
    {
        if(Artists == null && await CacheAccess.GetAllArtists() is ArtistsResponse artists  && artists.Items != null) 
        {
            Artists = artists.Items;
            lstMain.ItemsSource = artists.Items;
        }
    }
}