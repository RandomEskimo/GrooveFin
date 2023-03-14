using JellyFinAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace JellyFinAPI.DefaultImplementations
{
    internal class DefaultWebAccess : IWebAccess
    {
        public async Task<byte[]?> MakeBinaryRequest(string Url, Dictionary<string, string> Headers, string? Method, object? Body)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);

            HttpMethod method = Method switch
            {
                "GET" => HttpMethod.Get,
                "POST" => HttpMethod.Post,
                "PUT" => HttpMethod.Put,
                "DELETE" => HttpMethod.Delete,
                "PATCH" => HttpMethod.Patch,
                _ => HttpMethod.Get
            };

            HttpRequestMessage req = new HttpRequestMessage(method, Url);

            if (Method != "GET" && Body != null)
            {
                string jsonString = System.Text.Json.JsonSerializer.Serialize(Body);
                req.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            }

            if (Headers.Count > 0)
            {
                foreach (string key in Headers.Keys)
                {
                    req.Headers.TryAddWithoutValidation(key, Headers[key]);
                }
            }

            HttpResponseMessage response = await client.SendAsync(req);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using var memStr = new MemoryStream();
                response.Content.ReadAsStream().CopyTo(memStr);
                return memStr.ToArray();
            }
            else if(response.StatusCode == HttpStatusCode.PartialContent)
            {
                throw new Exception("Partial content!!!!!!!!!!!");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Received status code: {(int)response.StatusCode} for request: {Url}");
            }
            return null;
        }

        public async Task<T?> MakeJsonRequest<T>(string Url, Dictionary<string, string> Headers, string? Method, object? Body)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);

            HttpMethod method = Method switch
            {
                "GET" => HttpMethod.Get,
                "POST" => HttpMethod.Post,
                "PUT" => HttpMethod.Put,
                "DELETE" => HttpMethod.Delete,
                "PATCH" => HttpMethod.Patch,
                _ => HttpMethod.Get
            };

            HttpRequestMessage req = new HttpRequestMessage(method, Url);

            if (Method != "GET" && Body != null)
            {
                string jsonString = System.Text.Json.JsonSerializer.Serialize(Body);
                req.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                //req.Headers.Add("Content-Type", "application/json");
            }

            if (Headers.Count > 0)
            {
                foreach (string key in Headers.Keys)
                {
                    req.Headers.TryAddWithoutValidation(key, Headers[key]);
                }
            }

            HttpResponseMessage response = await client.SendAsync(req);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string strResult = await response.Content.ReadAsStringAsync();
                if (typeof(T) == typeof(string))
                {
                    strResult = string.Format("\"{0}\"", strResult);
                }
                return System.Text.Json.JsonSerializer.Deserialize<T>(strResult);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Received status code: {(int)response.StatusCode} for request: {Url}");
                System.Diagnostics.Debug.WriteLine($"Error body: {await response.Content.ReadAsStringAsync()}");
            }
            return default;
        }
    }
}
