namespace Papyrus.Tests.WebServices {

    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    
    internal class RestClient {
        private readonly HttpClient httpClient;

        public RestClient(string baseAddress) {
            httpClient = new HttpClient {BaseAddress = new Uri(baseAddress)};
        }

        public async Task<T> Get<T>(string path) {
            var response = httpClient.GetAsync(path).Result;
            return await response.Content.ReadAsAsync<T>();
        }
    }
}
