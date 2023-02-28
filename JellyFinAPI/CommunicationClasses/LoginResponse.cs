using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class LoginResponse
    {
        public User? User { get; set; }
        public SessionInfo? SessionInfo { get; set; }
        public string? AccessToken { get; set; }
        public string? ServerId { get; set; }
    }
}
