using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GuitarShop.Providers;

namespace GuitarShop
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            DependencyResolver.SetResolver(new SenderDependencyResolver());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
