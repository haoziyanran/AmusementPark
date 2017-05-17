using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace ConsoleApplication
{
    public class HttpApiClient
    {
        #region Properties

        public static HttpClient Client { get; private set; }

        #endregion Properties

        static HttpApiClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; rv:48.0) Gecko/20100101 Firefox/48.0");
            Client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        }

        public static Task<HttpResponseMessage> GetAsync(string url, CancellationToken token = default(CancellationToken))
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return Client.SendAsync(request, HttpCompletionOption.ResponseContentRead, token);
        }

        public static HttpResponseMessage GetSync(string url, Action<HttpRequestMessage> configureRequest = null, Action<HttpRequestHeaders> headerProcessor = null)
        {
            var task = GetAsync(url);
            task.Wait();
            return task.Result;
        }
    }
}
