using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace ExpressForms.Inputs
{
    public class ExpressFormsDropDownList : ExpressFormsInput
    {
        public ExpressFormsDropDownList() : base() { }

        public ExpressFormsDropDownList(ExpressFormsInput input) : base(input) { }

        public IEnumerable<SelectListItem> SelectListItems { get; set; }

        public override MvcHtmlString WriteInput(HtmlHelper helper, object htmlAttributes)
        {
            IDictionary<string, object> efHtmlAttributes = new RouteValueDictionary(htmlAttributes);
            AddCommonHtmlAttributes(efHtmlAttributes);
            return helper.DropDownList(this.InputName, this.SelectListItems, efHtmlAttributes);
        }
    }
}
