using System.Web;
using System.Web.Optimization;

namespace TCTE
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/assets/js/jquery-1.11.1.min.js"));
			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate.min.js",
						"~/Scripts/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/xenon").Include(
                      "~/assets/js/bootstrap.min.js",
                      "~/assets/js/TweenMax.min.js",
                      "~/assets/js/resizeable.js",
                      "~/assets/js/joinable.js",
                      "~/assets/js/xenon-api.js",
                      "~/assets/js/xenon-toggles.js",
                      "~/assets/js/xenon-custom.js"
            ));

			bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
						"~/assets/js/datatables/js/jquery.dataTables.min.js",
						"~/assets/js/datatables/dataTables.bootstrap.js",
						"~/assets/js/datatables/yadcf/jquery.dataTables.yadcf.js",
						"~/assets/js/datatables/tabletools/dataTables.tableTools.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"
						));

			bundles.Add(new StyleBundle("~/Content/css/datatable").Include(
						"~/assets/js/datatables/dataTables.bootstrap.css"));


            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/assets/css/fonts/linecons/css/linecons.css" ,
                "~/assets/css/fonts/fontawesome/css/font-awesome.min.css",
                "~/assets/css/bootstrap.css",
                "~/assets/css/xenon-core.css",
                "~/assets/css/xenon-forms.css",
                "~/assets/css/xenon-components.css",
                "~/assets/css/xenon-skins.css",
                "~/assets/css/custom.css"
            ));
        }
    }
}
