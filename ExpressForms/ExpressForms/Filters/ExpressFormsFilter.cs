using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using System.Reflection.Emit;

namespace ExpressForms.Filters
{
    public abstract class ExpressFormsFilter        
    {
        public ExpressFormsFilter() { }

        public ExpressFormsFilter(ExpressFormsFilter filter)
        {            
            FilterName = filter.FilterName;            
        }

        /// <summary>
        /// the name of this filter to use on the form
        /// </summary>
        public string FilterName { get; set; }

        /// <summary>
        /// the name to display for this input (not necessarily the same as the name of this input)
        /// </summary>
        public string FilterDisplayName { get; set; }

        protected string GetFilterTypeName()
        {
            return this.GetType().Name;
        }

        /// <summary>
        /// Adds to the HTML attributes the common attributes used by all filters regardless of type.
        /// This should be called from within the WriteInput implementation in all inherited classes.
        /// </summary>
        /// <param name="htmlAttributes"></param>
        protected void AddCommonHtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            // not sure if I'm going to use this function with filters

            htmlAttributes.Add("data-name", this.FilterName);
            htmlAttributes.Add("data-filtername", this.FilterName);
            htmlAttributes.Add("data-type", this.GetFilterTypeName());
            htmlAttributes.Add("class", "ExpressForms");
        }        

        /// <summary>
        /// gets the HTML of this input to put in an HTML document
        /// </summary>        
        public abstract MvcHtmlString WriteFilter(HtmlHelper helper, object htmlAttributes);
        
        /// <summary>
        /// Adds IL instructions to the ILGenerator that compose an anonymous method that is used for filtering records.
        /// This is very tricky to program, but it is done this way because Entity Framework chokes on reflection.
        /// The generated code should read the filter values and output code that will return false (0) when the
        /// record does not match.  If the record does match, then there should be no return statement and
        /// nothing on the execution stack when it finishes running.
        /// When this method has been called on all filters, a call will be added on to the end to return true
        /// if none of the other filters emitted code to return false.
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="filterValues"></param>
        public abstract void GenerateFilterIl(ILGenerator generator, Dictionary<string, string> filterValues, PropertyInfo property);        

        /// <summary>
        /// TODO: Write documentation here.
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="startingIndex"></param>
        /// <param name="endingIndex"></param>
        public abstract void GenerateFilterLocalDeclarations(ILGenerator generator, int startingIndex, out int endingIndex);   
                     
        /// <summary>
        /// Return a method that will tell whether or not a record matches a given filter to be used for autocomplete.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterValues"></param>
        /// <param name="propertyToTest"></param>
        /// <returns></returns>
        public Func<T, bool> GetAutocompleteMatchesMethod<T>(Dictionary<string,string> filterValues, PropertyInfo propertyToTest)
        {
            DynamicMethod method = new DynamicMethod("GetFilterAutocompleteMatches", typeof(bool), new Type[] { typeof(T) });
            ILGenerator generator = method.GetILGenerator();            

            int lastIndex;
            GenerateFilterLocalDeclarations(generator, 0, out lastIndex);
            GenerateFilterIl(generator, filterValues, propertyToTest);
            // If the code for the filter ran without returning false, return true.
            generator.EmitReturnTrue();
            return (Func<T, bool>)(method.CreateDelegate(typeof(Func<T, bool>)));
        }
    }
}
