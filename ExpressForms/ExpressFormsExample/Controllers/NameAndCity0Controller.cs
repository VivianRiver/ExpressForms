using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressForms.Extensions.jquery.dataTables;

namespace ExpressFormsExample.Controllers
{
    public class NameAndCity0Controller : BaseController<NameAndCity>
    {
        public NameAndCity0Controller()
        {
            IndexAjaxExtension = new ExpressFormsDataTablesIndexAjaxExtension();
            IndexViewName = "jqueryDataTablesIndexView";
        }
    }
}
