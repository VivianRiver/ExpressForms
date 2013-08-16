using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ExpressForms.IndexAjaxExtension;

namespace ExpressForms.Extensions.jquery.dataTables
{
    public class DataTablesRecordPagination : RecordPagination
    {
        public int StartIndex { get; set; }
        public int HowMany { get; set; }

        public DataTablesRecordPagination() { }

        public override void Initialize(HttpRequestBase request)
        {
            StartIndex = Convert.ToInt32(request.QueryString["iDisplayStart"]);
            HowMany = Convert.ToInt32(request.QueryString["iDisplayLength"]);
        }

        public override IEnumerable<T> GetPageOfRecords<T>(IEnumerable<T> filteredRecords)
        {
            return filteredRecords
                .Skip(StartIndex)
                .Take(HowMany);
        }
    }
}
