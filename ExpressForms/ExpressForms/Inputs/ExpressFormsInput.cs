using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace ExpressForms.Inputs
{
    /// <summary>
    /// Represents a single input that the end-user will use to fill a form.
    /// In order to implement new input types, this class should be inherited.
    /// </summary>        
    public abstract class ExpressFormsInput
    {        
        public ExpressFormsInput()
        {
            // Inputs should be visible by default.
            IsVisible = true;            
        }

        public ExpressFormsInput(ExpressFormsInput input)
        {
            FormName = input.FormName;
            InputName = input.InputName;
            Value = input.Value;
            IsVisible = input.IsVisible;
        }

        /// <summary>
        /// the name of the form that this input belongs to.
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// the name of this input to use on the form
        /// </summary>
        public string InputName { get; set; }

        /// <summary>
        /// the name to display for this input (not necessarily the same as the name of this input)
        /// </summary>
        public string InputDisplayName { get; set; }

        /// <summary>
        /// The value to display on the form before HTML encoding.
        /// </summary>
        public string Value { get; set; }        

        public bool IsVisible { get; set; }


        protected string GetInputTypeName()
        {
            return this.GetType().Name;
        }

        /// <summary>
        /// Adds to the HTML attributes the common attributes used by all form inputs regardless of type.
        /// This should be called from within the WriteInput implementation in all inherited classes.
        /// </summary>
        /// <param name="htmlAttributes"></param>
        protected void AddCommonHtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("data-name", this.InputName);
            htmlAttributes.Add("data-inputname", this.InputName);
            htmlAttributes.Add("data-formname", this.FormName);
            htmlAttributes.Add("data-inputtype", this.GetInputTypeName());
            htmlAttributes.Add("class", "ExpressForms");
        }

        /// <summary>
        /// gets the HTML of this input to put in an HTML document
        /// </summary>        
        public abstract MvcHtmlString WriteInput(HtmlHelper helper, object htmlAttributes);
    }                
}
