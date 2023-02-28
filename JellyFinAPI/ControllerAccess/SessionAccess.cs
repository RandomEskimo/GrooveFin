using JellyFinAPI.CommunicationClasses;
using JellyFinAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.ControllerAccess
{
	internal class SessionAccess : ISessions
	{
		private JellyfinAccess Access;

		public SessionAccess(JellyfinAccess Access)
		{
			this.Access = Access;
		}

		public Task SubmitPlayingProgress(
			bool IsMuted,
			bool IsPaused,
			string RepeatMode, 
			string ShuffleMode, 
			TimeSpan Position, 
			DateTime PlaybackStartTime, 
			int PlaybackRate,
			string ItemId
		)
		{
			var update = new ProgressUpdate()
			{
				VolumeLevel = 100,
				IsMuted = IsMuted,
				IsPaused = IsPaused,
				RepeatMode = RepeatMode,
				ShuffleMode = ShuffleMode,
				MaxStreamingBitrate = 0,
				PositionTicks = Position.Ticks,
				PlaybackStartTimeTicks = PlaybackStartTime.Ticks,
				PlaybackRate = 1,
				//PlaySessionId = Access.SessionId,
				MediaSourceId = ItemId,
				CanSeek = true,
				ItemId = ItemId,
			};
			return Task.CompletedTask;
		}
	}
}
