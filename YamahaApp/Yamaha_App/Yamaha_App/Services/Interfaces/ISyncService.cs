using System.Threading.Tasks;

namespace Yamaha_App.Services.Interfaces
{
    public interface ISyncService
    {
        Task CreateEventInternetChanged();
        Task UpdateDatabaseLocalToServer();
        Task<bool> SyncNeeded();
        Task SyncDataChanged(bool hasChanged, string version = null);
    }
}
