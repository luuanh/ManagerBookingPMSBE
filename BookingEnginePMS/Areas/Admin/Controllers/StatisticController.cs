using BookingEnginePMS.Helper;
using BookingEnginePMS.Models;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class RoomType_Statistic
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public int TotalRoom { get; set; }
        public List<int> NumberRoomAvailable { get; set; }
    }
    public class Report_AvailableAccommodations
    {
        public List<DateTime> dateTimes { get; set; }
        public List<RoomType_Statistic> roomTypes { get; set; }
        public List<int> totalNumberAvailable { get; set; }
        public int TotalRoomOfAllRoomType { get; set; }
        public Report_AvailableAccommodations(List<DateTime> dates, List<RoomType_Statistic> roomType_Statistics, List<int> totalNumberAvail, int total)
        {
            dateTimes = dates;
            roomTypes = roomType_Statistics;
            totalNumberAvailable = totalNumberAvail;
            TotalRoomOfAllRoomType = total;
        }
    }
    public class ServiceCategory_Statistic
    {
        public int ServiceCategoryId { get; set; }
        public string ServiceCategoryName { get; set; }
        public double TotalPrice { get; set; }
    }
    public class Extrabed_Statistic
    {
        public int ExtrabedId { get; set; }
        public string ExtrabedName { get; set; }
        public double TotalPrice { get; set; }
    }
    public class StatisticController : SercurityController
    {
        // GET: Admin/Statistic
        public ActionResult CheckIn()
        {
            if (!CheckSecurity(23))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 23
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult CheckOut()
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
                           screenId = 23
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult Inhouse()
        {
            if (!CheckSecurity(25))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 23
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult NoShow()
        {
            if (!CheckSecurity(26))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 23
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult Cancel()
        {
            if (!CheckSecurity(27))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 23
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult Activity()
        {
            if (!CheckSecurity(28))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 23
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult GuestCheckIn()
        {
            if (!CheckSecurity(29))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 23
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult GuestCheckOut()
        {
            if (!CheckSecurity(30))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 23
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult AvailableAccommodations()
        {
            if (!CheckSecurity(31))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 23
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult InvoiceBreakDown()
        {
            if (!CheckSecurity(32))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 23
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult CashDrawerReport()
        {
            if (!CheckSecurity(59))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 59
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult SummaryReport()
        {
            if (!CheckSecurity(62))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 62
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult ReportByUser()
        {
            if (!CheckSecurity(63))
                return Redirect("/Admin/Login/Index");
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 4
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public ActionResult ReportAllUser()
        {
            if (!CheckSecurity(64))
                return Redirect("/Admin/Login/Index");
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 4
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public ActionResult StatisticRevenue()
        {
            if (!CheckSecurity(65))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 65
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }

        public JsonResult GetCheckIn(DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Statistic_Get_CheckIn",
                    new
                    {
                        HotelId = HotelId,
                        FromDate = fromDate,
                        ToDate = toDate
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (bookings is null) bookings = new List<Booking_Reservation>();
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                        new
                        {
                            HotelId = HotelId
                        },
                        commandType: CommandType.StoredProcedure);
                bookings.ForEach(y =>
                {
                    float taxFeesForBooking = 0;
                    using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = y.BookingId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                        y.TotalRoom = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalExtrabed = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalService = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);

                        if (includeTaxFee)
                            taxFeesForBooking = taxFees;
                        float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                        float discount = (float)Math.Round(total * y.Discount / 100 + 0.001, 0);
                        y.Total = (float)Math.Round((total - discount) * (100 + taxFeesForBooking) / 100, 0);
                    }
                });
                float totalAmount = bookings.Sum(x => x.Total);
                float totalPaid = bookings.Sum(x => x.PrePaid + x.Paid);
                Session["dataReport"] = bookings;
                return Json(JsonConvert.SerializeObject(new
                {
                    bookings = bookings,
                    totalAmount = Math.Round(totalAmount, 0),
                    totalPaid = Math.Round(totalPaid, 0)
                }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCheckOut(DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Statistic_Get_CheckOut",
                    new
                    {
                        HotelId = HotelId,
                        FromDate = fromDate,
                        ToDate = toDate
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (bookings is null) bookings = new List<Booking_Reservation>();
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                        new
                        {
                            HotelId = HotelId
                        },
                        commandType: CommandType.StoredProcedure);
                bookings.ForEach(y =>
                {
                    float taxFeesForBooking = 0;
                    using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = y.BookingId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                        y.TotalRoom = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalExtrabed = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalService = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);

                        if (includeTaxFee)
                            taxFeesForBooking = taxFees;
                        float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                        float discount = (float)Math.Round(total * y.Discount / 100 + 0.001, 0);
                        y.Total = (float)Math.Round((total - discount) * (100 + taxFeesForBooking) / 100, 0);
                    }
                });
                float totalAmount = bookings.Sum(x => x.Total);
                float totalPaid = bookings.Sum(x => x.PrePaid + x.Paid);
                Session["dataReport"] = bookings;
                return Json(JsonConvert.SerializeObject(new
                {
                    bookings = bookings,
                    totalAmount = Math.Round(totalAmount, 0),
                    totalPaid = Math.Round(totalPaid, 0)
                }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetInhouse(DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Statistic_Get_Inhouse",
                    new
                    {
                        HotelId = HotelId,
                        FromDate = fromDate,
                        ToDate = toDate
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (bookings is null) bookings = new List<Booking_Reservation>();
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                        new
                        {
                            HotelId = HotelId
                        },
                        commandType: CommandType.StoredProcedure);
                bookings.ForEach(y =>
                {
                    float taxFeesForBooking = 0;
                    using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = y.BookingId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                        y.TotalRoom = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalExtrabed = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalService = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);

                        if (includeTaxFee)
                            taxFeesForBooking = taxFees;
                        float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                        float discount = (float)Math.Round(total * y.Discount / 100 + 0.001, 0);
                        y.Total = (float)Math.Round((total - discount) * (100 + taxFeesForBooking) / 100, 0);
                    }
                });
                float totalAmount = bookings.Sum(x => x.Total);
                float totalPaid = bookings.Sum(x => x.PrePaid + x.Paid);
                Session["dataReport"] = bookings;
                return Json(JsonConvert.SerializeObject(new
                {
                    bookings = bookings,
                    totalAmount = Math.Round(totalAmount, 0),
                    totalPaid = Math.Round(totalPaid, 0)
                }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetNoShow(DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Statistic_Get_NoShow",
                    new
                    {
                        HotelId = HotelId,
                        FromDate = fromDate,
                        ToDate = toDate
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (bookings is null) bookings = new List<Booking_Reservation>();
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                        new
                        {
                            HotelId = HotelId
                        },
                        commandType: CommandType.StoredProcedure);
                bookings.ForEach(y =>
                {
                    float taxFeesForBooking = 0;
                    using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = y.BookingId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                        y.TotalRoom = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalExtrabed = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalService = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);

                        if (includeTaxFee)
                            taxFeesForBooking = taxFees;
                        float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                        float discount = (float)Math.Round(total * y.Discount / 100 + 0.001, 0);
                        y.Total = (float)Math.Round((total - discount) * (100 + taxFeesForBooking) / 100, 0);
                    }
                });
                float totalAmount = bookings.Sum(x => x.Total);
                float totalPaid = bookings.Sum(x => x.PrePaid + x.Paid);
                Session["dataReport"] = bookings;
                return Json(JsonConvert.SerializeObject(new
                {
                    bookings = bookings,
                    totalAmount = Math.Round(totalAmount, 0),
                    totalPaid = Math.Round(totalPaid, 0)
                }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCancel(DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Statistic_Get_Cancel",
                    new
                    {
                        HotelId = HotelId,
                        FromDate = fromDate,
                        ToDate = toDate
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (bookings is null) bookings = new List<Booking_Reservation>();
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                        new
                        {
                            HotelId = HotelId
                        },
                        commandType: CommandType.StoredProcedure);
                bookings.ForEach(y =>
                {
                    float taxFeesForBooking = 0;
                    using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = y.BookingId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                        y.TotalRoom = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalExtrabed = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalService = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);

                        if (includeTaxFee)
                            taxFeesForBooking = taxFees;
                        float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                        float discount = (float)Math.Round(total * y.Discount / 100 + 0.001, 0);
                        y.Total = (float)Math.Round((total - discount) * (100 + taxFeesForBooking) / 100, 0);
                    }
                });
                float totalAmount = bookings.Sum(x => x.Total);
                float totalPaid = bookings.Sum(x => x.PrePaid + x.Paid);
                Session["dataReport"] = bookings;
                return Json(JsonConvert.SerializeObject(new
                {
                    bookings = bookings,
                    totalAmount = Math.Round(totalAmount, 0),
                    totalPaid = Math.Round(totalPaid, 0)
                }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetActivity(DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Statistic_Get_Activity",
                    new
                    {
                        HotelId = HotelId,
                        FromDate = fromDate,
                        ToDate = toDate
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (bookings is null) bookings = new List<Booking_Reservation>();
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                        new
                        {
                            HotelId = HotelId
                        },
                        commandType: CommandType.StoredProcedure);
                bookings.ForEach(y =>
                {
                    float taxFeesForBooking = 0;
                    using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = y.BookingId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                        y.TotalRoom = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalExtrabed = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalService = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);

                        if (includeTaxFee)
                            taxFeesForBooking = taxFees;
                        float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                        float discount = (float)Math.Round(total * y.Discount / 100 + 0.001, 0);
                        y.Total = (float)Math.Round((total - discount) * (100 + taxFeesForBooking) / 100, 0);
                    }
                });
                float totalAmount = bookings.Sum(x => x.Total);
                float totalPaid = bookings.Sum(x => x.PrePaid + x.Paid);
                Session["dataReport"] = bookings;
                return Json(JsonConvert.SerializeObject(new
                {
                    bookings = bookings,
                    totalAmount = Math.Round(totalAmount, 0),
                    totalPaid = Math.Round(totalPaid, 0)
                }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetGuestCheckIn(DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Statistic_Get_GuestCheckIn",
                    new
                    {
                        HotelId = HotelId,
                        FromDate = fromDate,
                        ToDate = toDate
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (bookings is null) bookings = new List<Booking_Reservation>();
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                        new
                        {
                            HotelId = HotelId
                        },
                        commandType: CommandType.StoredProcedure);
                bookings.ForEach(y =>
                {
                    float taxFeesForBooking = 0;
                    using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = y.BookingId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                        y.TotalRoom = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalExtrabed = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalService = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);

                        if (includeTaxFee)
                            taxFeesForBooking = taxFees;
                        float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                        float discount = (float)Math.Round(total * y.Discount / 100 + 0.001, 0);
                        y.Total = (float)Math.Round((total - discount) * (100 + taxFeesForBooking) / 100, 0);
                    }
                });
                float totalAmount = bookings.Sum(x => x.Total);
                float totalPaid = bookings.Sum(x => x.PrePaid + x.Paid);
                Session["dataReport"] = bookings;
                return Json(JsonConvert.SerializeObject(new
                {
                    bookings = bookings,
                    totalAmount = Math.Round(totalAmount, 0),
                    totalPaid = Math.Round(totalPaid, 0)
                }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetGuestCheckOut(DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Statistic_Get_GuestCheckOut",
                    new
                    {
                        HotelId = HotelId,
                        FromDate = fromDate,
                        ToDate = toDate
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (bookings is null) bookings = new List<Booking_Reservation>();
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                        new
                        {
                            HotelId = HotelId
                        },
                        commandType: CommandType.StoredProcedure);
                bookings.ForEach(y =>
                {
                    float taxFeesForBooking = 0;
                    using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = y.BookingId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                        y.TotalRoom = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalExtrabed = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalService = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);

                        if (includeTaxFee)
                            taxFeesForBooking = taxFees;
                        float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                        float discount = (float)Math.Round(total * y.Discount / 100 + 0.001, 0);
                        y.Total = (float)Math.Round((total - discount) * (100 + taxFeesForBooking) / 100, 0);
                    }
                });
                float totalAmount = bookings.Sum(x => x.Total);
                float totalPaid = bookings.Sum(x => x.PrePaid + x.Paid);
                Session["dataReport"] = bookings;
                return Json(JsonConvert.SerializeObject(new
                {
                    bookings = bookings,
                    totalAmount = Math.Round(totalAmount, 0),
                    totalPaid = Math.Round(totalPaid, 0)
                }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetNumberFutureRoom(DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            List<DateTime> dateTimes = new List<DateTime>();
            List<int> totalNumberAvailable = new List<int>();
            int TotalRoomOfAllRoomType = 0;
            for (DateTime date = fromDate.Date; date <= toDate.Date; date = date.AddDays(1))
            {
                dateTimes.Add(date);
            }
            int rangeTime = dateTimes.Count;
            using (var connection = DB.ConnectionFactory())
            {
                List<RoomType_Statistic> roomTypes = connection.Query<RoomType_Statistic>("RoomType_GetSample_PlusNumberRoom",
                    new
                    {
                        HotelId = HotelId
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                if (roomTypes is null)
                    roomTypes = new List<RoomType_Statistic>();
                roomTypes.ForEach(x =>
                {
                    if (x.NumberRoomAvailable is null) x.NumberRoomAvailable = new List<int>();
                    int numberRoomOfRoomType = connection.QuerySingleOrDefault<int>("RoomType_Get_NumberRoom",
                           new
                           {
                               RoomTypeId = x.RoomTypeId
                           }, commandType: CommandType.StoredProcedure);
                    TotalRoomOfAllRoomType += numberRoomOfRoomType;
                    dateTimes.ForEach(y =>
                    {
                        int numberRoomUsed = connection.QuerySingleOrDefault<int>("Booking_Get_NumberRoomUsed_By_RoomTypeId",
                            new
                            {
                                RoomTypeId = x.RoomTypeId,
                                Date = y
                            }, commandType: CommandType.StoredProcedure);
                        x.NumberRoomAvailable.Add(numberRoomOfRoomType - numberRoomUsed);
                    });
                });
                for (int i = 0; i < rangeTime; i++)
                {
                    int sum = 0;
                    roomTypes.ForEach(x =>
                    {
                        sum += x.NumberRoomAvailable[i];
                    });
                    totalNumberAvailable.Add(sum);
                }
                Session["dataReport"] = new Report_AvailableAccommodations(dateTimes, roomTypes, totalNumberAvailable, TotalRoomOfAllRoomType);
                return Json(JsonConvert.SerializeObject(new
                {
                    dateTimes = dateTimes,
                    roomTypes = roomTypes,
                    totalNumberAvailable = totalNumberAvailable,
                    TotalRoomOfAllRoomType = TotalRoomOfAllRoomType
                }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetInvoiceBreakDown(DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Statistic_Get_InvoiceBreakDown",
                    new
                    {
                        HotelId = HotelId,
                        FromDate = fromDate,
                        ToDate = toDate
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (bookings is null) bookings = new List<Booking_Reservation>();
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                        new
                        {
                            HotelId = HotelId
                        },
                        commandType: CommandType.StoredProcedure);
                bookings.ForEach(y =>
                {
                    float taxFeesForBooking = 0;
                    using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = y.BookingId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                        y.TotalRoom = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalExtrabed = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);
                        y.TotalService = (float)Math.Round(multi2.Read<float>().SingleOrDefault(), 0);

                        if (includeTaxFee)
                            taxFeesForBooking = taxFees;
                        float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                        float discount = (float)Math.Round(total * y.Discount / 100 + 0.001, 0);
                        y.Total = (float)Math.Round((total - discount) * (100 + taxFeesForBooking) / 100, 0);
                    }
                });
                float totalAmount = bookings.Sum(x => x.Total);
                float totalPaid = bookings.Sum(x => x.PrePaid + x.Paid);
                Session["dataReport"] = bookings;
                return Json(JsonConvert.SerializeObject(new
                {
                    bookings = bookings,
                    totalAmount = Math.Round(totalAmount, 0),
                    totalPaid = Math.Round(totalPaid, 0)
                }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCashDrawerReport(DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<CashHistory> cashHistories = connection.Query<CashHistory>("CashDrawerReport_Get",
                    new
                    {
                        HotelId = HotelId,
                        FromDate = fromDate,
                        ToDate = toDate
                    }, commandType: CommandType.StoredProcedure).ToList();

                float totalStartBalance = cashHistories.Sum(x => x.StartBalance);
                float totalDrawerBalance = cashHistories.Sum(x => x.DrawerBalance);
                float totalCashDrop = cashHistories.Sum(x => x.CashDrop);

                Session["dataReport"] = cashHistories;
                return Json(JsonConvert.SerializeObject(new
                {
                    cashHistories = cashHistories,
                    totalStartBalance = Math.Round(totalStartBalance, 0),
                    totalDrawerBalance = Math.Round(totalDrawerBalance, 0),
                    totalCashDrop = Math.Round(totalCashDrop, 0)
                }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSummaryReport(DateTime fromDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Statistic_SummaryReport",
                    new
                    {
                        Date = fromDate,
                        HotelId = HotelId,
                        LanguageId = LanguageId
                    }, commandType: CommandType.StoredProcedure))
                {
                    int inHouse = multi.Read<int>().SingleOrDefault();
                    int checkin = multi.Read<int>().SingleOrDefault();
                    int checkout = multi.Read<int>().SingleOrDefault();
                    int book = multi.Read<int>().SingleOrDefault();
                    int noShow = multi.Read<int>().SingleOrDefault();
                    int cancel = multi.Read<int>().SingleOrDefault();
                    int roomClean = multi.Read<int>().SingleOrDefault();
                    int roomDirty = multi.Read<int>().SingleOrDefault();
                    int roomRepair = multi.Read<int>().SingleOrDefault();
                    List<Booking_Reservation> bookings = multi.Read<Booking_Reservation>().ToList();
                    List<ServiceCategory_Statistic> serviceCategories = multi.Read<ServiceCategory_Statistic>().ToList();
                    List<ServiceCategory_Statistic> serviceCategoriesOfOrder = multi.Read<ServiceCategory_Statistic>().ToList();
                    float totalExtrabedsPrice = multi.Read<float>().SingleOrDefault();

                    if (bookings is null) bookings = new List<Booking_Reservation>();
                    if (serviceCategories is null) serviceCategories = new List<ServiceCategory_Statistic>();
                    if (serviceCategoriesOfOrder is null) serviceCategoriesOfOrder = new List<ServiceCategory_Statistic>();
                    serviceCategories.ForEach(x =>
                    {
                        List<ServiceCategory_Statistic> temp = serviceCategoriesOfOrder.FindAll(y => y.ServiceCategoryId == x.ServiceCategoryId);
                        if (temp is null) temp = new List<ServiceCategory_Statistic>();
                        x.TotalPrice += temp.Sum(y => y.TotalPrice);
                    });
                    double totalPriceAllRoom = 0;
                    bookings.ForEach(x => totalPriceAllRoom += x.TotalRoom);
                    double totalAllService = totalPriceAllRoom; // tổng tiền tất cả dịch vụ, tiền phòng, tiền extrabed
                    serviceCategories.ForEach(x => totalAllService += x.TotalPrice);
                    serviceCategoriesOfOrder.ForEach(x => totalAllService += x.TotalPrice);
                    totalAllService += totalExtrabedsPrice;
                    return Json(JsonConvert.SerializeObject(new
                    {
                        inHouse = inHouse,
                        checkin = checkin,
                        checkout = checkout,
                        book = book,
                        noShow = noShow,
                        cancel = cancel,
                        roomClean = roomClean,
                        roomDirty = roomDirty,
                        roomRepair = roomRepair,
                        bookings = bookings,
                        serviceCategories = serviceCategories,
                        totalExtrabedsPrice = totalExtrabedsPrice,
                        totalAllService = totalAllService,
                        totalPriceAllRoom = totalPriceAllRoom
                    }), JsonRequestBehavior.AllowGet);

                }
            }
        }
        public JsonResult GetReportAllUser(DateTime fromDate, DateTime toDate, string user)
        {
            if (!CheckSecurity(64))
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                           new
                           {
                               HotelId = HotelId
                           },
                           commandType: CommandType.StoredProcedure);
                List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Booking_GetByAllCreate",
                    new
                    {
                        UserCreate = user,
                        HotelId = HotelId,
                        fromDate = fromDate,
                        toDate = toDate
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (bookings is null) bookings = new List<Booking_Reservation>();
                bookings.ForEach(y =>
                {
                    float taxFeesForBooking = 0;
                    using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = y.BookingId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                        y.TotalRoom = multi2.Read<float>().SingleOrDefault();
                        y.TotalExtrabed = multi2.Read<float>().SingleOrDefault();
                        y.TotalService = multi2.Read<float>().SingleOrDefault();

                        if (includeTaxFee)
                            taxFeesForBooking = taxFees;

                        float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                        float discount = (float)Math.Round(total * y.Discount / 100, 0);
                        y.Total = (float)Math.Round((total - discount) * (100 + taxFeesForBooking) / 100, 0);
                    }
                });
                double totalpriceAllBooking = 0;
                bookings.ForEach(x =>
                {
                    totalpriceAllBooking += x.Total;
                });
                return Json(JsonConvert.SerializeObject(new
                {
                    bookings = bookings,
                    totalpriceAllBooking = totalpriceAllBooking
                }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetReportByUser(DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity(63))
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                           new
                           {
                               HotelId = HotelId
                           },
                           commandType: CommandType.StoredProcedure);
                List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Booking_GetByUserCreate",
                    new
                    {
                        UserCreate = user.UserName,
                        HotelId = HotelId,
                        fromDate = fromDate,
                        toDate = toDate
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (bookings is null) bookings = new List<Booking_Reservation>();
                bookings.ForEach(y =>
                {
                    float taxFeesForBooking = 0;
                    using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = y.BookingId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                        y.TotalRoom = multi2.Read<float>().SingleOrDefault();
                        y.TotalExtrabed = multi2.Read<float>().SingleOrDefault();
                        y.TotalService = multi2.Read<float>().SingleOrDefault();

                        if (includeTaxFee)
                            taxFeesForBooking = taxFees;

                        float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                        float discount = (float)Math.Round(total * y.Discount / 100, 0);
                        y.Total = (float)Math.Round((total - discount) * (100 + taxFeesForBooking) / 100, 0);
                    }
                });
                double totalpriceAllBooking = 0;
                bookings.ForEach(x =>
                {
                    totalpriceAllBooking += x.Total;
                });
                return Json(JsonConvert.SerializeObject(new
                {
                    bookings = bookings,
                    totalpriceAllBooking = totalpriceAllBooking
                }), JsonRequestBehavior.AllowGet);
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
                if (users is null) users = new List<User>();
                return Json(users, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetStatisticRevenue(int month, int year)
        {
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Statistic_GetStatisticRevenue",
                       new
                       {
                           HotelId = HotelId,
                           month = month,
                           year = year
                       },
                       commandType: System.Data.CommandType.StoredProcedure))
                {
                    List<BookingPrice> bookingPrices = multi.Read<BookingPrice>().ToList();
                    List<BookingService> bookingServices = multi.Read<BookingService>().ToList();
                    List<GuestOrderService> guestOrderServices = multi.Read<GuestOrderService>().ToList();
                    List<BookingExtrabed> bookingExtrabeds = multi.Read<BookingExtrabed>().ToList();

                    if (bookingPrices is null) bookingPrices = new List<BookingPrice>();
                    if (bookingServices is null) bookingServices = new List<BookingService>();
                    if (guestOrderServices is null) guestOrderServices = new List<GuestOrderService>();
                    if (bookingExtrabeds is null) bookingExtrabeds = new List<BookingExtrabed>();

                    int rangeDate = (new DateTime(month == 12 ? year + 1 : year, month == 12 ? 12 : month + 1, 1) - new DateTime(year, month, 1)).Days;
                    List<string> lables = new List<string>();
                    float[] booking = new float[rangeDate];
                    float[] service = new float[rangeDate];
                    float[] extrabed = new float[rangeDate];
                    float[] all = new float[rangeDate];
                    for (int i = 1; i <= rangeDate; i++)
                    {
                        lables.Add("Ngày " + i);
                        booking[i - 1] = bookingPrices.FindAll(x => x.Date.Day == i).Sum(x => x.Price);
                        service[i - 1] = bookingServices.FindAll(x => x.DateCreate.Day == i).Sum(x => x.Price) +
                                           guestOrderServices.FindAll(x => x.CreateDate.Day == i).Sum(x => x.Price);
                        extrabed[i - 1] = bookingExtrabeds.FindAll(x => x.DateCreate.Day == i).Sum(x => x.Price);
                        all[i - 1] = booking[i - 1] + service[i - 1] + extrabed[i - 1];
                    }
                    return Json(new
                    {
                        lables = lables,
                        booking = booking,
                        service = service,
                        extrabed = extrabed,
                        all = all
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Download()
        {
            if (!CheckSecurity())
                return Redirect("/Admin/Login/Index");

            string filename = "report.xlsx";
            string sourcefile = AppDomain.CurrentDomain.BaseDirectory + "Helper\\TemplateFile\\ReportReservation.xlsx";
            string path = AppDomain.CurrentDomain.BaseDirectory + "Helper\\FileData\\Excel\\" + filename;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            System.IO.File.Copy(sourcefile, path);
            List<Booking_Reservation> data = (List<Booking_Reservation>)Session["dataReport"];
            ExcelHelper excelHelper = new ExcelHelper();
            List<string> titles = new List<string>()
            {
                "Mã đặt phòng","Folio","Tên khách","Loại","Chỗ ở",
                "Phòng","Nhận phòng","Trả phòng","Tổng (₫)","Đã thanh toán (₫)",
                "Còn lại (₫)","Trạng thái","Nguồn","Người dùng"
            };
            excelHelper.Write("Report", data, titles, path);
            byte[] download = System.IO.File.ReadAllBytes(path);
            return File(download, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
        public ActionResult DownloadActivity()
        {
            if (!CheckSecurity())
                return Redirect("/Admin/Login/Index");

            string filename = "report.xlsx";
            string sourcefile = AppDomain.CurrentDomain.BaseDirectory + "Helper\\TemplateFile\\ReportReservation.xlsx";
            string path = AppDomain.CurrentDomain.BaseDirectory + "Helper\\FileData\\Excel\\" + filename;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            System.IO.File.Copy(sourcefile, path);
            List<Booking_Reservation> data = (List<Booking_Reservation>)Session["dataReport"];
            ExcelHelper excelHelper = new ExcelHelper();
            List<string> titles = new List<string>()
            {
                "Mã đặt phòng","Folio","Tên khách","Loại","Chỗ ở",
                "Phòng","Nhận phòng","Trả phòng","Tổng (₫)","Đã thanh toán (₫)",
                "Còn lại (₫)","Nguồn","Người dùng"
            };
            excelHelper.WriteActivity("Report", data, titles, path);
            byte[] download = System.IO.File.ReadAllBytes(path);
            return File(download, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
        public ActionResult DownloadInvoiceBreakDown()
        {
            if (!CheckSecurity())
                return Redirect("/Admin/Login/Index");

            string filename = "report.xlsx";
            string sourcefile = AppDomain.CurrentDomain.BaseDirectory + "Helper\\TemplateFile\\ReportReservation.xlsx";
            string path = AppDomain.CurrentDomain.BaseDirectory + "Helper\\FileData\\Excel\\" + filename;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            System.IO.File.Copy(sourcefile, path);
            List<Booking_Reservation> data = (List<Booking_Reservation>)Session["dataReport"];
            ExcelHelper excelHelper = new ExcelHelper();
            List<string> titles = new List<string>()
            {
                "Mã đặt phòng","Folio","Tên khách","Loại","Chỗ ở",
                "Phòng","Nhận phòng","Trả phòng","Tổng (₫)","Đã thanh toán (₫)",
                "Còn lại (₫)","Trạng thái"
            };
            excelHelper.WriteInvoiceBreakDown("Report", data, titles, path);
            byte[] download = System.IO.File.ReadAllBytes(path);
            return File(download, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
        public ActionResult DownloadAvailableAccommodations()
        {
            if (!CheckSecurity())
                return Redirect("/Admin/Login/Index");

            string filename = "report.xlsx";
            string sourcefile = AppDomain.CurrentDomain.BaseDirectory + "Helper\\TemplateFile\\ReportAvailableAccommodation.xlsx";
            string path = AppDomain.CurrentDomain.BaseDirectory + "Helper\\FileData\\Excel\\" + filename;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            System.IO.File.Copy(sourcefile, path);
            Report_AvailableAccommodations data = (Report_AvailableAccommodations)Session["dataReport"];
            ExcelHelper excelHelper = new ExcelHelper();
            List<string> titles = new List<string>();
            data.dateTimes.ForEach(x => titles.Add(x.ToString("dd/MM")));

            excelHelper.WriteAvailableAccommodations("Report", data, titles, path);
            byte[] download = System.IO.File.ReadAllBytes(path);
            return File(download, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
        public ActionResult DownloadCashDrawer()
        {
            if (!CheckSecurity())
                return Redirect("/Admin/Login/Index");

            string filename = "report.xlsx";
            string sourcefile = AppDomain.CurrentDomain.BaseDirectory + "Helper\\TemplateFile\\ReportCashDrawer.xlsx";
            string path = AppDomain.CurrentDomain.BaseDirectory + "Helper\\FileData\\Excel\\" + filename;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            System.IO.File.Copy(sourcefile, path);
            List<CashHistory> cashHistories = (List<CashHistory>)Session["dataReport"];
            ExcelHelper excelHelper = new ExcelHelper();
            List<string> titles = new List<string>() { "Tên ngăn kéo", "Ngày mở", "Ngày đóng", "Số dư ban đầu (₫)", "Số dư ngăn kéo (₫)", "Rút tiền mặt (₫)", "Ghi chú mở ngăn kéo", "Ghi chú đóng ngăn kéo", "Phiên làm việc", "Trạng thái" };

            excelHelper.WriteCashDrawer("Report", cashHistories, titles, path);
            byte[] download = System.IO.File.ReadAllBytes(path);
            return File(download, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
    }
}