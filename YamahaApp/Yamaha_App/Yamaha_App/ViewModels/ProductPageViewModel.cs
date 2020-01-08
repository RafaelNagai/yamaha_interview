using Prism.Navigation;
using System.Collections.ObjectModel;
using Yamaha_App.Models;

namespace Yamaha_App.ViewModels
{
    public class ProductPageViewModel : ViewModelBase
    {
        public ObservableCollection<ProductModel> Product { get; set; }

        public ProductPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Interview Yamaha";
            Product = new ObservableCollection<ProductModel>
            {
                new ProductModel{ Name = "Rafael Kenji Nagai" },
                new ProductModel{ Name = "Lyan Masterson" },
                new ProductModel{ Name = "Larissa Machado Fernandes" },
            };
        }
    }
}
