using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using ExpressForms;
using ExpressForms.IndexAjaxExtension;

namespace ExpressForms.Extensions.jquery.dataTables
{
    public class ExpressFormsDataTablesIndexAjaxExtension : ExpressFormsIndexAjaxExtension
    {
        protected override void SetupFilter(ControllerContext ctx, IEnumerable<string> propertyNames)
        {
            RecordFilter = new DataTablesFilter();
            RecordFilter.Initialize(ctx.HttpContext.Request, propertyNames);
        }

        protected override void SetupSorting(ControllerContext ctx, IEnumerable<string> propertyNames)
        {
            RecordSorting = new DataTablesSorting();
            RecordSorting.Initialize(ctx.HttpContext.Request, propertyNames);
        }

        protected override void SetupPagination(ControllerContext ctx)
        {
            RecordPagination = new DataTablesRecordPagination();
            RecordPagination.Initialize(ctx.HttpContext.Request);
        }        
    }
}
