using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpressForms.Buttons;

namespace ExpressForms
{
    public class ExpressFormsIndexViewModel
    {        
        public string Title { get; set; }
        
        /// <summary>
        /// The URL from which to load the table data using AJAX
        /// </summary>
        public string GetAjaxUrl { get; set; }
        /// <summary>
        /// The URL from which to get autocomplete matches using AJAX
        /// </summary>
        public string GetIndexFilterAutocompleteDataUrl { get; set; }

        /// <summary>
        /// Contains the HTML for each column header
        /// </summary>        
        public ExpressFormsIndexHeader IndexHeader { get; set; }

        /// <summary>
        /// Contains the HTML to printed for each record
        /// </summary>
        public IEnumerable<ExpressFormsIndexRecord> IndexRecords { get; set; }

        /// <summary>
        /// Optionally contains filters that the user can use to filter data.
        /// (Currently only supported with jQuery.dataTables AJAX)
        /// </summary>
        public Dictionary<string, Filters.ExpressFormsFilter> Filters { get; set; }

        public DefaultIndexFilterAutocompleteModeEnum DefaultIndexFilterAutocompleteMode { get; set; }

        /// <summary>
        /// Tells where on the page the filter should be displayed (if at all)
        /// </summary>
        public FilterPlacementEnum FilterPlacement { get; set; }

        /// <summary>
        /// The .net type of the records on the page.
        /// </summary>
        public Type RecordType { get; set; }

        /// <summary>
        /// Whether or not index filters use autocomplete by default, where available.
        /// This could have been boolean, but I wanted to leave this open for more options in the future.        
        /// </summary>
        public enum DefaultIndexFilterAutocompleteModeEnum : byte
        {
            Off = 0,
            On = 1
        }

        /// <summary>
        /// Where the filter should be displayed on the screen.
        /// "Dialog" is experimental for now.  It still has some problems.
        /// </summary>
        public enum FilterPlacementEnum : byte
        {
            None = 0,
            Top = 1,
            Bottom = 2,
            Dialog = 3
        }
    }
}
