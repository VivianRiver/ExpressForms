using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpressForms.Inputs;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace ExpressForms.Extensions.Ace
{
    public class ExpressFormsAceInput : ExpressForms.Inputs.ExpressFormsInput
    {
        public ExpressFormsAceInput() : base() { }

        public ExpressFormsAceInput(ExpressFormsInput input) : base(input) { }        

        public override MvcHtmlString WriteInput(HtmlHelper helper, object htmlAttributes)
        {
            TagBuilder tb = new TagBuilder("div");            
            IDictionary<string, object> efHtmlAttributes = new RouteValueDictionary(htmlAttributes);
            AddCommonHtmlAttributes(efHtmlAttributes);
            foreach (var kvp in efHtmlAttributes)
            {
                KeyValuePair<string, string> kvpToAdd = new KeyValuePair<string, string>(kvp.Key, Convert.ToString(kvp.Value));
                tb.Attributes.Add(kvpToAdd);
            }
            tb.Attributes.Add("id", InputName);
            tb.Attributes.Add("style","position:relative; width: 500px; height: 200px;");

            tb.InnerHtml = helper.Encode(this.Value);

            string scriptFormat =
                @"<script>
                    $(function() {{
                        var editor = ace.edit({0});
                        editor.setTheme('ace/theme/monokai');
                        editor.getSession().setMode('ace/mode/xml');
                    }});  
                </script>";
            //string script = string.Format(scriptFormat, InputName);
            string script = string.Format(scriptFormat, getJavascriptString(helper, InputName));

            return new MvcHtmlString(tb.ToString() + script);                    
        }

        private string getJavascriptString(HtmlHelper helper, string s)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(s);
        }
    }
}
