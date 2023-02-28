using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class UserData
    {
        public int UnplayedItemCount { get; set; }
        public long PlaybackPositionTicks { get; set; }
        public int PlayCount { get; set; }
        public bool IsFavorite { get; set; }
        public bool Played { get; set; }
        public string? Key { get; set; }
    }
}
