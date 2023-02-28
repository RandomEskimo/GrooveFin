using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrooveFin.DataClasses
{
    public record class UserProfile(
        string Username,
        string UserId,
        string ServerAddress,
        string AccessToken,
        string SessionId
        );
}
