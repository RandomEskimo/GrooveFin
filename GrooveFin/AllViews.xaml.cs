namespace GrooveFin;

public partial class AllViews : TabbedPage
{
	public AllViews()
	{
		InitializeComponent();
        Children.Add(new ArtistsPage());
    }
}