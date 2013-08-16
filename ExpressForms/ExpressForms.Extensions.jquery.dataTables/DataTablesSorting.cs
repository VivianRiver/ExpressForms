using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Reflection;
using System.Reflection.Emit;
using ExpressForms.IndexAjaxExtension;

namespace ExpressForms.Extensions.jquery.dataTables
{
    public class DataTablesSorting : RecordSorting
    {
        private int NumberOfSorts { get; set; }
        private int[] ColumnSortOrder { get; set; }
        private string[] ColumnSortDirection { get; set; }
        private string[] PropertyNames { get; set; }                               

        public override void Initialize(HttpRequestBase request, IEnumerable<string> propertyNames)
        {            
            NumberOfSorts = Convert.ToInt32(request["iSortingCols"]);
            ColumnSortOrder = new int[NumberOfSorts];
            ColumnSortDirection = new string[NumberOfSorts];
            PropertyNames = propertyNames.ToArray();
            for (int i = 0; i < NumberOfSorts; i++)
            {
                ColumnSortOrder[i] = Convert.ToInt32(request["iSortCol_" + i.ToString()]);
                ColumnSortDirection[i] = request["sSortDir_" + i.ToString()];
            }
        }

        public override IEnumerable<T> GetSortedRecords<T>(IEnumerable<T> filteredRecords) 
        {
            // The goal here is to allow for a developer to provide a method for sorting records,
            // but to also provide a default method that will work with all instances of EntityObject.
            // To that end, I believe it is necessary to use the fun fun DynamicMethod object.

            // Note - check for ASC vs DESC
            
            if (NumberOfSorts == 0)
                return filteredRecords;

            IEnumerable<T> sortedRecords = filteredRecords;
            for (int i = 0; i < NumberOfSorts; i++)
            {
                string propertyName = PropertyNames[ColumnSortOrder[i]];
                PropertyInfo property = typeof(T).GetProperties().Single(p => p.Name == propertyName);

                // This references a DynamicMethod that I haven't gotten to work yet,
                // so for now, I'm using a method that just uses reflection.

                //Func<IEnumerable<T>, PropertyInfo, IEnumerable<T>> sortingFunction = GetFunctionToSortRecords<T>(sortedRecords, property);
                //sortedRecords = GetFunctionToSortRecords<T>(sortedRecords, property)(sortedRecords, property);

                sortedRecords = GetRecordsSortedByProperty(sortedRecords, property, ColumnSortDirection[i]);                

                //break;                            
            }
            return sortedRecords;                        
        }

        // This function works using straight reflection.
        // Can I use DynamicMethod to make it faster?
        private static IEnumerable<T> GetRecordsSortedByProperty<T>(IEnumerable<T> records, PropertyInfo property, string sortDirection)        
        {                        
            MethodInfo GetDefaultKeySelectorForProperty = typeof(DataTablesSorting).GetMethod("GetDefaultKeySelectorForProperty")
                .MakeGenericMethod(typeof(T), property.PropertyType);

            string orderByMethodName = sortDirection.ToUpper() == "ASC" ? "OrderBy" : "OrderByDescending";
            MethodInfo EnumerableOrderBy = typeof(Enumerable).GetMethods()
                .Single(m => m.Name == orderByMethodName && m.GetParameters().Count() == 2)
                .MakeGenericMethod(typeof(T), property.PropertyType);

            var keySelector = GetDefaultKeySelectorForProperty.Invoke(null, new[] { property });
            var sortedRecords = EnumerableOrderBy.Invoke(null, new[] { records, keySelector });

            return (IEnumerable<T>)sortedRecords;
        }

        delegate TKey GetDefaultKeySelectorForPropertyDelegate<T, TKey>(T t);
        public static Func<T, TKey> GetDefaultKeySelectorForProperty<T, TKey>(PropertyInfo property)
        {
            DynamicMethod method = new DynamicMethod("GetKeySelector", typeof(TKey), new Type[] { typeof(T) });
            ILGenerator generator = method.GetILGenerator();

            MethodInfo GetPropertyValue = property.GetGetMethod();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Callvirt, GetPropertyValue);
            generator.Emit(OpCodes.Ret);
            
            return ((GetDefaultKeySelectorForPropertyDelegate<T, TKey>)(method.CreateDelegate(typeof(GetDefaultKeySelectorForPropertyDelegate<T, TKey>)))).Invoke;
        }

        // This doesn't work for some reason.

        //delegate IOrderedEnumerable<T> GetFunctionToSortRecordsDelegate<T>(IEnumerable<T> records, PropertyInfo propertyToSortOn);
        //public static Func<IEnumerable<T>, PropertyInfo, IOrderedEnumerable<T>> GetFunctionToSortRecords<T>(IEnumerable<T> records, PropertyInfo propertyToSortOn)
        //{
        //    Type propertyType = propertyToSortOn.GetType();

        //    DynamicMethod method = new DynamicMethod("SortRecords", typeof(IOrderedEnumerable<T>), new Type[] { typeof(IEnumerable<T>), typeof(PropertyInfo) });
        //    ILGenerator generator = method.GetILGenerator();

        //    MethodInfo GetPropertyValue = propertyToSortOn.GetGetMethod();
        //    MethodInfo GetDefaultKeySelectorForProperty = typeof(DataTablesSorting).GetMethod("GetDefaultKeySelectorForProperty")
        //        .MakeGenericMethod(new Type[] { typeof(T), propertyToSortOn.PropertyType });

        //    MethodInfo EnumerableOrderBy = typeof(Enumerable).GetMethods()
        //        .Single(m => m.Name == "OrderBy" && m.GetParameters().Count() == 2)
        //        .MakeGenericMethod(typeof(T), propertyToSortOn.PropertyType);

        //    // Get the default key selector for the property passed in.            
        //    generator.Emit(OpCodes.Ldarg_1); // property
        //    generator.Emit(OpCodes.Call, GetDefaultKeySelectorForProperty);

        //    // Save the default key selector at location 0
        //    generator.Emit(OpCodes.Stloc_0);

        //    generator.Emit(OpCodes.Ldarg_0); // records
        //    generator.Emit(OpCodes.Ldloc_0); // default key selector
        //    generator.Emit(OpCodes.Call, EnumerableOrderBy);
        //    generator.Emit(OpCodes.Ret);

        //    return ((GetFunctionToSortRecordsDelegate<T>)(method.CreateDelegate(typeof(GetFunctionToSortRecordsDelegate<T>)))).Invoke;
        //}       
    }
}
