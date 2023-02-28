using JellyFinAPI.CommunicationClasses;
using JellyFinAPI.ControllerAccess;
using JellyFinAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI
{
    internal class JellyfinAccess : IJellyfinAccess
    {
        public string AccessToken { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string SessionId { get; set; }
        public AppAndDeviceDetails Details { get; set; }
        public string ServerAddress { get; set; }
        public IWebAccess WebAccess { get; set; }
        public IArtists Artists {  get; set; }
        public IImages Images { get; set; }
        public IAlbums Albums { get; set; }
        public ISongs Songs { get; set; }
        public ISessions Sessions { get; set; }

        public JellyfinAccess(AppAndDeviceDetails Details, string ServerAddress, IWebAccess WebAccess, string AccessToken, string UserId, string Username, string SessionId)
        {
            this.AccessToken = AccessToken;
            this.UserId = UserId;
            this.Username = Username;
            this.Details = Details;
            this.ServerAddress = ServerAddress;
            this.WebAccess = WebAccess;
            this.SessionId = SessionId;
            Artists = new ArtistsAccess(this);
            Images = new ImagesAccess(this);
            Albums = new AlbumsAccess(this);
            Songs = new SongsAccess(this);
            Sessions = new SessionAccess(this);
        }

        public Task<UserItemsResponse?> GetUserItems() =>
            WebAccess.MakeJsonRequest<UserItemsResponse?>(
                $"{ServerAddress}/Users/{UserId}/Items",
                JellyfinAccessCreator.BuildHeaders(Details, AccessToken),
                "GET",
                null
                );
    }
}
