using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using ExpressForms;
using ExpressForms.Buttons;

namespace ExpressForms
{
    public class ExpressFormsIndexHeader
    {
        public void Initialize<T, TId>(string[] propertyNames, IEnumerable<ExpressFormsButton<T, TId>> buttons, ControllerContext controllerContext)
        {
            // This class is going to render some HTML directly.  Some folks think that's bad practice, but it's what I'm doing.
            HtmlHelper htmlHelper = new HtmlHelper(new ViewContext(controllerContext, new WebFormView(controllerContext, "whatever"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
            Initialize(propertyNames, buttons, htmlHelper);
        }

        public void Initialize<T, TId>(string[] propertyNames, IEnumerable<ExpressFormsButton<T, TId>> buttons, HtmlHelper htmlHelper)
        {
            this.HtmlHelper = htmlHelper;            

            // This gets the HTML for the column headers over the data fields
            IEnumerable<string> headerHtmlList = propertyNames.Select(n => htmlHelper.Encode(n));            

            // This gets the HTML for the column headers over the button fields
            IEnumerable<string> buttonHtmlList = buttons.Select(b => htmlHelper.Encode(b.Text));
            
            HeaderHtml = headerHtmlList.Concat(buttonHtmlList);
        }                

        /// <summary>
        /// An array of strings representing the HTML to print for each column header
        /// </summary>
        public IEnumerable<string> HeaderHtml { get; set; }

        private HtmlHelper HtmlHelper { get; set; }
    }
}
