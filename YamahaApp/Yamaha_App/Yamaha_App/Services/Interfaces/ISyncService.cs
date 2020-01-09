using System.Threading.Tasks;

namespace Yamaha_App.Services.Interfaces
{
    public interface ISyncService
    {
        Task Initialize();
        Task UpdateDatabaseLocalToServer();
        Task<bool> SyncNeeded();
    }
}
