using Yamaha_App.Models.Interfaces;

namespace Yamaha_App.Models.Http.Requests
{
    public class CheckVersionDataRequest : IRequest
    {
        public string Version { get; set; }
    }
}
