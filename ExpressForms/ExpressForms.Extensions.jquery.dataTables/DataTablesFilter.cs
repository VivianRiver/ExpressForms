using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Reflection;
using System.Reflection.Emit;
using ExpressForms.IndexAjaxExtension;
using System.Web;

namespace ExpressForms.Extensions.jquery.dataTables
{
    public class DataTablesFilter : RecordFilter
    {
        public IEnumerable<string> PropertyNames { get; set; }
        public string SearchKey { get; set; }

        public DataTablesFilter() { }

        public override void Initialize(HttpRequestBase request, IEnumerable<string> propertyNames)
        {
            PropertyNames = propertyNames;
            SearchKey = request.QueryString["sSearch"];
        }        

        public override IEnumerable<T> GetFilteredRecords<T>(IEnumerable<T> records)
        {
            // The goal here is to allow for a developer to provide a method for filtering records,
            // but to also provide a default method that will work with all instances of EntityObject.
            // To that end, I believe it is necessary to use the fun fun DynamicMethod object.

            // Don't attempt to filter if there is no search key provided.
            if (string.IsNullOrWhiteSpace(SearchKey))
                return records;

            Func<T, bool> filter = GetDefaultFilter<T>();

            return records.Where(filter);
        }

        delegate bool FilterDelegate<T>(T t);
        public Func<T, bool> GetDefaultFilter<T>()
        {
            PropertyInfo[] propertiesToFilterOn = typeof(T).GetProperties()
                .Where(p => p.PropertyType.Name=="String" && PropertyNames.Contains(p.Name))
                .ToArray();

            DynamicMethod method = new DynamicMethod("Filter", typeof(bool), new Type[] { typeof(T) });
            ILGenerator generator = method.GetILGenerator();

            string searchKeyLower = SearchKey.ToLower();
            MethodInfo StringToLower = typeof(string).GetMethod("ToLower", new Type[] { });
            MethodInfo StringStartsWith = typeof(String).GetMethod("StartsWith", new Type[] { typeof(string) });            
            
            foreach (PropertyInfo property in propertiesToFilterOn)
            {
                Label checkNextPropertyLabel = generator.DefineLabel();
                MethodInfo GetPropertyValue = property.GetGetMethod();

                // First, check to see if this property is null.
                // If it is, go on to the next property.                
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Callvirt, GetPropertyValue);
                generator.Emit(OpCodes.Brfalse_S, checkNextPropertyLabel);
                // Load the next property of the argument and make it lower-case.
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Callvirt, GetPropertyValue);
                generator.Emit(OpCodes.Callvirt, StringToLower);
                // Load the lower-cased search key and see if the lower-cased property starts with it.
                generator.Emit(OpCodes.Ldstr, searchKeyLower);
                generator.Emit(OpCodes.Callvirt, StringStartsWith);
                // If the search key doesn't match, then go on to the next property.
                generator.Emit(OpCodes.Brfalse_S, checkNextPropertyLabel);
                // If it does match, then return true.
                generator.Emit(OpCodes.Ldc_I4_1);
                generator.Emit(OpCodes.Ret);
                generator.MarkLabel(checkNextPropertyLabel);
            }
            // After all the properties to test have been enumerated and no match has been found, return false.
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Ret);

            return ((FilterDelegate<T>)(method.CreateDelegate(typeof(FilterDelegate<T>)))).Invoke;
        }
    }
}
