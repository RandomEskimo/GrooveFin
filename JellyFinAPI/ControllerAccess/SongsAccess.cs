using JellyFinAPI.CommunicationClasses;
using JellyFinAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.ControllerAccess
{
    internal class SongsAccess : ISongs
    {
        private JellyfinAccess Access;

        public SongsAccess(JellyfinAccess Access)
        {
            this.Access = Access;
        }

        public Task<SongsResponse?> GetSongs(string CollectionId) =>
            Access.WebAccess.MakeJsonRequest<SongsResponse?>(
                $"{Access.ServerAddress}/Users/{Access.UserId}/Items?ParentId={CollectionId}&SortOrder=Ascending&SortBy=SortName",
                JellyfinAccessCreator.BuildHeaders(Access.Details, Access.AccessToken),
                "GET",
                null
                );

        public string MakeSongUrl(string SongId) => $"{Access.ServerAddress}/Audio/{SongId}/universal" +
            $"?AudioCodec=aac" +
            $"&Container=opus,webm|opus,mp3,aac,m4a|aac,m4a|alac,m4b|aac,flac,webma,webm|webma,wav,ogg" +
            $"&TranscodingContainer=ts" +
            $"&TranscodingProtocol=hls" +
            $"&UserId={Access.UserId}" +
            $"&DeviceId={Access.Details.DeviceID}" +
            $"&api_key={Access.AccessToken}";

    }
}
