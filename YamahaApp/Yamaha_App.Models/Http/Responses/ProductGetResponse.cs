using System.Collections;
using System.Collections.Generic;
using Yamaha_App.Models.Interfaces;

namespace Yamaha_App.Models.Http.Responses
{
    public class ProductGetResponse : IResponse
    {
        public IList<ProductModel> Products { get; set; }

        public string Version { get; set; }
    }
}
