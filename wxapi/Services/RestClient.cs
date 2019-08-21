using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Web;

[assembly: InternalsVisibleTo("wxapi.tests")]
namespace wxapi.Services
{
    public class RestClient : IRestClient
    {
        private readonly IConfigService configService;
        private readonly HttpClient client;
        private bool disposed = false;

        public RestClient(IConfigService configService)
        {
            this.configService = configService;
            this.client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
		}

		public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                client.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var urlWithToken = GetUrlWithToken(url);

			var streamTask = client.GetStreamAsync(urlWithToken);

            var serializer = new DataContractJsonSerializer(typeof(T));
            var result = (T)serializer.ReadObject(await streamTask);

            return result;
        }

        internal string GetUrlWithToken(string url)
        {
            var uri = new Uri(url);
            var baseUri = uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped);
            var queryStrings = HttpUtility.ParseQueryString(uri.Query);
            queryStrings["token"] = queryStrings["token"] ?? configService.GetToken();
            return baseUri + "?" + queryStrings;
        }
    }
}
