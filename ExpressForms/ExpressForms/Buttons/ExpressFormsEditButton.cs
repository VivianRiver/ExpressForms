using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace ExpressForms.Buttons
{
    public class ExpressFormsEditButton : ExpressFormsButton
    {
        public ExpressFormsEditButton()
        {
            Parameters = new Dictionary<string, string>();
        }
        
        public string LinkUrl { get; set; }
        public Dictionary<string, string> Parameters { get; set; }

        public override MvcHtmlString WriteButton(HtmlHelper helper, object htmlAttributes)
        {
            // TODO: Use proper encoding.
            // Is there a class for this? (Yes, URL helper, but I don't want to pass it here because we don't want this to depend on that, or do we?)
            StringBuilder linkUrlSb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(LinkUrl))
            {
                linkUrlSb.Append(LinkUrl);
                if (Parameters != null && Parameters.Count > 0)
                {
                    linkUrlSb.Append('?');
                    foreach (KeyValuePair<string, string> kvp in Parameters)
                    {
                        linkUrlSb.Append(kvp.Key + "=" + kvp.Value + "&");
                    }
                }
            }

            TagBuilder tb = new TagBuilder("input");
            tb.Attributes.Add("type", "button");
            tb.Attributes.Add("class", "ExpressFormsEditButton");
            tb.Attributes.Add("value", Text);
            tb.Attributes.Add("data-linkurl", linkUrlSb.ToString());            
            if (!IsVisible)
                tb.Attributes.Add("style", "display: none;");

            IDictionary<string, object> efHtmlAttributes = new RouteValueDictionary(htmlAttributes);
            foreach (var kvp in efHtmlAttributes)
                tb.MergeAttribute(kvp.Key, Convert.ToString(kvp.Value));

            return new MvcHtmlString(tb.ToString(TagRenderMode.SelfClosing));
        }
    }
}