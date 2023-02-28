using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class Artist
    {
        public string? Name { get; set; }
        public string? ServerId { get; set; }
        public string? Id { get; set; }
        public string? ChannelId { get; set; }
        public long RunTimeTicks { get; set; }
        public bool IsFolder { get; set; }
        public string? Type { get; set; }
        public ImageTags? ImageTags { get; set; }
        public List<string>? BackdropImageTags { get; set; }
        public ImageBlurHashes? ImageBlurHashes { get; set; }
        public string? LocationType { get; set; }
    }
}
