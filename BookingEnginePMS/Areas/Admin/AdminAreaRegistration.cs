using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "index", controller = "Home", id = UrlParameter.Optional },
                namespaces: new[] { "BookingEnginePMS.Areas.Admin.Controllers" }
            );
        }
    }
}