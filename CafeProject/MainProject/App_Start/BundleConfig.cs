using System.Web;
using System.Web.Optimization;

namespace MainProject.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/siteCustom").Include("~/Scripts/IndexCustom.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"));



        }
    }
}