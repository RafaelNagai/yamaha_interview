using System.Collections.Generic;
using System.Threading.Tasks;
using Yamaha_App.Models;

namespace Yamaha_App.Services.Interfaces
{
    public interface IProductService
    {
        Task<IList<ProductModel>> GetAll();

        Task<int> Add(ProductModel model);

        Task Remove(ProductModel model);
    }
}
