using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class Configuration
    {
        public bool PlayDefaultAudioTrack { get; set; }
        public string? SubtitleLanguagePreference { get; set; }
        public bool DisplayMissingEpisodes { get; set; }
        //public List<object> GroupedFolders { get; set; }
        public string? SubtitleMode { get; set; }
        public bool DisplayCollectionsView { get; set; }
        public bool EnableLocalPassword { get; set; }
        //public List<object> OrderedViews { get; set; }
        //public List<object> LatestItemsExcludes { get; set; }
        //public List<object> MyMediaExcludes { get; set; }
        public bool HidePlayedInLatest { get; set; }
        public bool RememberAudioSelections { get; set; }
        public bool RememberSubtitleSelections { get; set; }
        public bool EnableNextEpisodeAutoPlay { get; set; }
    }
}
