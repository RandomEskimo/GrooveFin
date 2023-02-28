using JellyFinAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JellyFinAPI.ControllerAccess
{
    internal class ImagesAccess : IImages
    {
        private JellyfinAccess Access;

        public ImagesAccess(JellyfinAccess Access)
        {
            this.Access = Access;
        }

        public Task<byte[]?> GetImage(string ItemId) =>
            Access.WebAccess.MakeBinaryRequest(
                $"{Access.ServerAddress}/Items/{ItemId}/Images/Primary",
                JellyfinAccessCreator.BuildHeaders(Access.Details, Access.AccessToken),
                "GET",
                null
                );
    }
}
