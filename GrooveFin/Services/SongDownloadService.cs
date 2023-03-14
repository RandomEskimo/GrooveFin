using GrooveFin.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrooveFin.Services
{
	internal static class SongDownloadService
	{
		public static event EventHandler? DownloadStarted;
		public static event EventHandler? DownloadCompleted;
		public static event EventHandler? DownloadFailed;

		private static List<SongDownload> CompletedDownloads;
		private static SongDownload? CurrentDownload;
		private static Queue<SongDownload> DownloadQueue;

		private static Task? DownloadTask;

		public static List<SongDownload> CurrentQueue
		{
			get
			{
				lock(DownloadQueue)
				{
					return CompletedDownloads.ToList()
						.Also(it => { if (CurrentDownload != null) it.Add(CurrentDownload); })
						.Also(it => it.AddRange(DownloadQueue.ToList()));
				}
			}
		}

		static SongDownloadService()
		{
			DownloadQueue = new Queue<SongDownload>();
			CompletedDownloads = new List<SongDownload>();
		}

		public static async Task AddToDownloadQueue(string SongId, string SongName, string Artist)
		{
			if (!await CacheAccess.IsSongDownloadedAsync(SongId))
			{
				lock (DownloadQueue)
				{
					DownloadQueue.Enqueue(new SongDownload(SongId, SongName, Artist, SongDownloadState.Waiting));
					if (DownloadTask == null)
					{
						DownloadTask = Task.Run(DownloadRun);
					}
				}
			}
		}

		private static void DownloadRun()
		{
			while (true)
			{
				SongDownload? nextSong = null;
				lock (DownloadQueue)
				{
					if (DownloadQueue.Count > 0)
					{
						nextSong = DownloadQueue.Dequeue();
					}
				}

				if(nextSong != null)
				{
					DownloadSong(nextSong).Wait();
				}
				else
				{
					//end the task as we don't need it anymore
					lock(DownloadQueue)
					{
						if(DownloadQueue.Count == 0)
						{
							DownloadTask = null;
							return;
						}
					}
				}
			}
		}

		private static async Task DownloadSong(SongDownload Download)
		{
			CurrentDownload = Download with { State = SongDownloadState.Downloading };
			DownloadStarted?.Invoke(null, EventArgs.Empty);

			try
			{
				if (App.Jellyfin != null && await App.Jellyfin.Songs.DownloadSong(Download.SongId) is byte[] songData)
				{
					CacheAccess.AddSongDownload(Download.SongId, songData);
				}
				else
				{
					CompletedDownloads.Add(Download with { State = SongDownloadState.Failed });
					CurrentDownload = null;
					DownloadFailed?.Invoke(null, EventArgs.Empty);
					return;
				}
			}
			catch(Exception ex)
			{
				App.ReportException(ex);
				CompletedDownloads.Add(Download with { State = SongDownloadState.Failed });
				CurrentDownload = null;
				DownloadFailed?.Invoke(null, EventArgs.Empty);
				return;
			}
			CompletedDownloads.Add(Download with { State = SongDownloadState.Completed });
			CurrentDownload = null;
			DownloadCompleted?.Invoke(null, EventArgs.Empty);
		}
	}

	internal enum SongDownloadState
	{
		Waiting,
		Downloading,
		Completed,
		Failed
	}

	internal record class SongDownload(
		string SongId,
		string SongName,
		string Artist,
		SongDownloadState State
		);
}
