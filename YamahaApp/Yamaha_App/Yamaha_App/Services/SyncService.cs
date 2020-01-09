using System;
using System.Linq;
using System.Threading.Tasks;
using Yamaha_App.Models;
using Yamaha_App.Services.Interfaces;
using Yamaha_App.Models.Http.Requests;
using Yamaha_App.Models.Http.Responses;
using Xamarin.Essentials;

namespace Yamaha_App.Services
{
    public class SyncService : ISyncService
    {
        private HttpService _httpService;

        public SyncService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task Initialize()
        {
            Connectivity.ConnectivityChanged += async (object sender, ConnectivityChangedEventArgs e) =>
            {
                var access = e.NetworkAccess;

                if (access == NetworkAccess.Internet)
                {
                    await UpdateDatabaseLocalToServer();
                    await App.Current.MainPage.DisplayAlert("Aviso", "Tem Internet", "OK");
                }
            };
        }

        public async Task UpdateDatabaseLocalToServer()
        {
            // Consulta a versao sincronizada para verificar se a necessidade para sincronizar
            if(await SyncNeeded())
            {
                // Consulta a base local
                var products = await App.DatabaseProduct.GetAll();

                // Salvar os dados da base local na base da API
                var request = new SyncDataRequest
                {
                    Products = products
                };
                var syncResult = await _httpService.Post<SyncDataRequest, SyncDataResponse>(request);

                // Salva a versão da base de dados sincronizada com o server
                var syncData = new SyncronizeDataModel();
                syncData.Version = syncResult.Version;
                await App.DatabaseSyncronizeData.Insert(syncData);
            }
        }

        public async Task<bool> SyncNeeded()
        {
            // Pega a versão atual da base local
            var syncData = await App.DatabaseSyncronizeData.GetAll();

            var request = new CheckVersionDataRequest
            {
                Version = syncData.Last().Version
            };
            // Consulta na api se é a mesma versão que se encontra na base local e base da API
            var result = await _httpService.Get<CheckVersionDataRequest, CheckVersionDataResponse>(request);

            return !result.SameVersion;
        }
    }
}