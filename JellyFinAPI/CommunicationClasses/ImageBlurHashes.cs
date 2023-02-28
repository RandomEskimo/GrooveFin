using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class ImageBlurHashes
    {
        public Dictionary<string, string>? Backdrop { get; set; }
        public Dictionary<string, string>? Primary { get; set; }
        public Dictionary<string, string>? Logo { get; set; }
    }
}
