using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RestHttpClient
{
    public abstract class RestHttpClient
    {
        private readonly Uri _uri;
        private readonly string _mediaType;
        private readonly string _username;
        private readonly string _password;
        private readonly string _authentication;

        private const string DefaultAuthentication = "Basic";
        private const string DefaultMedia = "application/xml";
        private HttpClient _client;

        protected RestHttpClient(Uri uri, string username, string password, string mediaType = DefaultMedia, string authentication = DefaultAuthentication)
        {
            _uri = uri;
            _username = username;
            _password = password;
            _mediaType = mediaType;
            _authentication = authentication;
        }

        async Task RunAsync()
        {
            await Task.Delay(100);
            _client = new HttpClient(new HttpClientHandler
            {
                UseDefaultCredentials = false,
                Credentials = new CredentialCache
                {
                    {
                        _uri,
                        _authentication,
                        new NetworkCredential(_username, _password)
                    }
                }
            })
            {BaseAddress = _uri};


            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
        }


        private async Task<HttpClient> GetClient()
        {
            if (_client == null)
            {
                await RunAsync();
            }
            return _client;
        }

        public async Task<T> GetAsync<T>(string path)
        {
            var item = default(T);

            await GetClient(); // Don't dispose this: http://stackoverflow.com/questions/15705092/do-httpclient-and-httpclienthandler-have-to-be-disposed

            var response = await _client.GetAsync(_uri + path);

            if (response.IsSuccessStatusCode)
            {
                var buffer = await response.Content.ReadAsByteArrayAsync();
                using (var stream = new MemoryStream(buffer))
                {
                    var serializer = new XmlSerializer(typeof (T));
                    item = (T) serializer.Deserialize(stream);
                }
            }
            return item;
        }
    }
}