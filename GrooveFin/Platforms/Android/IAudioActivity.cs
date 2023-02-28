using GrooveFin.Platforms.Android.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrooveFin.Platforms.Android
{
	public interface IAudioActivity
	{
		public MediaPlayerServiceBinder? Binder { get; set; }

		public event StatusChangedEventHandler? StatusChanged;

		public event CoverReloadedEventHandler? CoverReloaded;

		public event PlayingEventHandler? Playing;

		public event BufferingEventHandler? Buffering;
	}
}
