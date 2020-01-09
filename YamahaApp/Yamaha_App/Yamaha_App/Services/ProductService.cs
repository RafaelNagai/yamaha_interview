using System.Collections.Generic;
using System.Threading.Tasks;
using Yamaha_App.Models;
using Yamaha_App.Services.Interfaces;

namespace Yamaha_App.Services
{
    public class ProductService : IProductService
    {
        public Task<int> Add(ProductModel model)
        {
            return App.DatabaseProduct.Insert(model);
        }

        public Task<IList<ProductModel>> GetAll()
        {
            return App.DatabaseProduct.GetAll();
        }

        public Task Remove(ProductModel model)
        {
            return App.DatabaseProduct.Delete(model);
        }
    }
}
