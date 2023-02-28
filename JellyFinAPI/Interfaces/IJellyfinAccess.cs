using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.Interfaces
{
    public interface IJellyfinAccess
    {
        string AccessToken { get; }
        string Username { get; }
        string UserId { get; }
        string SessionId { get; }
        IArtists Artists { get; }
        IImages Images { get; }
        IAlbums Albums { get; }
        ISongs Songs { get; }
    }
}
