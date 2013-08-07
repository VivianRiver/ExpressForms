using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpressFormsExample.Controllers
{
    public class Engineer4Controller : BaseController<Engineer>
    {
        public Engineer4Controller()
            : base()
        {
            IndexTitle = "Engineer Roster";

            CustomPropertyNames.Add("FavoriteLanguage", "Favorite Language");
            CustomPropertyNames.Add("CodeSnippet", "Favorite Code Snippet");
            CustomPropertyNames.Add("Available", "Available For Hire");

            CustomEditorInputs.Add("CodeSnippet", new ExpressForms.Extensions.Ace.ExpressFormsAceInput()
            {
                FormName = this.FormName,
                InputName = "CodeSnippet",
            });

            CustomPropertyDisplay.Add("Available", (record) =>
            {
                Engineer engineer = (Engineer)record;
                string checkHtml = "<img src='{0}' alt='check' />".Replace("{0}", Url.Content("~/Images/check.png"));
                return engineer.Available ? checkHtml : "";
            }
            );
            string codeFormat = "<pre>{0}</pre>";
            CustomPropertyDisplay.Add("CodeSnippet", (record) =>
            {
                Engineer engineer = (Engineer)record;
                return string.Format(codeFormat, engineer.CodeSnippet.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;"));
            }
            );
        }
    }
}