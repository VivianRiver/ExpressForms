using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace ExpressForms.Inputs
{
    public class ExpressFormsCheckBox : ExpressFormsInput
    {
        public ExpressFormsCheckBox() : base() { }

        public ExpressFormsCheckBox(ExpressFormsInput input) : base(input) { }

        public bool IsChecked { get { return Value == "True"; } set { Value = value.ToString(); } }

        public override MvcHtmlString WriteInput(HtmlHelper helper, object htmlAttributes)
        {
            IDictionary<string, object> efHtmlAttributes = new RouteValueDictionary(htmlAttributes);
            AddCommonHtmlAttributes(efHtmlAttributes);            
            return helper.CheckBox(this.InputName, this.IsChecked, efHtmlAttributes);
        }
    }
}
