using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ExpressForms.IndexAjaxExtension
{
    public abstract class RecordFilter
    {
        public abstract void Initialize(HttpRequestBase request, IEnumerable<string> propertyNames);

        public abstract IEnumerable<T> GetFilteredRecords<T>(IEnumerable<T> records) where T : class, new();
    }
}
