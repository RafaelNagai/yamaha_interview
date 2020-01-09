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

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Yamaha_App
{
    public partial class App
    {
        public static DatabaseSQLite<ProductModel> databaseProduct;
        public static DatabaseSQLite<ProductModel> DatabaseProduct
        {
            get
            {
                if (databaseProduct == null)
                    databaseProduct = new DatabaseSQLite<ProductModel>();
                return databaseProduct;
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
