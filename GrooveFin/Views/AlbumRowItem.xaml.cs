
using JellyFinAPI.CommunicationClasses;
using System.Runtime.CompilerServices;

namespace GrooveFin.Views;

public partial class AlbumRowItem : ContentView
{
	public static BindableProperty AlbumProperty = BindableProperty.Create(nameof(Album), typeof(JellyFinAPI.CommunicationClasses.Album), typeof(AlbumRowItem));

	public JellyFinAPI.CommunicationClasses.Album? Album
	{
		get => GetValue(AlbumProperty) as JellyFinAPI.CommunicationClasses.Album;
		set => SetValue(AlbumProperty, value);
	}

	public AlbumRowItem()
	{
		InitializeComponent();
	}

    protected override async void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

		if(propertyName == nameof(Album) && Album != null && Album.Name != null && Album.Id != null)
		{
			try
			{
				lblAlbumName.Text = Album.Name.Replace("<", "&lt;").Replace(">", "&gt;");
				//imgAlbum.Source = ImageSource.FromStream(ct => CacheAccess.GetSmallImage(Album.Id));
				//if (await CacheAccess.GetSmallImagePath(Album.Id) is string imagePath)
					//imgAlbum.Source = ImageSource.FromFile(imagePath);
			}
			catch (Exception ex)
			{
				App.ReportException(ex, nameof(AlbumRowItem));
			}
        }
    }
}