using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Yamaha_App.Extensions;
using Yamaha_App.Services.Interfaces;

namespace Yamaha_App.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _client;

        public HttpService()
        {
            _client = new HttpClient
            {
                MaxResponseContentBufferSize = 256000
            };
        }

        public async Task<TResponse> Delete<TRequest, TResponse>(string url, TRequest request)
        {
            try
            {
                var uri = new Uri(string.Format(url, request.ToUrlQuery()));
                var response = await _client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResponse>(content);
                }
                return default;
            }
            catch(Exception ex)
            {
                //loga o erro
                throw ex;
            }
        }

        public async Task<TResponse> Get<TRequest, TResponse>(string url, TRequest request)
        {
            try
            {
                var uri = new Uri(string.Format(url, string.Empty));
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResponse>(content);
                }
                return default;
            }
            catch (Exception ex)
            {
                //loga o erro
                throw ex;
            }
        }

        public async Task<TResponse> Post<TRequest, TResponse>(string url, TRequest request)
        {
            try
            {
                var uri = new Uri(string.Format(url, request.ToUrlQuery()));

                var jsonRequest = JsonConvert.SerializeObject(request);
                var contentRequest = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(uri, contentRequest);
                if (response.IsSuccessStatusCode)
                {
                    var contentResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResponse>(contentResponse);
                }
                return default;
            }
            catch (Exception ex)
            {
                //loga o erro
                throw ex;
            }
        }
    }
}
