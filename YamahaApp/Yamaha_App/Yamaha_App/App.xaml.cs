using Prism;
using Prism.Ioc;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yamaha_App.Configurations;
using Yamaha_App.Database;
using Yamaha_App.Models;
using Yamaha_App.Models.Http.Requests;
using Yamaha_App.Models.Http.Responses;
using Yamaha_App.Services;
using Yamaha_App.Services.Interfaces;
using Yamaha_App.ViewModels;
using Yamaha_App.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Yamaha_App
{
    public partial class App
    {
        private static DatabaseSQLite<ProductModel> _databaseProduct;
        private static DatabaseSQLite<SyncronizeDataModel> _databaseSyncronizeData;

        public static DatabaseSQLite<ProductModel> DatabaseProduct
        {
            get
            {
                if (_databaseProduct == null)
                    _databaseProduct = new DatabaseSQLite<ProductModel>();
                return _databaseProduct;
            }
        }

        public static DatabaseSQLite<SyncronizeDataModel> DatabaseSyncronizeData
        {
            get
            {
                if (_databaseSyncronizeData == null)
                    _databaseSyncronizeData = new DatabaseSQLite<SyncronizeDataModel>();
                return _databaseSyncronizeData;
            }
        }

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/ProductPage");

            // Inicializa a database.
            await DatabaseProduct.Initialize();
            await DatabaseSyncronizeData.Initialize();

            // Declara as instancias
            var syncService = Container.Resolve<ISyncService>();
            var httpService = Container.Resolve<IHttpService>();
            var productService = Container.Resolve<IProductService>();

            try
            {
                // Se a versão for nula, é a primeira vez que ele entra no app.
                // Então vai sincronizar o server com o aplicativo, trazendo os dados que já existem la.
                var dataSync = await DatabaseSyncronizeData.GetAll();
                if (dataSync.Count == 0 || dataSync.Last().Version == null)
                {
                    var productResponse = await httpService.Get<ProductGetRequest, ProductGetResponse>(HttpConfiguration.URL_PRODUCT, new ProductGetRequest());
                    // Grava a versão do server.
                    await syncService.SyncDataChanged(false, productResponse.Version);
                    // Grava todos os produtos novos na base.
                    productResponse.Products.ToList().ForEach(product => DatabaseProduct.Insert(product));
                }

                // Cria o evento de internet e sincroniza a database local com a do server
                await syncService.CreateEventInternetChanged();
                await syncService.UpdateDatabaseLocalToServer();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IProductService, ProductService>();
            containerRegistry.Register<ISyncService, SyncService>();
            containerRegistry.Register<IHttpService, HttpService>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ProductPage, ProductPageViewModel>();
        }
    }
}
