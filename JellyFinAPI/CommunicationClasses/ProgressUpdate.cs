using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
	public class ProgressUpdate
	{
		public int VolumeLevel { get; set; }
		public bool IsMuted { get; set; }
		public bool IsPaused { get; set; }
		public string? RepeatMode { get; set; }
		public string? ShuffleMode { get; set; }
		public int MaxStreamingBitrate { get; set; }
		public long PositionTicks { get; set; }
		public long PlaybackStartTimeTicks { get; set; }
		public int PlaybackRate { get; set; }
		public List<BufferedRange>? BufferedRanges { get; set; }
		public string? PlayMethod { get; set; }
		public string? PlaySessionId { get; set; }
		public string? PlaylistItemId { get; set; }
		public string? MediaSourceId { get; set; }
		public bool CanSeek { get; set; }
		public string? ItemId { get; set; }
		public string? EventName { get; set; }
	}
}
