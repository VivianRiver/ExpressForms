using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Collections.Specialized;

namespace ExpressForms.Utility
{
    public static class QueryStringUtility
    {
        public static RouteValueDictionary ToRouteValues(this NameValueCollection col, Object obj)
        {
            var values = obj == null ? new RouteValueDictionary() : new RouteValueDictionary(obj);
            if (col != null)
            {
                foreach (string key in col)
                {
                    //values passed in object override those already in collection
                    if (!values.ContainsKey(key)) values[key] = col[key];
                }
            }
            return values;
        }

        public static RouteValueDictionary CurrentQueryStringAsRouteValues
        {
            get
            {
                return System.Web.HttpContext.Current.Request.QueryString.ToRouteValues(null);
            }
        }
    }
}
