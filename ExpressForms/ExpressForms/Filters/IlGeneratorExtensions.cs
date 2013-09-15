using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace ExpressForms.Filters
{
    public static class IlGeneratorExtensions
    {
        /// <summary>
        /// Generate the IL to indicate that the record matches the filter.
        /// </summary>
        /// <param name="generator"></param>
        public static void EmitReturnTrue(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Generate the IL to indicate that the record doesn't match the filter.
        /// </summary>
        /// <param name="generator"></param>
        public static void EmitReturnFalse(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Generates IL to get the specified property value from the argument; which in ExpressFormsFilter, will always be the record being filtered.
        /// </summary>        
        public static void EmitGetPropertyValueFromArgument(this ILGenerator generator, PropertyInfo property)
        {
            MethodInfo method = property.GetGetMethod();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Callvirt, method);
        }

        /// <summary>
        /// Emits IL that push 1 onto the stack if the property has a non-null value, 0 if it is null.
        /// If the property value is non-nullable, push 1 regardless of the value.
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property"></param>        
        public static void EmitCheckHasValue(this ILGenerator generator, PropertyInfo property, int nullableStorageIndex)
        {
            bool isNullable = property.PropertyType.Name == "Nullable`1";
            if (isNullable)
            {
                MethodInfo NullableGetHasValue = property.PropertyType.GetProperty("HasValue").GetGetMethod();
                generator.EmitGetPropertyValueFromArgument(property);                
                generator.Emit(OpCodes.Stloc, nullableStorageIndex);
                generator.Emit(OpCodes.Ldloca_S, nullableStorageIndex);                
                generator.Emit(OpCodes.Call, NullableGetHasValue); // will push 1 if has value, 0 if null                
            }
            else
            {
                // not nullable;
                generator.Emit(OpCodes.Ldc_I4_1);
            }
        }

        /// <summary>
        /// If property is nullable, return the value.  If not nullable, return the value.
        /// This method assumes we have already checked for null.
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property"></param>
        public static void EmitGetValueFromNullableProperty(this ILGenerator generator, PropertyInfo property, int nullableStorageIndex)
        {            
            bool isNullable = property.PropertyType.Name == "Nullable`1";
            if (isNullable)
            {
                MethodInfo NullableGetValue = property.PropertyType.GetProperty("Value").GetGetMethod();            
                generator.EmitGetPropertyValueFromArgument(property);
                generator.Emit(OpCodes.Stloc, nullableStorageIndex);
                generator.Emit(OpCodes.Ldloca_S, nullableStorageIndex);                
                generator.Emit(OpCodes.Call, NullableGetValue); // push the value from nullable property.                
            }
            else
            {
                // not nullable, so just load the property.
                generator.EmitGetPropertyValueFromArgument(property);                
            }
        }
    }
}
