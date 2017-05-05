using System.Web;
using System.Web.Optimization;

namespace FashionStore
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/homejs").Include(
                       "~/Scripts/bootstrap.js",
                        "~/Scripts/easyResponsiveTabs.js",
                        "~/Scripts/imagezoom.js",
                        "~/Scripts/jquery.flexisel.js",
                        "~/Scripts/jquery.flexslider.js",
                        "~/Scripts/jquery.wmuSlider.js",
                        "~/Scripts/jquery-1.11.1.min.js",
                        "~/Scripts/minicart.js"));
 

            bundles.Add(new ScriptBundle("~/bundles/loginjs").Include(
                               "~/Scripts/jquery.min.js",
                                "~/Scripts/jquery-ui.min.js",
                                "~/Scripts/jquery-migrate-1.2.1.js"));
     

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/homecss").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/flexslider.css",
                        "~/Content/fontawesome/font-awesome.css",
                        "~/Content/fontawesome/font-awesome.min.css",
                        "~/Content/style.css"));

            bundles.Add(new StyleBundle("~/Content/logincss").Include(
                             "~/Content/bootstrap.css",
                             "~/Content/metisMenu.css",
                             "~/Content/sb-admin-2.css"));

            
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}