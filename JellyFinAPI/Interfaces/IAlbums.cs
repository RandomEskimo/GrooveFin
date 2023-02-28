using JellyFinAPI.CommunicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.Interfaces
{
    public interface IAlbums
    {
        Task<AlbumsResponse?> GetAlbumsAsync(string ArtistId);
        Task<AlbumsResponse?> GetAllAlbumsAsync();
    }
}
