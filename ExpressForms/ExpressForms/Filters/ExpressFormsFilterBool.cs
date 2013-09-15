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
    public class ExpressFormsFilterBool : ExpressFormsFilter
    { 
        public ExpressFormsFilterBool()
            : base()
        {
            PartialViewName = "ExpressFormsFilters/ExpressFormsFilterBool";
        }

        public ExpressFormsFilterBool(ExpressFormsFilter filter)
            : base(filter)
        {
            PartialViewName = "ExpressFormsFilters/ExpressFormsFilterBool";
        }

        public string PartialViewName { get; set; }

        /// <summary>
        /// True if the value being filtered is nullable, false otherwise.
        /// </summary>
        public bool IsNullable { get; set; }

        public override System.Web.Mvc.MvcHtmlString WriteFilter(System.Web.Mvc.HtmlHelper helper, object htmlAttributes)
        {
            IDictionary<string, object> efHtmlAttributes = new RouteValueDictionary(htmlAttributes);
            AddCommonHtmlAttributes(efHtmlAttributes);

            return helper.Partial(PartialViewName, this);            
        }

        int nullableBoolStorage;              

        public override void GenerateFilterIl(ILGenerator generator, Dictionary<string, string> filterValues, PropertyInfo property)
        {
            // Note: If the user specifies both true and false, we can skip this instance                                                            

            string selection = filterValues["selection"];
            string selectionLower = selection.ToLower();

            bool selectionContainsTrue = selectionLower.Contains("true");
            bool selectionContainsFalse = selectionLower.Contains("false");
            bool selectionContainsNull = selectionLower.Contains("null");

            Label lblReturnFalse = generator.DefineLabel();
            Label lblCheckNextProperty = generator.DefineLabel();
            
            generator.EmitCheckHasValue(property, nullableBoolStorage); // push 1 onto the stack if property has value; 0 if it is null.            
            generator.Emit(OpCodes.Pop);

            // If the filter matches null, check if the value is null.
            if (selectionContainsNull)
            {
                generator.EmitCheckHasValue(property, nullableBoolStorage);
                generator.Emit(OpCodes.Brfalse, lblCheckNextProperty);
            }
            // If the property didn't match null and has no value, return false.
            generator.EmitCheckHasValue(property, nullableBoolStorage);
            generator.Emit(OpCodes.Brfalse, lblReturnFalse);

            // From here on out, we assume the property has a value.
            if (selectionContainsTrue)
            {                
                generator.EmitGetValueFromNullableProperty(property, nullableBoolStorage);                                                
                generator.Emit(OpCodes.Brtrue, lblCheckNextProperty);
            }
            if (selectionContainsFalse)
            {                
                generator.EmitGetValueFromNullableProperty(property, nullableBoolStorage);
                generator.Emit(OpCodes.Brfalse, lblCheckNextProperty);
            }

            // If we've gotten this far without finding one of the values the user passed in, the record doesn't match, so return false.
            generator.MarkLabel(lblReturnFalse);
            generator.EmitReturnFalse();

            generator.MarkLabel(lblCheckNextProperty);            
        }
        
        public override void GenerateFilterLocalDeclarations(ILGenerator generator, int startingIndex, out int endingIndex)
        {
            nullableBoolStorage = startingIndex; // The index where a bool? value may be stored.
            generator.DeclareLocal(typeof(bool?));

            endingIndex = startingIndex + 1;
        }
    }
}
