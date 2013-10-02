using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpressForms.Buttons;

namespace ExpressForms
{
    public class ExpressFormsEditorModel
    {        
        public object Record { get; set; }        
        public Dictionary<string, Inputs.ExpressFormsInput> Inputs { get; set; }
        public bool IsNew { get; set; }

        /// <summary>
        /// An array containing the HTML of each button to be printed on the form.
        /// </summary>
        public string[] ButtonHtml { get; set; }
    }
}
