using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ExpressForms.Filters;

namespace ExpressForms.IndexAjaxExtension
{
    public abstract class RecordFilter
    {
        public abstract void Initialize(HttpRequestBase request, IEnumerable<string> propertyNames);

        public abstract IEnumerable<T> GetFilteredRecords<T>(IEnumerable<T> records, Dictionary<string, ExpressFormsFilter> filters, Dictionary<string, Dictionary<string, string>> filterEntries) where T : class, new();
    }
}
