using JellyFinAPI.CommunicationClasses;
using JellyFinAPI.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrooveFin
{
    internal static class CacheAccess
    {
        private static TimeSpan MaxAgeForData = TimeSpan.FromMinutes(5);

        private static string AppDir = FileSystem.Current.AppDataDirectory;
        private static string SettingsFilePath = $"{AppDir}/Settings.json";
        private static string ArtistCache = $"{AppDir}/Artists";
        private static string ImageCache = $"{AppDir}/Images";
        private static string SmallImageCache = $"{AppDir}/SmallImages";
        private static string SongsCache = $"{AppDir}/Songs";
        private static string SongDownloadDir = $"{AppDir}/Downloads";

        private static Dictionary<string, string>? CacheValues;
        private static HashSet<string>? DownloadedSongs;
        private static object DownloadedSongsLockObj = new object();

        private const long MaxInMemCacheSize = 10 * 1024 * 1024; //~10mb
        private static Dictionary<string, Tuple<DateTime, byte[]>> InMemSmallImageCache;
        private static long InMemCacheSize;

        static CacheAccess()
        {
            InMemSmallImageCache = new Dictionary<string, Tuple<DateTime, byte[]>>();
            InMemCacheSize = 0;
        }

        #region Settings
        public static string? AccessToken
        {
            get => GetSettingsObject<string>("AccessToken");
            set => SetSettingsObject("AccessToken", value);
        }

        public static string? Username
        {
            get => GetSettingsObject<string>("Username");
            set => SetSettingsObject("Username", value);
        }

        public static string? UserId
        {
            get => GetSettingsObject<string>("UserId");
            set => SetSettingsObject("UserId", value);
        }

        public static string? ServerAddress
        {
            get => GetSettingsObject<string>("ServerAddress");
            set => SetSettingsObject("ServerAddress", value);
        }

        public static string? SessionId
        {
            get => GetSettingsObject<string>("SessionId");
            set => SetSettingsObject("SessionId", value);
        }

        public static T? GetSettingsObject<T>(string Key)
        {
            if(CacheValues == null)
            {
                LoadCacheValues();
            }
            if (CacheValues!.ContainsKey(Key))
            {
                string settingsString = CacheValues[Key];
                if (JsonSerializer.Deserialize<T>(settingsString) is T result)
                    return result;
            }
            return default;
        }

        private static void LoadCacheValues()
        {
            if (CacheValues == null && File.Exists(SettingsFilePath))
            {
                string allSettingsStr = File.ReadAllText(SettingsFilePath);
                CacheValues = JsonSerializer.Deserialize<Dictionary<string, string>>(allSettingsStr);
            }
            if (CacheValues == null)
            {
                CacheValues = new Dictionary<string, string>();
            }
        }

        private static void SaveCacheValues() 
        { 
            if(CacheValues != null)
            {
                string serialised = JsonSerializer.Serialize(CacheValues);
                File.WriteAllText(SettingsFilePath, serialised);
            }
        }

        public static void SetSettingsObject<T>(string Key, T Value)
        {
            if(CacheValues == null)
            {
                LoadCacheValues();
            }
            string stringObject = JsonSerializer.Serialize(Value);
            if(CacheValues!.ContainsKey(Key))
            {
                CacheValues[Key] = stringObject;
            }
            else
            {
                CacheValues.Add(Key, stringObject);
            }
            SaveCacheValues();
        }

        #endregion
        #region Artists
        public static Task<ArtistsResponse?> GetAllArtists()
        {
            if (!Directory.Exists(ArtistCache))
                Directory.CreateDirectory(ArtistCache);
            string path = $"{ArtistCache}/all.json";
            return GetCachedData<ArtistsResponse?>(
                path, jellyfin => jellyfin.Artists.GetAllArtists(),
                MaxAgeForData
                );
        }
        #endregion
        #region Images
        public static async Task<Stream?> GetImage(string ItemId)
        {
            if (!Directory.Exists(ImageCache))
                Directory.CreateDirectory(ImageCache);
            string path = $"{ImageCache}/{ItemId}";
            if(File.Exists(path))
            {
                return File.OpenRead(path);
            }
            if(App.Jellyfin != null)
            {
                byte[]? image = await App.Jellyfin.Images.GetImage(ItemId);
                if(image != null)
                {
                    File.WriteAllBytes(path, image);
                    return new MemoryStream(image);
                }
            }
            return null;
        }
		public static async Task<string?> GetImagePath(string ItemId)
		{
			string path = $"{ImageCache}/{ItemId}";
			if (File.Exists(path))
				return path;
			if (await GetImage(ItemId) is Stream imgStream)
			{
				imgStream.Close();
				return path;
			}
			return null;
		}
		public static async Task<Stream?> GetSmallImage(string ItemId)
        {
            if (!Directory.Exists(SmallImageCache))
                Directory.CreateDirectory(SmallImageCache);
            string path = $"{SmallImageCache}/{ItemId}";
            if(GetSmallImageFromMemCache(ItemId) is byte[] imageData)
            {
                return new MemoryStream(imageData);
            }
            return await Task.Run<Stream?>(async () =>
            {
                byte[]? imageBytes = null;
                if (File.Exists(path))
                {
                    imageBytes = File.ReadAllBytes(path);
                }
                else if (await GetImage(ItemId) is Stream imageStream)
                {
                    using MemoryStream ms = new MemoryStream();
                    imageStream.CopyTo(ms);
                    byte[] fullImageBytes = ms.ToArray();
                    imageBytes = ScaleImage(fullImageBytes, 100, 100);
                    File.WriteAllBytes(path, imageBytes);
                }

                if (imageBytes != null)
                {
                    PutSmallImageInMemCache(ItemId, imageBytes);
                    return new MemoryStream(imageBytes);
                }
                return null;
            });
        }
        public static async Task<string?> GetSmallImagePath(string ItemId)
        {
            string path = $"{SmallImageCache}/{ItemId}";
            if(File.Exists(path))
                return path;
            if (await GetSmallImage(ItemId) is Stream imgStream)
            {
                imgStream.Close();
                return path;
            }
            return null;
        }
        private static byte[]? GetSmallImageFromMemCache(string ItemId)
        {
            lock (InMemSmallImageCache)
            {
                if (InMemSmallImageCache.ContainsKey(ItemId))
                {
                    var result = InMemSmallImageCache[ItemId].Item2;
                    InMemSmallImageCache[ItemId] = new Tuple<DateTime, byte[]>(DateTime.Now, result);
                    return result;
                }
            }
            return null;
        }
        private static void PutSmallImageInMemCache(string ItemId, byte[] ImageData)
        {
            lock(InMemSmallImageCache)
            {
                InMemSmallImageCache.Add(ItemId, new Tuple<DateTime, byte[]>(DateTime.Now, ImageData));
                InMemCacheSize += ImageData.Length;
                if(InMemCacheSize > MaxInMemCacheSize)
                {
                    Debug.WriteLine("Cleaning small image cache");
                    var oldest = InMemSmallImageCache.MinBy(i => i.Value.Item1);
                    InMemSmallImageCache.Remove(oldest.Key);
                    InMemCacheSize -= oldest.Value.Item2.Length;
                }
            }
        }
        #endregion
        #region Albums
        public static Task<AlbumsResponse?> GetAlbumsForArtist(string ArtistId)
        {
            if (!Directory.Exists(ArtistCache))
                Directory.CreateDirectory(ArtistCache);
            string artistPath = $"{ArtistCache}/{ArtistId}";
            if(!Directory.Exists(artistPath))
                Directory.CreateDirectory(artistPath);
            string path = $"{artistPath}/albums.json";
            return GetCachedData<AlbumsResponse?>(
                path,
                jellyfin => jellyfin.Albums.GetAlbumsAsync(ArtistId),
                MaxAgeForData
                );
        }

        public static Task<AlbumsResponse?> GetAllAlbums()
        {
            string path = $"{AppDir}/albums.json";
            return GetCachedData(
                path,
                jellfin => jellfin.Albums.GetAllAlbumsAsync(),
                MaxAgeForData
                );
        }
		#endregion
		#region Songs
        public static Task<SongsResponse?> GetSongs(string CollectionId)
        {
            if(!Directory.Exists(SongsCache))
                Directory.CreateDirectory(SongsCache);
            string path = $"{SongsCache}/{CollectionId}.json";
            return GetCachedData(
                path,
                jellyfin => jellyfin.Songs.GetSongs(CollectionId),
                MaxAgeForData
                );
        }
        #endregion

        #region Song Downloads
        public static void AddSongDownload(string SongId, byte[] SongData)
        {
            if(!Directory.Exists(SongDownloadDir))
                Directory.CreateDirectory(SongDownloadDir);
            File.WriteAllBytes($"{SongDownloadDir}/{SongId}", SongData);
            lock(DownloadedSongsLockObj)
            {
                if (DownloadedSongs != null)
                    DownloadedSongs.Add(SongId);
            }
        }

        public static string GetDownloadedSongPath(string SongId) => $"{SongDownloadDir}/{SongId}";

        public static Task<bool> IsSongDownloadedAsync(string SongId) =>
            AreSongsDownloadedAsync(new List<string>() { SongId });

        public static Task<bool> AreSongsDownloadedAsync(List<string> SongIds)
        {
            lock(DownloadedSongsLockObj)
            {
                if(DownloadedSongs != null)
                    return Task.FromResult(SongIds.All(DownloadedSongs.Contains));
            }
	        return Task.Run(() =>
			{
				lock (DownloadedSongsLockObj)
				{
					if (DownloadedSongs == null)
					{
						DownloadedSongs = new HashSet<string>();
						if (!Directory.Exists(SongDownloadDir))
							Directory.CreateDirectory(SongDownloadDir);
						string[] songs = Directory.GetFiles(SongDownloadDir);
                        foreach(var songPath in songs)
                        {
                            string songId = Path.GetFileName(songPath);
                            DownloadedSongs.Add(songId);
                        }
					}
					return SongIds.All(DownloadedSongs.Contains);
				}
			});
        }
        #endregion

        #region DataCache
        private static async Task<T?> GetCachedData<T>(string CachePath, Func<IJellyfinAccess, Task<T?>> GetFromAPI, TimeSpan DataExpiry)
        {
            T? cachedData = default;
            if (File.Exists(CachePath))
            {
                string json = File.ReadAllText(CachePath);
                cachedData = JsonSerializer.Deserialize<T?>(json);
            }
            if (cachedData != null && DateTime.Now - File.GetLastWriteTime(CachePath) < DataExpiry)
            {
                return cachedData;
            }
            if (App.Jellyfin != null)
            {
                try
                {
                    var freshData = await GetFromAPI(App.Jellyfin);
                    if (freshData != null)
                    {
                        string json = JsonSerializer.Serialize(freshData);
                        File.WriteAllText(CachePath, json);
                    }
                    else
                    {
                        return cachedData;
                    }
                    return freshData;
                }
                catch { }
            }
            return cachedData;
        }
        #endregion

        public static byte[] ScaleImage(byte[] ImageBytes, int MaxWidth, int MaxHeight)
        {
            using MemoryStream memStream = new MemoryStream();
            memStream.Write(ImageBytes);
            memStream.Position = 0;

            using var largeImage = SixLabors.ImageSharp.Image.Load(memStream);

            using var smallImage = ScaleImage(largeImage, 200, 200);
            MemoryStream resultStream = new MemoryStream();
            smallImage.SaveAsWebp(resultStream);

            return resultStream.ToArray();
        }

        public static SixLabors.ImageSharp.Image ScaleImage(SixLabors.ImageSharp.Image InputImage, int MaxWidth, int MaxHeight)
        {
            int width = InputImage.Width;
            int height = InputImage.Height;
            if (InputImage.Width > MaxWidth || InputImage.Height > MaxHeight)
            {
                if (InputImage.Width >= InputImage.Height)
                {
                    height = (int)(MaxWidth * (InputImage.Height / (double)InputImage.Width));
                    width = MaxWidth;
                }
                else if (InputImage.Height > InputImage.Width)
                {
                    width = (int)(MaxHeight * (InputImage.Width / (double)InputImage.Height));
                    height = MaxHeight;
                }
            }

            return InputImage.Clone(x => {
                x.Resize(new ResizeOptions()
                {
                    Size = new SixLabors.ImageSharp.Size(width, height),
                    Mode = SixLabors.ImageSharp.Processing.ResizeMode.Pad
                });
                var orientation = InputImage.Metadata?.ExifProfile?.GetValue(SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag.Orientation);
                if (orientation != null)
                {
                    switch (orientation.Value)
                    {
                        case SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifOrientationMode.BottomLeft: // The 0th row at the bottom, the 0th column on the left.
                            x.Flip(FlipMode.Vertical);
                            break;
                        case SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifOrientationMode.BottomRight: // The 0th row at the bottom, the 0th column on the right.
                            x.Flip(FlipMode.Horizontal);
                            x.Flip(FlipMode.Vertical);
                            break;
                        case SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifOrientationMode.LeftBottom: // The 0th row on the left, the 0th column at the bottom.
                            x.Rotate(RotateMode.Rotate270);
                            break;
                        case SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifOrientationMode.LeftTop: // The 0th row on the left, the 0th column at the top.
                            x.Flip(FlipMode.Vertical);
                            x.Rotate(RotateMode.Rotate90);
                            break;
                        case SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifOrientationMode.RightBottom: // The 0th row on the right, the 0th column at the bottom.
                            x.Flip(FlipMode.Vertical);
                            x.Rotate(RotateMode.Rotate270);
                            break;
                        case SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifOrientationMode.RightTop: // The 0th row at the right, the 0th column at the top.
                            x.Rotate(RotateMode.Rotate90);
                            break;
                        case SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifOrientationMode.TopLeft: // The 0th row at the top, the 0th column on the left.
                            // normal orientation, nothing to do
                            break;
                        case SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifOrientationMode.TopRight: // The 0th row at the top, the 0th column on the right.
                            x.Flip(FlipMode.Horizontal);
                            break;
                    }
                }
            });
        }
    }
}
