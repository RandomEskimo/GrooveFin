using System.Security.Cryptography.X509Certificates;
using JellyFinAPI.CommunicationClasses;
using JellyFinAPI.DefaultImplementations;
using JellyFinAPI.Helpers;
using JellyFinAPI.Interfaces;

namespace JellyFinAPI
{
    public class JellyfinAccessCreator
    {
        private AppAndDeviceDetails Details;
        private IWebAccess WebAccess;

        public JellyfinAccessCreator(AppAndDeviceDetails Details)
        {
            this.Details = Details;
            this.WebAccess = new DefaultWebAccess();
        }

        public JellyfinAccessCreator(AppAndDeviceDetails Details, IWebAccess WebAccess)
        {
            this.Details = Details;
            this.WebAccess = WebAccess;
        }

        public async Task<IJellyfinAccess?> Login(string ServerAddress, string Username, string Password) =>
            (await WebAccess.MakeJsonRequest<LoginResponse?>(
                $"{ServerAddress}/Users/AuthenticateByName",
                BuildHeaders(Details, null),
                "POST",
                new LoginRequest() { Username = Username, Pw = Password }
                ))?.Then(response =>
                {
                    if(!string.IsNullOrWhiteSpace(response.AccessToken) && 
                        response.User != null && 
                        !string.IsNullOrWhiteSpace(response.User.Name) &&
                        !string.IsNullOrWhiteSpace(response.User.Id) && 
                        !string.IsNullOrWhiteSpace(response.SessionInfo?.Id)
                        )
                    {
                        return new JellyfinAccess(
                            Details, 
                            ServerAddress, 
                            WebAccess, 
                            response.AccessToken, 
                            response.User.Id, 
                            response.User.Name,
                            response.SessionInfo.Id
                            );
                    }
                    return null;
                });

        public IJellyfinAccess CreateFromToken(string ServerAddress, string AccessToken, string UserId, string Username, string SessionId)
        {
            return new JellyfinAccess(Details, ServerAddress, WebAccess, AccessToken, UserId, Username, SessionId);
        }

        public static Dictionary<string,string> BuildHeaders(AppAndDeviceDetails Details, string? Token)
        {
            string auth = $"MediaBrowser Client=\"{Details.AppName}\", Device=\"{Details.Device}\", DeviceId=\"{Details.DeviceID}\", Version=\"{Details.Version}\"";
            if (Token != null)
                auth += $", Token=\"{Token}\"";
            return new Dictionary<string, string>() { { "Authorization", auth } };
        }
    }
}