using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using System.Web.Mvc.Html;
using System.Web.Routing;
using ExpressForms;
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
        public static MvcHtmlString WriteStaticTable(this HtmlHelper helper, string tableName, ExpressFormsIndexHeader indexHeader, IEnumerable<ExpressFormsIndexRecord> indexRecords)
        {
            // TODO: Make this generic so that the data can be formatted in structures besides tables.
            
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<table data-formname='{0}'>", helper.Encode(tableName));
            sb.Append(WriteStaticTableHead(helper, tableName, indexHeader));
            sb.Append(WriteStaticTableBody(helper, tableName, indexRecords));            
            sb.AppendLine("</table>");
            return new MvcHtmlString(sb.ToString());
        }        

        private static string WriteStaticTableHead(this HtmlHelper helper, string tableName, ExpressFormsIndexHeader indexHeader)
        {
            const string headerFormat = "<th>{0}</th>";
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<thead><tr>");
            foreach (string header in indexHeader.HeaderHtml)
            {
                sb.Append(string.Format(headerFormat, header));
            }
            sb.AppendLine("</tr></thead>");

            return sb.ToString();
        }

        private static MvcHtmlString WriteStaticTableBody(this HtmlHelper helper, string tableName, IEnumerable<ExpressFormsIndexRecord> indexRecords)
        {
            const string dataFormat = "<td>{0}</td>";
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tbody>");
            foreach (ExpressFormsIndexRecord indexRecord in indexRecords)
            {
                sb.AppendLine(string.Format("<tr data-rowid='{0}'>", helper.Encode(indexRecord.IdString)));
                foreach (string html in indexRecord.FieldHtml)
                {
                    sb.AppendFormat(dataFormat, html);
                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</tbody>");
            return new MvcHtmlString(sb.ToString());
        }        
        #endregion

        #region methods that print buttons
        //public static MvcHtmlString WriteButton(this HtmlHelper helper, ExpressForms.Buttons.ExpressFormsButton button, object htmlAttributes)
        //{            
        //    return button.WriteButton(helper, htmlAttributes);
        //}                             
        #endregion
    }
}