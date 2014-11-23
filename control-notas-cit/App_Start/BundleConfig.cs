using System.Web.Optimization;

namespace IdentitySample
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //           "~/Scripts/modernizr-*"));

            /*bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Content/js/jquery-migrate-1.0.0.min.js",
                      "~/Content/js/jquery-ui-1.10.0.custom.min.js",
                      "~/Content/js/jquery.ui.touch-punch.js",
                      "~/Content/js/modernizr.js",
                      "~/Content/js/bootstrap.min.js",
                      "~/Content/js/jquery.cookie.js",
                      "~/Content/js/fullcalendar.min.js",
                      "~/Content/js/jquery.dataTables.min.js",
                      "~/Content/js/excanvas.js",
                      "~/Content/js/jquery.flot.js",
                      "~/Content/js/jquery.flot.pie.js",
                      "~/Content/js/jquery.flot.stack.js",
                      "~/Content/js/jquery.flot.resize.min.js",
                      "~/Content/js/jquery.dataTables.min.js",
                      "~/Content/js/jquery.dataTables.min.js",
                      "~/Content/js/jquery.dataTables.min.js",
                      "~/Content/js/jquery.dataTables.min.js",
                      "~/Content/js/jquery.dataTables.min.js",
                      "~/Content/js/*.js"));
            */
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/*.css"));
        }
    }
}
