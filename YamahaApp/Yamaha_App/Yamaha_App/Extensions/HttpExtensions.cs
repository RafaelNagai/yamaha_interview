using System;
using System.Linq;
using System.Web;

namespace Yamaha_App.Extensions
{
    public static class HttpExtensions
    {
        /// <summary>
        /// Converte o seu objeto em uma string no formato URL query
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToUrlQuery(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
            return String.Concat("?", String.Join("&", properties.ToArray()));
        }
    }
}
