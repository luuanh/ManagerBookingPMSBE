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
    public class OrderController : SercurityController
    {
        // GET: Admin/Order
        public ActionResult Index()
        {
            if (!CheckSecurity(10))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            List<string> countries = ConfigData.GetAllCountry();
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 10
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public ActionResult NewOrder()
        {
            if (!CheckSecurity())
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            List<string> countries = ConfigData.GetAllCountry();
            using (var connection = DB.ConnectionFactory())
            {
                List<Guest_Sample> guests = connection.Query<Guest_Sample>("Guest_GetAll_Sample",
                     new
                     {
                         HotelId = HotelId
                     }, commandType: CommandType.StoredProcedure).ToList();
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 9
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                ViewData["countries"] = countries;
                ViewData["guests"] = guests;
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            if (!CheckSecurity())
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            List<string> countries = ConfigData.GetAllCountry();
            using (var connection = DB.ConnectionFactory())
            {
                List<Guest_Sample> guests = connection.Query<Guest_Sample>("Guest_GetAll_Sample",
                     new
                     {
                         HotelId = HotelId
                     }, commandType: CommandType.StoredProcedure).ToList();
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 9
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                ViewData["countries"] = countries;
                ViewData["guests"] = guests;
                return View();
            }
        }
        public JsonResult Post(Guest guest, List<ReservationService> services)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (guest is null) guest = new Guest();
            if (services is null) services = new List<ReservationService>();
            int HotelId = (int)Session["HotelId"];
            User user = (User)Session["User"];
            DateTime datetimeNow = DatetimeHelper.DateTimeUTCNow();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    if (guest.GuestId < 0)
                    {
                        int guestId = connection.QuerySingleOrDefault<int>("Guest_Post_Order",
                            new
                            {
                                HotelId = HotelId,
                                FirstName = guest.FirstName,
                                SurName = guest.SurName,
                                Country = guest.Country,
                                Phone = guest.Phone,
                                Email = guest.Email,
                                Note = guest.Note,
                                DateCreate = datetimeNow,
                                CreateBySource = "PMS"
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                        guest.GuestId = guestId;
                    }
                    int guestOrderId = connection.QuerySingleOrDefault<int>("GuestOrder_Post",
                        new
                        {
                            GuestId = guest.GuestId,
                            CreateDate = datetimeNow,
                            CreateBy = user.UserName,
                            HotelId = HotelId
                        },
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    services.ForEach(x =>
                    {
                        connection.Execute("GuestOrderService_Post",
                            new
                            {
                                GuestOrderId = guestOrderId,
                                ServiceId = x.ServiceId,
                                Number = x.Number,
                                Price = x.Price
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }

        }
        public JsonResult Get(int status, string keySearch, DateTime fromDate, DateTime toDate,
                        int pageNumber = 1, int pageSize = 100)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("GuestOrder_Get",
                    new
                    {
                        HotelId = HotelId,
                        status = status,
                        keySearch = keySearch,
                        fromDate = fromDate,
                        toDate = toDate,
                        pageNumber = pageNumber,
                        pageSize = pageSize
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<GuestOrder> guestOrders = multi.Read<GuestOrder>().ToList();
                    int totalItems = multi.Read<int>().SingleOrDefault();
                    if (guestOrders is null)
                    {
                        totalItems = 0;
                        guestOrders = new List<GuestOrder>();
                    }
                    return Json(JsonConvert.SerializeObject(new
                    {
                        guestOrders = guestOrders,
                        totalItems = totalItems
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Detail(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("GuestOrder_Detail",
                    new
                    {
                        GuestOrderId = id,
                        LanguageId = LanguageId
                    }, commandType: CommandType.StoredProcedure))
                {
                    GuestOrder guestOrder = multi.Read<GuestOrder>().SingleOrDefault();
                    Guest guest = multi.Read<Guest>().SingleOrDefault();
                    List<ReservationService> services = multi.Read<ReservationService>().ToList();
                    if (guest is null) guest = new Guest();
                    if (services is null) services = new List<ReservationService>();
                    return Json(new
                    {
                        guest = guest,
                        services = services,
                        status = guestOrder.Paid,
                        GuestOrderCode = guestOrder.GuestOrderCode
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(Guest guest, List<ReservationService> services, int guestOrderId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (guest is null) guest = new Guest();
            if (services is null) services = new List<ReservationService>();
            int HotelId = (int)Session["HotelId"];
            User user = (User)Session["User"];
            DateTime datetimeNow = DatetimeHelper.DateTimeUTCNow();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    if (guest.GuestId < 0)
                    {
                        int guestId = connection.QuerySingleOrDefault<int>("Guest_Post_Order",
                            new
                            {
                                HotelId = HotelId,
                                FirstName = guest.FirstName,
                                SurName = guest.SurName,
                                Country = guest.Country,
                                Phone = guest.Phone,
                                Email = guest.Email,
                                Note = guest.Note,
                                DateCreate = datetimeNow,
                                CreateBySource = "PMS"
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                        guest.GuestId = guestId;
                    }
                    else
                    {
                        connection.Execute("Guest_Put_Order",
                            new
                            {
                                GuestId = guest.GuestId,
                                FirstName = guest.FirstName,
                                SurName = guest.SurName,
                                Country = guest.Country,
                                Phone = guest.Phone,
                                Email = guest.Email,
                                Note = guest.Note
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    }
                    connection.Execute("GuestOrder_Put",
                        new
                        {
                            GuestOrderId = guestOrderId,
                            GuestId = guest.GuestId,
                        },
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    connection.Execute("GuestOrder_DeleteAllService",
                        new
                        {
                            GuestOrderId = guestOrderId
                        },
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    services.ForEach(x =>
                    {
                        connection.Execute("GuestOrderService_Post",
                            new
                            {
                                GuestOrderId = guestOrderId,
                                ServiceId = x.ServiceId,
                                Number = x.Number,
                                Price = x.Price
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }

        }
        public JsonResult CloseOrder(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("GuestOrder_Paid",
                    new
                    {
                        GuestOrderId = id,
                        Cashier = user.UserName
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AssignOrderBooking(int bookingId, int guestOrderId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute("GuestOrder_Assign_Booking",
                    new
                    {
                        GuestOrderId = guestOrderId,
                        BookingId = bookingId
                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        // chuyển hóa đơn sang booking
        // Chọn reservation
        // asign tới booking ( booking trong trạng thái new,checkin )
    }
}