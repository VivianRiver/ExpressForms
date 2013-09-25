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
    public class ExpressFormsFilterNumber : ExpressFormsFilter
    {
        public ExpressFormsFilterNumber()
            : base()
        {
            PartialViewName = "ExpressFormsFilters/ExpressFormsFilterNumber";
        }

        public ExpressFormsFilterNumber(ExpressFormsFilter filter)
            : base(filter)
        {
            PartialViewName = "ExpressFormsFilters/ExpressFormsFilterNumber";
        }

        public string PartialViewName { get; set; }

        /// <summary>
        /// True if the value being filtered is nullable, false otherwise.
        /// </summary>
        public bool IsNullable { get; set; }

        public override System.Web.Mvc.MvcHtmlString WriteFilter(HtmlHelper helper, object htmlAttributes)
        {
            IDictionary<string, object> efHtmlAttributes = new RouteValueDictionary(htmlAttributes);
            AddCommonHtmlAttributes(efHtmlAttributes);

            return helper.Partial(PartialViewName, this);
        }

        int nullableByteStorage, nullableInt16Storage, nullableInt32Storage, nullableInt64Storage,
            nullableSingleStorage, nullableDoubleStorage;

        public override void GenerateFilterIl(ILGenerator generator, Dictionary<string, string> filterValues, PropertyInfo property)
        {                        
            Label lblReturnFalse = generator.DefineLabel();
            Label lblNextProperty = generator.DefineLabel();

            string minNumberString = filterValues["minNumber"];
            string maxNumberString = filterValues["maxNumber"];

            // If this is a nullable type such as bool?, the type is wrapped inside Nullable<> and we need to get the generic argument.
            string myPropertyType = IsNullable ?
                property.PropertyType.GetGenericArguments().Single().Name :
                property.PropertyType.Name;

            int storageIndex;
            
            switch (myPropertyType)
            {
                // Regardless of the width of the integer coming in, we treat it as a 64-bit integer.                
                case "Byte":
                case "Int16":
                case "Int32":
                case "Int64":
                    if (myPropertyType == "Byte") storageIndex = nullableByteStorage;
                    else if (myPropertyType == "Int16") storageIndex = nullableInt16Storage;
                    else if (myPropertyType == "Int32") storageIndex = nullableInt32Storage;
                    else storageIndex = nullableInt64Storage;                    

                    if (!string.IsNullOrWhiteSpace(minNumberString))
                    {
                        generator.EmitCheckHasValue(property, storageIndex);
                        generator.Emit(OpCodes.Brfalse, lblReturnFalse); // return false if the property is null

                        // the integer from the record.                        
                        generator.EmitGetValueFromNullableProperty(property, storageIndex);
                        generator.Emit(OpCodes.Conv_I8); // convert to 64-bit integer
                        // the minimum integer                            
                        generator.Emit(OpCodes.Ldc_I8, Convert.ToInt64(minNumberString));
                        // If the integer is less than the minimum, return false.
                        generator.Emit(OpCodes.Blt, lblReturnFalse);
                    }
                    if (!string.IsNullOrWhiteSpace(maxNumberString))
                    {
                        generator.EmitCheckHasValue(property, storageIndex);
                        generator.Emit(OpCodes.Brfalse, lblReturnFalse); // return false if the property is null

                        // the integer from the record.                        
                        generator.EmitGetValueFromNullableProperty(property, storageIndex);
                        generator.Emit(OpCodes.Conv_I8); // convert to 64-bit integer
                        // the maximum integer
                        generator.Emit(OpCodes.Ldc_I8, Convert.ToInt64(maxNumberString));
                        // If the integer is greater than the maximum, return false.
                        generator.Emit(OpCodes.Bgt, lblReturnFalse);
                    }
                    break;
                // Floating point types
                case "Single":
                case "Double":
                    if (myPropertyType == "Single") storageIndex = nullableSingleStorage;
                    else storageIndex = nullableDoubleStorage;
                    if (!string.IsNullOrWhiteSpace(minNumberString))
                    {                        
                        generator.EmitCheckHasValue(property, storageIndex);                        
                        generator.Emit(OpCodes.Brfalse, lblReturnFalse); // return false if the integer is false                                                
                        
                        // the number from the record.                        
                        generator.EmitGetValueFromNullableProperty(property, storageIndex);                        
                        generator.Emit(OpCodes.Conv_R8); // convert to 64-bit floating-point number                                                
                        
                        // the minimum number                            
                        generator.Emit(OpCodes.Ldc_R8, Convert.ToDouble(minNumberString));
                        // If the number is less than the minimum, return false.                        
                        generator.Emit(OpCodes.Blt, lblReturnFalse);
                    }
                    if (!string.IsNullOrWhiteSpace(maxNumberString))
                    {
                        generator.EmitCheckHasValue(property, storageIndex);
                        generator.Emit(OpCodes.Brfalse, lblReturnFalse); // return false if the integer is false

                        // the number from the record.
                        generator.EmitGetValueFromNullableProperty(property, storageIndex);
                        generator.Emit(OpCodes.Conv_R8); // convert to 64-bit floating-point number
                        // the maximum number
                        generator.Emit(OpCodes.Ldc_R8, Convert.ToDouble(maxNumberString));
                        // If the number is greater than the maximum, return false.
                        generator.Emit(OpCodes.Bgt, lblReturnFalse);
                    }
                    break;
                // TODO: DECIMAL type                    
            }
            generator.Emit(OpCodes.Br, lblNextProperty);

            generator.MarkLabel(lblReturnFalse);
            generator.EmitReturnFalse();

            generator.MarkLabel(lblNextProperty);
        }

        public override void GenerateFilterLocalDeclarations(ILGenerator generator, int startingIndex, out int endingIndex)
        {
            generator.DeclareLocal(typeof(Byte?));
            nullableByteStorage = startingIndex;
            generator.DeclareLocal(typeof(Int16?));
            nullableInt16Storage = startingIndex + 1;
            generator.DeclareLocal(typeof(Int32?));
            nullableInt32Storage = startingIndex + 2;
            generator.DeclareLocal(typeof(Int64?));
            nullableInt64Storage = startingIndex + 3;
            generator.DeclareLocal(typeof(Single?));
            nullableSingleStorage = startingIndex + 4;
            generator.DeclareLocal(typeof(Double?));
            nullableDoubleStorage = startingIndex + 5;
            endingIndex = startingIndex + 6;
        }
    }
}
