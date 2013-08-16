using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace ExpressForms.IndexAjaxExtension
{
    public abstract class RecordSorting
    {
        public abstract void Initialize(HttpRequestBase request, IEnumerable<string> propertyNames);

        /// <summary>
        /// Gets sorted records.
        /// Should be called on the Enumeration returned by RecordFilter.GetFilteredRecords
        /// and then passed in to RecordPagination.GetPageOfRecords
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filteredRecords"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> GetSortedRecords<T>(IEnumerable<T> filteredRecords) where T : class, new();
    }
}
