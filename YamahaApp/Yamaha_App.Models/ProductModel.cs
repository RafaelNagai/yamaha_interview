using SQLite;
using Yamaha_App.Models.Interfaces;

namespace Yamaha_App.Models
{
    public class ProductModel : IModelBase
    {
        public int Id { get; set; }

        [PrimaryKey, AutoIncrement]
        public int IdLocal { get; set; }

        public string Name { get; set; }
    }
}
