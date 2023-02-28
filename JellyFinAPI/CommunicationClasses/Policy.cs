using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class Policy
    {
        public bool IsAdministrator { get; set; }
        public bool IsHidden { get; set; }
        public bool IsDisabled { get; set; }
        //public List<object> BlockedTags { get; set; }
        public bool EnableUserPreferenceAccess { get; set; }
        //public List<object> AccessSchedules { get; set; }
        //public List<object> BlockUnratedItems { get; set; }
        public bool EnableRemoteControlOfOtherUsers { get; set; }
        public bool EnableSharedDeviceControl { get; set; }
        public bool EnableRemoteAccess { get; set; }
        public bool EnableLiveTvManagement { get; set; }
        public bool EnableLiveTvAccess { get; set; }
        public bool EnableMediaPlayback { get; set; }
        public bool EnableAudioPlaybackTranscoding { get; set; }
        public bool EnableVideoPlaybackTranscoding { get; set; }
        public bool EnablePlaybackRemuxing { get; set; }
        public bool ForceRemoteSourceTranscoding { get; set; }
        public bool EnableContentDeletion { get; set; }
        //public List<object> EnableContentDeletionFromFolders { get; set; }
        public bool EnableContentDownloading { get; set; }
        public bool EnableSyncTranscoding { get; set; }
        public bool EnableMediaConversion { get; set; }
        //public List<object> EnabledDevices { get; set; }
        public bool EnableAllDevices { get; set; }
        //public List<object> EnabledChannels { get; set; }
        public bool EnableAllChannels { get; set; }
        //public List<object> EnabledFolders { get; set; }
        public bool EnableAllFolders { get; set; }
        public int InvalidLoginAttemptCount { get; set; }
        public int LoginAttemptsBeforeLockout { get; set; }
        public int MaxActiveSessions { get; set; }
        public bool EnablePublicSharing { get; set; }
        //public List<object> BlockedMediaFolders { get; set; }
        //public List<object> BlockedChannels { get; set; }
        public int RemoteClientBitrateLimit { get; set; }
        public string? AuthenticationProviderId { get; set; }
        public string? PasswordResetProviderId { get; set; }
        public string? SyncPlayAccess { get; set; }
    }
}
