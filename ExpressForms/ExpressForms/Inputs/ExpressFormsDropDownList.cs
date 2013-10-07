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

            // Make sure that the select list item matching the Value property is selected before rending the list.
            IEnumerable<SelectListItem> selectListItems = this.SelectListItems.Select(x =>
                new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value,
                    Selected = x.Value == Value
                });

            return helper.DropDownList(this.InputName, selectListItems, efHtmlAttributes);
        }
    }
}
