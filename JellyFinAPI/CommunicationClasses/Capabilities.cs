using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class Capabilities
    {
        //public List<object> PlayableMediaTypes { get; set; }
        //public List<object> SupportedCommands { get; set; }
        public bool SupportsMediaControl { get; set; }
        public bool SupportsContentUploading { get; set; }
        public bool SupportsPersistentIdentifier { get; set; }
        public bool SupportsSync { get; set; }
    }
}
