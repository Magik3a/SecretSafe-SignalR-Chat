using System.Web;
using System.Web.Mvc;

namespace SecretSafe
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequreSecureConnectionFilter());
        }

    }
}
