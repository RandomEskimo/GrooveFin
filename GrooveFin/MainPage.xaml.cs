using JellyFinAPI;

namespace GrooveFin;

public partial class MainPage : ContentPage
{
	private bool First = true;

	private static MainPage? _instance;
	public static MainPage Instance => _instance ?? throw new Exception("MainPage not instanciated yet");

	private Stack<AbstractContentPage> Views;

	public AbstractContentPage? CurrentView => Views?.FirstOrDefault();

	public MainPage()
	{
		InitializeComponent();
		_instance = this;
		Views = new Stack<AbstractContentPage>();
	}

	public void PushView(AbstractContentPage NewView)
	{
		Views.Push(NewView);
		stkForControls.Children.Clear();
		stkForControls.Children.Add(NewView);
		ResetScroll();
		NewView.OnAppearing();
	}

	public bool PopView()
	{
		if (Views.Count > 0)
		{
			CurrentView?.OnDisappearing();
			Views.Pop();
		}
		stkForControls.Children.Clear();
		if (Views.Count > 0 && CurrentView != null)
		{
			stkForControls.Children.Add(CurrentView);
			ResetScroll();
			CurrentView?.OnAppearing();
			return true;
		}
		else
			return false;
	}

	private void ResetScroll()
	{
		svForControls.ScrollToAsync(svForControls.ScrollX, svForControls.ScrollY, false);
	}

	protected override bool OnBackButtonPressed()
	{
		return PopView();
	}

	protected override async void OnAppearing()
    {
        base.OnAppearing();
		string logFilePath = $"{FileSystem.Current.AppDataDirectory}/debug.log";
		if (File.Exists(logFilePath))
		{
			string message = File.ReadAllText(logFilePath);
			stkForControls.Children.Add(new Editor() { Text = message, VerticalOptions = LayoutOptions.Start });
			var closeErrors = new AwaitableButton() { Text = "Close" };
			stkForControls.Children.Add(closeErrors);
			svForControls.IsVisible = true;
			stkLogin.IsVisible = false;
			File.Delete(logFilePath);
			await closeErrors.WaitForClickAsync();
			stkForControls.Children.Clear();
			svForControls.IsVisible = false;
		}
		if (First)
		{
			First = false;
			if (App.LoggedInUser == null || App.Jellyfin == null)
			{
				if (CacheAccess.AccessToken is string accessToken &&
					CacheAccess.UserId is string userId &&
					CacheAccess.Username is string username &&
					CacheAccess.ServerAddress is string serverAddress &&
					CacheAccess.SessionId is string sessionId)
				{
					JellyfinAccessCreator creator = new JellyfinAccessCreator(App.DeviceDetails);
					App.Jellyfin = creator.CreateFromToken(serverAddress, accessToken, userId, username, sessionId);
					App.LoggedInUser = new DataClasses.UserProfile(username, userId, serverAddress, accessToken, sessionId);
				}
			}

			if (App.LoggedInUser != null && App.Jellyfin != null)
			{
				//await Task.Delay(1000);
				stkLogin.IsVisible = false;
				svForControls.IsVisible = true;
				mniPlayer.IsVisible = true;
				PushView(new Dashboard());
			}
		}
    }

    private void lclLogin_LoginSuccess(object sender, EventArgs e)
    {
		PushView(new AlbumsPage());
		stkLogin.IsVisible = false;
		svForControls.IsVisible = true;
		mniPlayer.IsVisible = true;
	}

	private void btnLogout_Clicked(object sender, EventArgs e)
	{
		CacheAccess.AccessToken = null;
		CacheAccess.UserId = null;
		CacheAccess.Username = null;
		CacheAccess.ServerAddress = null;
		CacheAccess.SessionId = null;
		App.LoggedInUser = null;
		App.Jellyfin = null;
		stkLogin.IsVisible = true;
		svForControls.IsVisible = false;
		mniPlayer.IsVisible = true;
	}
}

