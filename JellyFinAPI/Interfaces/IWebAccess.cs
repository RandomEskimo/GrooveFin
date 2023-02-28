using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.Interfaces
{
    public interface IWebAccess
    {
        Task<T?> MakeJsonRequest<T>(string Url, Dictionary<string, string> Headers, string? Method, object? Body);
        Task<byte[]?> MakeBinaryRequest(string Url, Dictionary<string,string> Headers, string? Method, object? Body);
    }
}
