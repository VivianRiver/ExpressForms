using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace ExpressForms.Inputs
{
    public class ExpressFormsListBox : ExpressFormsInput
    {
        public ExpressFormsListBox() : base() { }

        public ExpressFormsListBox(ExpressFormsInput input) : base(input) { }

        public IEnumerable<SelectListItem> SelectListItems { get; set; }

        public override MvcHtmlString WriteInput(HtmlHelper helper, object htmlAttributes)
        {
            IDictionary<string, object> efHtmlAttributes = new RouteValueDictionary(htmlAttributes);
            AddCommonHtmlAttributes(efHtmlAttributes);

            // This is an ugly hack.  For some reason, when inputName is used here, the Select attributes don't render and the selection is lost.
            // See here for possible explanation: http://stackoverflow.com/questions/3737985/asp-net-mvc-multiselectlist-with-selected-values-not-selecting-properly            
            return helper.ListBox(this.InputName + "01", this.SelectListItems, efHtmlAttributes);
        }
    }
}
