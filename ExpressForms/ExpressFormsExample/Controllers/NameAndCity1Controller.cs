using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressForms.Extensions.jquery.dataTables;

namespace ExpressFormsExample.Controllers
{
    public class NameAndCity1Controller : BaseController<NameAndCity>
    {
           public NameAndCity1Controller()
            : base()
        {
            IndexTitle = "Names And Cities";
            IndexViewName = "jqueryDataTablesIndexView";            
            IndexAjaxExtension = new ExpressFormsDataTablesIndexAjaxExtension();            
            CustomPropertyNames = new Dictionary<string, string>()
            {
                {"First_Name", "First Name"},
                {"Middle_Name", "Middle Initial"},
                {"Last_Name", "Last Name"}
            };
            CustomPropertyDisplay = new Dictionary<string, Func<NameAndCity, string>>()
            {
                {"Middle_Name", r => r.Middle_Name.First().ToString() }
            };
        }   
    }
}
