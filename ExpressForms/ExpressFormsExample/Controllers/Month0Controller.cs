using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressForms.Extensions.jquery.dataTables;

namespace ExpressFormsExample.Controllers
{
    public class Month0Controller : BaseController<Month>
    {
        public Month0Controller()
        {
            IndexAjaxExtension = new ExpressFormsDataTablesIndexAjaxExtension();
            IndexViewName = "jqueryDataTablesIndexView";

            IndexFilterPlacement = ExpressForms.ExpressFormsIndexViewModel.FilterPlacementEnum.Top;
        }
    }
}