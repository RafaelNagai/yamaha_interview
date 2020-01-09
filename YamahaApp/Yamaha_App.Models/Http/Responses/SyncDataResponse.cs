using Yamaha_App.Models.Interfaces;

namespace Yamaha_App.Models.Http.Responses
{
    public class SyncDataResponse : IResponse
    {
        public string Version { get; set; }
    }
}
