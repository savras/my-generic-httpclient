using System;
using System.Threading.Tasks;
using RestHttpClient.Interface;

namespace RestHttpClient
{
    public class MySiteHttpClient : RestHttpClient, IMySiteHttpClient
    {
        public MySiteHttpClient(Uri uri, string username, string password, string mediaType = "application/xml", string authentication = "Basic")
            : base(uri, username, password, mediaType, authentication)
        {
        }

        public async Task<T> GetSomeData<T>(string path)
        {
            return await GetAsync<T>(path);
        }
    }
}