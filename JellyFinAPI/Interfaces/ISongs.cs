using JellyFinAPI.CommunicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.Interfaces
{
    public interface ISongs
    {
        Task<SongsResponse?> GetSongs(string CollectionId);
        string MakeSongUrl(string SongId);
        Task<byte[]?> DownloadSong(string SongId);
    }
}
