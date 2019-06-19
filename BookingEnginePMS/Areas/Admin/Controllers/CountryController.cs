using BookingEnginePMS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class CountryController : SercurityController
    {
        // GET: Admin/Country
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Get()
        {
            return Json(ConfigData.GetAllCountry(), JsonRequestBehavior.AllowGet);
        }
    }
}