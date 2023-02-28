using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class SessionInfo
    {
        public PlayState? PlayState { get; set; }
        //public List<object> AdditionalUsers { get; set; }
        public Capabilities? Capabilities { get; set; }
        public string? RemoteEndPoint { get; set; }
        //public List<object> PlayableMediaTypes { get; set; }
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Client { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public DateTime? LastPlaybackCheckIn { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceId { get; set; }
        public string? ApplicationVersion { get; set; }
        public bool IsActive { get; set; }
        public bool SupportsMediaControl { get; set; }
        public bool SupportsRemoteControl { get; set; }
        //public List<object>? NowPlayingQueue { get; set; }
        //public List<object>? NowPlayingQueueFullItems { get; set; }
        public bool HasCustomDeviceName { get; set; }
        public string? ServerId { get; set; }
        //public List<object> SupportedCommands { get; set; }
    }
}
