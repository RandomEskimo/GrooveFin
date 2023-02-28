using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Pw { get; set; }
    }
}
