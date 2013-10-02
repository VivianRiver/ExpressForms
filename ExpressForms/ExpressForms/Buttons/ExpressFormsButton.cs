using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ExpressForms.Buttons
{
    public abstract class ExpressFormsButton<T, TId>
    {
        public ExpressFormsButton()
        {
            // IsVisible should be true unless otherwise specified.
            IsVisible = true;            
        }

        /// <summary>
        /// Whether or not the button is initially visible.
        /// If set to false, the button will be printed with style='display:none;'        
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// The text to display on the button
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// A function that is called once per record in the ExpressFormsIndexRecord class.
        /// It adds values to the button object that are specific to each record.
        /// </summary>
        /// <param name="record"></param>
        /// <param name="id"></param>
        public abstract void InitializeWithRecord(T record, TId id);

        /// <summary>
        /// gets the HTML of this button to put in an HTML document
        /// </summary>        
        public abstract MvcHtmlString WriteButton(HtmlHelper helper, object htmlAttributes);
    }
}
