using System.Web;
using System.Web.Optimization;

namespace BAL9035
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryUI").Include(
                       "~/Scripts/jquery-ui-1.12.1.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/js/validate9035.js",
                "~/Scripts/js/filter.js",
                "~/Scripts/js/saveData.js",
                "~/Scripts/js/addCredentials.js",
                "~/Scripts/js/ddlMethods.js",
                "~/Scripts/js/saveDataLoc.js",
                "~/Scripts/js/sectionFPopup.js",
                "~/Scripts/js/modalEvents.js",
                "~/Scripts/js/comboBoxUiWidget.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/ddlCounty").Include(
                "~/Scripts/ddlCounty/ddlCounty.js",
                "~/Scripts/ddlCounty/ddlSmallCounty.js"
                ));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/style.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/jqueryUIcss").Include(
                      "~/Content/themes/base/jquery-ui.min.css"
                      ));
        }
    }
}
