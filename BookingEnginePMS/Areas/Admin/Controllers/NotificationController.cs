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
    public class NotificationController : SercurityController
    {
        // GET: Admin/Notification
        public ActionResult Index()
        {
            if (!CheckSecurity(60))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 60
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Get(string keySearch, int pageNumber, int pageSize)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            User user = (User)Session["User"];
            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Notification_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        LanguageId = LanguageId,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Notification> notifications = multi.Read<Notification>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    if (notifications is null) notifications = new List<Notification>();
                    notifications.ForEach(x =>
                    {
                        x.AllowDelete = x.UserCreate == user.UserName;
                    });
                    return Json(JsonConvert.SerializeObject(new
                    {
                        notifications = notifications,
                        totalRecord = totalRecord
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Detail(int id)
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
                using (var multi = connection.QueryMultiple("Notification_Detail_Full",
                    new
                    {
                        NotificationId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    Notification notification = multi.Read<Notification>().SingleOrDefault();
                    List<UserNotification> userNotifications = multi.Read<UserNotification>().ToList();
                    users.ForEach(x =>
                    {
                        int index = userNotifications.FindIndex(y => y.UserName == x.UserName);
                        if (index < 0)
                            x.Access = false;
                        else
                            x.Access = userNotifications[index].Access;
                    });

                    return Json(JsonConvert.SerializeObject(new
                    {
                        notification = notification,
                        users = users
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Post(Notification notification, List<User> users)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            if (users is null) users = new List<User>();
            int HotelId = (int)Session["HotelId"];
            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    int notificationId = connection.QuerySingleOrDefault<int>("Notification_Post",
                        new
                        {
                            HotelId = HotelId,
                            Date = notification.Date,
                            Title = notification.Title,
                            Content = notification.Content,
                            CreateDate = DatetimeHelper.DateTimeUTCNow(),
                            UserCreate = user.UserName,

                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    users.ForEach(x =>
                    {
                        connection.Execute("UserNotification_Post",
                            new
                            {
                                UserName = x.UserName,
                                NotificationId = notificationId,
                                Access = x.Access
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(Notification notification, List<User> users)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            if (users is null) users = new List<User>();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute("Notification_Put",
                        new
                        {
                            NotificationId = notification.NotificationId,
                            Date = notification.Date,
                            Title = notification.Title,
                            Content = notification.Content,
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                    connection.Execute("UserNotification_Delete",
                        new
                        {
                            NotificationId = notification.NotificationId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                    users.ForEach(x =>
                        {
                            connection.Execute("UserNotification_Post",
                                new
                                {
                                    UserName = x.UserName,
                                    NotificationId = notification.NotificationId,
                                    Access = x.Access
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Delete(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("Notification_Delete",
                    new
                    {
                        NotificationId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ReadNotify(int NotificationId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];
            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("UserNotification_Read",
                    new
                    {
                        NotificationId = NotificationId,
                        UserName = user.UserName
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
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
                return Json(users, JsonRequestBehavior.AllowGet);
            }
        }
    }
}