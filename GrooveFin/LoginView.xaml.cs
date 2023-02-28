using JellyFinAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrooveFin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentView
    {
        public event EventHandler? LoginSuccess;
        
        public LoginView()
        {
            InitializeComponent();
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtServerAddress.Text) &&
                !string.IsNullOrWhiteSpace(txtUsername.Text) &&
                !string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                try
                {
                    JellyfinAccessCreator jac = new JellyfinAccessCreator(App.DeviceDetails);
                    App.Jellyfin = await jac.Login(txtServerAddress.Text, txtUsername.Text, txtPassword.Text);
                    if (App.Jellyfin != null)
                    {
                        CacheAccess.AccessToken = App.Jellyfin.AccessToken;
                        CacheAccess.Username = App.Jellyfin.Username;
                        CacheAccess.UserId = App.Jellyfin.UserId;
                        CacheAccess.ServerAddress = txtServerAddress.Text;
                        CacheAccess.SessionId = App.Jellyfin.SessionId;
                        LoginSuccess?.Invoke(this, EventArgs.Empty);
                    }
                }
                catch(Exception ex)
                {
                    
                }
            }
        }
    }
}