﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class Album
    {
        public string? Name { get; set; }
        public string? ServerId { get; set; }
        public string? Id { get; set; }
        public string? ChannelId { get; set; }
        public bool IsFolder { get; set; }
        public string? Type { get; set; }
        public string? ParentLogoItemId { get; set; }
        public string? ParentBackdropItemId { get; set; }
        public List<string>? ParentBackdropImageTags { get; set; }
        public UserData UserData { get; set; }
        public ImageTags? ImageTags { get; set; }
        public List<object>? BackdropImageTags { get; set; }
        public string? ParentLogoImageTag { get; set; }
        public ImageBlurHashes? ImageBlurHashes { get; set; }
        public string? LocationType { get; set; }
        public DateTime? PremiereDate { get; set; }
        public long? RunTimeTicks { get; set; }
        public int? ProductionYear { get; set; }
        public List<string>? Artists { get; set; }
        public List<ArtistItem>? ArtistItems { get; set; }
        public string? AlbumArtist { get; set; }
        public List<AlbumArtist>? AlbumArtists { get; set; }
    }
}
