using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Reflection;
using System.Reflection.Emit;
using ExpressForms.IndexAjaxExtension;
using ExpressForms.Filters;
using System.Web;
using System.Web.Script.Serialization;

namespace ExpressForms.Extensions.jquery.dataTables
{
    public class DataTablesFilter : RecordFilter
    {
        public IEnumerable<string> PropertyNames { get; set; }
        public string SearchKey { get; set; }
        public string AdvancedSearchJson { get; set; }

        public DataTablesFilter() { }

        public override void Initialize(HttpRequestBase request, IEnumerable<string> propertyNames)
        {
            PropertyNames = propertyNames;
            SearchKey = request.QueryString["sSearch"];            
        }                
        /// <summary>
        /// Get records filtered according to the data the user keyed into the filter form.
        /// </summary>        
        /// <param name="records">the records to filter</param>
        /// <param name="filters">a dictionary where the keys are the names of the columns and the values are the filter associated with each one</param>
        /// <param name="filterEntries">a dictionary of dictionaries where the keys are the names of the columns and the values (which are dictionaries of strings)
        /// contain the values input on each filter form.</param>
        /// <returns></returns>
        public override IEnumerable<T> GetFilteredRecords<T>(IEnumerable<T> records, Dictionary<string, ExpressFormsFilter> filters, Dictionary<string, Dictionary<string,string>> filterEntries)
        {                                    
            Func<T, bool> advancedFilter = GetFunctionForExpressFormsFilters<T>(filters, filterEntries);
                                    
            return records                
                .Where(advancedFilter);            
        }

        public Func<T, bool> GetFunctionForExpressFormsFilters<T>(Dictionary<string, ExpressFormsFilter> filters, Dictionary<string, Dictionary<string, string>> filterEntries)
        {                       
            DynamicMethod method = new DynamicMethod("Filter", typeof(bool), new Type[] { typeof(T) });
            ILGenerator generator = method.GetILGenerator();

            PropertyInfo[] propertiesToFilterOn = typeof(T).GetProperties()
                .Where(p => PropertyNames.Contains(p.Name))
                .ToArray();

            int variableIndex = 0;
            foreach (PropertyInfo property in propertiesToFilterOn)
            {
                // Generate IL variable declarations.
                // First, check that a filter has been provided for this property to avoid errors.
                if (!filters.Keys.Contains(property.Name))
                    continue;
                ExpressFormsFilter filter = filters[property.Name];
                filter.GenerateFilterLocalDeclarations(generator, variableIndex, out variableIndex);
            }

            foreach (PropertyInfo property in propertiesToFilterOn)
            {
                // Generate IL to implement filtering                
                // First, check that a filter has been provided for this property to avoid errors.
                if (!filters.Keys.Contains(property.Name) || !filterEntries.Keys.Contains(property.Name))
                    continue;
                ExpressFormsFilter filter = filters[property.Name];
                Dictionary<string, string> filterEntry = filterEntries[property.Name];
                filter.GenerateFilterIl(generator, filterEntry, property);                
            }

            generator.EmitReturnTrue();

            return (Func<T, bool>)(method.CreateDelegate(typeof(Func<T, bool>)));
        }
    }
}
