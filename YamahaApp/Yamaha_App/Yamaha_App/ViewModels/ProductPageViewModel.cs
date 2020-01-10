using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using Yamaha_App.Models;
using Yamaha_App.Services.Interfaces;

namespace Yamaha_App.ViewModels
{
    public class ProductPageViewModel : ViewModelBase
    {
        private readonly IProductService _productService;
        private readonly ISyncService _syncService;

        public ObservableCollection<ProductModel> Product { get; set; } = new ObservableCollection<ProductModel>();

        public DelegateCommand AddProduct { get; private set; }

        public DelegateCommand<ProductModel> RemoveProduct { get; private set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public ProductPageViewModel(
            INavigationService navigationService,
            IProductService productService,
            ISyncService syncService
        ) : base(navigationService)
        {
            Title = "Interview Yamaha";

            _productService = productService;
            _syncService = syncService;

            _productService.GetAll().ContinueWith((Task<IList<ProductModel>> products) => {
                var result = products.Result;
                result.ForEach((ProductModel product) => Product.Add(product));
            });

            AddProduct = new DelegateCommand(async () =>
            {
                var product = new ProductModel
                {
                    Name = Name
                };
                // Adiciona na database
                int idLocal = await _productService.Add(product);
                product.IdLocal = idLocal;
                // Adiciona na lista
                Product.Add(product);
                Name = string.Empty;
                // Se houve uma alteração, ele deixa a versao da sincronização zerada, para forçar ele a sincronizar na proxima
                await _syncService.SyncDataChanged(true);
                await _syncService.UpdateDatabaseLocalToServer();
            });

            RemoveProduct = new DelegateCommand<ProductModel>(async (ProductModel product) =>
            {
                // Remove da database local
                await _productService.Remove(product);
                // Remove da lista de produtos
                Product.Remove(product);
                // Se houve uma alteração, ele deixa a versao da sincronização zerada, para forçar ele a sincronizar na proxima
                await _syncService.SyncDataChanged(true);
                await _syncService.UpdateDatabaseLocalToServer();
            });
        }
    }
}
