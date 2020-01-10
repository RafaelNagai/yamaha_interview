using SQLite;
using Yamaha_App.Models.Interfaces;

namespace Yamaha_App.Models
{
    public class SyncronizeDataModel : IBaseModel
    {
        [PrimaryKey]
        public string Version { get; set; }

        public bool HasChanged { get; set; }
    }
}
