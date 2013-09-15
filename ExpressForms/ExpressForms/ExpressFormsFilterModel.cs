using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpressForms.Filters;

namespace ExpressForms
{
    public class ExpressFormsFilterModel
    {        
        /// <summary>
        /// The keys of this dictionary are the names of the properties of a class
        /// and the values are the ExpressFormsFilter object to be displayed for each one.
        /// </summary>
        public Dictionary<string, ExpressFormsFilter> Filters { get; set; }        
    }
}
