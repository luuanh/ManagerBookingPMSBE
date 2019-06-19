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
    public class ChatController : SercurityController
    {
        public JsonResult GetAllUser()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<User> users = connection.Query<User>("User_GetAll",
                    new
                    {
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure).ToList();
                users.RemoveAll(x => x.UserName == "duongnx");
                return Json(users, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetMessage(string friend)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            User user = (User)Session["User"];
            if (user.UserName == friend)
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<UserChat> messages = connection.Query<UserChat>("UserChat_Detail",
                    new
                    {
                        FirstUser = user.UserName,
                        SecondUser = friend
                    }, commandType: CommandType.StoredProcedure).ToList();
                return Json(JsonConvert.SerializeObject(messages), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAllMessageAndNotify()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            User user = (User)Session["User"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("User_GetAllMessageAndNotify",
                    new
                    {
                        UserName = user.UserName,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<UserChat> userChats = multi.Read<UserChat>().ToList();
                    List<Notification> notifications = multi.Read<Notification>().ToList();

                    if (userChats is null) userChats = new List<UserChat>();
                    if (notifications is null) notifications = new List<Notification>();
                    userChats.ForEach(x =>
                    {
                        x.Read = x.UserSend == user.UserName;
                    });

                    int numberMessageNotRead = userChats.Count(x => !x.Read);
                    int numberNotify = notifications.Count(x => x.Status);
                    return Json(JsonConvert.SerializeObject(new
                    {
                        userChats = userChats,
                        notifications = notifications,
                        numberMessageNotRead = numberMessageNotRead,
                        numberNotify = numberNotify
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult SendMessage(string firstUser, string secondUser, string message)
        {
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("UserChat_Post",
                new
                {
                    HotelId = HotelId,
                    FirstUser = firstUser,
                    SecondUser = secondUser,
                    Date = DatetimeHelper.DateTimeUTCNow(),
                    Content = message,
                    UserSend = firstUser
                }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}