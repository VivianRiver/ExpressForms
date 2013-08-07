using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpressFormsExample.Controllers
{
    public class Engineer1Controller : BaseController<Engineer>
    {
        public Engineer1Controller()
            : base()
        {
            IndexTitle = "Engineer Roster";

            CustomPropertyNames.Add("FavoriteLanguage", "Favorite Language");
            CustomPropertyNames.Add("CodeSnippet", "Favorite Code Snippet");
            CustomPropertyNames.Add("Available", "Available For Hire");
        }
    }
}
