using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Mvc;
using ExpressForms.Buttons;

namespace ExpressForms
{
    /// <summary>
    /// Represents a row of data to be printed on an "Index Screen".
    /// Provides an array of strings representing the HTML for each element.
    /// This can be used to either print a table as static content, or send the data in response to an AJAX request.
    /// </summary>    
    public class ExpressFormsIndexRecord
    {        
        public void Initialize<T>(T record, string idPropertyName, string[] propertyNames, IEnumerable<ExpressFormsButton> buttons, Dictionary<string, Func<T, string>> customPropertyDisplay, ControllerContext controllerContext)
        {
            // This class is going to render some HTML directly.  Some folks think that's bad practice, but it's what I'm doing.
            HtmlHelper htmlHelper = new HtmlHelper(new ViewContext(controllerContext, new WebFormView(controllerContext, "whatever"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
            Initialize<T>(record, idPropertyName, propertyNames, buttons, customPropertyDisplay, htmlHelper);
        }

        public void Initialize<T>(T record, string idPropertyName, string[] propertyNames, IEnumerable<ExpressFormsButton> buttons, Dictionary<string, Func<T, string>> customPropertyDisplay, HtmlHelper htmlHelper)
        {            
            this.HtmlHelper = htmlHelper;
            
            // If customPropertyDisplay is null, initialize it to a new dictionary to avoid null-reference exceptions.
            customPropertyDisplay = customPropertyDisplay ?? new Dictionary<string, Func<T, string>>();

            Properties = getProperties<T>(propertyNames);

            // This gets the HTML for the properties of the record to display.
            IEnumerable<string> fieldHtmlList = Properties.Select(p =>
            {
                bool hasCustomDisplay = customPropertyDisplay.Keys.Contains(p.Name);
                if (hasCustomDisplay)
                    return customPropertyDisplay[p.Name].Invoke(record);
                else
                    return HtmlHelper.Encode(p.GetValue(record, null));
            }).ToList();
            
            // If the class has a single valid ID property, use it to initialize [Edit] and [Remove] buttons.
            // If none is found, then skip over this and print no buttons.
            IEnumerable<PropertyInfo> idProperties = Properties
                .Where(p => p.Name == idPropertyName);
            if (idProperties.Count() == 1)
            {
                Id = Convert.ToString(Properties
                    .Single(p => p.Name == idPropertyName)
                    .GetValue(record, null));

                // This gets the HTML for the buttons to display.
                IEnumerable<string> buttonHtmlList = buttons.Select(b =>
                {
                    // This is ugly as sin.
                    // TODO: FIX THIS!
                    if (b.GetType().ToString() == typeof(ExpressFormsModifyDataButton).ToString())
                    {
                        ((ExpressFormsModifyDataButton)(b)).IdForDeletion = Convert.ToInt32(Id);
                    }
                    // If this is an "edit" button , add in the ID
                    if (b.GetType().ToString() == typeof(ExpressFormsEditButton).ToString())
                    {
                        ((ExpressFormsEditButton)(b)).Parameters["Id"] = Id;
                    }

                    return b.WriteButton(HtmlHelper, new object { }).ToString();
                });
                FieldHtml = fieldHtmlList.Concat(buttonHtmlList);
            }
            else
            {
                IEnumerable<string> buttonHtmlList = buttons.Select(b => " ");
                FieldHtml = fieldHtmlList.Concat(buttonHtmlList);
            }
        }

        private PropertyInfo[] getProperties<T>(string[] propertyNames)
        {
            // TODO: Make this ignore properties, as appropriate.
            return typeof(T).GetProperties()
                .Where(p => propertyNames.Contains(p.Name))
                .ToArray();
        }

        private PropertyInfo[] Properties { get; set; }

        /// <summary>
        /// The string representation of a unique identifier for the record.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// An array of strings representing the HTML to print for each column for this record.
        /// </summary>
        public IEnumerable<string> FieldHtml { get; set; }

        private HtmlHelper HtmlHelper { get; set; }
    }
}
