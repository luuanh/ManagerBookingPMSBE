using Dapper;
using ManageSystemPMSBE.DTCore;
using ManageSystemPMSBE.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ManageSystemPMSBE.Controllers
{
    public class HomeController : SercurityController
    {
        public ActionResult Dashboard()
        {
            if (!CheckSecurity())
                return Redirect("/Login/Index");
            return View();
        }
        public JsonResult GetDashboard()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                using (var multi = connection.QueryMultiple("ManageSystem_Dashboard", commandType: CommandType.StoredProcedure))
                {
                    int totalHotel = multi.Read<int>().SingleOrDefault();
                    int totalHotelUsePMS = multi.Read<int>().SingleOrDefault();
                    int totalHotelUseBE = multi.Read<int>().SingleOrDefault();
                    int totalHotelUseBEPMS = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        totalHotel = totalHotel,
                        totalHotelUsePMS = totalHotelUsePMS,
                        totalHotelUseBE = totalHotelUseBE,
                        totalHotelUseBEPMS = totalHotelUseBEPMS
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }

      
    }
}