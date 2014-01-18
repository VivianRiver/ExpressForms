using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.Reflection;
using System.Reflection.Emit;

namespace ExpressForms.Filters
{
    /// <summary>
    /// An implementation of ExpressFormsFilter that matches all records.
    /// This is meant to be used where there is no logical filtering to be done.
    /// </summary>
    public class ExpressFormsPassThruFilter : ExpressFormsFilter
    {
        public ExpressFormsPassThruFilter()
            : base()
        {
            PartialViewName = "ExpressFormsFilters/NoFilterAvailable";
        }

        public ExpressFormsPassThruFilter(ExpressFormsFilter filter)
            : base(filter)
        {
            PartialViewName = "ExpressFormsFilters/NoFilterAvailable";
        }

        public string PartialViewName { get; set; }

        public override System.Web.Mvc.MvcHtmlString WriteFilter(System.Web.Mvc.HtmlHelper helper, object htmlAttributes)
        {
            IDictionary<string, object> efHtmlAttributes = new RouteValueDictionary(htmlAttributes);
            AddCommonHtmlAttributes(efHtmlAttributes);

            return helper.Partial(PartialViewName, this);
        }

        // With a pass-thru filter, there is nothing to filter, so these methods do nothing to add to the filter IL.
        public override void GenerateFilterIl(ILGenerator generator, Dictionary<string, string> filterValues, PropertyInfo property) { }        
        public override void GenerateFilterLocalDeclarations(ILGenerator generator, int startingIndex, out int endingIndex)
        {
            endingIndex = startingIndex;
        }
    }
}
