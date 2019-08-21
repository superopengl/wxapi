using System;
using System.Threading.Tasks;

namespace wxapi.Services
{
    public interface IRestClient : IDisposable
    {
        Task<T> GetAsync<T>(string url);
    }
}
