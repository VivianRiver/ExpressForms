using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressForms
{
    public class ExpressFormsIndexViewModel
    {
        public Type RecordType { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// Optional.
        /// A dictionary that specifies one or more custom headers to be displayed in place of property names.
        /// For example, it can be used to display "First Name" instead of "First_Name"
        /// </summary>
        public Dictionary<string, string> CustomIndexHeaders { get; set; }

        // Dynamic here refers to the type of record on the display.
        // I don't know of any way to use a generic viewmodel.
        // TODO: More research        

        /// <summary>
        /// A dictionary of functions to compute the HTML of various columns.
        /// Please note that the output is printed directly, so be sure to encode special characters < > &
        /// </summary>
        public Dictionary<string, Func<dynamic, string>> CustomPropertyDisplay { get; set; }

        public IEnumerable<object> Records { get; set; }
    }
}
