using System.Collections.Generic;
using Yamaha_App.Models.Interfaces;

namespace Yamaha_App.Models.Http.Requests
{
    public class ProductPostRequest : IRequest
    {
        public IList<ProductModel> Products { get; set; }
    }
}
