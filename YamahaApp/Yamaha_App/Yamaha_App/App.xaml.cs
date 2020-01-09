using Prism;
using Prism.Ioc;
using Yamaha_App.ViewModels;
using Yamaha_App.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yamaha_App.Database;
using Yamaha_App.Models;
using Yamaha_App.Services.Interfaces;
using Yamaha_App.Services;
using Xamarin.Essentials;

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

            //Inicializa a database
            await DatabaseProduct.Initialize();

            await NavigationService.NavigateAsync("NavigationPage/ProductPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IProductService, ProductService>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ProductPage, ProductPageViewModel>();
        }
    }
}
