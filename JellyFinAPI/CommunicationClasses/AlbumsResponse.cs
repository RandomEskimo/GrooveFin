using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.CommunicationClasses
{
    public class AlbumsResponse
    {
        public List<Album>? Items { get; set; }
        public int TotalRecordCount { get; set; }
        public int StartIndex { get; set; }
    }
}
