using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Net.Http
{
    internal static class HttpClientExtensions
    {
        internal static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string requestUri, object content)
        {
            return await httpClient.PostAsync(requestUri, CreateHttpContent(content));
        }

        internal static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string requestUri, object content)
        {
            return await httpClient.PutAsync(requestUri, CreateHttpContent(content));
        }

        private static HttpContent CreateHttpContent(object data)
        {
            var content = JsonConvert.SerializeObject(data);
            return new StringContent(content, Encoding.Unicode, "application/json");
        }
    }
}