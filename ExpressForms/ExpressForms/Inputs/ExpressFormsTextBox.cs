using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace ExpressForms.Inputs
{
    public class ExpressFormsTextBox : ExpressFormsInput
    {
        public ExpressFormsTextBox() : base() { }

        public ExpressFormsTextBox(ExpressFormsInput input) : base(input) { }

        public override MvcHtmlString WriteInput(HtmlHelper helper, object htmlAttributes)
        {
            IDictionary<string, object> efHtmlAttributes = new RouteValueDictionary(htmlAttributes);
            AddCommonHtmlAttributes(efHtmlAttributes);
            return helper.TextBox(this.InputName, this.Value, efHtmlAttributes);
        }
    }
}
