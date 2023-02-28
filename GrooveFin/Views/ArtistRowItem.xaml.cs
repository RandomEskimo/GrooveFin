using JellyFinAPI.Interfaces;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GrooveFin.Views;

public partial class ArtistRowItem : ContentView
{
	public static BindableProperty ArtistProperty = BindableProperty.Create(nameof(Artist), typeof(JellyFinAPI.CommunicationClasses.Artist), typeof(ArtistRowItem));

	public JellyFinAPI.CommunicationClasses.Artist? Artist
	{
		get => GetValue(ArtistProperty) as JellyFinAPI.CommunicationClasses.Artist;
		set => SetValue(ArtistProperty, value);
	}

	public ArtistRowItem()
	{
		InitializeComponent();
	}

    protected override async void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

		if(propertyName == nameof(Artist) && Artist != null) 
		{
			lblArtistName.Text = Artist.Name;
			if(Artist.Name != null && Artist.Id != null)
			{
				//imgArtist.Source = ImageSource.FromStream(ct => CacheAccess.GetSmallImage(Artist.Id));
				/*var imgStream = await CacheAccess.GetSmallImage(Artist.Id);
				if(imgStream != null)
				{
					imgArtist.Source = ImageSource.FromStream(() => imgStream);
				}*/
				if (await CacheAccess.GetSmallImagePath(Artist.Id) is string imagePath)
				{
					imgArtist.Source = ImageSource.FromFile(imagePath);
				}
			}
		}
    }
}