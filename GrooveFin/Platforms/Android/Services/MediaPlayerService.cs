using Android.App;
using Android.Content;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrooveFin.Platforms.Android.Services
{
	internal class MediaPlayerService : Service
	{
		public override IBinder? OnBind(Intent? intent)
		{
			return null;
		}
	}
}
