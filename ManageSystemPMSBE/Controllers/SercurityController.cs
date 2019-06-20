using System.Web.Mvc;

namespace ManageSystemPMSBE.Controllers
{
    public class SercurityController : Controller
    {
        public bool CheckSecurity()
        {
            return Session["statusLogin"] != null && (bool)Session["statusLogin"] == true;
        }
    }
}