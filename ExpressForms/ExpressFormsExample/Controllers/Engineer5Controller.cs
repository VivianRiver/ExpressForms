using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpressFormsExample.Controllers
{
    public class Engineer5Controller : BaseController<Engineer>
    {
        public Engineer5Controller()
            : base()
        {
            EditorViewName = "Engineer5Editor";
            IndexTitle = "Engineer Roster";

            CustomPropertyNames.Add("FavoriteLanguage", "Favorite Language");
            CustomPropertyNames.Add("CodeSnippet", "Favorite Code Snippet");
            CustomPropertyNames.Add("Available", "Available For Hire");

            CustomEditorInputs.Add("FavoriteLanguage", new ExpressForms.Inputs.ExpressFormsDropDownList()
            {
                FormName = this.FormName,
                InputName = "FavoriteLanguage",
                SelectListItems = new SelectListItem[]
                {
                    new SelectListItem() { Value="csharp", Text="C#"},
                    new SelectListItem() { Value="javascript", Text="Javascript"},
                    new SelectListItem() { Value="ruby", Text="Ruby"},
                    new SelectListItem() { Value="html", Text="HTML"},
                    new SelectListItem() { Value="xml", Text="XML"}
                }
            });
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