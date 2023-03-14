using Android.OS;
using Android.Service.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrooveFin.Platforms.Android.Services
{
	internal class GrooveFinMediaBrowserService : MediaBrowserService
	{
		public override BrowserRoot? OnGetRoot(string clientPackageName, int clientUid, Bundle? rootHints)
		{
			if(rootHints is Bundle hints && NativeAudioService.Instance is NativeAudioService audioService) 
			{
				if(rootHints.GetBoolean(BrowserRoot.ExtraRecent))
				{
					
				}
			}
			return null;
		}

		public override void OnLoadChildren(string parentId, Result result)
		{
			
		}
	}
}
