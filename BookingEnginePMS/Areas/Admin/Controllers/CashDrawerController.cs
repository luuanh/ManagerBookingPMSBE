using BookingEnginePMS.Helper;
using BookingEnginePMS.Models;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class CashDrawerController : SercurityController
    {
        // GET: Admin/CashDrawer
        public ActionResult Index()
        {
            if (!CheckSecurity(22))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 22
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public JsonResult Get(string keySearch = "", int pageNumber = 1, int pageSize = 1000)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("CashDrawer_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<CashDrawer> cashDrawers = multi.Read<CashDrawer>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    int cashIdUsed = connection.QuerySingleOrDefault<int>("CashDrawer_GetUsed",
                        new
                        {
                            HotelId = HotelId
                        }, commandType: CommandType.StoredProcedure);
                    if (cashIdUsed > 0)
                        cashDrawers.Find(x => x.CashDrawerId == cashIdUsed).Active = 3;
                    return Json(JsonConvert.SerializeObject(new
                    {
                        cashDrawers = cashDrawers,
                        totalRecord = totalRecord
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Detail(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                CashDrawer cashDrawer = connection.QuerySingleOrDefault<CashDrawer>("CashDrawer_Detail",
                    new
                    {
                        CashDrawerId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(JsonConvert.SerializeObject(cashDrawer), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Post(CashDrawer cashDrawer)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute("CashDrawer_Post",
                        new
                        {
                            HotelId = HotelId,
                            Name = cashDrawer.Name,
                            LastBalance = cashDrawer.LastBalance
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(CashDrawer cashDrawer)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("CashDrawer_Put",
                    new
                    {
                        CashDrawerId = cashDrawer.CashDrawerId,
                        Name = cashDrawer.Name
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("CashDrawer_Delete",
                    new
                    {
                        HotelId = HotelId,
                        CashDrawerId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        // cash drawer advance
        public JsonResult GetStatusDrawer()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                bool status = connection.QuerySingleOrDefault<bool>("CashDrawer_GetStatusDrawer",
                    new
                    {
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure);
                return Json(status, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCashDrawer()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<CashDrawer> cashDrawers = connection.Query<CashDrawer>("CashDrawer_Get_All_Active",
                    new
                    {
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure).ToList();
                return Json(cashDrawers, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDrawerLastest()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                var cashDrawers = connection.QuerySingleOrDefault<dynamic>("CashDrawer_Get_DrawerLastest",
                    new
                    {
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure);
                return Json(JsonConvert.SerializeObject(cashDrawers), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult OpenDrawer(int cashDrawerId, float startBalance, string noteOpen)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            User user = (User)Session["User"];
            DateTime datetimeNow = DatetimeHelper.DateTimeUTCNow();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("CashHistory_Post",
                    new
                    {
                        HotelId = HotelId,
                        CashDrawerId = cashDrawerId,
                        DateOpened = datetimeNow,
                        StartBalance = startBalance,
                        NoteOpen = noteOpen,
                        UserSession = user.UserName
                    }, commandType: CommandType.StoredProcedure);
                connection.Execute("CashDrawer_Put_RealTime",
                    new
                    {
                        CashDrawerId = cashDrawerId,
                        LastOpen = datetimeNow,
                        LastOpenedBy = user.UserName,
                        LastBalance = startBalance
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CloseCashDrawer(CashHistory cashHistory)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute("CashDrawer_CloseDrawer",
                    new
                    {
                        CashHistoryId = cashHistory.CashHistoryId,
                        DrawerBalance = cashHistory.DrawerBalance,
                        CashDrop = cashHistory.CashDrop,
                        NoteClose = cashHistory.NoteClose,
                        LastClose = DatetimeHelper.DateTimeUTCNow()
                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);

                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}