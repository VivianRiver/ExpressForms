using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Reflection;
using System.Reflection.Emit;

namespace ExpressForms.Filters
{
    public class ExpressFormsFilterText : ExpressFormsFilter
    {
        public ExpressFormsFilterText()
            : base()
        {
            PartialViewName = "ExpressFormsFilters/ExpressFormsFilterText";
        }

        public ExpressFormsFilterText(ExpressFormsFilter filter)
            : base(filter)
        {
            PartialViewName = "ExpressFormsFilters/ExpressFormsFilterText";
        }

        public string PartialViewName { get; set; }

        public override System.Web.Mvc.MvcHtmlString WriteFilter(System.Web.Mvc.HtmlHelper helper, object htmlAttributes)
        {
            IDictionary<string, object> efHtmlAttributes = new RouteValueDictionary(htmlAttributes);
            AddCommonHtmlAttributes(efHtmlAttributes);

            return helper.Partial(PartialViewName, this);

            //return helper.TextBox(this.InputName, this.Value, efHtmlAttributes);
        }

        public override void GenerateFilterIl(System.Reflection.Emit.ILGenerator generator, Dictionary<string, string> filterValues, PropertyInfo property)
        {
            MethodInfo StringToLower = typeof(string).GetMethod("ToLower", new Type[] { });
            MethodInfo StringStartsWith = typeof(String).GetMethod("StartsWith", new Type[] { typeof(string) });
            MethodInfo StringContains = typeof(String).GetMethod("Contains", new Type[] { typeof(string) });
            MethodInfo StringIsNullOrWhiteSpace = typeof(string).GetMethod("IsNullOrWhiteSpace", new Type[] { typeof(string) });

            Label lblNextProperty = generator.DefineLabel();
            Label lblNotNull = generator.DefineLabel();

            string filterText = filterValues["filterText"];
            string filterMode = filterValues["filterMode"];

            // Do nothing for this property is the filterText is blank and the filterMode is "Starts With" or "Contains"
            if (string.IsNullOrWhiteSpace(filterText) && new string[] { "Starts With", "Contains" }.Contains(filterMode))
                return;

            switch (filterMode)
            {
                case "Starts With":
                case "Contains":

                    // Check that the property is not null.  If it is null, return false; otherwise, continue.
                    generator.EmitGetPropertyValueFromArgument(property);
                    generator.Emit(OpCodes.Brtrue, lblNotNull);
                    generator.EmitReturnFalse();
                    generator.MarkLabel(lblNotNull);
                    // Load the  property of the argument and make it lower-case.
                    generator.EmitGetPropertyValueFromArgument(property);
                    generator.Emit(OpCodes.Callvirt, StringToLower);
                    // Load the lower-cased search key and see if the lower-cased property starts with it.
                    generator.Emit(OpCodes.Ldstr, filterText.ToLower());
                    // We use either String.StartsWith or String.Contains according to what the user specified.
                    generator.Emit(OpCodes.Callvirt, filterMode == "Starts With" ? StringStartsWith : StringContains);
                    // If the search key doesn't match, then return false; otherwise, go on to the next property
                    generator.Emit(OpCodes.Brtrue, lblNextProperty);
                    generator.EmitReturnFalse();
                    break;
                case "Blank":
                    // Load the  property of the argument and see if it's null or whitespace
                    generator.EmitGetPropertyValueFromArgument(property);
                    generator.Emit(OpCodes.Call, StringIsNullOrWhiteSpace);
                    // If we didn't get back true, then return false; otherwise, go on to the next property.
                    generator.Emit(OpCodes.Brtrue, lblNextProperty);
                    generator.EmitReturnFalse();
                    break;
            }

            generator.MarkLabel(lblNextProperty);
        }

        public override void GenerateFilterLocalDeclarations(ILGenerator generator, int startingIndex, out int endingIndex)
        {
            endingIndex = startingIndex;
        }
    }
}
