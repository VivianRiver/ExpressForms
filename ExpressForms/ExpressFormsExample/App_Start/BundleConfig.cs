using System.Web;
using System.Web.Optimization;

namespace ExpressFormsExample
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        new [] {
                            "~/Scripts/jquery-{version}.js",                            
                        }));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/ExpressForms").Include(
                new[] {
                    "~/Scripts/ExpressForms-{version}.js",
                    "~/Scripts/ExpressForms.Inputs-{version}.js",
                    "~/Scripts/ExpressForms.Filters-{version}.js",
                    "~/Scripts/ExpressForms.Buttons-{version}.js",
                    "~/Scripts/ExpressForms.Ace-{version}.js"
                }));

            // This script is separate because we don't want to use it on every page.
            bundles.Add(new ScriptBundle("~/bundles/ExpressForms.jquery.dataTables").Include(
                new[] {
                    "~/Scripts/ExpressForms.jquery.dataTables-{version}.js"
                }));
        }
    }
}