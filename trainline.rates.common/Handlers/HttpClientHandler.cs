using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace trainline.rates.common.handlers
{
    [ExcludeFromCodeCoverage]
    public class HttpClientHandler : IHttpHandler
    {
        private HttpClient _client = new HttpClient();

        /// <summary>
        /// Adds the specified headers parameter list to the Http Client
        /// </summary>
        /// <param name="headers">Http Headers</param>
        public void SetHeaders(Dictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                _client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        public HttpResponseMessage Get(string url)
        {
            return GetAsync(url).Result;
        }

        public HttpResponseMessage Post(string url, HttpContent content)
        {
            return PostAsync(url, content).Result;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await _client.PostAsync(url, content);
        }
    }
}
