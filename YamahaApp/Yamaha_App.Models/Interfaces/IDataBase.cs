﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yamaha_App.Models.Interfaces
{
    public interface IDataBase<TModel> : IModelBase
    {
        Task Initialize();

        Task<IList<TModel>> GetAll();

        Task Insert(TModel model);

        Task Delete(TModel model);
    }
}