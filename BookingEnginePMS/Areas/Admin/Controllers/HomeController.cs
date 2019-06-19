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
    public class Booking_Home
    {
        public int BookingId { get; set; }
        public int TypeBooking { get; set; }
        public int ReservationId { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomId { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Status { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string GuestName { get; set; }
        public string RoomTypeName { get; set; }
        public string RoomCode { get; set; }
        public float Prepaid { get; set; }
        public float Paid { get; set; }
        public float TotalAmount { get; set; }
        public float Balance { get; set; }
        public float Discount { get; set; }

    }
    public class HomeController : SercurityController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (!CheckSecurity(1))
                return Redirect("/Admin/Login/Index");
            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            #region data for chart
            DateTime dateNow = DatetimeHelper.DateTimeUTCNow();
            List<DateTime> date = new List<DateTime>() { dateNow.Date, dateNow.AddDays(1).Date };
            List<int> roomUsed = new List<int>();
            List<int> roomAvailable = new List<int>();
            List<string> dateTitleChart = new List<string>();
            using (var connection = DB.ConnectionFactory())
            {
                int totalRoomInHotel = connection.QuerySingleOrDefault<int>("Hotel_Get_NumberRoom",
                    new
                    {
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure);
                date.ForEach(x =>
                {
                    int numberRoomUsed = connection.QuerySingleOrDefault<int>("Booking_Get_NumberRoomUsed",
                            new
                            {
                                HotelId = HotelId,
                                Date = x
                            }, commandType: CommandType.StoredProcedure);
                    roomUsed.Add(numberRoomUsed);
                    roomAvailable.Add(totalRoomInHotel - numberRoomUsed);
                    dateTitleChart.Add(x.ToString("dd-MM-yyyy"));
                });

                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                    new
                    {
                        languageId = LanguageId,
                        screenId = 1
                    }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            ViewData["dataChart"] = JsonConvert.SerializeObject(new
            {
                dateTitleChart = dateTitleChart,
                roomUsed = roomUsed,
                roomAvailable = roomAvailable
            });
            #endregion

            return View();
        }
        public JsonResult GetSituationOnDay(int rangeToday = 0)
        {
            if (!CheckSecurity())
                return Json("",JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            DateTime date = DatetimeHelper.DateTimeUTCNow();
            date = date.AddDays(rangeToday);
            using (var connection = DB.ConnectionFactory())
            {
                int totalRoomInHotel = connection.QuerySingleOrDefault<int>("Hotel_Get_NumberRoom",
                       new
                       {
                           HotelId = HotelId
                       }, commandType: CommandType.StoredProcedure);
                int numberRoomUsed = connection.QuerySingleOrDefault<int>("Booking_Get_NumberRoomUsed",
                            new
                            {
                                HotelId = HotelId,
                                Date = date
                            }, commandType: CommandType.StoredProcedure);
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                    new
                    {
                        HotelId = HotelId
                    },
                    commandType: CommandType.StoredProcedure);
                using (var multi = connection.QueryMultiple("Home_GetSituationOnDay",
                    new
                    {
                        HotelId = HotelId,
                        Date = date
                    }, commandType: System.Data.CommandType.StoredProcedure))
                {
                    List<Booking_Home> arrivals = multi.Read<Booking_Home>().ToList();
                    List<Booking_Home> departures = multi.Read<Booking_Home>().ToList();
                    List<Booking_Home> inhouses = multi.Read<Booking_Home>().ToList();
                    List<Booking_Home> stayovers = multi.Read<Booking_Home>().ToList();
                    if (arrivals is null) arrivals = new List<Booking_Home>();
                    if (departures is null) departures = new List<Booking_Home>();
                    if (inhouses is null) inhouses = new List<Booking_Home>();
                    if (stayovers is null) stayovers = new List<Booking_Home>();
                    departures.ForEach(x =>
                    {
                        using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = x.BookingId
                        }, commandType: CommandType.StoredProcedure))
                        {
                            float TotalRoom = multi2.Read<float>().SingleOrDefault();
                            float TotalExtrabed = multi2.Read<float>().SingleOrDefault();
                            float TotalService = multi2.Read<float>().SingleOrDefault();
                            float total = (TotalRoom  + TotalExtrabed + TotalService);
                            float discount = (float)Math.Round(total * x.Discount / 100, 0);
                            x.TotalAmount = (float)Math.Round((total - discount) * (100 + taxFees) / 100, 0);
                            x.Balance = (float)Math.Round(x.TotalAmount - x.Prepaid - x.Paid, 0);
                        }
                    });
                    return Json(JsonConvert.SerializeObject(new
                    {
                        totalRoomInHotel = totalRoomInHotel,
                        numberRoomUsed = numberRoomUsed,
                        arrivals = arrivals,
                        departures = departures,
                        inhouses = inhouses,
                        stayovers = stayovers
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}