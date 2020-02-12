using DoCover.Filter;
using System.Web;
using System.Web.Mvc;

namespace DoCover
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckFilterAttribute());
        }
    }
}
