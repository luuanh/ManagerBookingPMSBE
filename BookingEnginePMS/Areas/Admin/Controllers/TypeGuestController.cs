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
    public class TypeGuestController : SercurityController
    {
        // GET: Admin/TypeGuest
        public ActionResult Index()
        {
            if (!CheckSecurity(43))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 43
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Get(string keySearch = "", int pageNumber = 1, int pageSize = 1000)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("TypeGuest_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<TypeGuest> typeGuests = multi.Read<TypeGuest>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        typeGuests = typeGuests,
                        totalRecord = totalRecord
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Detail(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                TypeGuest typeGuest = connection.QuerySingleOrDefault<TypeGuest>("TypeGuest_Detail",
                    new
                    {
                        TypeGuestId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(typeGuest, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Post(TypeGuest typeGuest)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("TypeGuest_Post",
                    new
                    {
                        HotelId = HotelId,
                        TypeGuestName = typeGuest.TypeGuestName,
                        MinAmount = typeGuest.MinAmount,
                        Discount = typeGuest.Discount
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(TypeGuest typeGuest)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("TypeGuest_Put",
                    new
                    {
                        TypeGuestId = typeGuest.TypeGuestId,
                        TypeGuestName = typeGuest.TypeGuestName,
                        MinAmount = typeGuest.MinAmount,
                        Discount = typeGuest.Discount
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("TypeGuest_Delete",
                    new
                    {
                        TypeGuestId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}