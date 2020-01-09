using System.Threading.Tasks;

namespace Yamaha_App.Services.Interfaces
{
    public interface IHttpService
    {
        Task<TResponse> Get<TRequest, TResponse>(TRequest request);
        Task<TResponse> Post<TRequest, TResponse>(TRequest request);
        Task<TResponse> Delete<TRequest, TResponse>(TRequest request);
    }
}
