using System.Collections.Generic;
using Yamaha_App.Models.Interfaces;

namespace Yamaha_App.Models.Http.Requests
{
    public class SyncDataRequest : IRequest
    {
        public IList<ProductModel> Products { get; set; }
    }
}
