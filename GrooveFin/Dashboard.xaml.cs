namespace GrooveFin;

public partial class Dashboard : AbstractContentPage
{
	public Dashboard()
	{
		InitializeComponent();
	}

	private void btnArtists_Clicked(object sender, EventArgs e)
	{
		//MainPage.Instance.PushView(new ArtistsPage());
    }

	private void btnAlbums_Clicked(object sender, EventArgs e)
	{
		MainPage.Instance.PushView(new AlbumsPage());
    }

	private void btnPlaylists_Clicked(object sender, EventArgs e)
	{

    }

	private void btnDownloads_Clicked(object sender, EventArgs e)
	{
		MainPage.Instance.PushView(new Downloads());
    }
}