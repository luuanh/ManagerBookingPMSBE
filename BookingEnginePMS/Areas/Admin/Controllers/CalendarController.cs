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
    #region class helper
    public class RatioOverBooking
    {
        public float ratio { get; set; }
        public int numberOver { get; set; }
    }
    public class RateRoomType
    {
        public int Number { get; set; }
        public float Price { get; set; }
        public int Status { get; set; }
    }
    public class RoomType_Calender
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public List<RateRoomType> rateRoomTypes { get; set; }
        public List<Room_Calendar> Rooms { get; set; }
    }
    public class Room_Calendar
    {
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
    }
    public class ModelDataCalendar
    {
        public RoomType_Calender roomType { get; set; }
        public Room_Calendar room { get; set; }
        public List<DateTime> dateTimes = new List<DateTime>();
        public int typeTitle { get; set; }
    }
    public class BookingCalendar
    {
        public int BookingId { get; set; }
        public int TypeBooking { get; set; }
        public int ReservationId { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomId { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public float Paid { get; set; }
        public float PrePaid { get; set; }
        public float Discount { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public string Flight { get; set; }
        public string GuestName { get; set; }
        public string ReminiscentName { get; set; }
        public string Color { get; set; }

        public float TotalRoom { get; set; }
        public float TotalExtrabed { get; set; }
        public float TotalService { get; set; }
        public float Total { get; set; }
        public List<ReservationNote> reservationNotes { get; set; }
    }
    public class BookingOverCalendar
    {
        public int ReservationId { get; set; }
        public int BookingId { get; set; }
        public string GuestName { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomId { get; set; }
        public int TypeBooking { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public List<RoomTypeCalendar> roomTypeCalendars { get; set; }
        public List<RoomCalendar> roomCalendars { get; set; }
    }
    public class RoomTypeCalendar
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public List<RoomCalendar> roomCalendars { get; set; }
    }
    public class RoomCalendar
    {
        public int RoomId { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomCode { get; set; }
    }
    #endregion
    public class CalendarController : SercurityController
    {
        // GET: Admin/Calendar
        public ActionResult Index()
        {
            if (!CheckSecurity(2))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 2
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult NewReservation(DateTime checkin, DateTime checkout, int typeBooking, int roomTypeId, int roomId)
        {
            if (!CheckSecurity())
                return Redirect("/Admin/Login/Index");

            List<string> countries = ConfigData.GetAllCountry();
            int HotelId = (int)Session["HotelId"];
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Guest_Sample> guests = connection.Query<Guest_Sample>("Guest_GetAll_Sample",
                     new
                     {
                         HotelId = HotelId
                     }, commandType: CommandType.StoredProcedure).ToList();
                List<Guest_Sample> agencies = connection.Query<Guest_Sample>("Company_GetAll_Sample_As_Guest",
                     new
                     {
                         HotelId = HotelId
                     }, commandType: CommandType.StoredProcedure).ToList();
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 3
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                ViewData["countries"] = countries;
                ViewData["guests"] = guests;
                ViewData["agencies"] = agencies;
                return View();
            }
        }
        public JsonResult Get(DateTime fromDate, int numberDay, bool changeStartDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            if (changeStartDate)
                fromDate = fromDate.AddDays(numberDay);
            DateTime toDate = fromDate.AddDays(numberDay);
            List<RatioOverBooking> ratioOverBookings = new List<RatioOverBooking>();
            List<DateTime> dateTimes = new List<DateTime>();
            List<BookingCalendar> bookings = new List<BookingCalendar>();
            List<ModelDataCalendar> modelDataCalendars = new List<ModelDataCalendar>();
            for (DateTime date = fromDate.Date; date <= toDate.Date; date = date.AddDays(1))
            {
                dateTimes.Add(date);
            }
            using (var connection = DB.ConnectionFactory())
            {
                int TotalRoomOfAllRoomType = 0;
                #region create data sample
                List<RoomType_Calender> roomTypes = connection.Query<RoomType_Calender>("RoomType_GetSample",
                    new
                    {
                        HotelId = HotelId
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                if (roomTypes is null)
                    roomTypes = new List<RoomType_Calender>();
                roomTypes.ForEach(x =>
                {
                    int numberRoomOfRoomType = connection.QuerySingleOrDefault<int>("RoomType_Get_NumberRoom",
                        new
                        {
                            RoomTypeId = x.RoomTypeId
                        }, commandType: CommandType.StoredProcedure);
                    TotalRoomOfAllRoomType += numberRoomOfRoomType;
                    x.rateRoomTypes = new List<RateRoomType>();
                    for (DateTime date = fromDate.Date; date <= toDate.Date; date = date.AddDays(1))
                    {
                        using (var multi = connection.QueryMultiple("Calender_RoomType_GetNumberPrice",
                            new
                            {
                                RoomTypeId = x.RoomTypeId,
                                Date = date
                            }, commandType: CommandType.StoredProcedure))
                        {
                            float price = multi.Read<float>().SingleOrDefault();
                            int number = multi.Read<int>().SingleOrDefault();
                            int status = multi.Read<int>().SingleOrDefault();
                            x.rateRoomTypes.Add(new RateRoomType()
                            {
                                Number = numberRoomOfRoomType - number,
                                Price = price,
                                Status = status
                            });
                        }
                    }
                    x.Rooms = connection.Query<Room_Calendar>("RoomType_Get_Room",
                        new
                        {
                            RoomTypeId = x.RoomTypeId
                        }, commandType: CommandType.StoredProcedure).ToList();
                });
                bookings = connection.Query<BookingCalendar>("Booking_Get_RangeDate",
                    new
                    {
                        FromDate = fromDate,
                        ToDate = toDate
                    }, commandType: CommandType.StoredProcedure).ToList();
                roomTypes.ForEach(x =>
                {
                    modelDataCalendars.Add(new ModelDataCalendar()
                    {
                        typeTitle = 1,
                        dateTimes = dateTimes
                    });
                    modelDataCalendars.Add(new ModelDataCalendar()
                    {
                        typeTitle = 2,
                        roomType = x
                    });
                    x.Rooms.ForEach(y =>
                    {
                        modelDataCalendars.Add(new ModelDataCalendar()
                        {
                            typeTitle = 3,
                            room = y,
                            roomType = new RoomType_Calender() { RoomTypeId = x.RoomTypeId }
                        });
                    });
                    x.Rooms = null;
                });
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                   new
                   {
                       HotelId = HotelId
                   },
                   commandType: CommandType.StoredProcedure);
                bookings.ForEach(x =>
                {
                    using (var multi = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = x.BookingId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        x.TotalRoom = multi.Read<float>().SingleOrDefault();
                        x.TotalExtrabed = multi.Read<float>().SingleOrDefault();
                        x.TotalService = multi.Read<float>().SingleOrDefault();
                        float total = (x.TotalRoom + x.TotalExtrabed + x.TotalService);
                        float discount = (float)Math.Round(total * x.Discount / 100 + 0.001, 0);
                        x.Total = (float)Math.Round((total - discount) * (100 + taxFees) / 100, 0);
                    }
                    x.reservationNotes = connection.Query<ReservationNote>("Booking_Get_ReservationNote",
                        new
                        {
                            BookingId = x.BookingId
                        }, commandType: CommandType.StoredProcedure).ToList();
                });
                #endregion
                #region create bar ratio and overbooking
                dateTimes.ForEach(x =>
                {
                    using (var multi = connection.QueryMultiple("Calendar_Get_RatioOverBooking",
                        new
                        {
                            HotelId = HotelId,
                            Date = x
                        }, commandType: CommandType.StoredProcedure))
                    {
                        int numberBookingUsed = multi.Read<int>().SingleOrDefault();
                        int over = multi.Read<int>().SingleOrDefault();
                        ratioOverBookings.Add(new RatioOverBooking()
                        {
                            numberOver = over,
                            ratio = (float)(Math.Round((float)numberBookingUsed * 100 / TotalRoomOfAllRoomType + 0.001, 0))
                        });
                    }
                });
                #endregion
                return Json(JsonConvert.SerializeObject(new
                {
                    dateTimes = dateTimes,
                    ratioOverBookings = ratioOverBookings,
                    bookings = bookings,
                    modelDataCalendars = modelDataCalendars
                }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetOverBooking(DateTime date)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<BookingOverCalendar> bookingOverCalendars = connection.Query<BookingOverCalendar>("Calendar_GetOverBookingByDate",
                    new
                    {
                        HotelId = HotelId,
                        Date = date
                    }, commandType: CommandType.StoredProcedure).ToList();
                List<RoomTypeCalendar> roomTypeCalendars = connection.Query<RoomTypeCalendar>("RoomType_GetSample",
                    new
                    {
                        HotelId = HotelId
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                if (roomTypeCalendars is null) roomTypeCalendars = new List<RoomTypeCalendar>();
                if (bookingOverCalendars is null) bookingOverCalendars = new List<BookingOverCalendar>();
                bookingOverCalendars.ForEach(x =>
                {
                    x.roomTypeCalendars = roomTypeCalendars;
                    x.roomTypeCalendars.ForEach(y =>
                    {
                        using (var multi = connection.QueryMultiple("Calendar_GetRoomAvailable",
                            new
                            {
                                RoomTypeId = y.RoomTypeId,
                                ArrivalDate = x.ArrivalDate,
                                DepartureDate = x.DepartureDate
                            }, commandType: CommandType.StoredProcedure))
                        {
                            List<RoomCalendar> roomCalendars = multi.Read<RoomCalendar>().ToList();
                            List<int> rooms = multi.Read<int>().ToList();

                            if (rooms is null) rooms = new List<int>();
                            if (roomCalendars is null) roomCalendars = new List<RoomCalendar>();
                            List<RoomCalendar> roomCalendarCurrent = new List<RoomCalendar>();
                            roomCalendars.ForEach(z =>
                            {
                                if (rooms.FindIndex(t => t == z.RoomId) < 0)
                                    roomCalendarCurrent.Add(z);
                            });
                            y.roomCalendars = roomCalendarCurrent;
                        }
                    });
                    if (x.roomTypeCalendars.Count > 0)
                    {
                        x.roomCalendars = x.roomTypeCalendars.Find(y => y.RoomTypeId == x.RoomTypeId).roomCalendars;
                    }
                });
                return Json(JsonConvert.SerializeObject(bookingOverCalendars), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AssignRoom(List<BookingOverCalendar> bookingOverCalendars)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (bookingOverCalendars is null) bookingOverCalendars = new List<BookingOverCalendar>();
            using (var connection = DB.ConnectionFactory())
            {
                bookingOverCalendars.ForEach(x =>
                {
                    if (x.RoomId > 0)
                    {
                        connection.Execute("Booking_AssignRoom",
                            new
                            {
                                BookingId = x.BookingId,
                                RoomTypeId = x.RoomTypeId,
                                RoomId = x.RoomId
                            }, commandType: CommandType.StoredProcedure);
                    }
                });
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

    }
}