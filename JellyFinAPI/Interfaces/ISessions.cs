using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.Interfaces
{
	public interface ISessions
	{
		Task SubmitPlayingProgress(
			bool IsMuted, 
			bool IsPaused, 
			string RepeatMode, 
			string ShuffleMode,
			TimeSpan Position,
			DateTime PlaybackStartTime,
			int PlaybackRate,
			string ItemId
		);
		//Task StartSession
	}
}
