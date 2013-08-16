using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ExpressForms;

namespace ExpressForms.IndexAjaxExtension
{
    public abstract class ExpressFormsIndexAjaxExtension
    {
        public IEnumerable<string> PropertyNames { get; set; }

        public RecordFilter RecordFilter { get; protected set; }
        public RecordSorting RecordSorting {get; protected set; }
        public RecordPagination RecordPagination { get; protected set; }

        protected ControllerContext ControllerContext { get; set; }

        public void Initialize(ControllerContext ctx, IEnumerable<string> propertyNames)
        {
            PropertyNames = propertyNames;
            ControllerContext = ctx;            
            SetupFilter(ctx, propertyNames);
            SetupSorting(ctx, PropertyNames);
            SetupPagination(ctx);
        }

        protected abstract void SetupFilter(ControllerContext ctx, IEnumerable<string> propertyNames);
        protected abstract void SetupSorting(ControllerContext ctx, IEnumerable<string> propertyNames);
        protected abstract void SetupPagination(ControllerContext ctx);
    }
}
