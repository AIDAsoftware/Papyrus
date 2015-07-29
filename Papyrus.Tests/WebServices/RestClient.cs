namespace Papyrus.Tests.WebServices {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Newtonsoft.Json;
    using Papyrus.Business.Documents;

    internal class RestClient {
        private readonly string baseAddress;

        public RestClient(string baseAddress) {
            this.baseAddress = baseAddress;
        }

        public async Task<HttpResponseMessage> PutAsJson<T>(string url, T body)
        {
            var httpRequest = await PreparePut(url, body);
            return await DoRequest(httpRequest);
        }

        private async Task<HttpRequestMessage> PreparePut<T>(string uri, T body)
        {
            var request = new HttpRequestMessageBuilder()
                    .WithUri(uri)
                    .WithMethod(HttpMethod.Put)
                    .WithLanguageCulture(CultureInfo.CurrentCulture.ToString())
                    .WithContent(new ObjectContent(typeof(T), body, new JsonMediaTypeFormatter()))
                    .CreateHttpRequestMessage();
            return request;
        }

        public virtual async Task<HttpResponseMessage> PostAsJson<T>(
            string uri, T body)
        {
            var httpRequest = await PreparePost(uri, body);
            return await DoRequest(httpRequest);
        }

        private async Task<HttpRequestMessage> PreparePost<T>(string uri, T body)
        {
            var request = new HttpRequestMessageBuilder()
                    .WithUri(uri)
                    .WithMethod(HttpMethod.Post)
                    .WithLanguageCulture(CultureInfo.CurrentCulture.ToString())
                    .WithContent(new ObjectContent(typeof(T), body, new JsonMediaTypeFormatter()))
                    .CreateHttpRequestMessage();
            return request;
        }

        private async Task<HttpResponseMessage> DoRequest(HttpRequestMessage httpRequest)
        {
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(baseAddress) })
                {
                    var response = await client.SendAsync(httpRequest).ConfigureAwait(false);
                    return response;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new UnexpectedResponseException("RequestException", ex);
            }
        }

        public async Task<T> Get<T>(string path) {
            var httpRequest = PrepareGet(path);
            return await DoRequest(httpRequest, JsonConvert.DeserializeObject<T>);
        }

        private HttpRequestMessage PrepareGet(string uri) {
            var request = new HttpRequestMessageBuilder()
                    .WithUri(uri)
                    .WithMethod(HttpMethod.Get)
                    .WithLanguageCulture(CultureInfo.CurrentCulture.ToString())
                    .CreateHttpRequestMessage();
            return request;
        }

        private async Task<T> DoRequest<T>(HttpRequestMessage httpRequest, Func<string, T> deserialize) {
            try {
                using (var client = new HttpClient {BaseAddress = new Uri(baseAddress)}) {
                    var response = await client.SendAsync(httpRequest).ConfigureAwait(false);
                    var responseResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (response.IsSuccessStatusCode) {
                        return deserialize(responseResult);
                    }
                    throw new HttpResponseException(response.StatusCode);
                }
            } catch (HttpRequestException ex) {
                throw new UnexpectedResponseException("RequestException", ex);
            }
        }
    }

    public class HttpRequestMessageBuilder {

        private HttpMethod method;
        private string uri;
        private string culture;
        private HttpContent content;

        public HttpRequestMessageBuilder WithMethod(HttpMethod method) {
            this.method = method ?? HttpMethod.Get;
            return this;
        }

        public HttpRequestMessageBuilder WithUri(string uri) {
            this.uri = uri;
            return this;
        }

        public HttpRequestMessageBuilder WithLanguageCulture(string culture) {
            this.culture = culture;

            return this;
        }

        public HttpRequestMessageBuilder WithContent(HttpContent content) {
            this.content = content;
            return this;
        }

        public HttpRequestMessage CreateHttpRequestMessage() {
            var httpRequestMessage = new HttpRequestMessage(method, uri);
            if (!String.IsNullOrWhiteSpace(culture)) {
                httpRequestMessage.Headers.Add("Accept-Language", culture);
            }
            httpRequestMessage.Content = content;
            return httpRequestMessage;
        }
    }

    public class ServerErrorResponse {
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public string StackTrace { get; set; }
    }

    public class UnexpectedResponseException : Exception {
        public string ErrorCode { get; private set; }
        public string ServerStackTrace { get; private set; }
        private readonly string serverErrorMessage = string.Empty;

        public override string Message {
            get {
                if (string.IsNullOrWhiteSpace(serverErrorMessage)) return base.Message; 
                return base.Message + ": " + serverErrorMessage;
            }
        }

        public UnexpectedResponseException(string responseResult)
            : base(responseResult) {
            ErrorCode = String.Empty;
        }

        public UnexpectedResponseException(string msg, Exception innerException): base(msg, innerException){
        }

        public UnexpectedResponseException(ServerErrorResponse serverErrorResponse)
            : base("ServerErrorResponse"){
            if (serverErrorResponse != null){
                serverErrorMessage = serverErrorResponse.Message;
                ErrorCode = serverErrorResponse.ErrorCode;
                ServerStackTrace = serverErrorResponse.StackTrace;
            }
            else{
                serverErrorMessage = "Null response";
            }
        }

        public bool IsNotFound() {
            return "404".Equals(ErrorCode);
        }

        public bool ContainsErrorCode(string code) {
            return !string.IsNullOrEmpty(ErrorCode) && ErrorCode.Contains(code);
        }
    }
}

