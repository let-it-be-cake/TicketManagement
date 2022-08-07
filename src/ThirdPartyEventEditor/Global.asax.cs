using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ThirdPartyEventEditor.Filters;

namespace ThirdPartyEventEditor
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            string appDataPath = Server.MapPath("~") + @"App_Data\";
            string jsonPath = appDataPath +
                ConfigurationManager.AppSettings["ConnectionFile"];

            GlobalFilters.Filters.Add(new ExceptionHandler());
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DIConfig.RegisterDI(jsonPath);
        }
    }
}