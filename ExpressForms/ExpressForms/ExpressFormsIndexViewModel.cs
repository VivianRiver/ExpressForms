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

        public string GetAjaxUrl { get; set; }

        /// <summary>
        /// Contains the HTML for each column header
        /// </summary>        
        public ExpressFormsIndexHeader IndexHeader { get; set; }

        /// <summary>
        /// Contains the HTML to printed for each record
        /// </summary>
        public IEnumerable<ExpressFormsIndexRecord> IndexRecords { get; set; }
    }
}
