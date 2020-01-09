using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yamaha_App.Database.Extensions;
using Yamaha_App.Models.Interfaces;

namespace Yamaha_App.Database
{
    public class DatabaseSQLite<TModel> : IDataBase<TModel> where TModel : IModelBase, new ()
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(DatabaseConfiguration.DatabasePath, DatabaseConfiguration.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public DatabaseSQLite()
        {
            Initialize().SafeFireAndForget(false);
        }

        public async Task Initialize()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(TModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(TModel)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

        public Task Delete(TModel model)
        {
            return Database.DeleteAsync(model);
        }

        public async Task<IList<TModel>> GetAll()
        {
            return await Database.Table<TModel>().ToListAsync();
        }

        public Task<int> Insert(TModel model)
        {
            return Database.InsertAsync(model);
        }
    }
}
