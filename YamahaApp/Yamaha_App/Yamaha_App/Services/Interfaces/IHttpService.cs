using System.Threading.Tasks;

namespace Yamaha_App.Services.Interfaces
{
    public interface IHttpService
    {
        Task<TResponse> Get<TRequest, TResponse>(string url, TRequest request);
        Task<TResponse> Post<TRequest, TResponse>(string url, TRequest request);
        Task<TResponse> Delete<TRequest, TResponse>(string url, TRequest request);
    }
}
