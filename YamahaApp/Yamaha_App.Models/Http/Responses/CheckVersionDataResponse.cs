using Yamaha_App.Models.Interfaces;

namespace Yamaha_App.Models.Http.Responses
{
    public class CheckVersionDataResponse : IResponse
    {
        public bool SameVersion { get; set; }
    }
}
