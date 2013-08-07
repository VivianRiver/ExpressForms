using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpressFormsExample.Controllers
{
    public class Engineer2Controller : BaseController<Engineer>
    {
        public Engineer2Controller()
            : base()
        {
            IndexTitle = "Engineer Roster";

            CustomPropertyNames.Add("FavoriteLanguage", "Favorite Language");
            CustomPropertyNames.Add("CodeSnippet", "Favorite Code Snippet");
            CustomPropertyNames.Add("Available", "Available For Hire");

            CustomEditorInputs.Add("CodeSnippet", new ExpressForms.Inputs.ExpressFormsTextArea()
            {
                FormName = this.FormName,
                InputName = "CodeSnippet",
            });
        }
    }
}
