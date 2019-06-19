using BookingEnginePMS.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class AccomPriceHourController : SercurityController
    {
        // GET: Admin/AccomPriceHour
        public ActionResult Index()
        {
            if (!CheckSecurity(42))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 42
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Get(int RoomTypeId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                RoomTypePriceHour roomTypePriceHour = connection.QuerySingleOrDefault<RoomTypePriceHour>("RoomTypePriceHour_Get",
                    new
                    {
                        RoomTypeId = RoomTypeId
                    }, commandType: CommandType.StoredProcedure);
                if (roomTypePriceHour is null)
                    roomTypePriceHour = new RoomTypePriceHour();
                return Json(roomTypePriceHour, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(RoomTypePriceHour roomTypePriceHour)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("RoomTypePriceHour_Put",
                new
                {
                    RoomTypeId = roomTypePriceHour.RoomTypeId,
                    HourStart = roomTypePriceHour.HourStart,
                    PriceStart = roomTypePriceHour.PriceStart,
                    HourNext1 = roomTypePriceHour.HourNext1,
                    PriceNext1 = roomTypePriceHour.PriceNext1,
                    HourNext2 = roomTypePriceHour.HourNext2,
                    PriceNext2 = roomTypePriceHour.PriceNext2,
                    HourNext3 = roomTypePriceHour.HourNext3,
                    PriceNext3 = roomTypePriceHour.PriceNext3

                }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}