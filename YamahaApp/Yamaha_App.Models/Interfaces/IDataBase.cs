using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yamaha_App.Models.Interfaces
{
    public interface IDataBase<TModel> : IBaseModel
    {
        Task Initialize();

        Task<IList<TModel>> GetAll();

        Task<int> Insert(TModel model);

        Task Delete(TModel model);
    }
}
