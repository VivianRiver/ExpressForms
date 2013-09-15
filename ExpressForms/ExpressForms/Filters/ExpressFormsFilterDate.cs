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
    public class ExpressFormsFilterDate : ExpressFormsFilter
    {
        public ExpressFormsFilterDate()
            : base()
        {
            PartialViewName = "ExpressFormsFilters/ExpressFormsFilterDate";
        }

        public ExpressFormsFilterDate(ExpressFormsFilter filter)
            : base(filter)
        {
            PartialViewName = "ExpressFormsFilters/ExpressFormsFilterDate";
        }

        public string PartialViewName { get; set; }

        public override System.Web.Mvc.MvcHtmlString WriteFilter(System.Web.Mvc.HtmlHelper helper, object htmlAttributes)
        {
            IDictionary<string, object> efHtmlAttributes = new RouteValueDictionary(htmlAttributes);
            AddCommonHtmlAttributes(efHtmlAttributes);

            return helper.Partial(PartialViewName, this);
        }

        int dateTimeStorage, nullableDateTimeStorage;
        public override void GenerateFilterIl(System.Reflection.Emit.ILGenerator generator, Dictionary<string, string> filterValues, PropertyInfo property)
        {
            string minDateString = filterValues["minDate"];
            string maxDateString = filterValues["maxDate"];

            bool isNullable = property.PropertyType.Name == "Nullable`1";

            Label lblNextProperty = generator.DefineLabel();
            Label lblReturnFalse = generator.DefineLabel();

            // First, get the value loaded, but if it is null, then return false unless both minDate and maxDate are blank.
            if (isNullable)
            {
                // This block of code is so-far UNTESTED!!!

                MethodInfo GetNullableHasValue = typeof(DateTime?).GetProperty("HasValue").GetGetMethod();
                MethodInfo GetNullableValue = typeof(DateTime?).GetProperty("Value").GetGetMethod();

                bool minAndMaxBothBlank = string.IsNullOrWhiteSpace(minDateString) && string.IsNullOrWhiteSpace(maxDateString);
                Label whereToGoIfNull = minAndMaxBothBlank ? lblNextProperty : lblReturnFalse;

                // If the property is null and both min and max date are blank, then go to the next property.
                // If the property is null and either min or max date has a value, return false.                
                generator.EmitGetPropertyValueFromArgument(property);                
                generator.Emit(OpCodes.Stloc, nullableDateTimeStorage);
                generator.Emit(OpCodes.Ldloca_S, nullableDateTimeStorage);
                generator.Emit(OpCodes.Call, GetNullableHasValue);                
                generator.Emit(OpCodes.Brfalse_S, whereToGoIfNull);
                // If we haven't jumped, then the datetime is not null, so just save it.
                generator.Emit(OpCodes.Ldloca_S, nullableDateTimeStorage);
                generator.Emit(OpCodes.Call, GetNullableValue);
                generator.Emit(OpCodes.Stloc, dateTimeStorage);
            }
            else // not nullable
            {
                // The property is not nullable, so just save the value.
                generator.EmitGetPropertyValueFromArgument(property);
                generator.Emit(OpCodes.Stloc, dateTimeStorage);
            }
            // At this point, the record's datetime should be saved in memory.                        
            MethodInfo DateTimeParse = typeof(DateTime).GetMethod("Parse", new Type[] { typeof(string) });
            MethodInfo DateTimeLessThanOrEqual = typeof(DateTime).GetMethod("op_LessThanOrEqual");
            MethodInfo DateTimeGreaterThanOrEqual = typeof(DateTime).GetMethod("op_GreaterThanOrEqual");

            if (!string.IsNullOrWhiteSpace(minDateString))
            {
                generator.Emit(OpCodes.Ldstr, minDateString);
                generator.Emit(OpCodes.Call, DateTimeParse); // Get the mindate DateTime.                            
                generator.Emit(OpCodes.Ldloc, dateTimeStorage); // Get the record's DateTime from memory                            
                generator.Emit(OpCodes.Call, DateTimeLessThanOrEqual);
                generator.Emit(OpCodes.Brfalse, lblReturnFalse);
            }
            if (!string.IsNullOrWhiteSpace(maxDateString))
            {
                generator.Emit(OpCodes.Ldstr, maxDateString);
                generator.Emit(OpCodes.Call, DateTimeParse); // Get the maxdate DateTime.
                generator.Emit(OpCodes.Ldloc, dateTimeStorage); // Get the record's DateTime from memory
                generator.Emit(OpCodes.Call, DateTimeGreaterThanOrEqual);
                generator.Emit(OpCodes.Brfalse, lblReturnFalse);
            }
            generator.Emit(OpCodes.Br, lblNextProperty);

            generator.MarkLabel(lblReturnFalse);
            generator.EmitReturnFalse();

            generator.MarkLabel(lblNextProperty);
        }

        public override void GenerateFilterLocalDeclarations(ILGenerator generator, int startingIndex, out int endingIndex)
        {
            dateTimeStorage = startingIndex; // The index where a DateTime value may be stored.
            generator.DeclareLocal(typeof(DateTime));
            nullableDateTimeStorage = startingIndex + 1; // The index where a DateTime? value may be stored.
            generator.DeclareLocal(typeof(DateTime?));
            endingIndex = startingIndex + 2;
        }
    }
}
