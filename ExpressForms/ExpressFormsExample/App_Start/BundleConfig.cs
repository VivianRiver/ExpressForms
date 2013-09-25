using System.Web;
using System.Web.Optimization;

namespace ExpressFormsExample
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // jquery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"            
            ));

            //jquery-ui
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/ExpressForms").Include(
                new[] {
                    "~/Scripts/ExpressForms-{version}.js",
                    "~/Scripts/ExpressForms.Inputs-{version}.js",
                    "~/Scripts/ExpressForms.Buttons-{version}.js",
                    "~/Scripts/ExpressForms.Filters-{version}.js",
                    "~/Scripts/ExpressForms.Ace-{version}.js"
                }));

            // This script is separate because we don't want to use it on every page.
            bundles.Add(new ScriptBundle("~/bundles/ExpressForms.jquery.dataTables").Include(
                new[] {
                    "~/Scripts/ExpressForms.jquery.dataTables-{version}.js"
                }));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        //"~/Content/themes/base/jquery.ui.core.css",
                        //"~/Content/themes/base/jquery.ui.resizable.css",
                        //"~/Content/themes/base/jquery.ui.selectable.css",
                        //"~/Content/themes/base/jquery.ui.accordion.css",
                        //"~/Content/themes/base/jquery.ui.autocomplete.css",
                        //"~/Content/themes/base/jquery.ui.button.css",
                        //"~/Content/themes/base/jquery.ui.dialog.css",
                        //"~/Content/themes/base/jquery.ui.slider.css",
                        //"~/Content/themes/base/jquery.ui.tabs.css",
                        //"~/Content/themes/base/jquery.ui.datepicker.css",
                        //"~/Content/themes/base/jquery.ui.progressbar.css",
                        //"~/Content/themes/base/jquery.ui.theme.css"
                        "~/Content/themes/base/jquery.ui.*"
                        ));
        }
    }
}