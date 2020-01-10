using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Yamaha_App.Configurations;
using Yamaha_App.Models;
using Yamaha_App.Models.Http.Requests;
using Yamaha_App.Models.Http.Responses;
using Yamaha_App.Services.Interfaces;

namespace Yamaha_App.Services
{
    public class SyncService : ISyncService, IDisposable
    {
        private readonly HttpService _httpService;

        public SyncService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public Task CreateEventInternetChanged()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged; ;

            return Task.FromResult<object>(null);
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;

            if (access == NetworkAccess.Internet)
            {
                await UpdateDatabaseLocalToServer();
                await App.Current.MainPage.DisplayAlert("Aviso", "Sincronização feita com sucesso.", "OK");
            }
        }

        public async Task UpdateDatabaseLocalToServer()
        {
            try
            {
                var dataSync = await App.DatabaseSyncronizeData.GetAll();
                bool hasChanged = dataSync.Last()?.HasChanged ?? true;

                // Consulta a versao sincronizada para verificar se a necessidade para sincronizar
                if (await SyncNeeded() || hasChanged)
                {
                    // Consulta a base local
                    var products = await App.DatabaseProduct.GetAll();

                    // Salvar os dados da base local na base da API
                    var request = new ProductPostRequest
                    {
                        Products = products
                    };
                    var productResult = await _httpService.Post<ProductPostRequest, ProductPostResponse>(HttpConfiguration.URL_PRODUCT, request);

                    // Salva a versão da base de dados sincronizada com o server
                    await SyncDataChanged(false, productResult.Version);

                    await App.Current.MainPage.DisplayAlert("Sincronização", "Sincronizado com sucesso.", "OK");
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Aviso", "Salvo com sucesso, será feita a sincronização quando tiver internet.", "OK");
            }
        }

        public async Task<bool> SyncNeeded()
        {
            // Pega a versão atual da base local
            var syncData = await App.DatabaseSyncronizeData.GetAll();

            var request = new CheckVersionDataRequest
            {
                Version = syncData.Last()?.Version ?? null
            };
            // Consulta na api se é a mesma versão que se encontra na base local e base da API
            var result = await _httpService.Get<CheckVersionDataRequest, CheckVersionDataResponse>(HttpConfiguration.URL_SYNCDATA, request);

            return !result.SameVersion;
        }

        public async Task SyncDataChanged(bool hasChanged, string version = null)
        {
            // Ele deleta e gera novamente, porque so tera 1 registro de SyncData na base local.
            var syncDataTemp = new SyncronizeDataModel();
            var syncData = await App.DatabaseSyncronizeData.GetAll();
            if (syncData.Count != 0 && syncData.Last().Version != null)
            {
                syncDataTemp = syncData.Last();
                await App.DatabaseSyncronizeData.Delete(syncData.Last());
            }

            if (version != null)
                syncDataTemp.Version = version;
            syncDataTemp.HasChanged = hasChanged;
            await App.DatabaseSyncronizeData.Insert(syncDataTemp);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}