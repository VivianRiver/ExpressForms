using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ExpressForms.IndexAjaxExtension
{
    public abstract class RecordPagination
    {
        public abstract void Initialize(HttpRequestBase request);

        /// <summary>
        /// Gets a page of records.
        /// Should be called on the Enumeration returned by RecordFilter.GetFilteredRecords
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filteredRecords"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> GetPageOfRecords<T>(IEnumerable<T> filteredRecords);
    }
}
