using Dapper;
using ManageSystemPMSBE.DTCore;
using ManageSystemPMSBE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ManageSystemPMSBE.Controllers
{
    #region
    public class RoomType
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public int NumberRoom { get; set; }
        public string Extrabed { get; set; }
    }
    public class ServiceCategory
    {
        public int ServiceCategoryId { get; set; }
        public string ServiceCategoryName { get; set; }
        public string Services { get; set; }
    }
    public class SituationBooking
    {
        public int NumberBookingPMS { get; set; }
        public int NumberBookingBE { get; set; }
        public int NumberBookingBKC { get; set; }
        public int NumberBookingExpedia { get; set; }
        public int NumberBookingAgodar { get; set; }
        public int NumberBookingAirBNB { get; set; }
        public int NumberInvoice { get; set; }
        public int NumberOrder1 { get; set; }
        public int NumberOrder2 { get; set; }
        public int NumberService { get; set; }
        public int NumberExtrabed { get; set; }
        public double RevenuePMS { get; set; }
        public double RevenueBE { get; set; }
        public double RevenueBKC { get; set; }
        public double RevenueExpedia { get; set; }
        public double RevenueAGOR { get; set; }
        public double RevenueAirBNB { get; set; }
    }
    public class HotelDetail
    {
        public Hotel Hotel { get; set; }
        public SituationBooking SituationBooking { get; set; }
        public List<ServiceCategory> ServiceCategory { get; set; }
        public List<RoomType> RoomType { get; set; }
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
    #endregion
    public class HotelController : SercurityController
    {
        // GET: Hotel
        public ActionResult List()
        {
            if (!CheckSecurity())
                return Redirect("/Login/Index");
            return View();
        }
        public ActionResult Revenue()
        {
            if (!CheckSecurity())
                return Redirect("/Login/Index");
            return View();
        }
        public ActionResult Detail(int id)
        {
            if (!CheckSecurity())
                return Redirect("/Login/Index");
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                HotelDetail hotelDetail = new HotelDetail();
                hotelDetail.Hotel = connection.QuerySingleOrDefault<Hotel>("Hotel_Detail",
                    new
                    {
                        HotelId = id
                    }, commandType: CommandType.StoredProcedure);
                hotelDetail.RoomType = connection.Query<RoomType>("ManageSystem_Hotel_GetRoomType",
                    new
                    {
                        HotelId = id
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (hotelDetail.RoomType is null)
                    hotelDetail.RoomType = new List<RoomType>();
                hotelDetail.RoomType.ForEach(x =>
                {
                    List<string> extrabed = connection.Query<string>("ManageSystem_RoomType_GetExtrabed",
                        new
                        {
                            RoomTypeId = x.RoomTypeId
                        }, commandType: CommandType.StoredProcedure).ToList();
                    if (extrabed is null) extrabed = new List<string>();
                    x.Extrabed = string.Join(", ", extrabed.ToArray());
                });
                hotelDetail.ServiceCategory = connection.Query<ServiceCategory>("ManageSystem_Hotel_GetServiceCategory",
                    new
                    {
                        HotelId = id
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (hotelDetail.ServiceCategory is null) hotelDetail.ServiceCategory = new List<ServiceCategory>();
                hotelDetail.ServiceCategory.ForEach(x =>
                {
                    List<string> services = connection.Query<string>("ManageSystem_ServiceCategory_GetService",
                        new
                        {
                            ServiceCategoryId = x.ServiceCategoryId
                        }, commandType: CommandType.StoredProcedure).ToList();
                    if (services is null) services = new List<string>();
                    x.Services = string.Join(", ", services.ToArray());
                });
                hotelDetail.SituationBooking = new SituationBooking()
                {
                    RevenueAGOR = 0,
                    RevenueBE = 0,
                    RevenueBKC = 0,
                    RevenuePMS = 0,
                    RevenueAirBNB = 0,
                    RevenueExpedia = 0
                };
                List<Reservation_Sample> pms = new List<Reservation_Sample>();
                List<Reservation_Sample> be = new List<Reservation_Sample>();
                List<Reservation_Sample> bkc = new List<Reservation_Sample>();
                List<Reservation_Sample> exp = new List<Reservation_Sample>();
                List<Reservation_Sample> agor = new List<Reservation_Sample>();
                List<Reservation_Sample> airbnb = new List<Reservation_Sample>();
                using (var multi = connection.QueryMultiple("ManageSystem_Hotel_GetSituationBooking",
                    new
                    {
                        HotelId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    hotelDetail.SituationBooking.NumberBookingPMS = multi.Read<int>().SingleOrDefault();
                    hotelDetail.SituationBooking.NumberBookingBE = multi.Read<int>().SingleOrDefault();
                    hotelDetail.SituationBooking.NumberBookingBKC = multi.Read<int>().SingleOrDefault();
                    hotelDetail.SituationBooking.NumberBookingExpedia = multi.Read<int>().SingleOrDefault();
                    hotelDetail.SituationBooking.NumberBookingAgodar = multi.Read<int>().SingleOrDefault();
                    hotelDetail.SituationBooking.NumberBookingAirBNB = multi.Read<int>().SingleOrDefault();
                    hotelDetail.SituationBooking.NumberExtrabed = multi.Read<int>().SingleOrDefault();
                    hotelDetail.SituationBooking.NumberInvoice = multi.Read<int>().SingleOrDefault();
                    hotelDetail.SituationBooking.NumberOrder1 = multi.Read<int>().SingleOrDefault();
                    hotelDetail.SituationBooking.NumberOrder2 = multi.Read<int>().SingleOrDefault();
                    hotelDetail.SituationBooking.NumberService = multi.Read<int>().SingleOrDefault();
                    pms = multi.Read<Reservation_Sample>().ToList();
                    be = multi.Read<Reservation_Sample>().ToList();
                    bkc = multi.Read<Reservation_Sample>().ToList();
                    exp = multi.Read<Reservation_Sample>().ToList();
                    agor = multi.Read<Reservation_Sample>().ToList();
                    airbnb = multi.Read<Reservation_Sample>().ToList();
                    if (pms is null) pms = new List<Reservation_Sample>();
                    if (be is null) be = new List<Reservation_Sample>();
                    if (bkc is null) bkc = new List<Reservation_Sample>();
                    if (exp is null) exp = new List<Reservation_Sample>();
                    if (agor is null) agor = new List<Reservation_Sample>();
                    if (airbnb is null) airbnb = new List<Reservation_Sample>();
                }
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                        new
                        {
                            HotelId = id
                        },
                        commandType: CommandType.StoredProcedure);
                hotelDetail.SituationBooking.RevenuePMS = GetPriceByReservation(pms, taxFees, connection);
                hotelDetail.SituationBooking.RevenueBE = GetPriceByReservation(be, taxFees, connection);
                hotelDetail.SituationBooking.RevenueBKC = GetPriceByReservation(bkc, taxFees, connection);
                hotelDetail.SituationBooking.RevenueExpedia = GetPriceByReservation(exp, taxFees, connection);
                hotelDetail.SituationBooking.RevenueAGOR = GetPriceByReservation(agor, taxFees, connection);
                hotelDetail.SituationBooking.RevenueAirBNB = GetPriceByReservation(airbnb, taxFees, connection);
                ViewData["hotelDetail"] = hotelDetail;
            }
            return View();
        }
        private double GetPriceByReservation(List<Reservation_Sample> reservations, float taxFees, IDbConnection connection)
        {
            double totalReservation = 0;
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
                x.TotalPrice = (float)Math.Round(bookings.Sum(y => y.Total), 0);
                totalReservation += x.TotalPrice;
            });
            return totalReservation;
        }
        public JsonResult GetListHotel(DateTime startDate, DateTime endDate, int status, string keySearch = "", int pageNumber = 1, int pageSize = 100)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                //[]ManageSystem_Hotel_Get
                using (var multi = connection.QueryMultiple("ManageSystem_Hotel_Get", new
                {
                    startDate = startDate,
                    endDate = endDate,
                    status = status,
                    keySearch = keySearch,
                    pageNumber = pageNumber,
                    pageSize = pageSize
                }, commandType: CommandType.StoredProcedure))
                {
                    List<Hotel> hotels = multi.Read<Hotel>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(JsonConvert.SerializeObject(new
                    {
                        hotels = hotels,
                        totalRecord = totalRecord
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult GetListHotelUseBE(DateTime startDate, DateTime endDate, int status, string keySearch = "", int pageNumber = 1, int pageSize = 100)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                //[]ManageSystem_Hotel_Get
                using (var multi = connection.QueryMultiple("ManageSystem_Hotel_GetUseBE", new
                {
                    startDate = startDate,
                    endDate = endDate,
                    status = status,
                    keySearch = keySearch,
                    pageNumber = pageNumber,
                    pageSize = pageSize
                }, commandType: CommandType.StoredProcedure))
                {
                    List<Hotel> hotels = multi.Read<Hotel>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(JsonConvert.SerializeObject(new
                    {
                        hotels = hotels,
                        totalRecord = totalRecord
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult GetListHotelUsePMS(DateTime startDate, DateTime endDate, int status, string keySearch = "", int pageNumber = 1, int pageSize = 100)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                //[]ManageSystem_Hotel_Get
                using (var multi = connection.QueryMultiple("ManageSystem_Hotel_GetUsePMS", new
                {
                    startDate = startDate,
                    endDate = endDate,
                    status = status,
                    keySearch = keySearch,
                    pageNumber = pageNumber,
                    pageSize = pageSize
                }, commandType: CommandType.StoredProcedure))
                {
                    List<Hotel> hotels = multi.Read<Hotel>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(JsonConvert.SerializeObject(new
                    {
                        hotels = hotels,
                        totalRecord = totalRecord
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult GetListHotelUsePMSBE(DateTime startDate, DateTime endDate, int status, string keySearch = "", int pageNumber = 1, int pageSize = 100)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                //[]ManageSystem_Hotel_Get
                using (var multi = connection.QueryMultiple("ManageSystem_Hotel_GetUsePMSBE", new
                {
                    startDate = startDate,
                    endDate = endDate,
                    status = status,
                    keySearch = keySearch,
                    pageNumber = pageNumber,
                    pageSize = pageSize
                }, commandType: CommandType.StoredProcedure))
                {
                    List<Hotel> hotels = multi.Read<Hotel>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(JsonConvert.SerializeObject(new
                    {
                        hotels = hotels,
                        totalRecord = totalRecord
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }





        public JsonResult GetRevenue(int month, int year, int status, string keySearch = "", int pageNumber = 1, int pageSize = 100)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                using (var multi = connection.QueryMultiple("ManageSystem_Hotel_GetReveune",
                    new
                    {
                        keySearch = keySearch,
                        status = status,
                        pageNumber = pageNumber,
                        pageSize = pageSize
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Hotel_Revanue> hotels = multi.Read<Hotel_Revanue>().ToList();
                    List<Hotel_Revanue> listHotels = new List<Hotel_Revanue>();
                    foreach (var item in hotels)
                    {
                    
                        item.Month = month;
                        item.Year = year;
                        listHotels.Add(item);
                    }
                    
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    if (listHotels is null) listHotels = new List<Hotel_Revanue>();
                    listHotels.ForEach(x =>
                    {
                        float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                                   new
                                   {
                                       HotelId = x.HotelId
                                   },
                                   commandType: CommandType.StoredProcedure);
                        List<Booking_Reservation> bookings = connection.Query<Booking_Reservation>("ManageSystem_Booking_GetByHotel",
                            new
                            {
                                HotelId = x.HotelId,
                                Month = month,
                                Year = year
                            }, commandType: CommandType.StoredProcedure).ToList();
                        if (bookings is null)
                            bookings = new List<Booking_Reservation>();
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
                                x.Total += Math.Round((total - discount) * (100 + taxFeesForBooking) / 100, 0);
                            }
                        });

                        /// Số lượng booking đặt trong khách sạn
                        x.NumberBooking = bookings.Count;
                        // Lấy số lượng phòng đặt, số lượng hóa đơn, order,dịch vụ,giường phụ
                        using (var multiDetail = connection.QueryMultiple("ManageSystem_Hotel_GetNumberServiceUsed",
                            new
                            {
                                HotelId = x.HotelId,
                                Month = month,
                                Year = year
                            }, commandType: CommandType.StoredProcedure))
                        {
                            int numberInvoice = multiDetail.Read<int>().SingleOrDefault();
                            int numberOrder = multiDetail.Read<int>().SingleOrDefault();
                            int numberOrderMoved = multiDetail.Read<int>().SingleOrDefault();
                            int numberService = multiDetail.Read<int>().SingleOrDefault();
                            int numberExtrabed = multiDetail.Read<int>().SingleOrDefault();

                            // Cập nhật số lượng dịch vụ,hóa đơn,service,extrabed sử dụng
                            x.NumberInvoice = numberInvoice;
                            x.NumberOrder = numberOrder;
                            x.NumberOrderMoved = numberOrder;
                            x.NumberService = numberService;
                            x.NumberExtrabed = numberExtrabed;
                        }
                    });
                    return Json(new
                    {
                        hotels = listHotels,
                        totalRecord = totalRecord
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult GetAll(int type)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                List<Hotel> hotels = connection.Query<Hotel>("ManageSystem_Hotel_GetAll",
                    new
                    {
                        Status = type
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                return Json(hotels, JsonRequestBehavior.AllowGet);
            }
        }

         public ActionResult GetListUSerBooking(int id , int month, int year)
        {
           
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
               
                List<Booking_Reservation> userBooking = connection.Query<Booking_Reservation>("ManageSystem_Hotel_GetAllCheckOut",
                    new
                    {
                        HotelId = id,
                        Month = month,
                        Year = year
                    }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                if (userBooking is null) userBooking = new List<Booking_Reservation>();
                userBooking.ForEach(y =>
                {
                    float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                                  new
                                  {
                                      HotelId = y.HotelId

                                  },
                                  commandType: CommandType.StoredProcedure);
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
                ViewData["userBooking"] = userBooking;
                return View();
            }
        }
    }
}