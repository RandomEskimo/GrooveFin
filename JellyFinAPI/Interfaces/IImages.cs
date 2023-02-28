using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.Interfaces
{
    public interface IImages
    {
        Task<byte[]?> GetImage(string ItemId);
    }
}
