using BookingEnginePMS.Helper;
using BookingEnginePMS.Models;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    #region class helper
    public class RoomType_Reservation
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public int Children { get; set; }
        public int Adult { get; set; }
        public List<RoomTypeExtrabed_Reservation> RoomTypeExtrabeds { get; set; }
        public List<RateAvailability_Reservation> RateAvailabilitys { get; set; }
        public int NumberRoomAvailable { get; set; }
        //
        public int AdultChoose { get; set; }
        public int ChildrenChoose { get; set; }
        public int NumberRoomChoose { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public int SpaceDate { get; set; }
        public int GuestId { get; set; }
    }
    public class RoomTypeExtrabed_Reservation
    {
        public int RoomTypeId { get; set; }
        public int ExtrabedId { get; set; }
        public string ExtrabedName { get; set; }
        public float Price { get; set; }
        public int Number { get; set; }
    }
    public class RateAvailability_Reservation
    {
        public int RateAvailabilityId { get; set; }
        public int RoomTypeId { get; set; }
        public float Price { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
    }
    public class FilterRoomAvailable_Reservation
    {
        public int RoomTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TypeBooking { get; set; }
        public List<Room_Reservation> Rooms { get; set; }
    }
    public class Room_Reservation
    {
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public int Status { get; set; }
    }
    public class Reservation_Sample
    {
        public int ReservationId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Day { get; set; }
        public int Status { get; set; }
        public int BookingSource { get; set; }
        public string RoomTypeName_s { get; set; }
        public float TotalPrice { get; set; }
        public float Paid { get; set; }
    }
    public class Reservation_DebtTransfer
    {
        public int ReservationId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime TransferDebtDate { get; set; }
        public string Folios { get; set; }
        public float TotalAmount { get; set; }
        public float Paid { get; set; }
        //
        public string JsonBookingData { get; set; }
    }
    public class Company_DebtTransfer
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public float TotalAmount { get; set; }
        public float TotalPaid { get; set; }
        public List<Reservation_DebtTransfer> reservations { get; set; }
    }
    #endregion
    #region class helper detail Reservation
    public class Booking_Reservation
    {
        public int ReservationId { get; set; }
        public int BookingId { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public float Paid { get; set; }
        public float PrePaid { get; set; }
        public int Status { get; set; }
        public string GuestName { get; set; }
        public int TypeBooking { get; set; }
        public int BookingSource { get; set; }
        public float Discount { get; set; }
        public string UserCreate { get; set; }
        //

        public float TotalRoom { get; set; }
        public float TotalExtrabed { get; set; }
        public float TotalService { get; set; }
        public float Total { get; set; }
        //
        public List<BookingExtrabed> BookingExtrabeds { get; set; }
        public List<BookingService> BookingServices { get; set; }
        public List<BookingPrice> BookingPrices { get; set; }
        // pay debt
        public float PayDebt { get; set; }
    }
    public class BookingService_Resrvation
    {
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public int Number { get; set; }
        public string ServiceName { get; set; }
        public float Price { get; set; }
        public DateTime DateCreate { get; set; }
    }
    #endregion
    public class ReservationController : SercurityController
    {
        // GET: Admin/Reservation
        public ActionResult Index()
        {
            if (!CheckSecurity(4))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
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
        public ActionResult StayCheckIn()
        {
            if (!CheckSecurity(4))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
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
        public ActionResult StayCheckOut()
        {
            if (!CheckSecurity(4))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
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
        public ActionResult New()
        {
            if (!CheckSecurity(3))
                return Redirect("/Admin/Login/Index");

            List<string> countries = ConfigData.GetAllCountry();
            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
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
        public ActionResult Edit(int id)
        {
            if (!CheckSecurity(33))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            ViewData["countries"] = ConfigData.GetAllCountry();
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 33
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public ActionResult DebtTransferReservation()
        {
            if (!CheckSecurity(7))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 7
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public ActionResult DebtTransferAgencies()
        {
            if (!CheckSecurity(8))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 8
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult GetDebtTransferReservation(DateTime fromDate, DateTime toDate, int pageNumber = 1, int pageSize = 100)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Reservation_GetDebtTransferReservation",
                    new
                    {
                        fromDate = fromDate,
                        toDate = toDate,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Reservation_DebtTransfer> reservations = multi.Read<Reservation_DebtTransfer>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    if (reservations is null)
                        reservations = new List<Reservation_DebtTransfer>();
                    float totalPrice = reservations.Sum(x => x.TotalAmount);
                    float totalPaid = reservations.Sum(x => x.Paid);

                    return Json(JsonConvert.SerializeObject(new
                    {
                        reservations = reservations,
                        totalRecord = totalRecord,
                        totalPrice = Math.Round(totalPrice, 0),
                        totalPaid = Math.Round(totalPaid, 0)
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult GetDebtTransferAgencies(DateTime fromDate, DateTime toDate, int pageNumber = 1, int pageSize = 100)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Reservation_GetAgencyByDebtTransferReservation",
                    new
                    {
                        fromDate = fromDate,
                        toDate = toDate,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<int> companyIds = multi.Read<int>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();

                    if (companyIds is null) companyIds = new List<int>();
                    List<Company_DebtTransfer> companies = new List<Company_DebtTransfer>();
                    companyIds.ForEach(x =>
                    {
                        Company_DebtTransfer company = connection.QuerySingleOrDefault<Company_DebtTransfer>("ReservationTransfer_GetReservationByCompany",
                            new
                            {
                                CompanyId = x,
                                HotelId = HotelId
                            }, commandType: CommandType.StoredProcedure);
                        if (company is null) company = new Company_DebtTransfer();
                        companies.Add(company);

                    });
                    return Json(new
                    {
                        companies = companies,
                        totalRecord = totalRecord
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult GetBookingPayDebt(int reservationId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            return Json(getBookingByReservation(reservationId, 6), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReservationPayDebtByCompany(int companyId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                List<Reservation_DebtTransfer> reservations = connection.Query<Reservation_DebtTransfer>("ReservationDebt_GetByCompany",
                    new
                    {
                        CompanyId = companyId
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (reservations is null) reservations = new List<Reservation_DebtTransfer>();
                reservations.ForEach(x =>
                {
                    x.JsonBookingData = getBookingByReservation(x.ReservationId, 6, connection);
                });
                return Json(JsonConvert.SerializeObject(reservations), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PayDebtBooking(List<Booking_Reservation> bookings)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    bookings.ForEach(x =>
                    {
                        connection.Execute("Booking_Put_PayDebt",
                            new
                            {
                                BookingId = x.BookingId,
                                PayDebt = x.PayDebt < 0 ? 0 : x.PayDebt
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult PayAllReservationChoose(List<int> reservationIds)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (reservationIds is null) reservationIds = new List<int>();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    reservationIds.ForEach(x =>
                    {
                        connection.Execute("Booking_Put_PayDebtByReservationId",
                            new
                            {
                                ReservationId = x
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetRoomAvailability(int type, DateTime fromDate, DateTime toDate, string voucher = "")
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            if (type == 1)
                toDate = fromDate.AddDays(1);
            using (var connection = DB.ConnectionFactory())
            {
                List<int> roomtypeHasVoucher = new List<int>();
                float DiscountForRoom = 0;
                float DiscountForService = 0;
                List<RoomType_Reservation> roomType_ReservationsResult = new List<RoomType_Reservation>();
                if (voucher != null && voucher != "")
                {
                    using (var multi = connection.QueryMultiple("RoomType_Get_By_Voucher_Current",
                        new
                        {
                            VoucherCode = voucher,
                            Date = fromDate
                        }, commandType: CommandType.StoredProcedure))
                    {
                        int resultCheckCodeVoucher = multi.Read<int>().SingleOrDefault();
                        roomtypeHasVoucher = multi.Read<int>().ToList();
                        DiscountForRoom = multi.Read<float>().SingleOrDefault();
                        DiscountForService = multi.Read<float>().SingleOrDefault();
                        if (resultCheckCodeVoucher < 0)
                            return Json(-1, JsonRequestBehavior.AllowGet);
                        if (roomtypeHasVoucher is null)
                            roomtypeHasVoucher = new List<int>();
                    }
                }
                using (var multi = connection.QueryMultiple("Reservation_Get_RoomAvailability",
                    new
                    {
                        HotelId = HotelId,
                        LanguageId = LanguageId,
                        FromDate = fromDate,
                        ToDate = toDate
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<RoomType_Reservation> roomTypes = multi.Read<RoomType_Reservation>().ToList();
                    List<RoomTypeExtrabed_Reservation> roomTypeExtrabeds = multi.Read<RoomTypeExtrabed_Reservation>().ToList();
                    List<RateAvailability_Reservation> rateAvailabilitys = multi.Read<RateAvailability_Reservation>().ToList();
                    if (roomTypes is null)
                        roomTypes = new List<RoomType_Reservation>();
                    if (roomTypeExtrabeds is null)
                        roomTypeExtrabeds = new List<RoomTypeExtrabed_Reservation>();
                    if (rateAvailabilitys is null)
                        rateAvailabilitys = new List<RateAvailability_Reservation>();

                    roomTypes.ForEach(x =>
                    {
                        x.RoomTypeExtrabeds = roomTypeExtrabeds.FindAll(y => y.RoomTypeId == x.RoomTypeId);
                        x.RateAvailabilitys = rateAvailabilitys.FindAll(y => y.RoomTypeId == x.RoomTypeId);
                        if (x.RoomTypeExtrabeds is null)
                            x.RoomTypeExtrabeds = new List<RoomTypeExtrabed_Reservation>();
                        if (x.RateAvailabilitys is null)
                            x.RateAvailabilitys = new List<RateAvailability_Reservation>();
                        if (type == 1)
                        {
                            float priceStart = connection.QuerySingleOrDefault<float>("RoomTypePriceHour_Get_PriceStart",
                                new
                                {
                                    RoomTypeId = x.RoomTypeId
                                }, commandType: CommandType.StoredProcedure);
                            x.RateAvailabilitys.ForEach(y => y.Price = priceStart);
                        }
                    });
                    int NumberDateInHouse = (toDate.Date - fromDate.Date).Days;
                    Session["NumberDateInHouse"] = NumberDateInHouse;
                    roomTypes.ForEach(x =>
                    {
                        if (x.RateAvailabilitys.Count == NumberDateInHouse)
                        {
                            if (voucher != null && voucher != "")
                            {
                                if (roomtypeHasVoucher.FindIndex(y => y == x.RoomTypeId) >= 0)
                                {
                                    int numberAvailable = x.RateAvailabilitys.Min(y => y.Number);
                                    if (numberAvailable > 0)
                                    {
                                        x.NumberRoomAvailable = numberAvailable;
                                        x.NumberRoomChoose = 1;
                                        x.AdultChoose = x.Adult >= 2 ? 2 : x.Adult;
                                        x.ChildrenChoose = 0;
                                        x.ArrivalDate = fromDate;
                                        x.DepartureDate = toDate;
                                        x.SpaceDate = NumberDateInHouse;
                                        x.RateAvailabilitys.ForEach(y =>
                                        {
                                            y.Price = (float)Math.Round(y.Price * (100 - DiscountForRoom) / 100, 0);
                                        });
                                        roomType_ReservationsResult.Add(x);
                                    }
                                }
                            }
                            else
                            {
                                if (x.RateAvailabilitys != null && x.RateAvailabilitys.Count > 0)
                                {
                                    int numberAvailable = x.RateAvailabilitys.Min(y => y.Number);
                                    if (numberAvailable > 0)
                                    {
                                        x.NumberRoomAvailable = numberAvailable;
                                        x.NumberRoomChoose = 1;
                                        x.AdultChoose = x.Adult >= 2 ? 2 : x.Adult;
                                        x.ChildrenChoose = 0;
                                        x.ArrivalDate = fromDate;
                                        x.DepartureDate = toDate;
                                        x.SpaceDate = NumberDateInHouse;
                                        roomType_ReservationsResult.Add(x);
                                    }
                                }
                            }
                        }
                    });
                    return Json(JsonConvert.SerializeObject(roomType_ReservationsResult), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult GetRoomAvailable(List<FilterRoomAvailable_Reservation> filterRoomAvailable)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                List<FilterRoomAvailable_Reservation> filterRoomAvailablesResult = new List<FilterRoomAvailable_Reservation>();
                filterRoomAvailable.ForEach(x =>
                {
                    using (var multi = connection.QueryMultiple("Room_Get_Available",
                        new
                        {
                            RoomTypeId = x.RoomTypeId,
                            FromDate = x.FromDate.ToString("yyyy/MM/dd HH:mm:ss"),
                            ToDate = x.ToDate.ToString("yyyy/MM/dd HH:mm:ss"),
                            TypeBooking = x.TypeBooking
                        }, commandType: CommandType.StoredProcedure))
                    {
                        x.Rooms = new List<Room_Reservation>();
                        List<Room_Reservation> room_Reservations = multi.Read<Room_Reservation>().ToList();
                        List<int> roomAvailable = multi.Read<int>().ToList();
                        if (room_Reservations is null)
                            room_Reservations = new List<Room_Reservation>();
                        if (roomAvailable is null)
                            roomAvailable = new List<int>();
                        room_Reservations.ForEach(y =>
                        {
                            if (roomAvailable.FindIndex(z => z == y.RoomId) < 0)
                                x.Rooms.Add(y);
                        });
                    }

                    filterRoomAvailablesResult.Add(x);
                });
                DateTime arrivalDateMin = filterRoomAvailable.Min(x => x.FromDate);
                DateTime departureDateMax = filterRoomAvailable.Max(x => x.ToDate);
                return Json(JsonConvert.SerializeObject(new
                {
                    filterRoomAvailablesResult = filterRoomAvailablesResult,
                    arrivalDateMin = arrivalDateMin,
                    departureDateMax = departureDateMax
                }), JsonRequestBehavior.AllowGet);
            }
        }

        public string Get(int filter, bool untreated, string keySearch, int source, List<int> status,
                            DateTime fromDate, DateTime toDate, int pageNumber = 1, int pageSize = 100)
        {
            if (!CheckSecurity())
                return "";

            int HotelId = (int)Session["HotelId"];
            if (status is null)
                status = new List<int>();
            int statusNew = status.FindIndex(x => x == 1) >= 0 ? 1 : 0;
            int statusInHouse = status.FindIndex(x => x == 2) >= 0 ? 1 : 0;
            int statusCheckOut = status.FindIndex(x => x == 3) >= 0 ? 1 : 0;
            int statusNoShow = status.FindIndex(x => x == 4) >= 0 ? 1 : 0;
            int statusCancel = status.FindIndex(x => x == 5) >= 0 ? 1 : 0;
            int statusDebt = status.FindIndex(x => x == 6) >= 0 ? 1 : 0;
            using (var connection = DB.ConnectionFactory())
            {
                string storedProc = GetType_StoredProcedure(filter);
                using (var multi = connection.QueryMultiple(storedProc,
                    new
                    {
                        HotelId = HotelId,
                        untreated = untreated,
                        keySearch = keySearch,
                        source = source,
                        fromDate = fromDate,
                        toDate = toDate,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        statusNew = statusNew,
                        statusInHouse = statusInHouse,
                        statusCheckOut = statusCheckOut,
                        statusNoShow = statusNoShow,
                        statusCancel = statusCancel,
                        statusDebt = statusDebt
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Reservation_Sample> reservations = multi.Read<Reservation_Sample>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    double totalPrice = 0;
                    double totalPaid = 0;
                    if (reservations is null)
                        reservations = new List<Reservation_Sample>();

                    string[] taxFees = connection.QuerySingleOrDefault<string>("TaxFee_GetTotal_Lastest",
                        new
                        {
                            HotelId = HotelId
                        },
                        commandType: CommandType.StoredProcedure).Split(',');
                    float tax = float.Parse(taxFees[0]);
                    float fee = float.Parse(taxFees[1]);
                    reservations.ForEach(x =>
                    {
                        List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Reservation_Get_Booking",
                        new
                        {
                            ReservationId = x.ReservationId,
                            Status = -1
                        }, commandType: CommandType.StoredProcedure).ToList();
                        if (bookings is null)
                            bookings = new List<Booking_Reservation>();
                        bookings.ForEach(y =>
                        {
                            float taxForBooking = 0;
                            float feesForBooking = 0;
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
                                {
                                    taxForBooking = tax;
                                    feesForBooking = fee;
                                }

                                float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                                float discount = (float)Math.Round(total * y.Discount / 100, 0);
                                y.Total = (float)Math.Round((total - discount) * (100 + taxForBooking + feesForBooking + taxForBooking * feesForBooking / 100) / 100, 0);
                            }
                        });
                        x.TotalPrice = (float)Math.Round(bookings.Sum(y => y.Total), 0);
                        x.Paid = (float)Math.Round(bookings.Sum(y => y.Paid + y.PrePaid), 0);
                    });
                    reservations.ForEach(x =>
                    {
                        List<string> roomCurrent = new List<string>();
                        if (x.RoomTypeName_s != null)
                        {
                            string[] rooms = x.RoomTypeName_s.Split(',');
                            foreach (string room in rooms)
                            {
                                int numberDiscount = rooms.ToList().FindAll(y => roomCurrent.FindIndex(z => z.Split('(')[0].Trim() == room.Trim()) < 0 && y.Trim() == room.Trim()).Count;
                                if (numberDiscount > 0)
                                    roomCurrent.Add(room.Trim() + "(" + numberDiscount + ")");
                            }
                            x.RoomTypeName_s = string.Join(", ", roomCurrent.ToArray());
                        }
                        totalPrice += Math.Round(x.TotalPrice, 0);
                        totalPaid += Math.Round(x.Paid, 0);
                    });
                    return JsonConvert.SerializeObject(new
                    {
                        reservations = reservations,
                        totalRecord = totalRecord,
                        totalPrice = Math.Round(totalPrice, 0),
                        totalPaid = Math.Round(totalPaid, 0)
                    });
                }
            }
        }
        public string GetType_StoredProcedure(int type)
        {
            switch (type)
            {
                case -1:
                    return "Reservation_Get_Lastest";
                case 1:
                    return "Reservation_Get_DateBooked";
                case 2:
                    return "Reservation_Get_CheckIn";
                case 3:
                    return "Reservation_Get_CheckOut";
                default:
                    return "Reservation_Get_Lastest";
            }
        }
        // ------------------ Post reservation ----------------------------------
        public JsonResult Post(Reservation reservation)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            User user = (User)Session["User"];
            if (reservation is null)
                reservation = new Reservation();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                float tax = 0;
                float fee = 0;
                // kiểm tra nếu đặt phòng có tính thuế sẽ cập nhật lại phí thuế nếu không mức thuế áp đặt là 0%
                if (reservation.IncludeTaxFee)
                {
                    string[] taxFees = connection.QuerySingleOrDefault<string>("TaxFee_GetTotal_Lastest",
                        new
                        {
                            HotelId = HotelId
                        },
                        commandType: CommandType.StoredProcedure).Split(',');
                    tax = float.Parse(taxFees[0]);
                    fee = float.Parse(taxFees[1]);
                }
                using (var transaction = connection.BeginTransaction())
                {
                    int guestId = reservation.Guest.GuestId;
                    int companyId = reservation.Company.CompanyId;
                    DateTime datetimeNow = DatetimeHelper.DateTimeUTCNow();
                    if (reservation.ReservationServices is null)
                        reservation.ReservationServices = new List<ReservationService>();
                    // check guest book reservation
                    if (reservation.Guest.GuestId <= 0)
                    {
                        guestId = connection.QuerySingleOrDefault<int>("Guest_Post",
                        new
                        {
                            HotelId = HotelId,
                            FirstName = reservation.Guest.FirstName,
                            SurName = reservation.Guest.SurName,
                            Photo = "",
                            TypeGuestId = reservation.Guest.TypeGuestId,
                            ZIPCode = reservation.Guest.ZIPCode,
                            Company = reservation.Guest.Company,
                            Gender = reservation.Guest.Gender,
                            Dob = reservation.Guest.Dob == null ? "" : reservation.Guest.Dob.Value.ToString("yyyy/MM/dd"),
                            Region = reservation.Guest.Region,
                            Country = reservation.Guest.Country,
                            IdentityCart = reservation.Guest.IdentityCart,
                            Passport = reservation.Guest.Passport,
                            CreditCard = reservation.Guest.CreditCard,
                            DoIssueCreditCard = reservation.Guest.DoIssueCreditCard,
                            CVC = reservation.Guest.CVC,
                            Phone = reservation.Guest.Phone,
                            Fax = reservation.Guest.Fax,
                            Email = reservation.Guest.Email,
                            Address = reservation.Guest.Address,
                            Note = reservation.Guest.Note,
                            DateCreate = datetimeNow,
                            CreateBySource = "PMS"
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    }
                    else
                    {
                        connection.QuerySingleOrDefault<int>("Guest_Put",
                        new
                        {
                            GuestId = reservation.Guest.GuestId,
                            FirstName = reservation.Guest.FirstName,
                            SurName = reservation.Guest.SurName,
                            Photo = "",
                            ZIPCode = reservation.Guest.ZIPCode,
                            Company = reservation.Guest.Company,
                            Gender = reservation.Guest.Gender,
                            Dob = reservation.Guest.Dob,
                            Region = reservation.Guest.Region,
                            Country = reservation.Guest.Country,
                            IdentityCart = reservation.Guest.IdentityCart,
                            Passport = reservation.Guest.Passport,
                            CreditCard = reservation.Guest.CreditCard,
                            DoIssueCreditCard = reservation.Guest.DoIssueCreditCard,
                            CVC = reservation.Guest.CVC,
                            Phone = reservation.Guest.Phone,
                            Fax = reservation.Guest.Fax,
                            Email = reservation.Guest.Email,
                            Address = reservation.Guest.Address,
                            Note = reservation.Guest.Note

                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    }
                    if (reservation.Company.CompanyId <= 0)
                    {
                        companyId = connection.QuerySingleOrDefault<int>("Company_Post",
                            new
                            {
                                HotelId = HotelId,
                                GroupGuestId = reservation.Company.GroupGuestId,
                                SourceId = reservation.Company.SourceId,
                                CompanyName = reservation.Company.CompanyName,
                                CompanyCode = "",
                                TaxCode = reservation.Company.TaxCode,
                                Phone = reservation.Company.Phone,
                                Fax = reservation.Company.Fax,
                                Email = reservation.Company.Email,
                                Address = reservation.Company.Address,
                                ContactUsName = reservation.Company.ContactUsName,
                                ContactPhone = reservation.Company.ContactPhone,
                                ContactEmail = reservation.Company.ContactEmail
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    }
                    #region post reservation
                    int reservationId = connection.QuerySingleOrDefault<int>("Reservation_Post",
                        new
                        {
                            HotelId = HotelId,
                            GuestId = guestId,
                            CompanyId = companyId,
                            TypeReservation = reservation.TypeReservation,
                            ReminiscentName = reservation.ReminiscentName,
                            Color = reservation.Color,
                            Adult = reservation.Adult,
                            Children = reservation.Children,
                            Voucher = reservation.Voucher,
                            BookingSource = reservation.BookingSource,
                            ArrivalFlightDate = reservation.ArrivalFlightDate,
                            ArrivalFlightTime = reservation.ArrivalFlightTime,
                            DepartureFlightDate = reservation.DepartureFlightDate,
                            DepartureFlightTime = reservation.DepartureFlightTime,
                            CheckIn = reservation.CheckIn,
                            CheckOut = reservation.CheckOut,
                            PaymentType = reservation.PaymentType,
                            Deposit = Math.Round(reservation.Deposit, 0),
                            CreateDate = datetimeNow
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    connection.Execute("ReservationNote_Post",
                        new
                        {
                            ReservationId = reservationId,
                            Note = reservation.Note,
                            CreateDate = datetimeNow,
                            UserName = user.UserName
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    #endregion
                    if (reservation.Bookings is null)
                        reservation.Bookings = new List<Booking>();

                    int countBooking = 1;
                    double totalAmount = reservation.Amount;
                    reservation.Bookings.ForEach(x =>
                    {
                        List<ReservationService> personalServices = reservation.ReservationServices.FindAll(y =>
                        {
                            if (y.RoomTypeIdIndex == "-1")
                                return false;
                            string[] arr = y.RoomTypeIdIndex.Split('-');
                            return int.Parse(arr[0]) == x.RoomTypeId && int.Parse(arr[1]) == countBooking;
                        }
                        );
                        // tính tổng tiền của từng phòng. yêu cầu trùng khớp số thập phân với JS
                        if (x.BookingExtrabeds is null) x.BookingExtrabeds = new List<BookingExtrabed>();
                        if (personalServices is null) personalServices = new List<ReservationService>();
                        double SubTotalPrice = x.BookingPrices.Sum(y => y.Price);
                        double ServicePrice = personalServices.Sum(y => y.Number * y.Price);
                        double ExtrabedPrice = x.BookingExtrabeds.Sum(y => y.Price * y.Number);
                        double BalanceDue = Math.Round((SubTotalPrice + ServicePrice + ExtrabedPrice) * (100 + tax + fee + tax * fee / 100) * (100 - reservation.Guest.Discount) / 10000, 0);

                        x.DepositPecent = reservation.DepositPecent;
                        double depositForBooking = 0;
                        if (totalAmount > BalanceDue)
                        {
                            depositForBooking = BalanceDue;
                            totalAmount -= BalanceDue;
                        }
                        else
                        {
                            depositForBooking = totalAmount;
                            totalAmount = 0;
                        }
                        x.DepositMonney = (float)Math.Round(depositForBooking, 0);
                        int guestBookingId = x.Guest.GuestId;
                        // check guest booking
                        if (x.Guest.GuestId <= 0)
                        {
                            if (x.Guest.FirstName != null &&
                                x.Guest.SurName != null &&
                                x.Guest.Email != null &&
                                x.Guest.Phone != null)
                            {
                                guestBookingId = connection.QuerySingleOrDefault<int>("Guest_Post",
                                new
                                {
                                    HotelId = HotelId,
                                    FirstName = x.Guest.FirstName,
                                    SurName = x.Guest.SurName,
                                    Photo = x.Guest.Photo,
                                    TypeGuestId = x.Guest.TypeGuestId,
                                    ZIPCode = x.Guest.ZIPCode,
                                    Company = x.Guest.Company,
                                    Gender = x.Guest.Gender,
                                    Dob = x.Guest.Dob == null ? "" : x.Guest.Dob.Value.ToString("yyyy/MM/dd"),
                                    Region = x.Guest.Region,
                                    Country = x.Guest.Country,
                                    IdentityCart = x.Guest.IdentityCart,
                                    Passport = x.Guest.Passport,
                                    CreditCard = x.Guest.CreditCard,
                                    DoIssueCreditCard = x.Guest.DoIssueCreditCard,
                                    CVC = x.Guest.CVC,
                                    Phone = x.Guest.Phone,
                                    Fax = x.Guest.Fax,
                                    Email = x.Guest.Email,
                                    Address = x.Guest.Address,
                                    Note = x.Guest.Note,
                                    DateCreate = datetimeNow,
                                    CreateBySource = "PMS"
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            }
                        }
                        #region post booking
                        int bookingId = connection.QuerySingleOrDefault<int>("Booking_Post",
                        new
                        {
                            TypeBooking = x.TypeBooking,
                            ReservationId = reservationId,
                            RoomTypeId = x.RoomTypeId,
                            RoomId = x.RoomId,
                            Adult = x.AdultChoose,
                            Children = x.ChildrenChoose,
                            ArrivalDate = x.ArrivalDate,
                            DepartureDate = x.DepartureDate,
                            GuestId = guestBookingId,
                            TypeReservation = reservation.TypeReservation,
                            PaymentType = reservation.PaymentType,
                            UserCreate = user.UserName,
                            CreateDate = datetimeNow,
                            Prepaid = x.DepositMonney,
                            Discount = reservation.Guest.Discount,
                            DepositPecent = -1,
                            DepositMonney = x.DepositMonney,
                            IncludeVATAndServiceCharge = reservation.IncludeTaxFee
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                        #endregion
                        if (x.BookingExtrabeds is null)
                            x.BookingExtrabeds = new List<BookingExtrabed>();
                        x.BookingExtrabeds.ForEach(y =>
                        {
                            if (y.Number > 0)
                            {
                                connection.Execute("Booking_Extrabed_Post",
                                    new
                                    {
                                        BookingId = bookingId,
                                        ExtrabedId = y.ExtrabedId,
                                        Number = y.Number,
                                        Price = Math.Round(y.Price, 0),
                                        DateCreate = datetimeNow
                                    }, commandType: CommandType.StoredProcedure,
                                    transaction: transaction);
                            }
                        });
                        if (x.BookingPrices is null)
                            x.BookingPrices = new List<BookingPrice>();
                        x.BookingPrices.ForEach(y =>
                        {
                            connection.Execute("BookingPrice_Post",
                                new
                                {
                                    BookingId = bookingId,
                                    Date = y.Date,
                                    Price = Math.Round(y.Price, 0)
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                        if (personalServices is null)
                            personalServices = new List<ReservationService>();
                        personalServices.ForEach(y =>
                        {
                            connection.Execute("BookingService_Post",
                                new
                                {
                                    BookingId = bookingId,
                                    ServiceId = y.ServiceId,
                                    Number = y.Number,
                                    Price = Math.Round(y.Price, 0),
                                    DateCreate = y.DateUser
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                        countBooking += 1;
                    });

                    //connection.Execute("Reservation_Put_ExcessCash",
                    //    new
                    //    {
                    //        ReservationId = reservationId,
                    //        ExcessCash = totalAmount
                    //    }, commandType: CommandType.StoredProcedure,
                    //    transaction: transaction);
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //---------------------------------------------------------------------

        public JsonResult GetBooking(int id, int status)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            return Json(getBookingByReservation(id, status), JsonRequestBehavior.AllowGet);
        }
        public string getBookingByReservation(int id, int status = -1)
        {
            if (!CheckSecurity())
                return "";

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Reservation_Get_Booking",
                    new
                    {
                        ReservationId = id,
                        Status = status
                    }, commandType: CommandType.StoredProcedure).ToList();
                string[] taxFees = connection.QuerySingleOrDefault<string>("TaxFee_GetTotal_Lastest",
                       new
                       {
                           HotelId = HotelId
                       },
                       commandType: CommandType.StoredProcedure).Split(',');
                float tax = float.Parse(taxFees[0]);
                float fee = float.Parse(taxFees[1]);
                if (bookings is null)
                    bookings = new List<Booking_Reservation>();
                bookings.ForEach(x =>
                {
                    float taxForBooking = 0;
                    float feesForBooking = 0;
                    using (var multi = connection.QueryMultiple("Booking_Get_Multi_Price",
                        new
                        {
                            BookingId = x.BookingId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        bool includeTaxFee = multi.Read<bool>().SingleOrDefault();
                        x.TotalRoom = (float)Math.Round(multi.Read<float>().SingleOrDefault(), 0);
                        x.TotalExtrabed = (float)Math.Round(multi.Read<float>().SingleOrDefault(), 0);
                        x.TotalService = (float)Math.Round(multi.Read<float>().SingleOrDefault(), 0);

                        if (includeTaxFee)
                        {
                            taxForBooking = tax;
                            feesForBooking = fee;
                        }
                        float total = (x.TotalRoom + x.TotalExtrabed + x.TotalService);
                        float discount = (float)Math.Round(total * x.Discount / 100, 0);
                        x.Total = (float)Math.Round((total - discount) * (100 + taxForBooking + feesForBooking + taxForBooking * feesForBooking / 100) / 100, 0);
                    }
                });
                return JsonConvert.SerializeObject(bookings);
            }
        }
        private string getBookingByReservation(int id, int status = -1, DbConnection connection = null)
        {
            if (connection is null) return "";
            int HotelId = (int)Session["HotelId"];
            List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Reservation_Get_Booking",
                    new
                    {
                        ReservationId = id,
                        Status = status
                    }, commandType: CommandType.StoredProcedure).ToList();
            string[] taxFees = connection.QuerySingleOrDefault<string>("TaxFee_GetTotal_Lastest",
                       new
                       {
                           HotelId = HotelId
                       },
                       commandType: CommandType.StoredProcedure).Split(',');
            float tax = float.Parse(taxFees[0]);
            float fee = float.Parse(taxFees[1]);
            if (bookings is null)
                bookings = new List<Booking_Reservation>();
            bookings.ForEach(x =>
            {
                float taxForBooking = 0;
                float feesForBooking = 0;
                using (var multi = connection.QueryMultiple("Booking_Get_Multi_Price",
                    new
                    {
                        BookingId = x.BookingId
                    }, commandType: CommandType.StoredProcedure))
                {
                    bool includeTaxFee = multi.Read<bool>().SingleOrDefault();
                    x.TotalRoom = multi.Read<float>().SingleOrDefault();
                    x.TotalExtrabed = multi.Read<float>().SingleOrDefault();
                    x.TotalService = multi.Read<float>().SingleOrDefault();

                    if (includeTaxFee)
                    {
                        taxForBooking = tax;
                        feesForBooking = fee;
                    }
                    float total = (x.TotalRoom * (100 + taxForBooking + feesForBooking + taxForBooking * feesForBooking / 100) / 100 + x.TotalExtrabed + x.TotalService);
                    float discount = (float)Math.Round(total * x.Discount / 100, 0);
                    x.Total = (float)Math.Round(total - discount, 0);
                }
            });
            return JsonConvert.SerializeObject(bookings);
        }
        public JsonResult Detail(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Reservation_Detail_Sample",
                    new
                    {
                        LanguageId = LanguageId,
                        ReservationId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    Reservation reservation = multi.Read<Reservation>().SingleOrDefault();
                    if (reservation is null)
                        reservation = new Reservation();
                    reservation.Guest = multi.Read<Guest>().SingleOrDefault();
                    if (reservation.Guest is null)
                        reservation.Guest = new Guest();

                    reservation.Guest.ZIPCode = DataHelper.Decrypt(reservation.Guest.ZIPCode);
                    reservation.Guest.IdentityCart = DataHelper.Decrypt(reservation.Guest.IdentityCart);
                    reservation.Guest.Passport = DataHelper.Decrypt(reservation.Guest.Passport);
                    reservation.Guest.CreditCard = DataHelper.Decrypt(reservation.Guest.CreditCard);
                    reservation.Guest.CVC = DataHelper.Decrypt(reservation.Guest.CVC);

                    reservation.Company = multi.Read<Company>().SingleOrDefault();
                    if (reservation.Company is null)
                        reservation.Company = new Company();
                    List<ReservationNote> reservationNote = multi.Read<ReservationNote>().ToList();
                    if (reservationNote is null)
                        reservationNote = new List<ReservationNote>();
                    List<ReservationService> reservationServices = multi.Read<ReservationService>().ToList();
                    if (reservationServices is null)
                        reservationServices = new List<ReservationService>();
                    List<BookingService> bookingServices = multi.Read<BookingService>().ToList();
                    if (bookingServices is null)
                        bookingServices = new List<BookingService>();
                    List<ReservationEmailSent> reservationEmailSents = multi.Read<ReservationEmailSent>().ToList();
                    if (reservationEmailSents is null) reservationEmailSents = new List<ReservationEmailSent>();
                    ///////////////////////////////////////////////////////
                    #region get totalprice reservation
                    float TotalPriceReservationService = connection.QuerySingleOrDefault<float>("ReservationService_Get_TotalPrice_NotPaid",
                        new
                        {
                            ReservationId = id
                        }, commandType: CommandType.StoredProcedure);

                    string[] taxFees = connection.QuerySingleOrDefault<string>("TaxFee_GetTotal_Lastest",
                       new
                       {
                           HotelId = HotelId
                       },
                       commandType: CommandType.StoredProcedure).Split(',');
                    float tax = float.Parse(taxFees[0]);
                    float fee = float.Parse(taxFees[1]);

                    List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Reservation_Get_Booking",
                    new
                    {
                        ReservationId = id,
                        Status = -1
                    }, commandType: CommandType.StoredProcedure).ToList();
                    if (bookings is null)
                        bookings = new List<Booking_Reservation>();
                    float TotalRoomCharges = 0;
                    float TotalFees = 0;
                    float TotalServices = TotalPriceReservationService;
                    float TotalExtrabed = 0;
                    float TotalAmount = 0;
                    float TotalDiscount = 0;// TotalPriceReservationService * reservation.Guest.Discount / 100;
                    if (bookings.Count > 0)
                    {
                        TotalDiscount = TotalPriceReservationService * bookings[0].Discount / 100;
                    }
                    bookings.ForEach(x =>
                    {
                        float taxForBooking = 0;
                        float feesForBooking = 0;
                        using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                            new
                            {
                                BookingId = x.BookingId
                            }, commandType: CommandType.StoredProcedure))
                        {
                            bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                            x.TotalRoom = multi2.Read<float>().SingleOrDefault();
                            x.TotalExtrabed = multi2.Read<float>().SingleOrDefault();
                            x.TotalService = multi2.Read<float>().SingleOrDefault();

                            // kiểm tra nếu booking yêu cầu phí thuế sẽ cập nhật phí thuế theo taxFees
                            if (includeTaxFee)
                            {
                                taxForBooking = tax;
                                feesForBooking = fee;
                            }

                            float totalFeeRoomServiceExtrabed = (x.TotalRoom + x.TotalExtrabed + x.TotalService);
                            TotalDiscount += totalFeeRoomServiceExtrabed * x.Discount / 100;
                            TotalFees += totalFeeRoomServiceExtrabed * (100 - x.Discount) * (feesForBooking + taxForBooking + feesForBooking * taxForBooking / 100) / 10000;
                            TotalRoomCharges += x.TotalRoom;
                            TotalServices += x.TotalService;
                            TotalExtrabed += x.TotalExtrabed;
                            TotalAmount += x.Paid + x.PrePaid;
                            //x.Total = (float)Math.Round(x.TotalRoom * (100 + taxFees) / 100 + x.TotalExtrabed + x.TotalService, 2);
                        }
                    });
                    //TotalFees = (float)Math.Round(TotalRoomCharges * taxFees / 100, 2);
                    //TotalAmount += reservation.ExcessCash;
                    #endregion
                    return Json(JsonConvert.SerializeObject(new
                    {
                        reservation = reservation,
                        reservationNote = reservationNote,
                        reservationServices = reservationServices,
                        bookingServices = bookingServices,
                        reservationEmailSents = reservationEmailSents,
                        TotalRoomCharges = Math.Round(TotalRoomCharges, 0),
                        TotalFees = Math.Round(TotalFees, 0),
                        TotalServices = Math.Round(TotalServices, 0),
                        TotalExtrabed = Math.Round(TotalExtrabed, 0),
                        TotalAmount = Math.Round(TotalAmount, 0),
                        TotalDiscount = Math.Round(TotalDiscount, 0)
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }

        // detail
        public JsonResult PostNote(int id, string note)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("ReservationNote_Post",
                       new
                       {
                           ReservationId = id,
                           Note = note,
                           CreateDate = DatetimeHelper.DateTimeUTCNow(),
                           UserName = user.UserName
                       }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PutGeneralInfor(Reservation reservation)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                #region put guest
                connection.QuerySingleOrDefault<int>("Guest_Put",
                    new
                    {
                        GuestId = reservation.Guest.GuestId,
                        FirstName = reservation.Guest.FirstName,
                        SurName = reservation.Guest.SurName,
                        Photo = "",
                        ZIPCode = reservation.Guest.ZIPCode,
                        Company = reservation.Guest.Company,
                        Gender = reservation.Guest.Gender,
                        Dob = reservation.Guest.Dob,
                        Region = reservation.Guest.Region,
                        Country = reservation.Guest.Country,
                        IdentityCart = reservation.Guest.IdentityCart,
                        Passport = reservation.Guest.Passport,
                        CreditCard = reservation.Guest.CreditCard,
                        DoIssueCreditCard = reservation.Guest.DoIssueCreditCard,
                        CVC = reservation.Guest.CVC,
                        Phone = reservation.Guest.Phone,
                        Fax = reservation.Guest.Fax,
                        Email = reservation.Guest.Email,
                        Address = reservation.Guest.Address,
                        Note = reservation.Guest.Note

                    }, commandType: CommandType.StoredProcedure);
                #endregion
                if (reservation.Company.CompanyId <= 0)
                {
                    if (reservation.Company.CompanyName != null && reservation.Company.CompanyName != "" &&
                           reservation.Company.CompanyCode != null && reservation.Company.CompanyCode != "")
                    {
                        reservation.Company.CompanyId = connection.QuerySingleOrDefault<int>("Company_Post",
                        new
                        {
                            HotelId = HotelId,
                            GroupGuestId = reservation.Company.GroupGuestId,
                            SourceId = reservation.Company.SourceId,
                            CompanyName = reservation.Company.CompanyName,
                            CompanyCode = reservation.Company.CompanyCode,
                            TaxCode = reservation.Company.TaxCode,
                            Phone = reservation.Company.Phone,
                            Fax = reservation.Company.Fax,
                            Email = reservation.Company.Email,
                            Address = reservation.Company.Address,
                            ContactUsName = reservation.Company.ContactUsName,
                            ContactPhone = reservation.Company.ContactPhone,
                            ContactEmail = reservation.Company.ContactEmail
                        }, commandType: CommandType.StoredProcedure);
                    }
                }
                else
                    #region put company
                    connection.Execute("Company_Put",
                       new
                       {
                           CompanyId = reservation.Company.CompanyId,
                           GroupGuestId = reservation.Company.GroupGuestId,
                           SourceId = reservation.Company.SourceId,
                           CompanyName = reservation.Company.CompanyName,
                           CompanyCode = reservation.Company.CompanyCode,
                           TaxCode = reservation.Company.TaxCode,
                           Phone = reservation.Company.Phone,
                           Fax = reservation.Company.Fax,
                           Email = reservation.Company.Email,
                           Address = reservation.Company.Address,
                           ContactUsName = reservation.Company.ContactUsName,
                           ContactPhone = reservation.Company.ContactPhone,
                           ContactEmail = reservation.Company.ContactEmail
                       }, commandType: CommandType.StoredProcedure);
                #endregion

                #region put reservation
                connection.Execute("Reservation_Put_Sample",
                    new
                    {
                        ReservationId = reservation.ReservationId,
                        ReminiscentName = reservation.ReminiscentName,
                        Adult = reservation.Adult,
                        Children = reservation.Children,
                        Voucher = reservation.Voucher,
                        Color = reservation.Color,
                        ArrivalFlightDate = reservation.ArrivalFlightDate,
                        ArrivalFlightTime = reservation.ArrivalFlightTime,
                        DepartureFlightDate = reservation.DepartureFlightDate,
                        DepartureFlightTime = reservation.DepartureFlightTime,
                        CompanyId = reservation.Company.CompanyId,
                        GuestId = reservation.Guest.GuestId
                    }, commandType: CommandType.StoredProcedure);
                #endregion
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PutService(int id, List<BookingService_Resrvation> bookingServices)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (bookingServices is null)
                bookingServices = new List<BookingService_Resrvation>();
            DateTime dateNow = DatetimeHelper.DateTimeUTCNow();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // xóa toàn bộ dịch vụ chung của đặt phòng và dịch vụ của từng folio
                    List<BookingService> bookingServiceOld = connection.Query<BookingService>("ReservationService_Delete_All_NotPaid_Include_BookingService",
                         new
                         {
                             ReservationId = id
                         },
                         commandType: CommandType.StoredProcedure,
                         transaction: transaction).ToList();
                    if (bookingServiceOld is null) bookingServiceOld = new List<BookingService>();
                    bookingServices.ForEach(x =>
                    {
                        BookingService bookingService = bookingServiceOld.Find(y => y.ServiceId == x.ServiceId);
                        if (bookingService is null)
                            x.DateCreate = dateNow;
                        else
                            x.DateCreate = bookingService.DateCreate;

                        connection.Execute("BookingService_Post",
                               new
                               {
                                   BookingId = x.BookingId,
                                   ServiceId = x.ServiceId,
                                   Number = x.Number,
                                   Price = Math.Round(x.Price, 0),
                                   DateCreate = x.DateCreate
                               }, commandType: CommandType.StoredProcedure,
                               transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult GetBookingDetail(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                string[] taxFees = { "0", "0" };
                using (var multi = connection.QueryMultiple("Booking_Get_Detail",
                        new
                        {
                            BookingId = id,
                            LanguageId = LanguageId
                        }, commandType: CommandType.StoredProcedure))
                {
                    Booking booking = multi.Read<Booking>().SingleOrDefault();
                    List<BookingPrice> bookingPrices = multi.Read<BookingPrice>().ToList();
                    List<BookingExtrabed> bookingExtrabeds = multi.Read<BookingExtrabed>().ToList();
                    List<BookingService> bookingServices = multi.Read<BookingService>().ToList();
                    Guest guest = multi.Read<Guest>().SingleOrDefault();
                    VisaBooking visaBooking = multi.Read<VisaBooking>().SingleOrDefault();
                    List<InvoiceBooking> invoiceBookings = multi.Read<InvoiceBooking>().ToList();
                    string RoomCode = multi.Read<string>().SingleOrDefault();
                    List<BookingExtrabed> extrabeds = multi.Read<BookingExtrabed>().ToList();
                    CardBooking cardBooking = multi.Read<CardBooking>().SingleOrDefault();
                    ///////////////////
                    if (cardBooking is null) cardBooking = new CardBooking();
                    if (booking is null) booking = new Booking();
                    if (bookingPrices is null) bookingPrices = new List<BookingPrice>();
                    if (bookingExtrabeds is null) bookingExtrabeds = new List<BookingExtrabed>();
                    if (bookingServices is null) bookingServices = new List<BookingService>();
                    if (guest is null) guest = new Guest();
                    if (visaBooking is null) visaBooking = new VisaBooking();
                    if (invoiceBookings is null) invoiceBookings = new List<InvoiceBooking>();
                    if (RoomCode is null) RoomCode = "";
                    if (extrabeds is null) extrabeds = new List<BookingExtrabed>();

                    if (booking.IncludeVATAndServiceCharge)
                    {
                        taxFees = connection.QuerySingleOrDefault<string>("TaxFee_GetTotal_Lastest",
                       new
                       {
                           HotelId = HotelId
                       },
                       commandType: CommandType.StoredProcedure).Split(',');
                    }
                    float tax = float.Parse(taxFees[0]);
                    float fee = float.Parse(taxFees[1]);
                    #region Decrypt data
                    // guest
                    guest.ZIPCode = DataHelper.Decrypt(guest.ZIPCode);
                    guest.IdentityCart = DataHelper.Decrypt(guest.IdentityCart);
                    guest.Passport = DataHelper.Decrypt(guest.Passport);
                    guest.CreditCard = DataHelper.Decrypt(guest.CreditCard);
                    guest.CVC = DataHelper.Decrypt(guest.CVC);
                    // card
                    cardBooking.Number = DataHelper.Decrypt(cardBooking.Number);
                    cardBooking.Name = DataHelper.Decrypt(cardBooking.Name);
                    cardBooking.Code = DataHelper.Decrypt(cardBooking.Code);
                    // visa
                    visaBooking.SerialNo = DataHelper.Decrypt(visaBooking.SerialNo);
                    visaBooking.VisaNo = DataHelper.Decrypt(visaBooking.VisaNo);
                    #endregion


                    booking.CardBooking = cardBooking;
                    guest.GuestId = booking.GuestId;
                    booking.RoomCode = RoomCode;
                    booking.Guest = guest;
                    booking.BookingPrices = bookingPrices;
                    booking.BookingExtrabeds = bookingExtrabeds;
                    booking.BookingServices = bookingServices;
                    booking.VisaBookings = visaBooking;
                    booking.InvoiceBookings = invoiceBookings;
                    float TotalRoomCharges = bookingPrices.Sum(x => x.Price);
                    float TotalServices = bookingServices.Sum(x => x.Price * x.Number);
                    float TotalExtrabed = bookingExtrabeds.Sum(x => x.Price * x.Number);
                    float TotalDiscount = (float)Math.Round((TotalRoomCharges + TotalServices + TotalExtrabed) * booking.Discount / 100, 0);
                    float TotalFees = (TotalRoomCharges + TotalServices + TotalExtrabed) * (100 - booking.Discount) * (tax + fee + tax * fee / 100) / 10000;

                    return Json(JsonConvert.SerializeObject(new
                    {
                        booking = booking,
                        TotalRoomCharges = Math.Round(TotalRoomCharges, 0),
                        TotalFees = Math.Round(TotalFees, 0),
                        TotalServices = Math.Round(TotalServices, 0),
                        TotalExtrabed = Math.Round(TotalExtrabed, 0),
                        TotalDiscount = TotalDiscount,
                        Extrabeds = extrabeds,
                        taxFees = taxFees
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult PutBooking(Booking booking)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            DateTime dateNow = DatetimeHelper.DateTimeUTCNow();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute("Booking_Put_Sample",
                    new
                    {
                        BookingId = booking.BookingId,
                        Adult = booking.Adult,
                        Children = booking.Children,
                        Note = booking.Note,
                        PaymentType = booking.PaymentType
                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);
                    using (var multi = connection.QueryMultiple("Booking_Detail_Delete_All_Sample",
                        new
                        {
                            BookingId = booking.BookingId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction))
                    {
                        List<BookingExtrabed> bookingExtrabeds = multi.Read<BookingExtrabed>().ToList();
                        List<BookingService> bookingServices = multi.Read<BookingService>().ToList();
                        if (bookingExtrabeds is null) bookingExtrabeds = new List<BookingExtrabed>();
                        if (bookingServices is null) bookingServices = new List<BookingService>();

                        if (booking.BookingPrices is null) booking.BookingPrices = new List<BookingPrice>();
                        booking.BookingPrices.ForEach(x =>
                        {
                            if (!x.Paid)
                            {
                                connection.Execute("Booking_Price_Put",
                                new
                                {
                                    BookingId = booking.BookingId,
                                    Date = x.Date,
                                    Price = x.Price
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            }
                        });
                        if (booking.BookingExtrabeds is null) booking.BookingExtrabeds = new List<BookingExtrabed>();
                        booking.BookingExtrabeds.ForEach(x =>
                        {
                            if (!x.Paid)
                            {
                                //BookingExtrabed bookingExtrabed = bookingExtrabeds.Find(y => y.ExtrabedId == x.ExtrabedId);
                                //if (bookingExtrabed is null)
                                //    x.DateCreate = dateNow;
                                //else
                                //    x.DateCreate = bookingExtrabed.DateCreate;
                                connection.Execute("Booking_Extrabed_Post",
                                new
                                {
                                    BookingId = booking.BookingId,
                                    ExtrabedId = x.ExtrabedId,
                                    Number = x.Number,
                                    Price = Math.Round(x.Price, 2),
                                    DateCreate = x.DateCreate
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            }
                        });
                        if (booking.BookingServices is null) booking.BookingServices = new List<BookingService>();
                        booking.BookingServices.ForEach(x =>
                        {
                            if (!x.Paid)
                            {
                                //BookingService bookingService = bookingServices.Find(y => y.ServiceId == x.ServiceId);
                                //if (bookingService is null)
                                //    x.DateCreate = dateNow;
                                //else
                                //    x.DateCreate = bookingService.DateCreate;
                                connection.Execute("BookingService_Post",
                                new
                                {
                                    BookingId = booking.BookingId,
                                    ServiceId = x.ServiceId,
                                    Number = x.Number,
                                    Price = Math.Round(x.Price, 0),
                                    DateCreate = x.DateCreate
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            }
                        });
                    }

                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }

            }
        }
        public JsonResult PutGuestBooking(Guest guest, int bookingId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (guest is null)
                guest = new Guest();
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                if (guest.GuestId <= 0)
                {
                    guest.GuestId = connection.QuerySingleOrDefault<int>("Guest_Post",
                    new
                    {
                        HotelId = HotelId,
                        FirstName = guest.FirstName,
                        SurName = guest.SurName,
                        Photo = "",
                        TypeGuestId = guest.TypeGuestId,
                        ZIPCode = DataHelper.Encrypt(guest.ZIPCode),
                        Company = guest.Company,
                        Gender = guest.Gender,
                        Dob = guest.Dob == null ? "" : guest.Dob.Value.ToString("yyyy/MM/dd"),
                        Region = guest.Region,
                        Country = guest.Country,
                        IdentityCart = DataHelper.Encrypt(guest.IdentityCart),
                        Passport = DataHelper.Encrypt(guest.Passport),
                        CreditCard = DataHelper.Encrypt(guest.CreditCard),
                        DoIssueCreditCard = guest.DoIssueCreditCard,
                        CVC = DataHelper.Encrypt(guest.CVC),
                        Phone = guest.Phone,
                        Fax = guest.Fax,
                        Email = guest.Email,
                        Address = guest.Address,
                        Note = guest.Note,
                        DateCreate = DatetimeHelper.DateTimeUTCNow(),
                        CreateBySource = "PMS"
                    }, commandType: CommandType.StoredProcedure);
                }
                else
                {
                    connection.QuerySingleOrDefault<int>("Guest_Put",
                    new
                    {
                        GuestId = guest.GuestId,
                        FirstName = guest.FirstName,
                        SurName = guest.SurName,
                        Photo = "",
                        ZIPCode = DataHelper.Encrypt(guest.ZIPCode),
                        Company = guest.Company,
                        Gender = guest.Gender,
                        Dob = guest.Dob,
                        Region = guest.Region,
                        Country = guest.Country,
                        IdentityCart = DataHelper.Encrypt(guest.IdentityCart),
                        Passport = DataHelper.Encrypt(guest.Passport),
                        CreditCard = DataHelper.Encrypt(guest.CreditCard),
                        DoIssueCreditCard = guest.DoIssueCreditCard,
                        CVC = DataHelper.Encrypt(guest.CVC),
                        Phone = guest.Phone,
                        Fax = guest.Fax,
                        Email = guest.Email,
                        Address = guest.Address,
                        Note = guest.Note

                    }, commandType: CommandType.StoredProcedure);
                }
                connection.Execute("Booking_Put_Guest",
                    new
                    {
                        BookingId = bookingId,
                        GuestId = guest.GuestId
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PutVisaBooking(VisaBooking visa, int bookingId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (visa is null)
                visa = new VisaBooking();
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("VisaBooking_Put",
                    new
                    {
                        BookingId = bookingId,
                        SerialNo = DataHelper.Encrypt(visa.SerialNo),
                        VisaNo = DataHelper.Encrypt(visa.VisaNo),
                        VisaDate = visa.VisaDate,
                        VisaIssuePlace = visa.VisaIssuePlace,
                        ArrivalFrom = visa.ArrivalFrom,
                        ArrivalTrasportation = visa.ArrivalTrasportation,
                        VisaExpiryDate = visa.VisaExpiryDate,
                        DateOfArrivalIn = visa.DateOfArrivalIn,
                        TimeOfArrivalIn = visa.TimeOfArrivalIn,
                        PurposeOfVisit = visa.PurposeOfVisit,
                        GoingTo = visa.GoingTo,
                        DepartTransportation = visa.DepartTransportation,
                        ProposedDuration = visa.ProposedDuration,
                        VisaType = visa.VisaType
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PutCardBooking(CardBooking card, int bookingId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (card is null)
                card = new CardBooking();
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("CardBooking_Put",
                    new
                    {
                        BookingId = bookingId,
                        Name = DataHelper.Encrypt(card.Name),
                        Number = DataHelper.Encrypt(card.Number),
                        Code = DataHelper.Encrypt(card.Code),
                        ExpirationMonth = card.ExpirationMonth,
                        ExpirationYear = card.ExpirationYear
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteCardBooking(int bookingId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("CardBooking_Delete",
                    new
                    {
                        BookingId = bookingId
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRoomAvailableForBooking(int roomtypeId, DateTime fromDate, DateTime toDate, int typeBooking)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<RoomType> roomTypes = connection.Query<RoomType>("RoomType_Get_Full",
                    new
                    {
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure).ToList();
                using (var multi = connection.QueryMultiple("Room_Get_Available",
                        new
                        {
                            RoomTypeId = roomtypeId,
                            FromDate = fromDate.ToString("yyyy/MM/dd HH:mm:ss"),
                            ToDate = toDate.ToString("yyyy/MM/dd HH:mm:ss"),
                            TypeBooking = typeBooking
                        }, commandType: CommandType.StoredProcedure))
                {
                    List<Room> rooms = multi.Read<Room>().ToList();
                    List<int> roomAvailable = multi.Read<int>().ToList();
                    if (rooms is null)
                        rooms = new List<Room>();
                    if (roomAvailable is null)
                        roomAvailable = new List<int>();
                    roomAvailable.RemoveAll(x => x < 0);
                    List<Room> roomsResult = rooms.FindAll(x => roomAvailable.FindIndex(y => y == x.RoomId) < 0);
                    if (roomsResult is null)
                        roomsResult = new List<Room>();

                    return Json(new
                    {
                        roomTypes = roomTypes,
                        rooms = roomsResult
                    }, JsonRequestBehavior.AllowGet);
                }
            }

        }
        public JsonResult AssignRoom(int bookingId, int roomtypeId, int roomId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("Booking_AssignRoom",
                    new
                    {
                        BookingId = bookingId,
                        RoomTypeId = roomtypeId,
                        RoomId = roomId
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CheckIn(int bookingId)
        {
            // cập nhật ngày checkin sớm
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("Booking_Checkin",
                    new
                    {
                        BookingId = bookingId,
                        Date = DatetimeHelper.DateTimeUTCNow(),
                        UserCheckIn = user.UserName
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult UndoCheckIn(int bookingId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("Booking_UpdateStatus",
                    new
                    {
                        BookingId = bookingId,
                        Status = 1
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult NoShow(int bookingId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("Booking_NoShow",
                    new
                    {
                        BookingId = bookingId,
                        Date = DatetimeHelper.DateTimeUTCNow(),
                        UserNoShow = user.UserName
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Cancel(int bookingId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("Booking_Cancel",
                    new
                    {
                        BookingId = bookingId,
                        Date = DatetimeHelper.DateTimeUTCNow(),
                        UserCancel = user.UserName
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CheckOut(int bookingId, float totalAmount)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            // cập nhật checkout sớm(muộn) nếu có
            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("Booking_CheckOut",
                    new
                    {
                        BookingId = bookingId,
                        Date = DatetimeHelper.DateTimeUTCNow(),
                        UserCheckOut = user.UserName,
                        totalAmount = totalAmount
                    }, commandType: CommandType.StoredProcedure);
                connection.Execute("Booking_AddTotalPaid_ToGuest",
                    new
                    {
                        BookingId = bookingId,
                        totalAmount = totalAmount
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult TransferDebt(int bookingId, float totalAmount, float paid, float prepay)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            // cập nhật checkout sớm(muộn) nếu có
            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("Booking_TransferDebt",
                    new
                    {
                        BookingId = bookingId,
                        totalAmount = totalAmount,
                        paid = paid + prepay,
                        Date = DatetimeHelper.DateTimeUTCNow(),
                        UserTransfer = user.UserName
                    }, commandType: CommandType.StoredProcedure);
                connection.Execute("Booking_AddTotalPaid_ToGuest",
                    new
                    {
                        BookingId = bookingId,
                        totalAmount = prepay
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPriceRoomByRangeDate(int roomtypeId, DateTime fromDate, DateTime toDate, string voucher)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                List<BookingPrice> bookingPrices = new List<BookingPrice>();
                for (DateTime date = fromDate.Date; date < toDate.Date; date = date.AddDays(1))
                {
                    BookingPrice bookingPrice = connection.QuerySingleOrDefault<BookingPrice>("RateAvailability_GetPriceRoomByRangeDate",
                        new
                        {
                            RoomTypeId = roomtypeId,
                            Date = date
                        }, commandType: CommandType.StoredProcedure);
                    if (bookingPrice is null)
                        bookingPrice = new BookingPrice();
                    bookingPrices.Add(new BookingPrice()
                    {
                        Date = date,
                        Price = bookingPrice.Price,
                        Number = bookingPrice.Number
                    });
                }
                return Json(JsonConvert.SerializeObject(bookingPrices), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ChangeStay(int bookingId, DateTime fromDate, DateTime toDate, List<BookingPrice> scheduleRoomPrice)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                if (scheduleRoomPrice is null)
                    scheduleRoomPrice = new List<BookingPrice>();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute("Booking_ChangeStay",
                        new
                        {
                            BookingId = bookingId,
                            ArrivalDate = fromDate,
                            DepartureDate = toDate
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    scheduleRoomPrice.ForEach(x =>
                    {
                        connection.Execute("BookingPrice_Post",
                            new
                            {
                                BookingId = bookingId,
                                Date = x.Date,
                                Price = x.Price
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult PayBooking(int bookingId, float totalAmount, float totalRooms, float totalFees, float totalDiscount,
                                    bool roomCharge, List<BookingService> bookingServices,
                                    List<BookingExtrabed> bookingExtrabeds)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            //return Json(1, JsonRequestBehavior.AllowGet);
            User user = (User)Session["User"];
            DateTime dateNow = DatetimeHelper.DateTimeUTCNow();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute("Booking_Put_Paid",
                        new
                        {
                            BookingId = bookingId,
                            TotalAmount = Math.Round(totalAmount, 0)
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if (roomCharge)
                    {
                        connection.Execute("BookingPrice_Paid",
                            new
                            {
                                BookingId = bookingId
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    }
                    if (bookingServices is null) bookingServices = new List<BookingService>();
                    bookingServices.ForEach(x =>
                    {
                        if (x.check)
                        {
                            connection.Execute("BookingService_Paid",
                            new
                            {
                                BookingId = bookingId,
                                BookingServiceId = x.BookingServiceId,
                                DatePaid = dateNow
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                        }
                    });
                    if (bookingExtrabeds is null) bookingExtrabeds = new List<BookingExtrabed>();
                    bookingExtrabeds.ForEach(x =>
                    {
                        if (x.check)
                        {
                            connection.Execute("BookingExtrabed_Paid",
                            new
                            {
                                BookingId = bookingId,
                                BookingExtrabedId = x.BookingExtrabedId,
                                DatePaid = dateNow
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                        }
                    });
                    connection.Execute("InvoiceBooking_Post",
                        new
                        {
                            BookingId = bookingId,
                            Title = "Invoice",
                            CreateDate = DatetimeHelper.DateTimeUTCNow(),
                            JsonData = JsonConvert.SerializeObject(new
                            {
                                totalAmount = totalAmount,
                                totalRooms = totalRooms,
                                totalFees = totalFees,
                                roomCharge = roomCharge,
                                bookingServices = bookingServices.FindAll(x => x.check),
                                bookingExtrabeds = bookingExtrabeds.FindAll(x => x.check),
                                totalDiscount = totalDiscount
                            }),
                            UserCreate = user.UserName
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult OpenInvoice(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("InvoiceBooking_Get_Detail",
                    new
                    {
                        InvoiceBookingId = id,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    Hotel hotel = multi.Read<Hotel>().SingleOrDefault();
                    Guest guest = multi.Read<Guest>().SingleOrDefault();
                    Booking booking = multi.Read<Booking>().SingleOrDefault();
                    InvoiceBooking invoiceBooking = multi.Read<InvoiceBooking>().SingleOrDefault();
                    if (hotel is null) hotel = new Hotel();
                    if (guest is null) guest = new Guest();
                    if (booking is null) booking = new Booking();
                    if (invoiceBooking is null) invoiceBooking = new InvoiceBooking();
                    invoiceBooking.Hotel = hotel;
                    invoiceBooking.Guest = guest;
                    invoiceBooking.Booking = booking;
                    return Json(JsonConvert.SerializeObject(new
                    {
                        invoiceBooking = invoiceBooking
                    }), JsonRequestBehavior.AllowGet);
                }

            }
        }
        public JsonResult CalculatorRoomPrice(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var multi = connection.QueryMultiple("Booking_Get_DataForCalculatorRoomPrice",
                    new
                    {
                        BookingId = id,
                        Date = DatetimeHelper.DateTimeUTCNow()
                    }, commandType: CommandType.StoredProcedure))
                {
                    float totalSeconds = multi.Read<float>().SingleOrDefault();
                    RoomTypePriceHour roomTypePriceHour = multi.Read<RoomTypePriceHour>().SingleOrDefault();
                    if (roomTypePriceHour is null) roomTypePriceHour = new RoomTypePriceHour();
                    //
                    float totalHour = totalSeconds / 3600;
                    float totalPrice = roomTypePriceHour.PriceStart;
                    if (totalHour > roomTypePriceHour.HourStart)
                    {
                        totalHour = totalHour - roomTypePriceHour.HourStart;
                        if (totalHour > 0 && totalHour <= roomTypePriceHour.HourNext1)
                        {
                            totalPrice += totalHour * roomTypePriceHour.PriceNext1;
                        }
                        else if (totalHour > roomTypePriceHour.HourNext1)
                        {
                            totalPrice += roomTypePriceHour.HourNext1 * roomTypePriceHour.PriceNext1;
                            totalHour = totalHour - roomTypePriceHour.HourNext1;

                            if (totalHour > 0 && totalHour <= roomTypePriceHour.HourNext2)
                            {
                                totalPrice += totalHour * roomTypePriceHour.PriceNext2;
                            }
                            else if (totalHour > roomTypePriceHour.HourNext2)
                            {
                                totalPrice += roomTypePriceHour.HourNext2 * roomTypePriceHour.PriceNext2;
                                totalHour = totalHour - roomTypePriceHour.HourNext2;

                                if (totalHour > 0)
                                {
                                    totalPrice += totalHour * roomTypePriceHour.PriceNext3;
                                }
                            }
                        }
                    }
                    totalPrice = (float)Math.Round(totalPrice, 0);
                    connection.Execute("BookingPrice_Put_PriceForTypeHour",
                        new
                        {
                            BookingId = id,
                            Price = totalPrice
                        }, commandType: CommandType.StoredProcedure);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult PostAddBooking(int reservationId, List<Booking> bookings, bool includeVATAndServiceCharge)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (bookings is null) bookings = new List<Booking>();
            DateTime datetimeNow = DatetimeHelper.DateTimeUTCNow();
            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                float discount = connection.QuerySingleOrDefault<float>("Reservation_Get_DiscountOfGuest",
                    new
                    {
                        ReservationId = reservationId
                    }, commandType: CommandType.StoredProcedure);
                using (var transaction = connection.BeginTransaction())
                {
                    bookings.ForEach(x =>
                    {
                        int bookingId = connection.QuerySingleOrDefault<int>("Booking_Post",
                            new
                            {
                                TypeBooking = x.TypeBooking,
                                ReservationId = reservationId,
                                RoomTypeId = x.RoomTypeId,
                                RoomId = x.RoomId,
                                Adult = x.Adult,
                                Children = x.Children,
                                ArrivalDate = x.ArrivalDate,
                                DepartureDate = x.DepartureDate,
                                GuestId = -1,
                                TypeReservation = x.TypeBooking,
                                PaymentType = x.PaymentType,
                                UserCreate = user.UserName,
                                CreateDate = datetimeNow,
                                Prepaid = 0,
                                Discount = discount,
                                DepositPecent = 0,
                                DepositMonney = 0,
                                IncludeVATAndServiceCharge = includeVATAndServiceCharge
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                        if (x.BookingExtrabeds is null)
                            x.BookingExtrabeds = new List<BookingExtrabed>();
                        x.BookingExtrabeds.ForEach(y =>
                        {
                            if (y.Number > 0)
                            {
                                connection.Execute("Booking_Extrabed_Post",
                                    new
                                    {
                                        BookingId = bookingId,
                                        ExtrabedId = y.ExtrabedId,
                                        Number = y.Number,
                                        Price = Math.Round(y.Price, 0),
                                        DateCreate = datetimeNow
                                    }, commandType: CommandType.StoredProcedure,
                                    transaction: transaction);
                            }
                        });
                        if (x.BookingPrices is null)
                            x.BookingPrices = new List<BookingPrice>();
                        x.BookingPrices.ForEach(y =>
                        {
                            connection.Execute("BookingPrice_Post",
                                new
                                {
                                    BookingId = bookingId,
                                    Date = y.Date,
                                    Price = Math.Round(y.Price, 0)
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult AutoAssignRoom(int reservationId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                List<Booking> bookingsNeedToAssign = connection.Query<Booking>("Reservation_Get_BookingNeedToAssignRoom",
                    new
                    {
                        ReservationId = reservationId
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (bookingsNeedToAssign is null) bookingsNeedToAssign = new List<Booking>();
                List<int> folioOverBooking = new List<int>();
                bookingsNeedToAssign.ForEach(x =>
                {
                    if (x.TypeBooking == 1)
                        x.DepartureDate = x.ArrivalDate.AddDays(1);
                    using (var multi = connection.QueryMultiple("Room_Get_Available",
                        new
                        {
                            RoomTypeId = x.RoomTypeId,
                            FromDate = x.ArrivalDate.ToString("yyyy/MM/dd HH:mm:ss"),
                            ToDate = x.DepartureDate.ToString("yyyy/MM/dd HH:mm:ss"),
                            TypeBooking = x.TypeBooking
                        }, commandType: CommandType.StoredProcedure))
                    {
                        List<Room_Reservation> Rooms = new List<Room_Reservation>();
                        List<Room_Reservation> room_Reservations = multi.Read<Room_Reservation>().ToList();
                        List<int> roomAvailable = multi.Read<int>().ToList();
                        if (room_Reservations is null)
                            room_Reservations = new List<Room_Reservation>();
                        if (roomAvailable is null)
                            roomAvailable = new List<int>();
                        room_Reservations.ForEach(y =>
                        {
                            if (roomAvailable.FindIndex(z => z == y.RoomId) < 0)
                                Rooms.Add(y);
                        });
                        if (Rooms.Count == 0)
                            folioOverBooking.Add(x.BookingId);
                        else
                        {
                            connection.Execute("Booking_AssignRoom",
                                new
                                {
                                    BookingId = x.BookingId,
                                    RoomTypeId = x.RoomTypeId,
                                    RoomId = Rooms[0].RoomId
                                }, commandType: CommandType.StoredProcedure);
                        }
                    }
                });
                return Json(new
                {
                    result = folioOverBooking.Count == 0 ? 1 : -1,
                    message = folioOverBooking
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetBookingNeedToCheckIn(int reservationId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                List<Booking> bookingsNeedToAssign = connection.Query<Booking>("Reservation_Get_BookingNeedToCheckIn",
                    new
                    {
                        ReservationId = reservationId
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (bookingsNeedToAssign is null) bookingsNeedToAssign = new List<Booking>();
                return Json(JsonConvert.SerializeObject(bookingsNeedToAssign), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PostBookingNeedToCheckIn(List<Booking> bookingsNeedToCheckIn)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (bookingsNeedToCheckIn is null) bookingsNeedToCheckIn = new List<Booking>();
            DateTime datetimeNow = DatetimeHelper.DateTimeUTCNow();
            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                bookingsNeedToCheckIn.ForEach(x =>
                {
                    connection.Execute("Booking_Checkin",
                        new
                        {
                            BookingId = x.BookingId,
                            Date = datetimeNow,
                            UserCheckIn = user.UserName
                        }, commandType: CommandType.StoredProcedure);
                });
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PostBookingNeedToCheckOut(List<Booking> bookingsNeedToCheckOut)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (bookingsNeedToCheckOut is null) bookingsNeedToCheckOut = new List<Booking>();
            DateTime datetimeNow = DatetimeHelper.DateTimeUTCNow();
            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                bookingsNeedToCheckOut.ForEach(x =>
                {
                    connection.Execute("Booking_CheckOut",
                        new
                        {
                            BookingId = x.BookingId,
                            Date = datetimeNow,
                            UserCheckOut = user.UserName,
                            totalAmount = x.TotalAmount
                        }, commandType: CommandType.StoredProcedure);
                    connection.Execute("Booking_AddTotalPaid_ToGuest",
                    new
                    {
                        BookingId = x.BookingId,
                        totalAmount = x.TotalAmount
                    }, commandType: CommandType.StoredProcedure);
                });
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetReservationForClusters(int reservationId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                string[] taxFees = connection.QuerySingleOrDefault<string>("TaxFee_GetTotal_Lastest",
                       new
                       {
                           HotelId = HotelId
                       },
                       commandType: CommandType.StoredProcedure).Split(',');
                float tax = float.Parse(taxFees[0]);
                float fee = float.Parse(taxFees[1]);
                using (var multi = connection.QueryMultiple("Reservation_GetForClusters",
                    new
                    {
                        ReservationId = reservationId
                    }, commandType: CommandType.StoredProcedure))
                {
                    Reservation_Sample reservation = multi.Read<Reservation_Sample>().SingleOrDefault();
                    List<Booking_Reservation> bookings = multi.Read<Booking_Reservation>().ToList();
                    if (reservation is null) reservation = new Reservation_Sample();
                    if (bookings is null) bookings = new List<Booking_Reservation>();
                    bookings.ForEach(x =>
                    {
                        float taxForBooking = 0;
                        float feesForBooking = 0;
                        using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                            new
                            {
                                BookingId = x.BookingId
                            }, commandType: CommandType.StoredProcedure))
                        {
                            bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                            x.TotalRoom = multi2.Read<float>().SingleOrDefault();
                            x.TotalExtrabed = multi2.Read<float>().SingleOrDefault();
                            x.TotalService = multi2.Read<float>().SingleOrDefault();

                            // kiểm tra nếu booking yêu cầu phí thuế sẽ cập nhật phí thuế theo taxFees
                            if (includeTaxFee)
                            {
                                taxForBooking = tax;
                                feesForBooking = fee;
                            }
                            float total = (x.TotalRoom + x.TotalExtrabed + x.TotalService);
                            float discount = (float)Math.Round(total * x.Discount / 100, 0);
                            x.Total = (float)Math.Round((total - discount) * (100 + taxForBooking + feesForBooking + taxForBooking * feesForBooking / 100) / 100, 0);
                        }
                    });
                    return Json(JsonConvert.SerializeObject(new
                    {
                        bookings = bookings,
                        reservation = reservation
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult PostGroupClusters(int reservationId, List<Booking_Reservation> bookings)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (bookings is null) bookings = new List<Booking_Reservation>();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    bookings.ForEach(x =>
                    {
                        connection.Execute("Reservation_GroupClusters",
                            new
                            {
                                ReservationId = reservationId,
                                BookingId = x.BookingId
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        // API sample
        public JsonResult GetReservation(string userName, string password, int HotelId, int filter, string keySearch, int source, List<int> status,
                           DateTime fromDate, DateTime toDate, int pageNumber = 1, int pageSize = 100, bool untreated = true)
        {
            if (status is null)
                status = new List<int>();
            int statusNew = status.FindIndex(x => x == 1) >= 0 ? 1 : 0;
            int statusInHouse = status.FindIndex(x => x == 2) >= 0 ? 1 : 0;
            int statusCheckOut = status.FindIndex(x => x == 3) >= 0 ? 1 : 0;
            int statusNoShow = status.FindIndex(x => x == 4) >= 0 ? 1 : 0;
            int statusCancel = status.FindIndex(x => x == 5) >= 0 ? 1 : 0;
            int statusDebt = status.FindIndex(x => x == 6) >= 0 ? 1 : 0;
            using (var connection = DB.ConnectionFactory())
            {
                try
                {
                    string pass = DataHelper.Decrypt(password);
                    int resultCheckAccept = connection.QuerySingleOrDefault<int>("User_Check_AllowAccess_API",
                           new
                           {
                               Username = userName,
                               Password = DataHelper.CreateMD5(userName + pass + DataHelper.key),
                               HotelId = HotelId
                           }, commandType: System.Data.CommandType.StoredProcedure);
                    if (resultCheckAccept < 0)
                    {
                        return Json(JsonConvert.SerializeObject(new
                        {
                            status = -1,
                            message = "account or password is incorrect",
                            data = ""
                        }), JsonRequestBehavior.AllowGet);
                    }
                    string storedProc = GetType_StoredProcedure(filter);
                    using (var multi = connection.QueryMultiple(storedProc,
                        new
                        {
                            HotelId = HotelId,
                            untreated = untreated,
                            keySearch = keySearch,
                            source = source,
                            fromDate = fromDate,
                            toDate = toDate,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            statusNew = statusNew,
                            statusInHouse = statusInHouse,
                            statusCheckOut = statusCheckOut,
                            statusNoShow = statusNoShow,
                            statusCancel = statusCancel,
                            statusDebt = statusDebt
                        }, commandType: CommandType.StoredProcedure))
                    {
                        List<Reservation_Sample> reservations = multi.Read<Reservation_Sample>().ToList();
                        int totalRecord = multi.Read<int>().SingleOrDefault();
                        double totalPrice = 0;
                        double totalPaid = 0;
                        if (reservations is null)
                            reservations = new List<Reservation_Sample>();

                        string[] taxFees = connection.QuerySingleOrDefault<string>("TaxFee_GetTotal_Lastest",
                          new
                          {
                              HotelId = HotelId
                          },
                          commandType: CommandType.StoredProcedure).Split(',');
                        float tax = float.Parse(taxFees[0]);
                        float fee = float.Parse(taxFees[1]);
                        reservations.ForEach(x =>
                        {
                            List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Reservation_Get_Booking",
                            new
                            {
                                ReservationId = x.ReservationId,
                                Status = -1
                            }, commandType: CommandType.StoredProcedure).ToList();
                            if (bookings is null)
                                bookings = new List<Booking_Reservation>();
                            bookings.ForEach(y =>
                            {
                                float taxForBooking = 0;
                                float feesForBooking = 0;
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
                                    {
                                        taxForBooking = tax;
                                        feesForBooking = fee;
                                    }

                                    float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                                    float discount = (float)Math.Round(total * y.Discount / 100, 0);
                                    y.Total = (float)Math.Round((total - discount) * (100 + taxForBooking + feesForBooking + feesForBooking * taxForBooking / 100) / 100, 0);
                                }
                            });
                            x.TotalPrice = (float)Math.Round(bookings.Sum(y => y.Total), 0);
                            x.Paid = (float)Math.Round(bookings.Sum(y => y.Paid + y.PrePaid), 0);
                        });
                        reservations.ForEach(x =>
                        {
                            List<string> roomCurrent = new List<string>();
                            if (x.RoomTypeName_s != null)
                            {
                                string[] rooms = x.RoomTypeName_s.Split(',');
                                foreach (string room in rooms)
                                {
                                    int numberDiscount = rooms.ToList().FindAll(y => roomCurrent.FindIndex(z => z.Split('(')[0].Trim() == room.Trim()) < 0 && y.Trim() == room.Trim()).Count;
                                    if (numberDiscount > 0)
                                        roomCurrent.Add(room.Trim() + "(" + numberDiscount + ")");
                                }
                                x.RoomTypeName_s = string.Join(", ", roomCurrent.ToArray());
                            }
                            totalPrice += Math.Round(x.TotalPrice, 0);
                            totalPaid += Math.Round(x.Paid, 0);
                        });
                        return Json(JsonConvert.SerializeObject(new
                        {
                            status = 1,
                            message = "success",
                            data = new
                            {
                                reservations = reservations,
                                totalRecord = totalRecord,
                                totalPrice = Math.Round(totalPrice, 0),
                                totalPaid = Math.Round(totalPaid, 0)
                            }
                        }), JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    return Json(JsonConvert.SerializeObject(new
                    {
                        status = -1,
                        message = e.Message,
                        data = ""
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult DetailReservation(string userName, string password, string lang, int HotelId, int id)
        {
            using (var connection = DB.ConnectionFactory())
            {
                try
                {
                    string pass = DataHelper.Decrypt(password);
                    int resultCheckAccept = connection.QuerySingleOrDefault<int>("User_Check_AllowAccess_API",
                           new
                           {
                               Username = userName,
                               Password = DataHelper.CreateMD5(userName + pass + DataHelper.key),
                               HotelId = HotelId
                           }, commandType: System.Data.CommandType.StoredProcedure);
                    if (resultCheckAccept < 0)
                    {
                        return Json(JsonConvert.SerializeObject(new
                        {
                            status = -1,
                            message = "account or password is incorrect",
                            data = ""
                        }), JsonRequestBehavior.AllowGet);
                    }
                    int LanguageId = connection.QuerySingleOrDefault<int>("Language_GetId",
                        new
                        {
                            lang = lang
                        }, commandType: CommandType.StoredProcedure);
                    using (var multi = connection.QueryMultiple("Reservation_Detail_Sample",
                        new
                        {
                            LanguageId = LanguageId,
                            ReservationId = id
                        }, commandType: CommandType.StoredProcedure))
                    {
                        Reservation reservation = multi.Read<Reservation>().SingleOrDefault();
                        if (reservation is null)
                            reservation = new Reservation();
                        reservation.Guest = multi.Read<Guest>().SingleOrDefault();
                        if (reservation.Guest is null)
                            reservation.Guest = new Guest();

                        reservation.Guest.ZIPCode = DataHelper.Decrypt(reservation.Guest.ZIPCode);
                        reservation.Guest.IdentityCart = DataHelper.Decrypt(reservation.Guest.IdentityCart);
                        reservation.Guest.Passport = DataHelper.Decrypt(reservation.Guest.Passport);
                        reservation.Guest.CreditCard = DataHelper.Decrypt(reservation.Guest.CreditCard);
                        reservation.Guest.CVC = DataHelper.Decrypt(reservation.Guest.CVC);

                        reservation.Company = multi.Read<Company>().SingleOrDefault();
                        if (reservation.Company is null)
                            reservation.Company = new Company();
                        List<ReservationNote> reservationNote = multi.Read<ReservationNote>().ToList();
                        if (reservationNote is null)
                            reservationNote = new List<ReservationNote>();
                        List<ReservationService> reservationServices = multi.Read<ReservationService>().ToList();
                        if (reservationServices is null)
                            reservationServices = new List<ReservationService>();
                        List<BookingService> bookingServices = multi.Read<BookingService>().ToList();
                        if (bookingServices is null)
                            bookingServices = new List<BookingService>();
                        List<ReservationEmailSent> reservationEmailSents = multi.Read<ReservationEmailSent>().ToList();
                        if (reservationEmailSents is null) reservationEmailSents = new List<ReservationEmailSent>();
                        ///////////////////////////////////////////////////////
                        #region get totalprice reservation
                        float TotalPriceReservationService = connection.QuerySingleOrDefault<float>("ReservationService_Get_TotalPrice_NotPaid",
                            new
                            {
                                ReservationId = id
                            }, commandType: CommandType.StoredProcedure);

                        string[] taxFees = connection.QuerySingleOrDefault<string>("TaxFee_GetTotal_Lastest",
                         new
                         {
                             HotelId = HotelId
                         },
                         commandType: CommandType.StoredProcedure).Split(',');
                        float tax = float.Parse(taxFees[0]);
                        float fee = float.Parse(taxFees[1]);

                        List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Reservation_Get_Booking",
                        new
                        {
                            ReservationId = id,
                            Status = -1
                        }, commandType: CommandType.StoredProcedure).ToList();
                        if (bookings is null)
                            bookings = new List<Booking_Reservation>();
                        float TotalRoomCharges = 0;
                        float TotalFees = 0;
                        float TotalServices = TotalPriceReservationService;
                        float TotalExtrabed = 0;
                        float TotalAmount = 0;
                        float TotalDiscount = 0;// TotalPriceReservationService * reservation.Guest.Discount / 100;
                        if (bookings.Count > 0)
                        {
                            TotalDiscount = TotalPriceReservationService * bookings[0].Discount / 100;
                        }
                        bookings.ForEach(x =>
                        {
                            float taxForBooking = 0;
                            float feesForBooking = 0;
                            using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                                new
                                {
                                    BookingId = x.BookingId
                                }, commandType: CommandType.StoredProcedure))
                            {
                                bool includeTaxFee = multi2.Read<bool>().SingleOrDefault();
                                x.TotalRoom = multi2.Read<float>().SingleOrDefault();
                                x.TotalExtrabed = multi2.Read<float>().SingleOrDefault();
                                x.TotalService = multi2.Read<float>().SingleOrDefault();

                                // kiểm tra nếu booking yêu cầu phí thuế sẽ cập nhật phí thuế theo taxFees
                                if (includeTaxFee)
                                {
                                    taxForBooking = tax;
                                    feesForBooking = fee;
                                }

                                float totalFeeRoomServiceExtrabed = (x.TotalRoom + x.TotalExtrabed + x.TotalService);
                                TotalDiscount += totalFeeRoomServiceExtrabed * x.Discount / 100;
                                TotalFees += totalFeeRoomServiceExtrabed * (100 - x.Discount) * (feesForBooking + taxForBooking + taxForBooking * feesForBooking) / 10000;
                                TotalRoomCharges += x.TotalRoom;
                                TotalServices += x.TotalService;
                                TotalExtrabed += x.TotalExtrabed;
                                TotalAmount += x.Paid + x.PrePaid;
                                //x.Total = (float)Math.Round(x.TotalRoom * (100 + taxFees) / 100 + x.TotalExtrabed + x.TotalService, 2);
                            }
                        });
                        //TotalFees = (float)Math.Round(TotalRoomCharges * taxFees / 100, 2);
                        //TotalAmount += reservation.ExcessCash;
                        #endregion
                        return Json(JsonConvert.SerializeObject(new
                        {
                            status = 1,
                            message = "success",
                            data = new
                            {
                                reservation = reservation,
                                reservationNote = reservationNote,
                                reservationServices = reservationServices,
                                bookingServices = bookingServices,
                                reservationEmailSents = reservationEmailSents,
                                TotalRoomCharges = Math.Round(TotalRoomCharges, 0),
                                TotalFees = Math.Round(TotalFees, 0),
                                TotalServices = Math.Round(TotalServices, 0),
                                TotalExtrabed = Math.Round(TotalExtrabed, 0),
                                TotalAmount = Math.Round(TotalAmount, 0),
                                TotalDiscount = Math.Round(TotalDiscount, 0)
                            }
                        }), JsonRequestBehavior.AllowGet);
                    }
                }
                catch(Exception e)
                {
                    return Json(JsonConvert.SerializeObject(new
                    {
                        status = -1,
                        message = e.Message,
                        data = ""
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult GetDetailBookingByReservation(string userName, string password, int HotelId, int id, int status = -1)
        {
            using (var connection = DB.ConnectionFactory())
            {
                try
                {
                    string pass = DataHelper.Decrypt(password);
                    int resultCheckAccept = connection.QuerySingleOrDefault<int>("User_Check_AllowAccess_API",
                           new
                           {
                               Username = userName,
                               Password = DataHelper.CreateMD5(userName + pass + DataHelper.key),
                               HotelId = HotelId
                           }, commandType: System.Data.CommandType.StoredProcedure);
                    if (resultCheckAccept < 0)
                    {
                        return Json(JsonConvert.SerializeObject(new
                        {
                            status = -1,
                            message = "account or password is incorrect",
                            data = ""
                        }), JsonRequestBehavior.AllowGet);
                    }
                    List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("Reservation_Get_Booking",
                        new
                        {
                            ReservationId = id,
                            Status = status
                        }, commandType: CommandType.StoredProcedure).ToList();
                    string[] taxFees = connection.QuerySingleOrDefault<string>("TaxFee_GetTotal_Lastest",
                          new
                          {
                              HotelId = HotelId
                          },
                          commandType: CommandType.StoredProcedure).Split(',');
                    float tax = float.Parse(taxFees[0]);
                    float fee = float.Parse(taxFees[1]);
                    if (bookings is null)
                        bookings = new List<Booking_Reservation>();
                    bookings.ForEach(x =>
                    {
                        float taxForBooking = 0;
                        float feesForBooking = 0;
                        using (var multi = connection.QueryMultiple("Booking_Get_Multi_Price",
                            new
                            {
                                BookingId = x.BookingId
                            }, commandType: CommandType.StoredProcedure))
                        {
                            bool includeTaxFee = multi.Read<bool>().SingleOrDefault();
                            x.TotalRoom = (float)Math.Round(multi.Read<float>().SingleOrDefault(), 0);
                            x.TotalExtrabed = (float)Math.Round(multi.Read<float>().SingleOrDefault(), 0);
                            x.TotalService = (float)Math.Round(multi.Read<float>().SingleOrDefault(), 0);

                            if (includeTaxFee)
                            {
                                taxForBooking = tax;
                                feesForBooking = fee;
                            }
                            float total = (x.TotalRoom + x.TotalExtrabed + x.TotalService);
                            float discount = (float)Math.Round(total * x.Discount / 100, 0);
                            x.Total = (float)Math.Round((total - discount) * (100 + taxForBooking + feesForBooking + taxForBooking * feesForBooking / 100) / 100, 0);
                        }
                    });
                    return Json(JsonConvert.SerializeObject(new
                    {
                        status = 1,
                        message = "success",
                        data = bookings
                    }), JsonRequestBehavior.AllowGet);
                }
                catch(Exception e)
                {
                    return Json(JsonConvert.SerializeObject(new
                    {
                        status = -1,
                        message = e.Message,
                        data = ""
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult GetBookingDetailReservation(string userName, string password, string lang, int HotelId, int id)
        {
            using (var connection = DB.ConnectionFactory())
            {
                try
                {
                    string pass = DataHelper.Decrypt(password);
                    int resultCheckAccept = connection.QuerySingleOrDefault<int>("User_Check_AllowAccess_API",
                           new
                           {
                               Username = userName,
                               Password = DataHelper.CreateMD5(userName + pass + DataHelper.key),
                               HotelId = HotelId
                           }, commandType: System.Data.CommandType.StoredProcedure);
                    if (resultCheckAccept < 0)
                    {
                        return Json(JsonConvert.SerializeObject(new
                        {
                            status = -1,
                            message = "account or password is incorrect",
                            data = ""
                        }), JsonRequestBehavior.AllowGet);
                    }

                    string[] taxFees = { "0", "0" };
                    using (var multi = connection.QueryMultiple("Booking_Get_Detail",
                            new
                            {
                                BookingId = id,
                                LanguageId = lang
                            }, commandType: CommandType.StoredProcedure))
                    {
                        Booking booking = multi.Read<Booking>().SingleOrDefault();
                        List<BookingPrice> bookingPrices = multi.Read<BookingPrice>().ToList();
                        List<BookingExtrabed> bookingExtrabeds = multi.Read<BookingExtrabed>().ToList();
                        List<BookingService> bookingServices = multi.Read<BookingService>().ToList();
                        Guest guest = multi.Read<Guest>().SingleOrDefault();
                        VisaBooking visaBooking = multi.Read<VisaBooking>().SingleOrDefault();
                        List<InvoiceBooking> invoiceBookings = multi.Read<InvoiceBooking>().ToList();
                        string RoomCode = multi.Read<string>().SingleOrDefault();
                        List<BookingExtrabed> extrabeds = multi.Read<BookingExtrabed>().ToList();
                        CardBooking cardBooking = multi.Read<CardBooking>().SingleOrDefault();
                        ///////////////////
                        if (cardBooking is null) cardBooking = new CardBooking();
                        if (booking is null) booking = new Booking();
                        if (bookingPrices is null) bookingPrices = new List<BookingPrice>();
                        if (bookingExtrabeds is null) bookingExtrabeds = new List<BookingExtrabed>();
                        if (bookingServices is null) bookingServices = new List<BookingService>();
                        if (guest is null) guest = new Guest();
                        if (visaBooking is null) visaBooking = new VisaBooking();
                        if (invoiceBookings is null) invoiceBookings = new List<InvoiceBooking>();
                        if (RoomCode is null) RoomCode = "";
                        if (extrabeds is null) extrabeds = new List<BookingExtrabed>();

                        if (booking.IncludeVATAndServiceCharge)
                        {
                            taxFees = connection.QuerySingleOrDefault<string>("TaxFee_GetTotal_Lastest",
                           new
                           {
                               HotelId = HotelId
                           },
                           commandType: CommandType.StoredProcedure).Split(',');
                        }
                        float tax = float.Parse(taxFees[0]);
                        float fee = float.Parse(taxFees[1]);
                        #region Decrypt data
                        // guest
                        guest.ZIPCode = DataHelper.Decrypt(guest.ZIPCode);
                        guest.IdentityCart = DataHelper.Decrypt(guest.IdentityCart);
                        guest.Passport = DataHelper.Decrypt(guest.Passport);
                        guest.CreditCard = DataHelper.Decrypt(guest.CreditCard);
                        guest.CVC = DataHelper.Decrypt(guest.CVC);
                        // card
                        cardBooking.Number = DataHelper.Decrypt(cardBooking.Number);
                        cardBooking.Name = DataHelper.Decrypt(cardBooking.Name);
                        cardBooking.Code = DataHelper.Decrypt(cardBooking.Code);
                        // visa
                        visaBooking.SerialNo = DataHelper.Decrypt(visaBooking.SerialNo);
                        visaBooking.VisaNo = DataHelper.Decrypt(visaBooking.VisaNo);
                        #endregion


                        booking.CardBooking = cardBooking;
                        guest.GuestId = booking.GuestId;
                        booking.RoomCode = RoomCode;
                        booking.Guest = guest;
                        booking.BookingPrices = bookingPrices;
                        booking.BookingExtrabeds = bookingExtrabeds;
                        booking.BookingServices = bookingServices;
                        booking.VisaBookings = visaBooking;
                        booking.InvoiceBookings = invoiceBookings;
                        float TotalRoomCharges = bookingPrices.Sum(x => x.Price);
                        float TotalServices = bookingServices.Sum(x => x.Price * x.Number);
                        float TotalExtrabed = bookingExtrabeds.Sum(x => x.Price * x.Number);
                        float TotalDiscount = (float)Math.Round((TotalRoomCharges + TotalServices + TotalExtrabed) * booking.Discount / 100, 0);
                        float TotalFees = (TotalRoomCharges + TotalServices + TotalExtrabed) * (100 - booking.Discount) * (tax + fee + tax * fee / 100) / 10000;
                        return Json(JsonConvert.SerializeObject(new
                        {
                            status = 1,
                            message = "success",
                            data = new
                            {
                                booking = booking,
                                TotalRoomCharges = Math.Round(TotalRoomCharges, 0),
                                TotalFees = Math.Round(TotalFees, 0),
                                TotalServices = Math.Round(TotalServices, 0),
                                TotalExtrabed = Math.Round(TotalExtrabed, 0),
                                TotalDiscount = TotalDiscount,
                                Extrabeds = extrabeds,
                                taxFees = taxFees
                            }
                        }), JsonRequestBehavior.AllowGet);
                    }
                }
                catch(Exception e)
                {
                    return Json(JsonConvert.SerializeObject(new
                    {
                        status = -1,
                        message = e.Message,
                        data = ""
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}