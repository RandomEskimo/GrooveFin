using JellyFinAPI.CommunicationClasses;
using JellyFinAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.ControllerAccess
{
    internal class AlbumsAccess : IAlbums
    {
        private JellyfinAccess Access;

        public AlbumsAccess(JellyfinAccess Access)
        {
            this.Access = Access;
        }

        public Task<AlbumsResponse?> GetAlbumsAsync(string ArtistId) =>
            Access.WebAccess.MakeJsonRequest<AlbumsResponse?>(
                $"{Access.ServerAddress}/Users/{Access.UserId}/Items?parentId={ArtistId}",
                JellyfinAccessCreator.BuildHeaders(Access.Details, Access.AccessToken),
                "GET",
                null
                );

        public async Task<AlbumsResponse?> GetAllAlbumsAsync()
        {
            if((await Access.GetUserItems()) is UserItemsResponse items && items.Items != null && 
                items.Items.Find(i => i.Name == "Music") is UserItem musicItem &&
                musicItem.Id != null
                )
            {
                string url = $"{Access.ServerAddress}/Users/{Access.UserId}/Items" +
                    $"?parentId={musicItem.Id}&IncludeItemTypes=MusicAlbum&Recursive=true&" +
                    $"SortBy=SortName&SortOrder=Ascending";
                return await Access.WebAccess.MakeJsonRequest<AlbumsResponse?>(
                    url,
                    JellyfinAccessCreator.BuildHeaders(Access.Details, Access.AccessToken),
                    "GET",
                    null
                    );
            }
            return null;
        }
    }
}
