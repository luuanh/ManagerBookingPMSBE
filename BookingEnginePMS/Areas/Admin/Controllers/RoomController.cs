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
    public class RoomController : SercurityController
    {
        // GET: Admin/Room
        public ActionResult Index()
        {
            if (!CheckSecurity(40))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 40
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public ActionResult Clean()
        {
            if (!CheckSecurity(24))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 24
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Get(int RoomTypeId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Room> rooms = connection.Query<Room>("Room_Get",
                    new
                    {
                        RoomTypeId = RoomTypeId,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure).ToList();
                return Json(rooms, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetClean(int roomTypeId, int status)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Room> rooms = connection.Query<Room>("Room_GetClean",
                    new
                    {
                        RoomTypeId = roomTypeId,
                        Status = status,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure).ToList();
                return Json(rooms, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ChangeStatusRoom(int roomId, int status)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("Room_ChangeStatus",
                    new
                    {
                        RoomId = roomId,
                        Status = status
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CleanAll()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("Room_CleanAll",
                    new
                    {
                        HotelId = HotelId
                    },
                    commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Detail(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                Room room = connection.QuerySingleOrDefault<Room>("Room_Detail",
                    new
                    {
                        RoomId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(room, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Post(Room room)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                int result = connection.QuerySingleOrDefault<int>("Room_Post",
                new
                {
                    HotelId = HotelId,
                    RoomTypeId = room.RoomTypeId,
                    RoomCode = room.RoomCode,
                    Floor = room.Floor,
                    Status = room.Status
                }, commandType: CommandType.StoredProcedure);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(Room room)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("Room_Put",
                new
                {
                    RoomId = room.RoomId,
                    RoomTypeId = room.RoomTypeId,
                    Floor = room.Floor,
                    Status = room.Status
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
                connection.Execute("Room_Delete",
                    new
                    {
                        RoomId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

    }
}