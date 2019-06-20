using Dapper;
using ManageSystemPMSBE.DTCore;
using ManageSystemPMSBE.Models;
using System.Web.Mvc;

namespace ManageSystemPMSBE.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            using (var connection = DB.ConnectionFactoryBEPMS()) // sửa ở đây ConnectionFactoryBEPMS
            {
                int result = connection.QuerySingleOrDefault<int>("ManageSystem_CheckAccount",
                    new
                    {
                        UserName = user.UserName,
                        Password = Helper.Helper.CreateMD5(user.UserName + user.Password + Helper.Helper.key)
                    }, commandType: System.Data.CommandType.StoredProcedure);
                if (result > 0)
                {
                    Session["statusLogin"] = true;
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                    return RedirectToAction("Index");
            }
        }
        public ActionResult LogOut()
        {
            Session["statusLogin"] = null;
            return RedirectToAction("Index");
        }
    }
}