using System.Threading.Tasks;

namespace RestHttpClient.Interface
{
    public interface IMySiteHttpClient
    {
        Task<T> GetSomeData<T>(string path);
    }
}
