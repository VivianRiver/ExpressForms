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

            CustomPropertyDisplay["USHoliday"] = record => record.USHoliday.HasValue ? record.USHoliday.Value.ToString("yyyy/MM/dd") : "";

            IndexFilterPlacement = ExpressForms.ExpressFormsIndexViewModel.FilterPlacementEnum.Top;
        }
    }
}