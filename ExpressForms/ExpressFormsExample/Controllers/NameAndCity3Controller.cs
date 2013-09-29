using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressForms.Extensions.jquery.dataTables;

namespace ExpressFormsExample.Controllers
{
    public class NameAndCity3Controller : BaseController<NameAndCity>
    {
        public NameAndCity3Controller()
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
                {"Middle_Name", r =>
                    {
                        string middleName = r.Middle_Name;
                        if (string.IsNullOrWhiteSpace(middleName))
                            return "";
                        else
                            return middleName.First().ToString();
                    }
                }
            };
            IndexFilterPlacement = ExpressForms.ExpressFormsIndexViewModel.FilterPlacementEnum.Top;
            DefaultIndexFilterAutocompleteMode = ExpressForms.ExpressFormsIndexViewModel.DefaultIndexFilterAutocompleteModeEnum.On;
        }
    }
}