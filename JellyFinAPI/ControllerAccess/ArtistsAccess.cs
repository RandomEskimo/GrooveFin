using JellyFinAPI.CommunicationClasses;
using JellyFinAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.ControllerAccess
{
    internal class ArtistsAccess : IArtists
    {
        public JellyfinAccess Access;

        public ArtistsAccess(JellyfinAccess Access)
        {
            this.Access = Access;
        }

        public Task<ArtistsResponse?> GetAllArtists() =>
            Access.WebAccess.MakeJsonRequest<ArtistsResponse?>(
                $"{Access.ServerAddress}/Artists",
                JellyfinAccessCreator.BuildHeaders(Access.Details, Access.AccessToken),
                "GET",
                null);
    }
}
