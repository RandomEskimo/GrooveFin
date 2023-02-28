﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class ArtistsResponse
    {
        public List<Artist>? Items { get; set; }
        public int TotalRecordCount { get; set; }
        public int StartIndex { get; set; }
    }
}
