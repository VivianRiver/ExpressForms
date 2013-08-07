using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using System.Web.Mvc.Html;
using System.Web.Routing;
using ExpressForms.Inputs;
using ExpressForms.Buttons;

//namespace ExpressForms.HtmlHelpers
namespace System.Web.Mvc
{

    public static class EfHtmlHelperExtensions
    {
        #region methods that print forms
        public static MvcHtmlString EfLabel(this HtmlHelper helper, string propertyName, string displayName)
        {
            return helper.Label(propertyName, displayName);            
        }

        public static MvcHtmlString EfInput(this HtmlHelper helper, ExpressFormsInput input, object htmlAttributes)
        {
            return input.WriteInput(helper, htmlAttributes);
        }
        
        #endregion

        #region methods that print lists of records        

        public static MvcHtmlString WriteList(this HtmlHelper helper, string tableName, ExpressFormsButton[] buttons, IEnumerable<object> records, Dictionary<string,string> customColumnHeaders, Dictionary<string, Func<object, string>> columnMethods)
        {
            // Discover the properties of the records passed in and pass the properties to the appropriate WriteList overload.
            Type recordType = DiscoverRecordType(records);
            PropertyInfo[] properties = DiscoverProperties(recordType);                                    

            // Go thru the properties and use Convert.ToString() for the ones that don't have a method speficied. assume that each property is to be printed using Convert.ToString().
            if (columnMethods == null)
                columnMethods = new Dictionary<string, Func<object, string>>();
            foreach (PropertyInfo property in properties)
            {
                string name = property.Name;

                if (!columnMethods.Keys.Contains(property.Name))
                {
                    Func<object, string> defaultEvalFunction = getDefaultEvalFunction(helper, property);
                    columnMethods.Add(name, defaultEvalFunction);
                }                                                                         
            }

            return WriteList(helper, tableName, buttons, records, properties, customColumnHeaders, columnMethods);
        }

        public static MvcHtmlString WriteList(this HtmlHelper helper, string tableName, ExpressFormsButton[] buttons, IEnumerable<object> records)
        {           
            // Discover the properties of the records passed in and pass the properties to the appropriate WriteList overload.
            Type recordType = DiscoverRecordType(records);
            PropertyInfo[] properties = DiscoverProperties(recordType);

            // When no columnMethods object is passed in, assume that each property is to be printed using HTML.Encode.
            // Note that the evalFunction returns the raw HTML to print, so if the output from a function passed in isn't encoded, XSS attacks can happen.
            var columnMethods = new Dictionary<string, Func<object, string>>();
            foreach (PropertyInfo property in properties)
            {
                string name = property.Name;
                Func<object, string> defaultEvalFunction = getDefaultEvalFunction(helper, property);
                columnMethods.Add(name, defaultEvalFunction);
            }                        

            return WriteList(helper, tableName, buttons, records, properties, null, columnMethods);
        }

        private static Func<object, string> getDefaultEvalFunction(HtmlHelper helper, PropertyInfo property)
        {
            return (object record) => helper.Encode(record.GetType().GetProperty(property.Name).GetValue(record, null));
        }

        private static MvcHtmlString WriteList(this HtmlHelper helper, string tableName, ExpressFormsButton[] buttons, IEnumerable<object> records, PropertyInfo[] properties, Dictionary<string,string> customColumnHeaders, Dictionary<string, Func<object, string>> columnMethods)
        {
            // TODO: Make this generic so that the data can be formatted in structures besides tables.

            const string headerFormat = "<th>{0}</th>";
            const string dataFormat = "<td>{0}</td>";

            StringBuilder sb = new StringBuilder();            
            

            sb.AppendLine(string.Format("<table data-formname='{0}'>", helper.Encode(tableName)));
            sb.AppendLine("<thead><tr>");
            foreach (PropertyInfo property in properties)
            // Write column headers for each property
            {
                // If we have a custom header for this property, then use it.  Otherwise, just use the property name as the header
                string headerName = (customColumnHeaders != null && customColumnHeaders.Keys.Contains(property.Name)) ? customColumnHeaders[property.Name] : property.Name;

                if (property.Name != "Id")
                    sb.Append(string.Format(headerFormat, helper.Encode(headerName)));
            }

            foreach (ExpressForms.Buttons.ExpressFormsButton button in buttons)
            // Write column headers for each button
            {
                string headerName = button.Text;
                sb.Append(string.Format(headerFormat, helper.Encode(button.Text)));
            }

            sb.AppendLine("</tr></thead><tbody>");
            foreach (object record in records)
            {
                Dictionary<string, string> propertyDictionary = new Dictionary<string, string>();

                foreach(PropertyInfo property in properties)
                {
                    string columnName = property.Name;
                    string html = columnMethods[columnName].Invoke(record);
                    propertyDictionary[columnName] = html;
                }
                
                sb.AppendLine(string.Format("<tr data-rowid='{0}'>", helper.Encode(propertyDictionary["Id"])));
                foreach (string key in propertyDictionary.Keys)
                {
                    string html = propertyDictionary[key];
                    if (key != "Id")
                        sb.Append(string.Format(dataFormat, html));
                }

                foreach (ExpressFormsButton button in buttons)
                {
                    // This is a hack.  TODO: make this better.
                    // If this is a delete button, add in the ID here.
                    if (button.GetType().ToString() == typeof(ExpressFormsModifyDataButton).ToString())
                    {
                        ((ExpressFormsModifyDataButton)(button)).IdForDeletion = Convert.ToInt32(propertyDictionary["Id"]);
                    }
                    // If this is an "edit" button , add in the ID
                    if (button.GetType().ToString() == typeof(ExpressFormsEditButton).ToString())
                    {
                        ((ExpressFormsEditButton)(button)).Parameters["Id"] = (propertyDictionary["Id"]);
                    }

                    //sb.Append(string.Format(dataFormat, helper.WriteButton(tableName, button)));
                    sb.Append(string.Format(dataFormat, helper.WriteButton(button, new { })));
                }

                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</tbody></table>");

            return new MvcHtmlString(sb.ToString());
        } // end WriteList

        #endregion

        #region methods that print buttons
        public static MvcHtmlString WriteButton(this HtmlHelper helper, ExpressForms.Buttons.ExpressFormsButton button, object htmlAttributes)
        {            
            return button.WriteButton(helper, htmlAttributes);
        }                             
        #endregion

        #region reflection helper methods
        private static Type DiscoverRecordType(IEnumerable<object> records)
        {
            Type type = records.GetType().IsArray ? records.GetType().GetElementType() : records.GetType().GetGenericArguments().Single();
            return type;
        }

        private static PropertyInfo[] DiscoverProperties(Type type)
        {
            PropertyInfo[] properties = type.GetProperties()
             .Where(p => !ExpressForms.Common.PropertiesToIgnore.Contains(p.Name))
             .ToArray();
            return properties;
        }
        #endregion
    }
}