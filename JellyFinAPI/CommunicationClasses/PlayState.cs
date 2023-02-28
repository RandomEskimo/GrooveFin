using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class PlayState
    {
        public bool CanSeek { get; set; }
        public bool IsPaused { get; set; }
        public bool IsMuted { get; set; }
        public string RepeatMode { get; set; }
    }
}
