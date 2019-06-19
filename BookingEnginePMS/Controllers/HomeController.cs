using BookingEnginePMS.Helper;
using BookingEnginePMS.Models;
using Dapper;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace BookingEnginePMS.Controllers
{
    #region Class helper
    public class Promotion_Home
    {
        public int PromotionId { get; set; }
        public string PromotionName { get; set; }
        public int TypePromotion { get; set; }
        public int DayInHouse { get; set; }
        public int EarlyDay { get; set; }
        public int NightForFreeNight { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public float Deposit { get; set; }
        public int PolicyId { get; set; }
        public float AmountRate { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string PolicyContent { get; set; }
        public float TotalPrice { get; set; }


        public float TotalPriceAfterPromotion { get; set; }
        // plane rate
        public int PlaneRateId { get; set; }
        public string Name { get; set; } // planerate name
        public float Price { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        // đã chọn phòng với số lượng là ?
        public int ChooseRoom { get; set; }
        // giá hiện thị theo loại tiền tệ
        public float PriceExchangeRate { get; set; }
        public float TotalPriceAfterPromotionExchangeRate { get; set; }

    }
    public class RoomType_Home
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public string RoomTypeCode { get; set; }
        public int MaxPeople { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public bool HasExtrabed { get; set; }
        public string Image { get; set; }
        public string Size { get; set; }
        public int Index { get; set; }
        public List<RoomTypeGallery> RoomTypeGalleries { get; set; }
        public List<RoomTypeLanguage> RoomTypeLanguages { get; set; }
        public Promotion_Home PromotionHome { get; set; }
        public List<RateAvailability> RateAvailabilities { get; set; }
        public List<Amenity> Amenities { get; set; }
        //
        public string ExtrabedOption { get; set; }
        public string DescriptionBed { get; set; }
        public string DescriptionView { get; set; }
        public string Note { get; set; }
        public float TotalPrice { get; set; }
        public int NumberAvailable { get; set; }
        //
        public List<Extrabed_Home> ExtrabedHome { get; set; }
        public List<Service_Home> ServiceHomes { get; set; }
        public int AdultBook { get; set; }
        public int ChildrenBook { get; set; }
        // giá hiện thị theo loại tiền tệ
        public float TotalPriceExchangeRate { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    public class Extrabed_Home
    {
        public int ExtrabedId { get; set; }
        public int RoomTypeId { get; set; }
        public double Price { get; set; }
        public string Image { get; set; } // image of extrabed
        public string RoomTypeName { get; set; }
        public string ExtrabedName { get; set; }
        public string Description { get; set; }
        public int NumberChoose { get; set; }
        // giá hiện thị theo loại tiền tệ
        public float PriceExchangeRate { get; set; }

        public Extrabed_Home SplitNumberChoose(int numberChoose)
        {
            Extrabed_Home extrabedClone = new Extrabed_Home()
            {
                Description = this.Description,
                ExtrabedId = this.ExtrabedId,
                ExtrabedName = this.ExtrabedName,
                Image = this.Image,
                NumberChoose = numberChoose,
                Price = this.Price,
                RoomTypeId = this.RoomTypeId,
                RoomTypeName = this.RoomTypeName
            };
            this.NumberChoose -= numberChoose;
            return extrabedClone;
        }
    }
    public class Service_Home
    {
        public int ServiceId { get; set; }
        public int ServiceCategoryId { get; set; }
        public string ServiceCode { get; set; }
        public string Photo { get; set; }
        public int Index { get; set; }
        public bool BuyOnline { get; set; }
        public float Price { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int NumberChoose { get; set; }
        public float PriceForAddClient { get; set; }
        // giá hiện thị theo loại tiền tệ
        public float PriceExchangeRate { get; set; }
    }
    public class Guest_Home
    {
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string ArrivalFlightDate { get; set; }
        public string ArrivalFlightTime { get; set; }
        public int TypePaymentMethod { get; set; }
        //
        public string TypePaymentMethodName { get; set; }
        // card information
        public string Name { get; set; }
        public string Number { get; set; }
        public string Code { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }

    }
    public class PaymentMethod_Home
    {
        public int ConfigPaymentMethodId { get; set; }
        public string Name { get; set; }
        public bool RequireCard { get; set; }
        public string Policy { get; set; }
        public int PolicyId { get; set; }
    }
    public class Voucher_Home
    {
        public int VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public bool DiscountForService { get; set; }
        public float AmountForService { get; set; }
        public bool DiscountForRoom { get; set; }
        public float AmountForRoom { get; set; }
        //
        public float amountForVoucher { get; set; }
    }
    public class DataSavePayonline_Home
    {
        public int ReservationId { get; set; }
        public long Deposit { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public DataSavePayonline_Home() { }
        public DataSavePayonline_Home(int reservationId, long deposit, string firstName, string surName, string email)
        {
            this.ReservationId = reservationId;
            this.Deposit = deposit;
            this.FirstName = firstName;
            this.SurName = surName;
            this.Email = email;
        }
    }
    public class AllDataBook_Home
    {
        public int reservationId { get; set; }
        public Guest_Home guest { get; set; }
        public List<Extrabed_Home> extrabeds { get; set; }
        public List<Service_Home> services { get; set; }
        public Voucher_Home voucher { get; set; }
        public List<RoomType_Home> data { get; set; }
        public List<RoomType_Home> dataDefault { get; set; } // dữ liệu ban đầu chưa duỗi
        public ParamsQuery param { get; set; }
        public string Reference_Number { get; set; }
    }
    public class ParamsQuery
    {
        public int child { get; set; }
        public int adults { get; set; }
        public DateTime toDate { get; set; }
        public DateTime fromDate { get; set; }
        public string hotelCode { get; set; }
        public string hotelKey { get; set; }
        public string lang { get; set; }
        public string currency { get; set; }
        public double tax { get; set; }
        public double serviceCharge { get; set; }
        public double resultAmountForVoucher { get; set; }
    }
    public class Model_PriceAndDayofWeed
    {
        public float TotalPriceAfterPromotionMonday { get; set; }
        public float TotalPriceAfterPromotionTuesday { get; set; }
        public float TotalPriceAfterPromotionWednesday { get; set; }
        public float TotalPriceAfterPromotionThursday { get; set; }
        public float TotalPriceAfterPromotionFriday { get; set; }
        public float TotalPriceAfterPromotionSaturday { get; set; }
        public float TotalPriceAfterPromotionSunday { get; set; }
        public float TotalPriceAfterPromotionExchangeRateMonday { get; set; }
        public float TotalPriceAfterPromotionExchangeRateTuesday { get; set; }
        public float TotalPriceAfterPromotionExchangeRateWednesday { get; set; }
        public float TotalPriceAfterPromotionExchangeRateThursday { get; set; }
        public float TotalPriceAfterPromotionExchangeRateFriday { get; set; }
        public float TotalPriceAfterPromotionExchangeRateSaturday { get; set; }
        public float TotalPriceAfterPromotionExchangeRateSunday { get; set; }
        public float PriceMonday { get; set; }
        public float PriceTuesday { get; set; }
        public float PriceWednesday { get; set; }
        public float PriceThursday { get; set; }
        public float PriceFriday { get; set; }
        public float PriceSaturday { get; set; }
        public float PriceSunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public Promotion_Home PromotionHome { get; set; }
    }
    public class TotalPriceAfterPromotion
    {
        public List<Model_PriceAndDayofWeed> Model_PriceAndDayofWeed { get; set; } /// them vao
    }
    #endregion
    public class HomeController : Controller
    {
        public ActionResult Index(string hotelKey = "DLC", string hotelCode = "")
        {
            using (var connection = DB.ConnectionFactory())
            {
                Hotel hotel = new Hotel();
                hotel = connection.QuerySingleOrDefault<Hotel>("Hotel_GetHighLight",
                        new
                        {
                            GroupHoteCode = hotelKey
                        },
                        commandType: CommandType.StoredProcedure);
                if (hotel.TypeSoftware == 1)
                    return View("Error");
                ConfigViewBookingEngine config = connection.QuerySingleOrDefault<ConfigViewBookingEngine>("Hotel_GetConfigViewBE",
                       new
                       {
                           HotelId = hotel.HotelId
                       }, commandType: CommandType.StoredProcedure);
                if (config is null)
                {
                    config = new ConfigViewBookingEngine()
                    {
                        Currency = "VND",
                        Lang = "vi"
                    };
                }
                return RedirectToAction("SelectDate", new { hotelKey = hotelKey, hotelCode = "", lang = config.Lang, currency = config.Currency });
            }
        }

        public ActionResult SelectDate(string hotelKey, string hotelCode, string lang, string currency)
        {
            using (var connection = DB.ConnectionFactory())
            {
                Hotel hotel = new Hotel();
                if (hotelCode is null || hotelCode == "")
                {
                    hotel = connection.QuerySingleOrDefault<Hotel>("Hotel_GetHighLight",
                        new
                        {
                            GroupHoteCode = hotelKey
                        },
                        commandType: CommandType.StoredProcedure);
                }
                else
                {
                    hotel = connection.QuerySingleOrDefault<Hotel>("Hotel_GetHighLightByCode",
                        new
                        {
                            Code = hotelCode
                        },
                       commandType: CommandType.StoredProcedure);
                }
                if (hotel.TypeSoftware == 1)
                    return View("Error");
                Session["HotelForBooking"] = hotel;
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting_GetByKey",
                       new
                       {
                           Key = lang,
                           screenId = 61
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult SelectRoom(string hotelKey, string hotelCode, DateTime fromDate, DateTime toDate, int adults, int child, string lang = "vi", string currency = "VND")
        {
            using (var connection = DB.ConnectionFactory())
            {
                Hotel hotel = connection.QuerySingleOrDefault<Hotel>("Hotel_GetHighLightByCode",
                            new
                            {
                                Code = hotelCode
                            },
                           commandType: CommandType.StoredProcedure);

                if (hotel.TypeSoftware == 1)
                    return View("Error");
                Session["HotelForBooking"] = hotel;

                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting_GetByKey",
                       new
                       {
                           Key = lang,
                           screenId = 61
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                Session["currency"] = currency;
            }
            return View();
        }
        public ActionResult Confirm(string lang = "vi", string currency = "VND")
        {
            List<string> countries = ConfigData.GetAllCountry();
            ParamsQuery param = new ParamsQuery();
            if (Session["Params"] != null)
                param = (ParamsQuery)Session["Params"];
            param.lang = lang;
            param.currency = currency;
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting_GetByKey",
                       new
                       {
                           Key = lang,
                           screenId = 61
                       }, commandType: CommandType.StoredProcedure).ToList();

                List<PaymentMethod_Home> paymentMethods = connection.Query<PaymentMethod_Home>("PaymentMethod_GetBuyOnline",
                    new
                    {
                        HotelCode = param.hotelCode,
                        LanguageCode = lang
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (paymentMethods is null) paymentMethods = new List<PaymentMethod_Home>();
                Transition transition = new Transition(transitions);
                paymentMethods.ForEach(x =>
                {
                    switch (x.ConfigPaymentMethodId)
                    {
                        case 11:
                            x.Name = transition.Translate(502, x.Name);
                            break;
                        case 13:
                            x.Name = transition.Translate(2346, x.Name);
                            break;
                        case 14:
                            x.Name = transition.Translate(2347, x.Name);
                            break;
                        case 15:
                            x.Name = transition.Translate(2348, x.Name);
                            break;
                        case 16:
                            x.Name = transition.Translate(2349, x.Name);
                            break;
                        case 17:
                            x.Name = transition.Translate(2350, x.Name);
                            break;
                    }
                });
                Session["transitions"] = transitions;
                Session["paymentMethods"] = paymentMethods;
                ViewData["countries"] = countries;
                Session["currency"] = currency;
                Session["Params"] = param;
            }
            return View();
        }
        public ActionResult BookSuccess()
        {
            AllDataBook_Home allDataBook = (AllDataBook_Home)Session["allDataBook"];
            if (allDataBook is null)
                return RedirectToAction("BookError");
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting_GetByKey",
                       new
                       {
                           Key = allDataBook.param.lang,
                           screenId = 61
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["HotelForBooking"] = connection.QuerySingleOrDefault<Hotel>("Hotel_GetHighLightByCode",
                           new
                           {
                               Code = allDataBook.param.hotelCode
                           },
                          commandType: CommandType.StoredProcedure);
                Session["transitions"] = transitions;

                int tempRound = allDataBook.param.currency == "VND" ? 0 : 2;
                double totalPriceRoom = 0;
                double totalPriceExtrabed = 0;
                double totalPriceService = 0;
                double totalPriceVoucher = 0;
                double totalPrice = 0;
                double totalDeposit = 0;
                double totalVAT = 0;
                double totalServiceCharge = 0;
                allDataBook.data.ForEach(x =>
                {
                    if (allDataBook.param.currency == "VND")
                        totalPriceRoom += x.PromotionHome.TotalPriceAfterPromotion;
                    else
                        totalPriceRoom += x.PromotionHome.TotalPriceAfterPromotionExchangeRate;
                });
                allDataBook.extrabeds.ForEach(x =>
                {
                    if (allDataBook.param.currency == "VND")
                        totalPriceExtrabed += x.Price * x.NumberChoose;
                    else
                        totalPriceExtrabed += x.PriceExchangeRate * x.NumberChoose;
                });
                allDataBook.services.ForEach(x =>
                {
                    if (allDataBook.param.currency == "VND")
                        totalPriceService += x.Price * x.NumberChoose;
                    else
                        totalPriceService += x.PriceExchangeRate * x.NumberChoose;
                });
                totalPriceVoucher = Math.Round(allDataBook.voucher.amountForVoucher, tempRound);
                totalPrice = totalPriceRoom + totalPriceExtrabed + totalPriceService - totalPriceVoucher;
                totalVAT = Math.Round(totalPrice * allDataBook.param.tax * (100 + allDataBook.param.serviceCharge) / 10000, tempRound);
                totalServiceCharge = Math.Round(totalPrice * allDataBook.param.serviceCharge / 100, tempRound);
                #region cal deposit when has voucher

                using (var multi = connection.QueryMultiple("Voucher_CheckAcceptDeposit",
                    new
                    {
                        HotelCode = allDataBook.param.hotelCode,
                        VoucherCode = allDataBook.voucher.VoucherCode,
                        Date = DatetimeHelper.DateTimeUTCNow()
                    }, commandType: CommandType.StoredProcedure))
                {
                    int accept = multi.Read<int>().SingleOrDefault();
                    Voucher_Home voucherResult = new Voucher_Home();
                    if (accept == 1)
                    {
                        voucherResult = multi.Read<Voucher_Home>().SingleOrDefault();
                        List<int> roomtypeAccept = multi.Read<int>().ToList();
                        List<int> serviceAccept = multi.Read<int>().ToList();
                        if (roomtypeAccept is null) roomtypeAccept = new List<int>();
                        if (serviceAccept is null) serviceAccept = new List<int>();
                        if (voucherResult.DiscountForRoom)
                        {
                            allDataBook.data.ForEach(x =>
                            {
                                if (roomtypeAccept.FindIndex(y => y == x.RoomTypeId) >= 0)
                                {
                                    if (allDataBook.param.currency == "VND")
                                        totalDeposit += x.PromotionHome.TotalPriceAfterPromotion * (100 - voucherResult.AmountForRoom) * x.PromotionHome.AmountRate / 10000;
                                    else
                                        totalDeposit += x.PromotionHome.TotalPriceAfterPromotionExchangeRate * (100 - voucherResult.AmountForRoom) * x.PromotionHome.AmountRate / 10000;
                                }
                                else
                                {
                                    if (allDataBook.param.currency == "VND")
                                        totalDeposit += x.PromotionHome.TotalPriceAfterPromotion * x.PromotionHome.AmountRate / 100;
                                    else
                                        totalDeposit += x.PromotionHome.TotalPriceAfterPromotionExchangeRate * x.PromotionHome.AmountRate / 100;
                                }
                            });
                        }
                        else
                        {
                            allDataBook.data.ForEach(x =>
                            {
                                if (allDataBook.param.currency == "VND")
                                    totalDeposit += x.PromotionHome.TotalPriceAfterPromotion * x.PromotionHome.AmountRate / 100;
                                else
                                    totalDeposit += x.PromotionHome.TotalPriceAfterPromotionExchangeRate * x.PromotionHome.AmountRate / 100;
                            });
                        }
                    }
                    else
                    {
                        allDataBook.data.ForEach(x =>
                        {
                            if (allDataBook.param.currency == "VND")
                                totalDeposit += x.PromotionHome.TotalPriceAfterPromotion * x.PromotionHome.AmountRate / 100;
                            else
                                totalDeposit += x.PromotionHome.TotalPriceAfterPromotionExchangeRate * x.PromotionHome.AmountRate / 100;
                        });
                    }
                }
                #endregion
                totalDeposit = Math.Round(totalDeposit * (100 + (allDataBook.param.tax + allDataBook.param.serviceCharge + (allDataBook.param.tax * allDataBook.param.serviceCharge / 100))) / 100, tempRound);
                if (allDataBook.guest.TypePaymentMethod == 11 || allDataBook.guest.TypePaymentMethod == 15)
                    totalDeposit = 0;
                ViewBag.totalPriceRoom = totalPriceRoom;
                ViewBag.totalPriceExtrabed = totalPriceExtrabed;
                ViewBag.totalPriceService = totalPriceService;
                ViewBag.totalPriceVoucher = totalPriceVoucher;
                ViewBag.totalPrice = Math.Round(totalPrice, tempRound);
                ViewBag.totalDeposit = totalDeposit;
                ViewBag.totalVAT = totalVAT;
                ViewBag.totalServiceCharge = totalServiceCharge;

            }
            return View();
        }
        public ActionResult BookError()
        {
            AllDataBook_Home allDataBook = (AllDataBook_Home)Session["allDataBook"];
            if (allDataBook is null)
            {
                allDataBook = new AllDataBook_Home();
            }
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting_GetByKey",
                       new
                       {
                           Key = allDataBook.param.lang,
                           screenId = 61
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["HotelForBooking"] = connection.QuerySingleOrDefault<Hotel>("Hotel_GetHighLightByCode",
                           new
                           {
                               Code = allDataBook.param.hotelCode
                           },
                          commandType: CommandType.StoredProcedure);
                Session["transitions"] = transitions;
            }
            return View();
        }
        #region API
        public JsonResult GetLanguage()
        {
            using (var connection = DB.ConnectionFactory())
            {
                List<Language> language = connection.Query<Language>("Language_GetActive"
                    , commandType: CommandType.StoredProcedure).ToList();
                return Json(language, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCurrency()
        {
            using (var connection = DB.ConnectionFactory())
            {
                List<Currency> currencies = connection.Query<Currency>("Currency_Get"
                    , commandType: CommandType.StoredProcedure).ToList();
                if (currencies is null) currencies = new List<Currency>();
                currencies.ForEach(x => x.CurrencyCode = x.CurrencyCode.Trim());
                return Json(currencies, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetHotels(string hotelKey)
        {
            using (var connection = DB.ConnectionFactory())
            {
                List<Hotel> hotels = connection.Query<Hotel>("Hotel_GetAllByGroupHotelCode",
                    new
                    {
                        GroupHodelCode = hotelKey
                    }, commandType: CommandType.StoredProcedure).ToList();
                return Json(hotels, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetRoomAvailable(string hotelCode, DateTime fromDate, DateTime toDate, string lang = "vi", string currency = "VND")
        {
            return Json(GetRoomAvailable_FullPromotion(hotelCode, fromDate, toDate, lang, currency), JsonRequestBehavior.AllowGet);
        }
        private string GetRoomAvailable_FullPromotion(string hotelCode, DateTime fromDate, DateTime toDate, string lang = "vi", string currency = "VND")
        {
            // lấy ra ngày bắt đầu trong tuần.
            string weeddayoff = fromDate.DayOfWeek.ToString();

            //int weeddayoff = (int)fromDate.DayOfWeek;
            // lấy ra số lượng ngày 
            int rangeDate = DataHelper.RangeDate(fromDate, toDate);
            using (var connection = DB.ConnectionFactory())
            {
                int LanguageId = connection.QuerySingleOrDefault<int>("Language_GetIdByKey",
                    new
                    {
                        Key = lang
                    }, commandType: CommandType.StoredProcedure);
                Session["languageIdClient"] = LanguageId;
                // Khởi tạo danh sách đặt phòng ban đầu. Mỗi phòng tương ứng 1 promotion
                List<RoomType_Home> roomTypesResult = new List<RoomType_Home>();
                // lấy danh sách phòng
                List<RoomType_Home> roomTypes = connection.Query<RoomType_Home>("RoomType_GetByCodeHotel",
                    new
                    {
                        LanguageId = LanguageId,
                        HodelCode = hotelCode
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (roomTypes is null) roomTypes = new List<RoomType_Home>();
                // lấy tỷ giá hối đoái để cập nhật giá theo loại tiền tệ
                ConfigCurrency configCurrency = connection.QuerySingleOrDefault<ConfigCurrency>("ConfigCurrency_GetExchangeRate",
                    new
                    {
                        HodelCode = hotelCode,
                        CurrencyCode = currency
                    }, commandType: CommandType.StoredProcedure);
                if (configCurrency.AutoCalculator)
                {
                    configCurrency.Result = DataHelper.CurrencyConvertor(currency);
                }
                // lấy danh sách promotion theo từng loại phòng
                // b1: lấy danh sách promotion của loại phòng - đồng thời lấy thông tin chính sách
                // b2: lấy danh sách giá tương ứng phòng
                // b3: lấy tổng tiền và số lượng phòng còn trống
                //Promotion_Home [,] room=new Promotion_Home[10,10] ;
                List<RoomType_Home>[,] room = new List<RoomType_Home>[10, 10];
                int loaiphong = 0;
                roomTypes.ForEach(x =>
                {
                   
                    int sokm = 0;
                    using (var multi = connection.QueryMultiple("RoomType_GetDetailShowClient",
                        new
                        {
                            LanguageId = LanguageId,
                            RoomTypeId = x.RoomTypeId,
                            FromDate = fromDate,
                            ToDate = toDate,
                            DateNow = DatetimeHelper.DateTimeUTCNow()
                        }, commandType: CommandType.StoredProcedure))
                    {
                        //lấy ra số lượng khuyến mãi
                        List<Promotion_Home> promotions = multi.Read<Promotion_Home>().ToList();
                        //List<Promotion_Home> promotionsTest = new List<Promotion_Home>();
                        // test lay ra khuyeen mai


                        //

                        x.RateAvailabilities = multi.Read<RateAvailability>().ToList();
                        //var totlPrice = multi.Read<float>().FirstOrDefault();
                        //if(rangeDate > 7){
                        //    int totals = (int)totlPrice / 7;
                        //    x.TotalPrice = totals;
                        //}
                        //else
                        //{
                        //    x.TotalPrice = totlPrice / rangeDate;
                        //}
                        x.TotalPrice = multi.Read<float>().FirstOrDefault();
                        x.NumberAvailable = multi.Read<int>().SingleOrDefault();

                        if (x.RateAvailabilities is null) x.RateAvailabilities = new List<RateAvailability>();

                        // kiểm tra có còn phòng trống và trong khoảng thời gian đặt phòng có đóng phòng hay không
                        if (x.RateAvailabilities.Count == rangeDate && x.NumberAvailable > 0)
                        {
                            // Chuyển đổi ngoại tệ thành tiền tương ứng
                            x.TotalPriceExchangeRate = ConvertCurrency(x.TotalPrice, configCurrency.Result);
                            if (promotions is null) promotions = new List<Promotion_Home>();
                            List<Model_PriceAndDayofWeed> PriceAndDayofWeeds = new List<Model_PriceAndDayofWeed>();
                            if (x.RateAvailabilities is null) x.RateAvailabilities = new List<RateAvailability>(); //TotalPriceAfterPromotion
                            #region
                            //foreach (DateTime day in EachDay(fromDate, toDate))
                            //{
                            //    promotions.ForEach(y =>
                            //    {
                            //        Model_PriceAndDayofWeed PriceAndDayofWeed = new Model_PriceAndDayofWeed();

                            //        if (day.DayOfWeek.ToString() == "Monday" && y.Monday == true)
                            //        {
                            //            // kiểm tra promotion hợp lệ thỏa mãn điều kiện
                            //            bool ok = true;
                            //            switch (y.TypePromotion)
                            //            {
                            //                case 1:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
                            //                    break;
                            //                case 2:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
                            //                    break;
                            //                case 3:
                            //                    ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
                            //                    break;
                            //                case 4:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
                            //                    if (ok)
                            //                    {

                            //                        x.RateAvailabilities.Add(new RateAvailability()
                            //                        {
                            //                            Date = toDate.Date,
                            //                            RoomTypeId = x.RoomTypeId,
                            //                            Price = 0,
                            //                            PriceForAddClient = 0
                            //                        });
                            //                    }
                            //                    break;
                            //            }
                            //            if (ok)
                            //            {

                            //                y.ChooseRoom = 0;
                            //                PriceAndDayofWeed.PriceMonday += x.TotalPrice;
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionMonday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
                            //                PriceAndDayofWeed.Monday = y.Monday;
                            //                // gán giá sau khi tính tỷ giá theo tiền tệ
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateMonday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionMonday, configCurrency.Result);
                            //                PriceAndDayofWeed.PromotionHome = y;
                            //            }
                            //            PriceAndDayofWeeds.Add(PriceAndDayofWeed);
                            //        }
                            //        if (day.DayOfWeek.ToString() == "Tuesday" && y.Tuesday == true)
                            //        {
                            //            // kiểm tra promotion hợp lệ thỏa mãn điều kiện
                            //            bool ok = true;
                            //            switch (y.TypePromotion)
                            //            {
                            //                case 1:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
                            //                    break;
                            //                case 2:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
                            //                    break;
                            //                case 3:
                            //                    ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
                            //                    break;
                            //                case 4:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
                            //                    if (ok)
                            //                    {
                            //                        x.RateAvailabilities.Add(new RateAvailability()
                            //                        {
                            //                            Date = toDate.Date,
                            //                            RoomTypeId = x.RoomTypeId,
                            //                            Price = 0,
                            //                            PriceForAddClient = 0
                            //                        });
                            //                    }
                            //                    break;
                            //            }
                            //            if (ok)
                            //            {

                            //                y.ChooseRoom = 0;
                            //                PriceAndDayofWeed.PriceTuesday += x.TotalPrice;
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionTuesday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
                            //                PriceAndDayofWeed.Tuesday = y.Tuesday;
                            //                // gán giá sau khi tính tỷ giá theo tiền tệ
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateTuesday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionTuesday, configCurrency.Result);
                            //                PriceAndDayofWeed.PromotionHome = y;
                            //            }
                            //            PriceAndDayofWeeds.Add(PriceAndDayofWeed);
                            //        }
                            //        if (day.DayOfWeek.ToString() == "Wednesday" && y.Wednesday == true)
                            //        {
                            //            // kiểm tra promotion hợp lệ thỏa mãn điều kiện
                            //            bool ok = true;
                            //            switch (y.TypePromotion)
                            //            {
                            //                case 1:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
                            //                    break;
                            //                case 2:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
                            //                    break;
                            //                case 3:
                            //                    ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
                            //                    break;
                            //                case 4:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
                            //                    if (ok)
                            //                    {
                            //                        x.RateAvailabilities.Add(new RateAvailability()
                            //                        {
                            //                            Date = toDate.Date,
                            //                            RoomTypeId = x.RoomTypeId,
                            //                            Price = 0,
                            //                            PriceForAddClient = 0
                            //                        });
                            //                    }
                            //                    break;
                            //            }
                            //            if (ok)
                            //            {

                            //                y.ChooseRoom = 0;
                            //                PriceAndDayofWeed.PriceWednesday += x.TotalPrice;
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionWednesday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
                            //                PriceAndDayofWeed.Wednesday = y.Wednesday;
                            //                // gán giá sau khi tính tỷ giá theo tiền tệ
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateWednesday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionWednesday, configCurrency.Result);
                            //                PriceAndDayofWeed.PromotionHome = y;
                            //            }
                            //            PriceAndDayofWeeds.Add(PriceAndDayofWeed);
                            //        }
                            //        if (day.DayOfWeek.ToString() == "Thursday" && y.Thursday == true)
                            //        {
                            //            // kiểm tra promotion hợp lệ thỏa mãn điều kiện
                            //            bool ok = true;
                            //            switch (y.TypePromotion)
                            //            {
                            //                case 1:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
                            //                    break;
                            //                case 2:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
                            //                    break;
                            //                case 3:
                            //                    ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
                            //                    break;
                            //                case 4:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
                            //                    if (ok)
                            //                    {
                            //                        x.RateAvailabilities.Add(new RateAvailability()
                            //                        {
                            //                            Date = toDate.Date,
                            //                            RoomTypeId = x.RoomTypeId,
                            //                            Price = 0,
                            //                            PriceForAddClient = 0
                            //                        });
                            //                    }
                            //                    break;
                            //            }
                            //            if (ok)
                            //            {

                            //                y.ChooseRoom = 0;
                            //                PriceAndDayofWeed.PriceThursday += x.TotalPrice;
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionThursday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
                            //                PriceAndDayofWeed.Thursday = y.Thursday;
                            //                // gán giá sau khi tính tỷ giá theo tiền tệ
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateThursday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionThursday, configCurrency.Result);
                            //                PriceAndDayofWeed.PromotionHome = y;
                            //            }
                            //            PriceAndDayofWeeds.Add(PriceAndDayofWeed);
                            //        }
                            //        if (day.DayOfWeek.ToString() == "Friday" && y.Friday == true)
                            //        {
                            //            // kiểm tra promotion hợp lệ thỏa mãn điều kiện
                            //            bool ok = true;
                            //            switch (y.TypePromotion)
                            //            {
                            //                case 1:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
                            //                    break;
                            //                case 2:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
                            //                    break;
                            //                case 3:
                            //                    ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
                            //                    break;
                            //                case 4:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
                            //                    if (ok)
                            //                    {
                            //                        x.RateAvailabilities.Add(new RateAvailability()
                            //                        {
                            //                            Date = toDate.Date,
                            //                            RoomTypeId = x.RoomTypeId,
                            //                            Price = 0,
                            //                            PriceForAddClient = 0
                            //                        });
                            //                    }
                            //                    break;
                            //            }
                            //            if (ok)
                            //            {

                            //                y.ChooseRoom = 0;
                            //                PriceAndDayofWeed.PriceFriday += x.TotalPrice;
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionFriday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
                            //                PriceAndDayofWeed.Friday = y.Friday;
                            //                // gán giá sau khi tính tỷ giá theo tiền tệ
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateFriday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionFriday, configCurrency.Result);
                            //                PriceAndDayofWeed.PromotionHome = y;
                            //            }
                            //            PriceAndDayofWeeds.Add(PriceAndDayofWeed);
                            //        }
                            //        if (day.DayOfWeek.ToString() == "Saturday" && y.Saturday == true)
                            //        {
                            //            // kiểm tra promotion hợp lệ thỏa mãn điều kiện
                            //            bool ok = true;
                            //            switch (y.TypePromotion)
                            //            {
                            //                case 1:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
                            //                    break;
                            //                case 2:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
                            //                    break;
                            //                case 3:
                            //                    ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
                            //                    break;
                            //                case 4:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
                            //                    if (ok)
                            //                    {
                            //                        x.RateAvailabilities.Add(new RateAvailability()
                            //                        {
                            //                            Date = toDate.Date,
                            //                            RoomTypeId = x.RoomTypeId,
                            //                            Price = 0,
                            //                            PriceForAddClient = 0
                            //                        });
                            //                    }
                            //                    break;
                            //            }
                            //            if (ok)
                            //            {

                            //                y.ChooseRoom = 0;
                            //                PriceAndDayofWeed.PriceSaturday += x.TotalPrice;
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionSaturday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
                            //                PriceAndDayofWeed.Saturday = y.Saturday;
                            //                // gán giá sau khi tính tỷ giá theo tiền tệ
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateSaturday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionSaturday, configCurrency.Result);
                            //                PriceAndDayofWeed.PromotionHome = y;
                            //            }
                            //            PriceAndDayofWeeds.Add(PriceAndDayofWeed);
                            //        }
                            //        if (day.DayOfWeek.ToString() == "Sunday" && y.Sunday == true)
                            //        {
                            //            // kiểm tra promotion hợp lệ thỏa mãn điều kiện
                            //            bool ok = true;
                            //            switch (y.TypePromotion)
                            //            {
                            //                case 1:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
                            //                    break;
                            //                case 2:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
                            //                    break;
                            //                case 3:
                            //                    ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
                            //                    break;
                            //                case 4:
                            //                    ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
                            //                    if (ok)
                            //                    {
                            //                        x.RateAvailabilities.Add(new RateAvailability()
                            //                        {
                            //                            Date = toDate.Date,
                            //                            RoomTypeId = x.RoomTypeId,
                            //                            Price = 0,
                            //                            PriceForAddClient = 0
                            //                        });
                            //                    }
                            //                    break;
                            //            }
                            //            if (ok)
                            //            {

                            //                y.ChooseRoom = 0;
                            //                PriceAndDayofWeed.PriceSunday += x.TotalPrice;
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionSunday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
                            //                PriceAndDayofWeed.Sunday = y.Sunday;
                            //                // gán giá sau khi tính tỷ giá theo tiền tệ
                            //                PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateSunday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionSunday, configCurrency.Result);
                            //                PriceAndDayofWeed.PromotionHome = y;
                            //            }
                            //            PriceAndDayofWeeds.Add(PriceAndDayofWeed);
                            //        }
                            //    });
                            //}
                            #endregion

                            // lay khuyen mai thoa man
                            List<Promotion_Home> testPromotions = new List<Promotion_Home>();
                            promotions.ForEach(y =>
                            {
                                bool ok = true;
                                switch (y.TypePromotion)
                                {
                                    case 1:
                                        //ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
                                        ok = (fromDate >= y.FromDate) && (toDate <= y.ToDate);
                                        break;
                                    case 2:
                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
                                        break;
                                    case 3:
                                        ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
                                        break;
                                    case 4:
                                        ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
                                        if (ok)
                                        {

                                            x.RateAvailabilities.Add(new RateAvailability()
                                            {
                                                Date = toDate.Date,
                                                RoomTypeId = x.RoomTypeId,
                                                Price = 0,
                                                PriceForAddClient = 0
                                            });
                                        }
                                        break;
                                }
                                if (ok)
                                {
                                    testPromotions.Add(y);
                                }
                            });

                            float[,] arr = new float[10, 8];
                            for (int i = 0; i < 10; i++)
                            {
                                for (int ii = 0; ii < 8; ii++)
                                {
                                    arr[i, ii] = 0;
                                }
                            }
                            int j = 0;

                            // tra ve mang khuyen mai
                            foreach (Promotion_Home item in testPromotions)
                            {

                                //monday
                                if (testPromotions[j].Monday == true)
                                {
                                    arr[j, 0] = testPromotions[j].Deposit;
                                }
                                else
                                {
                                    for (int km = 0; km < testPromotions.Count; km++)
                                    {
                                        if (promotions[km].Monday == true)
                                        {
                                            if (arr[j, 0] < testPromotions[km].Deposit)
                                            {
                                                arr[j, 0] = testPromotions[km].Deposit;
                                            }
                                        }

                                    }
                                }
                                // tuesday
                                if (testPromotions[j].Tuesday == true)
                                {
                                    arr[j, 1] = testPromotions[j].Deposit;
                                }
                                else
                                {
                                    for (int km = 0; km < testPromotions.Count; km++)
                                    {
                                        if (testPromotions[km].Tuesday == true)
                                        {
                                            if (arr[j, 1] < testPromotions[km].Deposit)
                                            {
                                                arr[j, 1] = testPromotions[km].Deposit;
                                            }
                                        }

                                    }
                                }
                                //Wednesday
                                if (testPromotions[j].Wednesday == true)
                                {
                                    arr[j, 2] = testPromotions[j].Deposit;
                                }
                                else
                                {
                                    for (int km = 0; km < testPromotions.Count; km++)
                                    {
                                        if (testPromotions[km].Wednesday == true)
                                        {
                                            if (arr[j, 2] < testPromotions[km].Deposit)
                                            {
                                                arr[j, 2] = testPromotions[km].Deposit;
                                            }
                                        }

                                    }
                                }
                                //Thursday
                                if (testPromotions[j].Thursday == true)
                                {
                                    arr[j, 3] = testPromotions[j].Deposit;
                                }
                                else
                                {
                                    for (int km = 0; km < testPromotions.Count; km++)
                                    {
                                        if (testPromotions[km].Thursday == true)
                                        {
                                            if (arr[j, 3] < testPromotions[km].Deposit)
                                            {
                                                arr[j, 3] = testPromotions[km].Deposit;
                                            }
                                        }

                                    }
                                }
                                //Friday
                                if (testPromotions[j].Friday == true)
                                {
                                    arr[j, 4] = testPromotions[j].Deposit;
                                }
                                else
                                {
                                    for (int km = 0; km < testPromotions.Count; km++)
                                    {
                                        if (testPromotions[km].Friday == true)
                                        {
                                            if (arr[j, 4] < testPromotions[km].Deposit)
                                            {
                                                arr[j, 4] = testPromotions[km].Deposit;
                                            }
                                        }

                                    }
                                }
                                //Saturday
                                if (testPromotions[j].Saturday == true)
                                {
                                    arr[j, 5] = testPromotions[j].Deposit;
                                }
                                else
                                {
                                    for (int km = 0; km < testPromotions.Count; km++)
                                    {
                                        if (testPromotions[km].Saturday == true)
                                        {
                                            if (arr[j, 5] < testPromotions[km].Deposit)
                                            {
                                                arr[j, 5] = testPromotions[km].Deposit;
                                            }
                                        }

                                    }
                                }
                                //Sunday
                                if (testPromotions[j].Sunday == true)
                                {
                                    arr[j, 6] = testPromotions[j].Deposit;
                                }
                                else
                                {
                                    for (int km = 0; km < testPromotions.Count; km++)
                                    {
                                        if (testPromotions[km].Sunday == true)
                                        {
                                            if (arr[j, 6] < testPromotions[km].Deposit)
                                            {
                                                arr[j, 6] = testPromotions[km].Deposit;
                                            }
                                        }

                                    }
                                }

                                j++;


                            }

                            if (testPromotions.Count > 0)
                            {
                                //test tinh tien cac loai khuyen mai
                                List<Promotion_Home> Promotion_Home = new List<Promotion_Home>();
                                for (int i = 0; i < testPromotions.Count; i++)
                                {
                                    int thu = 0;
                                    float tongtien = 0;
                                    float tongtiengoc = 0;
                                    if (weeddayoff == "Monday")
                                    {
                                        thu = 0;
                                    }
                                    if (weeddayoff == "Tuesday") thu = 1;
                                    if (weeddayoff == "Wednesday") thu = 2;
                                    if (weeddayoff == "Thursday") thu = 3;
                                    if (weeddayoff == "Friday") thu = 4;
                                    if (weeddayoff == "Saturday") thu = 5;
                                    if (weeddayoff == "Sunday") thu = 6;
                                    foreach (DateTime day in EachDay(fromDate, toDate))
                                    {


                                        tongtien += x.TotalPrice * (1 - (arr[i, thu] / 100));
                                        tongtiengoc += x.TotalPrice;
                                        thu++;
                                        if (thu > 6) thu = 0;

                                    }
                                    Promotion_Home TotalPriceAfterPromotion1 = new Promotion_Home();
                                    TotalPriceAfterPromotion1.TotalPriceAfterPromotion = tongtien;

                                    TotalPriceAfterPromotion1.AmountRate = testPromotions[i].AmountRate;
                                    TotalPriceAfterPromotion1.Breakfast = testPromotions[i].Breakfast;
                                    TotalPriceAfterPromotion1.ChooseRoom = testPromotions[i].ChooseRoom;
                                    TotalPriceAfterPromotion1.DayInHouse = testPromotions[i].DayInHouse;
                                    TotalPriceAfterPromotion1.Deposit = testPromotions[i].Deposit;
                                    TotalPriceAfterPromotion1.Description = testPromotions[i].Description;
                                    TotalPriceAfterPromotion1.Dinner = testPromotions[i].Dinner;
                                    TotalPriceAfterPromotion1.EarlyDay = testPromotions[i].EarlyDay;
                                    TotalPriceAfterPromotion1.Lunch = testPromotions[i].Lunch;
                                    TotalPriceAfterPromotion1.Name = testPromotions[i].Name;
                                    TotalPriceAfterPromotion1.NightForFreeNight = testPromotions[i].NightForFreeNight;
                                    TotalPriceAfterPromotion1.Note = testPromotions[i].Note;
                                    TotalPriceAfterPromotion1.PlaneRateId = testPromotions[i].PlaneRateId;
                                    TotalPriceAfterPromotion1.PolicyContent = testPromotions[i].PolicyContent;
                                    TotalPriceAfterPromotion1.PolicyId = testPromotions[i].PolicyId;
                                    TotalPriceAfterPromotion1.Price = testPromotions[i].Price;
                                    TotalPriceAfterPromotion1.PriceExchangeRate = testPromotions[i].PriceExchangeRate;
                                    TotalPriceAfterPromotion1.PromotionId = testPromotions[i].PromotionId;
                                    TotalPriceAfterPromotion1.PromotionName = testPromotions[i].PromotionName;
                                    TotalPriceAfterPromotion1.TotalPriceAfterPromotionExchangeRate = testPromotions[i].TotalPriceAfterPromotionExchangeRate;
                                    TotalPriceAfterPromotion1.TypePromotion = testPromotions[i].TypePromotion;
                                    TotalPriceAfterPromotion1.TotalPrice = tongtiengoc;


                                    Promotion_Home.Add(TotalPriceAfterPromotion1);

                                   
                                }
                                Promotion_Home =    Promotion_Home.OrderByDescending(g => g.Deposit).GroupBy(g => g.TotalPriceAfterPromotion)
                     .Select(grp => grp.First())
                     .ToList();
                                Promotion_Home.ForEach(z =>
                                {
                                    List<RoomType_Home> item = new List<RoomType_Home>();
                                    z.TotalPriceAfterPromotionExchangeRate = ConvertCurrency(z.TotalPriceAfterPromotion, configCurrency.Result);
                                    x.TotalPriceExchangeRate = ConvertCurrency(z.TotalPrice, configCurrency.Result);
                                    x.PromotionHome = z; // gán promotion vào roomtype

                                    x.TotalPrice = z.TotalPrice;
                                    roomTypesResult.Add((RoomType_Home)x.Clone());
                                    item.Add((RoomType_Home)x.Clone());
                                    room[loaiphong, sokm] = item;
                                    //roomTypesResult.Clear();
                                    sokm++;
                                });
                               
                                //
                                List<Model_PriceAndDayofWeed> PriceStuff2 = new List<Model_PriceAndDayofWeed>();
                                List<Model_PriceAndDayofWeed> PriceStuff3 = new List<Model_PriceAndDayofWeed>();
                                List<Model_PriceAndDayofWeed> PriceStuff4 = new List<Model_PriceAndDayofWeed>();
                                List<Model_PriceAndDayofWeed> PriceStuff5 = new List<Model_PriceAndDayofWeed>();
                                List<Model_PriceAndDayofWeed> PriceStuff6 = new List<Model_PriceAndDayofWeed>();
                                List<Model_PriceAndDayofWeed> PriceStuff7 = new List<Model_PriceAndDayofWeed>();
                                List<Model_PriceAndDayofWeed> PriceStuffCn = new List<Model_PriceAndDayofWeed>();
                                // lấy ra số lượng khuyến mại tương ứng với ngày khuyến mãi 
                                #region
                                //foreach (DateTime day in EachDay(fromDate, toDate)) // đang fixx
                                //{
                                //    if (day.DayOfWeek.ToString() == "Monday")
                                //    {
                                //        var priceMonday = PriceAndDayofWeeds.Where(a => a.Monday == true).ToList();
                                //        PriceStuff2 = priceMonday;
                                //    }
                                //    if (day.DayOfWeek.ToString() == "Tuesday")
                                //    {
                                //        var priceTuesday = PriceAndDayofWeeds.Where(a => a.Tuesday == true).ToList();
                                //        PriceStuff3 = priceTuesday;
                                //    }
                                //    if (day.DayOfWeek.ToString() == "Wednesday")
                                //    {
                                //        var priceWednesday = PriceAndDayofWeeds.Where(a => a.Wednesday == true).ToList();
                                //        PriceStuff4 = priceWednesday;
                                //    }
                                //    if (day.DayOfWeek.ToString() == "Thursday")
                                //    {
                                //        var priceThursday = PriceAndDayofWeeds.Where(a => a.Thursday == true).ToList();
                                //        PriceStuff5 = priceThursday;
                                //    }
                                //    if (day.DayOfWeek.ToString() == "Friday")
                                //    {
                                //        var priceFriday = PriceAndDayofWeeds.Where(a => a.Friday == true).ToList();
                                //        PriceStuff6 = priceFriday;
                                //    }
                                //    if (day.DayOfWeek.ToString() == "Saturday")
                                //    {
                                //        var priceSaturday = PriceAndDayofWeeds.Where(a => a.Saturday == true).ToList();
                                //        PriceStuff7 = priceSaturday;
                                //    }
                                //    if (day.DayOfWeek.ToString() == "Sunday")
                                //    {
                                //        var priceSunday = PriceAndDayofWeeds.Where(a => a.Sunday == true).ToList();
                                //        PriceStuffCn = priceSunday;
                                //    }
                                //}
                                //List<TotalPriceAfterPromotion> PriceStuff = new List<TotalPriceAfterPromotion>();
                                //if (PriceStuff2.Count > 0)
                                //{
                                //    TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
                                //    {
                                //        Model_PriceAndDayofWeed = PriceStuff2
                                //    };
                                //    PriceStuff.Add(PriceSt);
                                //}
                                //if (PriceStuff3.Count > 0)
                                //{
                                //    TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
                                //    {
                                //        Model_PriceAndDayofWeed = PriceStuff3
                                //    };
                                //    PriceStuff.Add(PriceSt);
                                //}
                                //if (PriceStuff4.Count > 0)
                                //{
                                //    TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
                                //    {
                                //        Model_PriceAndDayofWeed = PriceStuff4
                                //    };
                                //    PriceStuff.Add(PriceSt);
                                //}
                                //if (PriceStuff5.Count > 0)
                                //{
                                //    TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
                                //    {
                                //        Model_PriceAndDayofWeed = PriceStuff5
                                //    };
                                //    PriceStuff.Add(PriceSt);
                                //}
                                //if (PriceStuff6.Count > 0)
                                //{
                                //    TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
                                //    {
                                //        Model_PriceAndDayofWeed = PriceStuff6
                                //    };
                                //    PriceStuff.Add(PriceSt);
                                //}
                                //if (PriceStuff7.Count > 0)
                                //{
                                //    TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
                                //    {
                                //        Model_PriceAndDayofWeed = PriceStuff7
                                //    };
                                //    PriceStuff.Add(PriceSt);
                                //}
                                //if (PriceStuffCn.Count > 0)
                                //{
                                //    TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
                                //    {
                                //        Model_PriceAndDayofWeed = PriceStuffCn
                                //    };
                                //    PriceStuff.Add(PriceSt);
                                //}
                                //List<Promotion_Home> Promotion_Home = new List<Promotion_Home>();
                                // test tinh tien
                                //float tong = 0;
                                //int test = 0;
                                //float TongGoc = 0;

                                //foreach (var i0 in PriceStuff[0].Model_PriceAndDayofWeed)
                                //{
                                //    test++;
                                //  float TotalPriceAfterPromotion0 = i0.TotalPriceAfterPromotionFriday + i0.TotalPriceAfterPromotionMonday + i0.TotalPriceAfterPromotionSaturday + i0.TotalPriceAfterPromotionSunday
                                //         + i0.TotalPriceAfterPromotionThursday + i0.TotalPriceAfterPromotionTuesday + i0.TotalPriceAfterPromotionWednesday;
                                //     float TotalPrice0 = i0.PriceFriday + i0.PriceMonday + i0.PriceSaturday + i0.PriceSunday
                                //         + i0.PriceThursday + i0.PriceTuesday + i0.PriceWednesday;
                                //    TongGoc += TotalPrice0;
                                //    tong  += TotalPriceAfterPromotion0;
                                //    if (PriceStuff.Count == 1)
                                //    {
                                //        i0.PromotionHome.TotalPriceAfterPromotion = tong;
                                //        i0.PromotionHome.TotalPrice = TongGoc;
                                //        Promotion_Home.Add(i0.PromotionHome);
                                //    }
                                //    if (PriceStuff.Count >= 2)
                                //    {

                                //            foreach (var i1 in PriceStuff[1].Model_PriceAndDayofWeed)
                                //            {
                                //                Promotion_Home TotalPriceAfterPromotion1 = new Promotion_Home();
                                //            TotalPriceAfterPromotion1.TotalPriceAfterPromotion = tong;
                                //            TotalPriceAfterPromotion1.TotalPrice = TongGoc;
                                //            //TotalPriceAfterPromotion1.TotalPriceAfterPromotion = TotalPriceAfterPromotion0 + i1.TotalPriceAfterPromotionFriday + i1.TotalPriceAfterPromotionMonday + i1.TotalPriceAfterPromotionSaturday + i1.TotalPriceAfterPromotionSunday
                                //            //+ i1.TotalPriceAfterPromotionThursday + i1.TotalPriceAfterPromotionTuesday + i1.TotalPriceAfterPromotionWednesday;
                                //            //TotalPriceAfterPromotion1.TotalPrice = TotalPrice0 + i1.PriceFriday + i1.PriceMonday + i1.PriceSaturday + i1.PriceSunday
                                //            //         + i1.PriceThursday + i1.PriceTuesday + i1.PriceWednesday;
                                //            if (PriceStuff[1].Model_PriceAndDayofWeed.Count >= test)
                                //            {
                                //                TongGoc += i1.PriceFriday + i1.PriceMonday + i1.PriceSaturday + i1.PriceSunday
                                //                    + i1.PriceThursday + i1.PriceTuesday + i1.PriceWednesday;

                                //                tong += i1.TotalPriceAfterPromotionFriday + i1.TotalPriceAfterPromotionMonday + i1.TotalPriceAfterPromotionSaturday + i1.TotalPriceAfterPromotionSunday
                                //                + i1.TotalPriceAfterPromotionThursday + i1.TotalPriceAfterPromotionTuesday + i1.TotalPriceAfterPromotionWednesday;
                                //                TotalPriceAfterPromotion1.TotalPriceAfterPromotion = tong;
                                //                TotalPriceAfterPromotion1.TotalPrice = TongGoc;
                                //            }


                                //            if (PriceStuff.Count == 2)
                                //                {
                                //                    TotalPriceAfterPromotion1.AmountRate = i1.PromotionHome.AmountRate;
                                //                    TotalPriceAfterPromotion1.Breakfast = i1.PromotionHome.Breakfast;
                                //                    TotalPriceAfterPromotion1.ChooseRoom = i1.PromotionHome.ChooseRoom;
                                //                    TotalPriceAfterPromotion1.DayInHouse = i1.PromotionHome.DayInHouse;
                                //                    TotalPriceAfterPromotion1.Deposit = i1.PromotionHome.Deposit;
                                //                    TotalPriceAfterPromotion1.Description = i1.PromotionHome.Description;
                                //                    TotalPriceAfterPromotion1.Dinner = i1.PromotionHome.Dinner;
                                //                    TotalPriceAfterPromotion1.EarlyDay = i1.PromotionHome.EarlyDay;
                                //                    TotalPriceAfterPromotion1.Lunch = i1.PromotionHome.Lunch;
                                //                    TotalPriceAfterPromotion1.Name = i1.PromotionHome.Name;
                                //                    TotalPriceAfterPromotion1.NightForFreeNight = i1.PromotionHome.NightForFreeNight;
                                //                    TotalPriceAfterPromotion1.Note = i1.PromotionHome.Note;
                                //                    TotalPriceAfterPromotion1.PlaneRateId = i1.PromotionHome.PlaneRateId;
                                //                    TotalPriceAfterPromotion1.PolicyContent = i1.PromotionHome.PolicyContent;
                                //                    TotalPriceAfterPromotion1.PolicyId = i1.PromotionHome.PolicyId;
                                //                    TotalPriceAfterPromotion1.Price = i1.PromotionHome.Price;
                                //                    TotalPriceAfterPromotion1.PriceExchangeRate = i1.PromotionHome.PriceExchangeRate;
                                //                    TotalPriceAfterPromotion1.PromotionId = i1.PromotionHome.PromotionId;
                                //                    TotalPriceAfterPromotion1.PromotionName = i1.PromotionHome.PromotionName;
                                //                    TotalPriceAfterPromotion1.TotalPriceAfterPromotionExchangeRate = i1.PromotionHome.TotalPriceAfterPromotionExchangeRate;
                                //                    TotalPriceAfterPromotion1.TypePromotion = i1.PromotionHome.TypePromotion;
                                //                    Promotion_Home.Add(TotalPriceAfterPromotion1);
                                //                }
                                //                if (PriceStuff.Count >= 3)
                                //                {
                                //                    foreach (var i2 in PriceStuff[2].Model_PriceAndDayofWeed)
                                //                    {
                                //                        Promotion_Home TotalPriceAfterPromotion2 = new Promotion_Home();
                                //                    TotalPriceAfterPromotion2.TotalPriceAfterPromotion = tong;
                                //                    TotalPriceAfterPromotion2.TotalPrice = TongGoc;
                                //                    //TotalPriceAfterPromotion2.TotalPriceAfterPromotion = TotalPriceAfterPromotion1.TotalPriceAfterPromotion + i2.TotalPriceAfterPromotionFriday + i2.TotalPriceAfterPromotionMonday + i2.TotalPriceAfterPromotionSaturday + i2.TotalPriceAfterPromotionSunday
                                //                    //+ i2.TotalPriceAfterPromotionThursday + i2.TotalPriceAfterPromotionTuesday + i2.TotalPriceAfterPromotionWednesday;
                                //                    //TotalPriceAfterPromotion2.TotalPrice = TotalPriceAfterPromotion1.TotalPrice + i2.PriceFriday + i2.PriceMonday + i2.PriceSaturday + i2.PriceSunday
                                //                    //         + i2.PriceThursday + i2.PriceTuesday + i2.PriceWednesday;
                                //                    if (PriceStuff[2].Model_PriceAndDayofWeed.Count >= test)
                                //                    {
                                //                        TongGoc += i2.PriceFriday + i2.PriceMonday + i2.PriceSaturday + i2.PriceSunday
                                //                             + i2.PriceThursday + i2.PriceTuesday + i2.PriceWednesday;

                                //                        tong += i2.TotalPriceAfterPromotionFriday + i2.TotalPriceAfterPromotionMonday + i2.TotalPriceAfterPromotionSaturday + i2.TotalPriceAfterPromotionSunday
                                //                        + i2.TotalPriceAfterPromotionThursday + i2.TotalPriceAfterPromotionTuesday + i2.TotalPriceAfterPromotionWednesday;
                                //                        TotalPriceAfterPromotion2.TotalPriceAfterPromotion = tong;
                                //                        TotalPriceAfterPromotion2.TotalPrice = TongGoc;
                                //                    }
                                //                    //tong += TotalPriceAfterPromotion2.TotalPriceAfterPromotion;
                                //                    if (PriceStuff.Count == 3)
                                //                        {
                                //                            TotalPriceAfterPromotion2.AmountRate = i2.PromotionHome.AmountRate;
                                //                            TotalPriceAfterPromotion2.Breakfast = i2.PromotionHome.Breakfast;
                                //                            TotalPriceAfterPromotion2.ChooseRoom = i2.PromotionHome.ChooseRoom;
                                //                            TotalPriceAfterPromotion2.DayInHouse = i2.PromotionHome.DayInHouse;
                                //                            TotalPriceAfterPromotion2.Deposit = i2.PromotionHome.Deposit;
                                //                            TotalPriceAfterPromotion2.Description = i2.PromotionHome.Description;
                                //                            TotalPriceAfterPromotion2.Dinner = i2.PromotionHome.Dinner;
                                //                            TotalPriceAfterPromotion2.EarlyDay = i2.PromotionHome.EarlyDay;
                                //                            TotalPriceAfterPromotion2.Lunch = i2.PromotionHome.Lunch;
                                //                            TotalPriceAfterPromotion2.Name = i2.PromotionHome.Name;
                                //                            TotalPriceAfterPromotion2.NightForFreeNight = i2.PromotionHome.NightForFreeNight;
                                //                            TotalPriceAfterPromotion2.Note = i2.PromotionHome.Note;
                                //                            TotalPriceAfterPromotion2.PlaneRateId = i2.PromotionHome.PlaneRateId;
                                //                            TotalPriceAfterPromotion2.PolicyContent = i2.PromotionHome.PolicyContent;
                                //                            TotalPriceAfterPromotion2.PolicyId = i2.PromotionHome.PolicyId;
                                //                            TotalPriceAfterPromotion2.Price = i2.PromotionHome.Price;
                                //                            TotalPriceAfterPromotion2.PriceExchangeRate = i2.PromotionHome.PriceExchangeRate;
                                //                            TotalPriceAfterPromotion2.PromotionId = i2.PromotionHome.PromotionId;
                                //                            TotalPriceAfterPromotion2.PromotionName = i2.PromotionHome.PromotionName;
                                //                            TotalPriceAfterPromotion2.TotalPriceAfterPromotionExchangeRate = i2.PromotionHome.TotalPriceAfterPromotionExchangeRate;
                                //                            TotalPriceAfterPromotion2.TypePromotion = i2.PromotionHome.TypePromotion;
                                //                            Promotion_Home.Add(TotalPriceAfterPromotion2);
                                //                        }
                                //                        if (PriceStuff.Count >= 4)
                                //                        {
                                //                            foreach (var i3 in PriceStuff[3].Model_PriceAndDayofWeed)
                                //                            {
                                //                                Promotion_Home TotalPriceAfterPromotion3 = new Promotion_Home();
                                //                            TotalPriceAfterPromotion3.TotalPriceAfterPromotion = tong;
                                //                            TotalPriceAfterPromotion3.TotalPrice = TongGoc;
                                //                            //TotalPriceAfterPromotion3.TotalPriceAfterPromotion = TotalPriceAfterPromotion2.TotalPriceAfterPromotion + i3.TotalPriceAfterPromotionFriday + i3.TotalPriceAfterPromotionMonday + i3.TotalPriceAfterPromotionSaturday + i3.TotalPriceAfterPromotionSunday
                                //                            //+ i3.TotalPriceAfterPromotionThursday + i3.TotalPriceAfterPromotionTuesday + i3.TotalPriceAfterPromotionWednesday;
                                //                            //TotalPriceAfterPromotion3.TotalPrice = TotalPriceAfterPromotion2.TotalPrice + i3.PriceFriday + i3.PriceMonday + i3.PriceSaturday + i3.PriceSunday
                                //                            //         + i3.PriceThursday + i3.PriceTuesday + i3.PriceWednesday;
                                //                            if (PriceStuff[3].Model_PriceAndDayofWeed.Count >= test)
                                //                            {
                                //                                TongGoc += i3.PriceFriday + i3.PriceMonday + i3.PriceSaturday + i3.PriceSunday
                                //                                     + i3.PriceThursday + i3.PriceTuesday + i3.PriceWednesday;

                                //                                tong += i3.TotalPriceAfterPromotionFriday + i3.TotalPriceAfterPromotionMonday + i3.TotalPriceAfterPromotionSaturday + i3.TotalPriceAfterPromotionSunday
                                //                                + i3.TotalPriceAfterPromotionThursday + i3.TotalPriceAfterPromotionTuesday + i3.TotalPriceAfterPromotionWednesday;
                                //                                TotalPriceAfterPromotion3.TotalPriceAfterPromotion = tong;
                                //                                TotalPriceAfterPromotion3.TotalPrice = TongGoc;
                                //                            }
                                //                            if (PriceStuff.Count == 4)
                                //                                {
                                //                                    TotalPriceAfterPromotion3.AmountRate = i3.PromotionHome.AmountRate;
                                //                                    TotalPriceAfterPromotion3.Breakfast = i3.PromotionHome.Breakfast;
                                //                                    TotalPriceAfterPromotion3.ChooseRoom = i3.PromotionHome.ChooseRoom;
                                //                                    TotalPriceAfterPromotion3.DayInHouse = i3.PromotionHome.DayInHouse;
                                //                                    TotalPriceAfterPromotion3.Deposit = i3.PromotionHome.Deposit;
                                //                                    TotalPriceAfterPromotion3.Description = i3.PromotionHome.Description;
                                //                                    TotalPriceAfterPromotion3.Dinner = i3.PromotionHome.Dinner;
                                //                                    TotalPriceAfterPromotion3.EarlyDay = i3.PromotionHome.EarlyDay;
                                //                                    TotalPriceAfterPromotion3.Lunch = i3.PromotionHome.Lunch;
                                //                                    TotalPriceAfterPromotion3.Name = i3.PromotionHome.Name;
                                //                                    TotalPriceAfterPromotion3.NightForFreeNight = i3.PromotionHome.NightForFreeNight;
                                //                                    TotalPriceAfterPromotion3.Note = i3.PromotionHome.Note;
                                //                                    TotalPriceAfterPromotion3.PlaneRateId = i3.PromotionHome.PlaneRateId;
                                //                                    TotalPriceAfterPromotion3.PolicyContent = i3.PromotionHome.PolicyContent;
                                //                                    TotalPriceAfterPromotion3.PolicyId = i3.PromotionHome.PolicyId;
                                //                                    TotalPriceAfterPromotion3.Price = i3.PromotionHome.Price;
                                //                                    TotalPriceAfterPromotion3.PriceExchangeRate = i3.PromotionHome.PriceExchangeRate;
                                //                                    TotalPriceAfterPromotion3.PromotionId = i3.PromotionHome.PromotionId;
                                //                                    TotalPriceAfterPromotion3.PromotionName = i3.PromotionHome.PromotionName;
                                //                                    TotalPriceAfterPromotion3.TotalPriceAfterPromotionExchangeRate = i3.PromotionHome.TotalPriceAfterPromotionExchangeRate;
                                //                                    TotalPriceAfterPromotion3.TypePromotion = i3.PromotionHome.TypePromotion;
                                //                                    Promotion_Home.Add(TotalPriceAfterPromotion3);
                                //                                }
                                //                                if (PriceStuff.Count >= 5)
                                //                                {

                                //                                    foreach (var i4 in PriceStuff[4].Model_PriceAndDayofWeed)
                                //                                    {
                                //                                        Promotion_Home TotalPriceAfterPromotion4 = new Promotion_Home();
                                //                                    TotalPriceAfterPromotion4.TotalPriceAfterPromotion = tong;
                                //                                    TotalPriceAfterPromotion4.TotalPrice = TongGoc;
                                //                                   //TotalPriceAfterPromotion4.TotalPriceAfterPromotion = TotalPriceAfterPromotion3.TotalPriceAfterPromotion + i4.TotalPriceAfterPromotionFriday + i4.TotalPriceAfterPromotionMonday + i4.TotalPriceAfterPromotionSaturday + i4.TotalPriceAfterPromotionSunday
                                //                                   //+ i4.TotalPriceAfterPromotionThursday + i4.TotalPriceAfterPromotionTuesday + i4.TotalPriceAfterPromotionWednesday;
                                //                                   //TotalPriceAfterPromotion4.TotalPrice = TotalPriceAfterPromotion3.TotalPrice + i4.PriceFriday + i4.PriceMonday + i4.PriceSaturday + i4.PriceSunday
                                //                                   //          + i4.PriceThursday + i4.PriceTuesday + i4.PriceWednesday;
                                //                                    if (PriceStuff[4].Model_PriceAndDayofWeed.Count >= test)
                                //                                    {
                                //                                        TongGoc += i4.PriceFriday + i4.PriceMonday + i4.PriceSaturday + i4.PriceSunday
                                //                                            + i4.PriceThursday + i4.PriceTuesday + i4.PriceWednesday;
                                //                                        tong += i4.TotalPriceAfterPromotionFriday + i4.TotalPriceAfterPromotionMonday + i4.TotalPriceAfterPromotionSaturday + i4.TotalPriceAfterPromotionSunday
                                //                                        + i4.TotalPriceAfterPromotionThursday + i4.TotalPriceAfterPromotionTuesday + i4.TotalPriceAfterPromotionWednesday;
                                //                                        TotalPriceAfterPromotion4.TotalPriceAfterPromotion = tong;
                                //                                        TotalPriceAfterPromotion4.TotalPrice = TongGoc;
                                //                                    }
                                //                                    if (PriceStuff.Count == 5)
                                //                                        {
                                //                                            TotalPriceAfterPromotion4.AmountRate = i4.PromotionHome.AmountRate;
                                //                                            TotalPriceAfterPromotion4.Breakfast = i4.PromotionHome.Breakfast;
                                //                                            TotalPriceAfterPromotion4.ChooseRoom = i4.PromotionHome.ChooseRoom;
                                //                                            TotalPriceAfterPromotion4.DayInHouse = i4.PromotionHome.DayInHouse;
                                //                                            TotalPriceAfterPromotion4.Deposit = i4.PromotionHome.Deposit;
                                //                                            TotalPriceAfterPromotion4.Description = i4.PromotionHome.Description;
                                //                                            TotalPriceAfterPromotion4.Dinner = i4.PromotionHome.Dinner;
                                //                                            TotalPriceAfterPromotion4.EarlyDay = i4.PromotionHome.EarlyDay;
                                //                                            TotalPriceAfterPromotion4.Lunch = i4.PromotionHome.Lunch;
                                //                                            TotalPriceAfterPromotion4.Name = i4.PromotionHome.Name;
                                //                                            TotalPriceAfterPromotion4.NightForFreeNight = i4.PromotionHome.NightForFreeNight;
                                //                                            TotalPriceAfterPromotion4.Note = i4.PromotionHome.Note;
                                //                                            TotalPriceAfterPromotion4.PlaneRateId = i4.PromotionHome.PlaneRateId;
                                //                                            TotalPriceAfterPromotion4.PolicyContent = i4.PromotionHome.PolicyContent;
                                //                                            TotalPriceAfterPromotion4.PolicyId = i4.PromotionHome.PolicyId;
                                //                                            TotalPriceAfterPromotion4.Price = i4.PromotionHome.Price;
                                //                                            TotalPriceAfterPromotion4.PriceExchangeRate = i4.PromotionHome.PriceExchangeRate;
                                //                                            TotalPriceAfterPromotion4.PromotionId = i4.PromotionHome.PromotionId;
                                //                                            TotalPriceAfterPromotion4.PromotionName = i4.PromotionHome.PromotionName;
                                //                                            TotalPriceAfterPromotion4.TotalPriceAfterPromotionExchangeRate = i4.PromotionHome.TotalPriceAfterPromotionExchangeRate;
                                //                                            TotalPriceAfterPromotion4.TypePromotion = i4.PromotionHome.TypePromotion;
                                //                                            Promotion_Home.Add(TotalPriceAfterPromotion4);
                                //                                        }
                                //                                        if (PriceStuff.Count >= 6)
                                //                                        {
                                //                                            foreach (var i5 in PriceStuff[5].Model_PriceAndDayofWeed)
                                //                                            {
                                //                                                Promotion_Home TotalPriceAfterPromotion5 = new Promotion_Home();
                                //                                            TotalPriceAfterPromotion5.TotalPriceAfterPromotion = tong;
                                //                                            TotalPriceAfterPromotion5.TotalPrice = TongGoc;
                                //                                            //TotalPriceAfterPromotion5.TotalPriceAfterPromotion = TotalPriceAfterPromotion4.TotalPriceAfterPromotion + i5.TotalPriceAfterPromotionFriday + i5.TotalPriceAfterPromotionMonday + i5.TotalPriceAfterPromotionSaturday + i5.TotalPriceAfterPromotionSunday
                                //                                            //+ i5.TotalPriceAfterPromotionThursday + i5.TotalPriceAfterPromotionTuesday + i5.TotalPriceAfterPromotionWednesday;
                                //                                            //TotalPriceAfterPromotion5.TotalPrice = TotalPriceAfterPromotion4.TotalPrice + i5.PriceFriday + i5.PriceMonday + i5.PriceSaturday + i5.PriceSunday
                                //                                            //         + i5.PriceThursday + i5.PriceTuesday + i5.PriceWednesday;
                                //                                            if (PriceStuff[5].Model_PriceAndDayofWeed.Count >= test)
                                //                                            {
                                //                                                TongGoc += i5.PriceFriday + i5.PriceMonday + i5.PriceSaturday + i5.PriceSunday
                                //                                                     + i5.PriceThursday + i5.PriceTuesday + i5.PriceWednesday;
                                //                                                tong += i5.TotalPriceAfterPromotionFriday + i5.TotalPriceAfterPromotionMonday + i5.TotalPriceAfterPromotionSaturday + i5.TotalPriceAfterPromotionSunday
                                //                                                + i5.TotalPriceAfterPromotionThursday + i5.TotalPriceAfterPromotionTuesday + i5.TotalPriceAfterPromotionWednesday;
                                //                                                TotalPriceAfterPromotion5.TotalPriceAfterPromotion = tong;
                                //                                                TotalPriceAfterPromotion5.TotalPrice = TongGoc;
                                //                                            }
                                //                                            if (PriceStuff.Count == 6)
                                //                                                {
                                //                                                    TotalPriceAfterPromotion5.AmountRate = i5.PromotionHome.AmountRate;
                                //                                                    TotalPriceAfterPromotion5.Breakfast = i5.PromotionHome.Breakfast;
                                //                                                    TotalPriceAfterPromotion5.ChooseRoom = i5.PromotionHome.ChooseRoom;
                                //                                                    TotalPriceAfterPromotion5.DayInHouse = i5.PromotionHome.DayInHouse;
                                //                                                    TotalPriceAfterPromotion5.Deposit = i5.PromotionHome.Deposit;
                                //                                                    TotalPriceAfterPromotion5.Description = i5.PromotionHome.Description;
                                //                                                    TotalPriceAfterPromotion5.Dinner = i5.PromotionHome.Dinner;
                                //                                                    TotalPriceAfterPromotion5.EarlyDay = i5.PromotionHome.EarlyDay;
                                //                                                    TotalPriceAfterPromotion5.Lunch = i5.PromotionHome.Lunch;
                                //                                                    TotalPriceAfterPromotion5.Name = i5.PromotionHome.Name;
                                //                                                    TotalPriceAfterPromotion5.NightForFreeNight = i5.PromotionHome.NightForFreeNight;
                                //                                                    TotalPriceAfterPromotion5.Note = i5.PromotionHome.Note;
                                //                                                    TotalPriceAfterPromotion5.PlaneRateId = i5.PromotionHome.PlaneRateId;
                                //                                                    TotalPriceAfterPromotion5.PolicyContent = i5.PromotionHome.PolicyContent;
                                //                                                    TotalPriceAfterPromotion5.PolicyId = i5.PromotionHome.PolicyId;
                                //                                                    TotalPriceAfterPromotion5.Price = i5.PromotionHome.Price;
                                //                                                    TotalPriceAfterPromotion5.PriceExchangeRate = i5.PromotionHome.PriceExchangeRate;
                                //                                                    TotalPriceAfterPromotion5.PromotionId = i5.PromotionHome.PromotionId;
                                //                                                    TotalPriceAfterPromotion5.PromotionName = i5.PromotionHome.PromotionName;
                                //                                                    TotalPriceAfterPromotion5.TotalPriceAfterPromotionExchangeRate = i5.PromotionHome.TotalPriceAfterPromotionExchangeRate;
                                //                                                    TotalPriceAfterPromotion5.TypePromotion = i5.PromotionHome.TypePromotion;
                                //                                                    Promotion_Home.Add(TotalPriceAfterPromotion5);
                                //                                                }
                                //                                                if (PriceStuff.Count >= 7 )
                                //                                                {
                                //                                                    foreach (var i6 in PriceStuff[6].Model_PriceAndDayofWeed)
                                //                                                    {
                                //                                                        Promotion_Home TotalPriceAfterPromotion6 = new Promotion_Home();
                                //                                                    TotalPriceAfterPromotion6.TotalPriceAfterPromotion = tong;
                                //                                                    TotalPriceAfterPromotion6.TotalPrice = TongGoc;
                                //                                                    //TotalPriceAfterPromotion6.TotalPriceAfterPromotion = TotalPriceAfterPromotion5.TotalPriceAfterPromotion + i6.TotalPriceAfterPromotionFriday + i6.TotalPriceAfterPromotionMonday + i6.TotalPriceAfterPromotionSaturday + i6.TotalPriceAfterPromotionSunday
                                //                                                    //+ i6.TotalPriceAfterPromotionThursday + i6.TotalPriceAfterPromotionTuesday + i6.TotalPriceAfterPromotionWednesday;
                                //                                                    //TotalPriceAfterPromotion6.TotalPrice = TotalPriceAfterPromotion5.TotalPrice + i6.PriceFriday + i6.PriceMonday + i6.PriceSaturday + i6.PriceSunday
                                //                                                    //         + i6.PriceThursday + i6.PriceTuesday + i6.PriceWednesday;
                                //                                                    if (PriceStuff[6].Model_PriceAndDayofWeed.Count >= test)
                                //                                                    {

                                //                                                        TongGoc += i6.PriceFriday + i6.PriceMonday + i6.PriceSaturday + i6.PriceSunday
                                //                                                             + i6.PriceThursday + i6.PriceTuesday + i6.PriceWednesday;
                                //                                                        tong += i6.TotalPriceAfterPromotionFriday + i6.TotalPriceAfterPromotionMonday + i6.TotalPriceAfterPromotionSaturday + i6.TotalPriceAfterPromotionSunday
                                //                                                        + i6.TotalPriceAfterPromotionThursday + i6.TotalPriceAfterPromotionTuesday + i6.TotalPriceAfterPromotionWednesday;
                                //                                                        TotalPriceAfterPromotion6.TotalPriceAfterPromotion = tong;
                                //                                                        TotalPriceAfterPromotion6.TotalPrice = TongGoc;

                                //                                                    }
                                //                                                    if (PriceStuff.Count == 7 && test == PriceStuff[0].Model_PriceAndDayofWeed.Count)
                                //                                                        {
                                //                                                            TotalPriceAfterPromotion6.AmountRate = i6.PromotionHome.AmountRate;
                                //                                                            TotalPriceAfterPromotion6.Breakfast = i6.PromotionHome.Breakfast;
                                //                                                            TotalPriceAfterPromotion6.ChooseRoom = i6.PromotionHome.ChooseRoom;
                                //                                                            TotalPriceAfterPromotion6.DayInHouse = i6.PromotionHome.DayInHouse;
                                //                                                            TotalPriceAfterPromotion6.Deposit = i6.PromotionHome.Deposit;
                                //                                                            TotalPriceAfterPromotion6.Description = i6.PromotionHome.Description;
                                //                                                            TotalPriceAfterPromotion6.Dinner = i6.PromotionHome.Dinner;
                                //                                                            TotalPriceAfterPromotion6.EarlyDay = i6.PromotionHome.EarlyDay;
                                //                                                            TotalPriceAfterPromotion6.Lunch = i6.PromotionHome.Lunch;
                                //                                                            TotalPriceAfterPromotion6.Name = i6.PromotionHome.Name;
                                //                                                            TotalPriceAfterPromotion6.NightForFreeNight = i6.PromotionHome.NightForFreeNight;
                                //                                                            TotalPriceAfterPromotion6.Note = i6.PromotionHome.Note;
                                //                                                            TotalPriceAfterPromotion6.PlaneRateId = i6.PromotionHome.PlaneRateId;
                                //                                                            TotalPriceAfterPromotion6.PolicyContent = i6.PromotionHome.PolicyContent;
                                //                                                            TotalPriceAfterPromotion6.PolicyId = i6.PromotionHome.PolicyId;
                                //                                                            TotalPriceAfterPromotion6.Price = i6.PromotionHome.Price;
                                //                                                            TotalPriceAfterPromotion6.PriceExchangeRate = i6.PromotionHome.PriceExchangeRate;
                                //                                                            TotalPriceAfterPromotion6.PromotionId = i6.PromotionHome.PromotionId;
                                //                                                            TotalPriceAfterPromotion6.PromotionName = i6.PromotionHome.PromotionName;
                                //                                                            TotalPriceAfterPromotion6.TotalPriceAfterPromotionExchangeRate = i6.PromotionHome.TotalPriceAfterPromotionExchangeRate;
                                //                                                            TotalPriceAfterPromotion6.TypePromotion = i6.PromotionHome.TypePromotion;
                                //                                                        Promotion_Home.Add(TotalPriceAfterPromotion6);

                                //                                                        }
                                //                                                    break;
                                //                                                }

                                //                                                }
                                //                                            break;

                                //                                        }

                                //                                    }
                                //                                    break;
                                //                                }


                                //                            }
                                //                            break;
                                //                        }


                                //                    }
                                //                    break;
                                //                }


                                //            }
                                //            break;
                                //        }



                                //    }

                                //}

                                //Promotion_Home.ForEach(z =>
                                //{
                                //    z.TotalPriceAfterPromotionExchangeRate = ConvertCurrency(z.TotalPriceAfterPromotion, configCurrency.Result);
                                //    x.TotalPriceExchangeRate = ConvertCurrency(z.TotalPrice, configCurrency.Result);
                                //    x.PromotionHome = z; // gán promotion vào roomtype

                                //    x.TotalPrice = z.TotalPrice;
                                //    roomTypesResult.Add((RoomType_Home)x.Clone());
                                //});
                                #endregion
                            }


                        }

                    }

                    loaiphong++;
                });
                
                roomTypesResult.OrderBy(x => x.PromotionHome.Price);
                var result = roomTypesResult.OrderBy(g=>g.Index).GroupBy(g => g.PromotionHome.TotalPriceAfterPromotion)
                   .Select(grp => grp.First())
                   .ToList();
                var test = room;
                var test1 = JsonConvert.SerializeObject(result);
                return JsonConvert.SerializeObject(result);
            }
        }

        //private string GetRoomAvailable_FullPromotion(string hotelCode, DateTime fromDate, DateTime toDate, string lang = "vi", string currency = "VND")
        //{
        //    // lấy ra ngày bắt đầu trong tuần.
        //    string weeddayoff = fromDate.DayOfWeek.ToString();

        //    //int weeddayoff = (int)fromDate.DayOfWeek;
        //    // lấy ra số lượng ngày 
        //    int rangeDate = DataHelper.RangeDate(fromDate, toDate);
        //    using (var connection = DB.ConnectionFactory())
        //    {
        //        int LanguageId = connection.QuerySingleOrDefault<int>("Language_GetIdByKey",
        //            new
        //            {
        //                Key = lang
        //            }, commandType: CommandType.StoredProcedure);
        //        Session["languageIdClient"] = LanguageId;
        //        // Khởi tạo danh sách đặt phòng ban đầu. Mỗi phòng tương ứng 1 promotion
        //        List<RoomType_Home> roomTypesResult = new List<RoomType_Home>();
        //        // lấy danh sách phòng
        //        List<RoomType_Home> roomTypes = connection.Query<RoomType_Home>("RoomType_GetByCodeHotel",
        //            new
        //            {
        //                LanguageId = LanguageId,
        //                HodelCode = hotelCode
        //            }, commandType: CommandType.StoredProcedure).ToList();
        //        if (roomTypes is null) roomTypes = new List<RoomType_Home>();
        //        // lấy tỷ giá hối đoái để cập nhật giá theo loại tiền tệ
        //        ConfigCurrency configCurrency = connection.QuerySingleOrDefault<ConfigCurrency>("ConfigCurrency_GetExchangeRate",
        //            new
        //            {
        //                HodelCode = hotelCode,
        //                CurrencyCode = currency
        //            }, commandType: CommandType.StoredProcedure);
        //        if (configCurrency.AutoCalculator)
        //        {
        //            configCurrency.Result = DataHelper.CurrencyConvertor(currency);
        //        }
        //        // lấy danh sách promotion theo từng loại phòng
        //        // b1: lấy danh sách promotion của loại phòng - đồng thời lấy thông tin chính sách
        //        // b2: lấy danh sách giá tương ứng phòng
        //        // b3: lấy tổng tiền và số lượng phòng còn trống
        //        roomTypes.ForEach(x =>
        //        {
        //            using (var multi = connection.QueryMultiple("RoomType_GetDetailShowClient",
        //                new
        //                {
        //                    LanguageId = LanguageId,
        //                    RoomTypeId = x.RoomTypeId,
        //                    FromDate = fromDate,
        //                    ToDate = toDate,
        //                    DateNow = DatetimeHelper.DateTimeUTCNow()
        //                }, commandType: CommandType.StoredProcedure))
        //            {
        //                //lấy ra số lượng khuyến mãi
        //                List<Promotion_Home> promotions = multi.Read<Promotion_Home>().ToList();

        //                x.RateAvailabilities = multi.Read<RateAvailability>().ToList();
        //                x.TotalPrice = multi.Read<float>().FirstOrDefault();
        //                x.NumberAvailable = multi.Read<int>().SingleOrDefault();

        //                if (x.RateAvailabilities is null) x.RateAvailabilities = new List<RateAvailability>();

        //                // kiểm tra có còn phòng trống và trong khoảng thời gian đặt phòng có đóng phòng hay không
        //                if (x.RateAvailabilities.Count == rangeDate && x.NumberAvailable > 0)
        //                {
        //                    // Chuyển đổi ngoại tệ thành tiền tương ứng
        //                    x.TotalPriceExchangeRate = ConvertCurrency(x.TotalPrice, configCurrency.Result);
        //                    if (promotions is null) promotions = new List<Promotion_Home>();
        //                    List<Model_PriceAndDayofWeed> PriceAndDayofWeeds = new List<Model_PriceAndDayofWeed>();
        //                    if (x.RateAvailabilities is null) x.RateAvailabilities = new List<RateAvailability>(); //TotalPriceAfterPromotion
        //                    #region
        //                    foreach (DateTime day in EachDay(fromDate, toDate))
        //                    {
        //                        promotions.ForEach(y =>
        //                        {
        //                            Model_PriceAndDayofWeed PriceAndDayofWeed = new Model_PriceAndDayofWeed();

        //                            if (day.DayOfWeek.ToString() == "Monday" && y.Monday == true)
        //                            {
        //                                // kiểm tra promotion hợp lệ thỏa mãn điều kiện
        //                                bool ok = true;
        //                                switch (y.TypePromotion)
        //                                {
        //                                    case 1:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
        //                                        break;
        //                                    case 2:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
        //                                        break;
        //                                    case 3:
        //                                        ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
        //                                        break;
        //                                    case 4:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
        //                                        if (ok)
        //                                        {
        //                                            x.RateAvailabilities.Add(new RateAvailability()
        //                                            {
        //                                                Date = toDate.Date,
        //                                                RoomTypeId = x.RoomTypeId,
        //                                                Price = 0,
        //                                                PriceForAddClient = 0
        //                                            });
        //                                        }
        //                                        break;
        //                                }
        //                                if (ok)
        //                                {
        //                                    y.ChooseRoom = 0;
        //                                    PriceAndDayofWeed.PriceMonday += x.TotalPrice;
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionMonday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
        //                                    PriceAndDayofWeed.Monday = y.Monday;
        //                                    // gán giá sau khi tính tỷ giá theo tiền tệ
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateMonday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionMonday, configCurrency.Result);
        //                                    PriceAndDayofWeed.PromotionHome = y;
        //                                }
        //                                PriceAndDayofWeeds.Add(PriceAndDayofWeed);
        //                            }
        //                            if (day.DayOfWeek.ToString() == "Tuesday" && y.Tuesday == true)
        //                            {
        //                                // kiểm tra promotion hợp lệ thỏa mãn điều kiện
        //                                bool ok = true;
        //                                switch (y.TypePromotion)
        //                                {
        //                                    case 1:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
        //                                        break;
        //                                    case 2:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
        //                                        break;
        //                                    case 3:
        //                                        ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
        //                                        break;
        //                                    case 4:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
        //                                        if (ok)
        //                                        {
        //                                            x.RateAvailabilities.Add(new RateAvailability()
        //                                            {
        //                                                Date = toDate.Date,
        //                                                RoomTypeId = x.RoomTypeId,
        //                                                Price = 0,
        //                                                PriceForAddClient = 0
        //                                            });
        //                                        }
        //                                        break;
        //                                }
        //                                if (ok)
        //                                {
        //                                    y.ChooseRoom = 0;
        //                                    PriceAndDayofWeed.PriceTuesday += x.TotalPrice;
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionTuesday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
        //                                    PriceAndDayofWeed.Tuesday = y.Tuesday;
        //                                    // gán giá sau khi tính tỷ giá theo tiền tệ
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateTuesday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionTuesday, configCurrency.Result);
        //                                    PriceAndDayofWeed.PromotionHome = y;
        //                                }
        //                                PriceAndDayofWeeds.Add(PriceAndDayofWeed);
        //                            }
        //                            if (day.DayOfWeek.ToString() == "Wednesday" && y.Wednesday == true)
        //                            {
        //                                // kiểm tra promotion hợp lệ thỏa mãn điều kiện
        //                                bool ok = true;
        //                                switch (y.TypePromotion)
        //                                {
        //                                    case 1:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
        //                                        break;
        //                                    case 2:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
        //                                        break;
        //                                    case 3:
        //                                        ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
        //                                        break;
        //                                    case 4:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
        //                                        if (ok)
        //                                        {
        //                                            x.RateAvailabilities.Add(new RateAvailability()
        //                                            {
        //                                                Date = toDate.Date,
        //                                                RoomTypeId = x.RoomTypeId,
        //                                                Price = 0,
        //                                                PriceForAddClient = 0
        //                                            });
        //                                        }
        //                                        break;
        //                                }
        //                                if (ok)
        //                                {
        //                                    y.ChooseRoom = 0;
        //                                    PriceAndDayofWeed.PriceWednesday += x.TotalPrice;
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionWednesday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
        //                                    PriceAndDayofWeed.Wednesday = y.Wednesday;
        //                                    // gán giá sau khi tính tỷ giá theo tiền tệ
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateWednesday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionWednesday, configCurrency.Result);
        //                                    PriceAndDayofWeed.PromotionHome = y;
        //                                }
        //                                PriceAndDayofWeeds.Add(PriceAndDayofWeed);
        //                            }
        //                            if (day.DayOfWeek.ToString() == "Thursday" && y.Thursday == true)
        //                            {
        //                                // kiểm tra promotion hợp lệ thỏa mãn điều kiện
        //                                bool ok = true;
        //                                switch (y.TypePromotion)
        //                                {
        //                                    case 1:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
        //                                        break;
        //                                    case 2:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
        //                                        break;
        //                                    case 3:
        //                                        ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
        //                                        break;
        //                                    case 4:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
        //                                        if (ok)
        //                                        {
        //                                            x.RateAvailabilities.Add(new RateAvailability()
        //                                            {
        //                                                Date = toDate.Date,
        //                                                RoomTypeId = x.RoomTypeId,
        //                                                Price = 0,
        //                                                PriceForAddClient = 0
        //                                            });
        //                                        }
        //                                        break;
        //                                }
        //                                if (ok)
        //                                {
        //                                    y.ChooseRoom = 0;
        //                                    PriceAndDayofWeed.PriceThursday += x.TotalPrice;
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionThursday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
        //                                    PriceAndDayofWeed.Thursday = y.Thursday;
        //                                    // gán giá sau khi tính tỷ giá theo tiền tệ
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateThursday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionThursday, configCurrency.Result);
        //                                    PriceAndDayofWeed.PromotionHome = y;
        //                                }
        //                                PriceAndDayofWeeds.Add(PriceAndDayofWeed);
        //                            }
        //                            if (day.DayOfWeek.ToString() == "Friday" && y.Friday == true)
        //                            {
        //                                // kiểm tra promotion hợp lệ thỏa mãn điều kiện
        //                                bool ok = true;
        //                                switch (y.TypePromotion)
        //                                {
        //                                    case 1:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
        //                                        break;
        //                                    case 2:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
        //                                        break;
        //                                    case 3:
        //                                        ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
        //                                        break;
        //                                    case 4:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
        //                                        if (ok)
        //                                        {
        //                                            x.RateAvailabilities.Add(new RateAvailability()
        //                                            {
        //                                                Date = toDate.Date,
        //                                                RoomTypeId = x.RoomTypeId,
        //                                                Price = 0,
        //                                                PriceForAddClient = 0
        //                                            });
        //                                        }
        //                                        break;
        //                                }
        //                                if (ok)
        //                                {
        //                                    y.ChooseRoom = 0;
        //                                    PriceAndDayofWeed.PriceFriday += x.TotalPrice;
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionFriday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
        //                                    PriceAndDayofWeed.Friday = y.Friday;
        //                                    // gán giá sau khi tính tỷ giá theo tiền tệ
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateFriday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionFriday, configCurrency.Result);
        //                                    PriceAndDayofWeed.PromotionHome = y;
        //                                }
        //                                PriceAndDayofWeeds.Add(PriceAndDayofWeed);
        //                            }
        //                            if (day.DayOfWeek.ToString() == "Saturday" && y.Saturday == true)
        //                            {
        //                                // kiểm tra promotion hợp lệ thỏa mãn điều kiện
        //                                bool ok = true;
        //                                switch (y.TypePromotion)
        //                                {
        //                                    case 1:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
        //                                        break;
        //                                    case 2:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
        //                                        break;
        //                                    case 3:
        //                                        ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
        //                                        break;
        //                                    case 4:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
        //                                        if (ok)
        //                                        {
        //                                            x.RateAvailabilities.Add(new RateAvailability()
        //                                            {
        //                                                Date = toDate.Date,
        //                                                RoomTypeId = x.RoomTypeId,
        //                                                Price = 0,
        //                                                PriceForAddClient = 0
        //                                            });
        //                                        }
        //                                        break;
        //                                }
        //                                if (ok)
        //                                {
        //                                    y.ChooseRoom = 0;
        //                                    PriceAndDayofWeed.PriceSaturday += x.TotalPrice;
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionSaturday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
        //                                    PriceAndDayofWeed.Saturday = y.Saturday;
        //                                    // gán giá sau khi tính tỷ giá theo tiền tệ
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateSaturday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionSaturday, configCurrency.Result);
        //                                    PriceAndDayofWeed.PromotionHome = y;
        //                                }
        //                                PriceAndDayofWeeds.Add(PriceAndDayofWeed);
        //                            }
        //                            if (day.DayOfWeek.ToString() == "Sunday" && y.Sunday == true)
        //                            {
        //                                // kiểm tra promotion hợp lệ thỏa mãn điều kiện
        //                                bool ok = true;
        //                                switch (y.TypePromotion)
        //                                {
        //                                    case 1:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse;
        //                                        break;
        //                                    case 2:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.DayInHouse && (fromDate.Date - DateTime.Now.Date).Days <= y.EarlyDay;
        //                                        break;
        //                                    case 3:
        //                                        ok = (fromDate.Date - DateTime.Now.Date).Days >= y.EarlyDay;
        //                                        break;
        //                                    case 4:
        //                                        ok = (toDate.Date - fromDate.Date).Days >= y.NightForFreeNight;
        //                                        if (ok)
        //                                        {
        //                                            x.RateAvailabilities.Add(new RateAvailability()
        //                                            {
        //                                                Date = toDate.Date,
        //                                                RoomTypeId = x.RoomTypeId,
        //                                                Price = 0,
        //                                                PriceForAddClient = 0
        //                                            });
        //                                        }
        //                                        break;
        //                                }
        //                                if (ok)
        //                                {
        //                                    y.ChooseRoom = 0;
        //                                    PriceAndDayofWeed.PriceSunday += x.TotalPrice;
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionSunday += (float)Math.Round(x.TotalPrice * (100 - y.Deposit) / 100 + y.Price * rangeDate, 2);
        //                                    PriceAndDayofWeed.Sunday = y.Sunday;
        //                                    // gán giá sau khi tính tỷ giá theo tiền tệ
        //                                    PriceAndDayofWeed.TotalPriceAfterPromotionExchangeRateSunday = ConvertCurrency(PriceAndDayofWeed.TotalPriceAfterPromotionSunday, configCurrency.Result);
        //                                    PriceAndDayofWeed.PromotionHome = y;
        //                                }
        //                                PriceAndDayofWeeds.Add(PriceAndDayofWeed);
        //                            }
        //                        });
        //                    }


        //                    List<Model_PriceAndDayofWeed> PriceStuff2 = new List<Model_PriceAndDayofWeed>();
        //                    List<Model_PriceAndDayofWeed> PriceStuff3 = new List<Model_PriceAndDayofWeed>();
        //                    List<Model_PriceAndDayofWeed> PriceStuff4 = new List<Model_PriceAndDayofWeed>();
        //                    List<Model_PriceAndDayofWeed> PriceStuff5 = new List<Model_PriceAndDayofWeed>();
        //                    List<Model_PriceAndDayofWeed> PriceStuff6 = new List<Model_PriceAndDayofWeed>();
        //                    List<Model_PriceAndDayofWeed> PriceStuff7 = new List<Model_PriceAndDayofWeed>();
        //                    List<Model_PriceAndDayofWeed> PriceStuffCn = new List<Model_PriceAndDayofWeed>();
        //                    // lấy ra số lượng khuyến mại tương ứng với ngày khuyến mãi 
        //                    foreach (DateTime day in EachDay(fromDate, toDate)) // đang fixx
        //                    {
        //                        if (day.DayOfWeek.ToString() == "Monday")
        //                        {
        //                            var priceMonday = PriceAndDayofWeeds.Where(a => a.Monday == true).ToList();
        //                            PriceStuff2 = priceMonday;
        //                        }
        //                        if (day.DayOfWeek.ToString() == "Tuesday")
        //                        {
        //                            var priceTuesday = PriceAndDayofWeeds.Where(a => a.Tuesday == true).ToList();
        //                            PriceStuff3 = priceTuesday;
        //                        }
        //                        if (day.DayOfWeek.ToString() == "Wednesday")
        //                        {
        //                            var priceWednesday = PriceAndDayofWeeds.Where(a => a.Wednesday == true).ToList();
        //                            PriceStuff4 = priceWednesday;
        //                        }
        //                        if (day.DayOfWeek.ToString() == "Thursday")
        //                        {
        //                            var priceThursday = PriceAndDayofWeeds.Where(a => a.Thursday == true).ToList();
        //                            PriceStuff5 = priceThursday;
        //                        }
        //                        if (day.DayOfWeek.ToString() == "Friday")
        //                        {
        //                            var priceFriday = PriceAndDayofWeeds.Where(a => a.Friday == true).ToList();
        //                            PriceStuff6 = priceFriday;
        //                        }
        //                        if (day.DayOfWeek.ToString() == "Saturday")
        //                        {
        //                            var priceSaturday = PriceAndDayofWeeds.Where(a => a.Saturday == true).ToList();
        //                            PriceStuff7 = priceSaturday;
        //                        }
        //                        if (day.DayOfWeek.ToString() == "Sunday")
        //                        {
        //                            var priceSunday = PriceAndDayofWeeds.Where(a => a.Sunday == true).ToList();
        //                            PriceStuffCn = priceSunday;
        //                        }
        //                    }
        //                    List<TotalPriceAfterPromotion> PriceStuff = new List<TotalPriceAfterPromotion>();
        //                    if (PriceStuff2.Count > 0)
        //                    {
        //                        TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
        //                        {
        //                            Model_PriceAndDayofWeed = PriceStuff2
        //                        };
        //                        PriceStuff.Add(PriceSt);
        //                    }
        //                    if (PriceStuff3.Count > 0)
        //                    {
        //                        TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
        //                        {
        //                            Model_PriceAndDayofWeed = PriceStuff3
        //                        };
        //                        PriceStuff.Add(PriceSt);
        //                    }
        //                    if (PriceStuff4.Count > 0)
        //                    {
        //                        TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
        //                        {
        //                            Model_PriceAndDayofWeed = PriceStuff4
        //                        };
        //                        PriceStuff.Add(PriceSt);
        //                    }
        //                    if (PriceStuff5.Count > 0)
        //                    {
        //                        TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
        //                        {
        //                            Model_PriceAndDayofWeed = PriceStuff5
        //                        };
        //                        PriceStuff.Add(PriceSt);
        //                    }
        //                    if (PriceStuff6.Count > 0)
        //                    {
        //                        TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
        //                        {
        //                            Model_PriceAndDayofWeed = PriceStuff6
        //                        };
        //                        PriceStuff.Add(PriceSt);
        //                    }
        //                    if (PriceStuff7.Count > 0)
        //                    {
        //                        TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
        //                        {
        //                            Model_PriceAndDayofWeed = PriceStuff7
        //                        };
        //                        PriceStuff.Add(PriceSt);
        //                    }
        //                    if (PriceStuffCn.Count > 0)
        //                    {
        //                        TotalPriceAfterPromotion PriceSt = new TotalPriceAfterPromotion
        //                        {
        //                            Model_PriceAndDayofWeed = PriceStuffCn
        //                        };
        //                        PriceStuff.Add(PriceSt);
        //                    }
        //                    List<Promotion_Home> Promotion_Home = new List<Promotion_Home>();

        //                    foreach (var i0 in PriceStuff[0].Model_PriceAndDayofWeed)
        //                    {
        //                        float TotalPriceAfterPromotion0 = i0.TotalPriceAfterPromotionFriday + i0.TotalPriceAfterPromotionMonday + i0.TotalPriceAfterPromotionSaturday + i0.TotalPriceAfterPromotionSunday
        //                             + i0.TotalPriceAfterPromotionThursday + i0.TotalPriceAfterPromotionTuesday + i0.TotalPriceAfterPromotionWednesday;
        //                        float TotalPrice0 = i0.PriceFriday + i0.PriceMonday + i0.PriceSaturday + i0.PriceSunday
        //                             + i0.PriceThursday + i0.PriceTuesday + i0.PriceWednesday;
        //                        if (PriceStuff.Count == 1)
        //                        {
        //                            i0.PromotionHome.TotalPriceAfterPromotion = TotalPriceAfterPromotion0;
        //                            i0.PromotionHome.TotalPrice = TotalPrice0;
        //                            Promotion_Home.Add(i0.PromotionHome);
        //                        }
        //                        if (PriceStuff.Count >= 2)
        //                        {
        //                            foreach (var i1 in PriceStuff[1].Model_PriceAndDayofWeed)
        //                            {
        //                                Promotion_Home TotalPriceAfterPromotion1 = new Promotion_Home();
        //                                TotalPriceAfterPromotion1.TotalPriceAfterPromotion = TotalPriceAfterPromotion0 + i1.TotalPriceAfterPromotionFriday + i1.TotalPriceAfterPromotionMonday + i1.TotalPriceAfterPromotionSaturday + i1.TotalPriceAfterPromotionSunday
        //                                + i1.TotalPriceAfterPromotionThursday + i1.TotalPriceAfterPromotionTuesday + i1.TotalPriceAfterPromotionWednesday;
        //                                TotalPriceAfterPromotion1.TotalPrice = TotalPrice0 + i1.PriceFriday + i1.PriceMonday + i1.PriceSaturday + i1.PriceSunday
        //                                     + i1.PriceThursday + i1.PriceTuesday + i1.PriceWednesday;
        //                                if (PriceStuff.Count == 2)
        //                                {
        //                                    TotalPriceAfterPromotion1.AmountRate = i1.PromotionHome.AmountRate;
        //                                    TotalPriceAfterPromotion1.Breakfast = i1.PromotionHome.Breakfast;
        //                                    TotalPriceAfterPromotion1.ChooseRoom = i1.PromotionHome.ChooseRoom;
        //                                    TotalPriceAfterPromotion1.DayInHouse = i1.PromotionHome.DayInHouse;
        //                                    TotalPriceAfterPromotion1.Deposit = i1.PromotionHome.Deposit;
        //                                    TotalPriceAfterPromotion1.Description = i1.PromotionHome.Description;
        //                                    TotalPriceAfterPromotion1.Dinner = i1.PromotionHome.Dinner;
        //                                    TotalPriceAfterPromotion1.EarlyDay = i1.PromotionHome.EarlyDay;
        //                                    TotalPriceAfterPromotion1.Lunch = i1.PromotionHome.Lunch;
        //                                    TotalPriceAfterPromotion1.Name = i1.PromotionHome.Name;
        //                                    TotalPriceAfterPromotion1.NightForFreeNight = i1.PromotionHome.NightForFreeNight;
        //                                    TotalPriceAfterPromotion1.Note = i1.PromotionHome.Note;
        //                                    TotalPriceAfterPromotion1.PlaneRateId = i1.PromotionHome.PlaneRateId;
        //                                    TotalPriceAfterPromotion1.PolicyContent = i1.PromotionHome.PolicyContent;
        //                                    TotalPriceAfterPromotion1.PolicyId = i1.PromotionHome.PolicyId;
        //                                    TotalPriceAfterPromotion1.Price = i1.PromotionHome.Price;
        //                                    TotalPriceAfterPromotion1.PriceExchangeRate = i1.PromotionHome.PriceExchangeRate;
        //                                    TotalPriceAfterPromotion1.PromotionId = i1.PromotionHome.PromotionId;
        //                                    TotalPriceAfterPromotion1.PromotionName = i1.PromotionHome.PromotionName;
        //                                    TotalPriceAfterPromotion1.TotalPriceAfterPromotionExchangeRate = i1.PromotionHome.TotalPriceAfterPromotionExchangeRate;
        //                                    TotalPriceAfterPromotion1.TypePromotion = i1.PromotionHome.TypePromotion;
        //                                    Promotion_Home.Add(TotalPriceAfterPromotion1);
        //                                }
        //                                if (PriceStuff.Count >= 3)
        //                                {
        //                                    foreach (var i2 in PriceStuff[2].Model_PriceAndDayofWeed)
        //                                    {
        //                                        Promotion_Home TotalPriceAfterPromotion2 = new Promotion_Home();
        //                                        TotalPriceAfterPromotion2.TotalPriceAfterPromotion = TotalPriceAfterPromotion1.TotalPriceAfterPromotion + i2.TotalPriceAfterPromotionFriday + i2.TotalPriceAfterPromotionMonday + i2.TotalPriceAfterPromotionSaturday + i2.TotalPriceAfterPromotionSunday
        //                                        + i2.TotalPriceAfterPromotionThursday + i2.TotalPriceAfterPromotionTuesday + i2.TotalPriceAfterPromotionWednesday;
        //                                        TotalPriceAfterPromotion2.TotalPrice = TotalPriceAfterPromotion1.TotalPrice + i2.PriceFriday + i2.PriceMonday + i2.PriceSaturday + i2.PriceSunday
        //                                             + i2.PriceThursday + i2.PriceTuesday + i2.PriceWednesday;
        //                                        if (PriceStuff.Count == 3)
        //                                        {
        //                                            TotalPriceAfterPromotion2.AmountRate = i2.PromotionHome.AmountRate;
        //                                            TotalPriceAfterPromotion2.Breakfast = i2.PromotionHome.Breakfast;
        //                                            TotalPriceAfterPromotion2.ChooseRoom = i2.PromotionHome.ChooseRoom;
        //                                            TotalPriceAfterPromotion2.DayInHouse = i2.PromotionHome.DayInHouse;
        //                                            TotalPriceAfterPromotion2.Deposit = i2.PromotionHome.Deposit;
        //                                            TotalPriceAfterPromotion2.Description = i2.PromotionHome.Description;
        //                                            TotalPriceAfterPromotion2.Dinner = i2.PromotionHome.Dinner;
        //                                            TotalPriceAfterPromotion2.EarlyDay = i2.PromotionHome.EarlyDay;
        //                                            TotalPriceAfterPromotion2.Lunch = i2.PromotionHome.Lunch;
        //                                            TotalPriceAfterPromotion2.Name = i2.PromotionHome.Name;
        //                                            TotalPriceAfterPromotion2.NightForFreeNight = i2.PromotionHome.NightForFreeNight;
        //                                            TotalPriceAfterPromotion2.Note = i2.PromotionHome.Note;
        //                                            TotalPriceAfterPromotion2.PlaneRateId = i2.PromotionHome.PlaneRateId;
        //                                            TotalPriceAfterPromotion2.PolicyContent = i2.PromotionHome.PolicyContent;
        //                                            TotalPriceAfterPromotion2.PolicyId = i2.PromotionHome.PolicyId;
        //                                            TotalPriceAfterPromotion2.Price = i2.PromotionHome.Price;
        //                                            TotalPriceAfterPromotion2.PriceExchangeRate = i2.PromotionHome.PriceExchangeRate;
        //                                            TotalPriceAfterPromotion2.PromotionId = i2.PromotionHome.PromotionId;
        //                                            TotalPriceAfterPromotion2.PromotionName = i2.PromotionHome.PromotionName;
        //                                            TotalPriceAfterPromotion2.TotalPriceAfterPromotionExchangeRate = i2.PromotionHome.TotalPriceAfterPromotionExchangeRate;
        //                                            TotalPriceAfterPromotion2.TypePromotion = i2.PromotionHome.TypePromotion;
        //                                            Promotion_Home.Add(TotalPriceAfterPromotion2);
        //                                        }
        //                                        if (PriceStuff.Count >= 4)
        //                                        {
        //                                            foreach (var i3 in PriceStuff[3].Model_PriceAndDayofWeed)
        //                                            {
        //                                                Promotion_Home TotalPriceAfterPromotion3 = new Promotion_Home();
        //                                                TotalPriceAfterPromotion3.TotalPriceAfterPromotion = TotalPriceAfterPromotion2.TotalPriceAfterPromotion + i3.TotalPriceAfterPromotionFriday + i3.TotalPriceAfterPromotionMonday + i3.TotalPriceAfterPromotionSaturday + i3.TotalPriceAfterPromotionSunday
        //                                                + i3.TotalPriceAfterPromotionThursday + i3.TotalPriceAfterPromotionTuesday + i3.TotalPriceAfterPromotionWednesday;
        //                                                TotalPriceAfterPromotion3.TotalPrice = TotalPriceAfterPromotion2.TotalPrice + i3.PriceFriday + i3.PriceMonday + i3.PriceSaturday + i3.PriceSunday
        //                                                     + i3.PriceThursday + i3.PriceTuesday + i3.PriceWednesday;
        //                                                if (PriceStuff.Count == 4)
        //                                                {
        //                                                    TotalPriceAfterPromotion3.AmountRate = i3.PromotionHome.AmountRate;
        //                                                    TotalPriceAfterPromotion3.Breakfast = i3.PromotionHome.Breakfast;
        //                                                    TotalPriceAfterPromotion3.ChooseRoom = i3.PromotionHome.ChooseRoom;
        //                                                    TotalPriceAfterPromotion3.DayInHouse = i3.PromotionHome.DayInHouse;
        //                                                    TotalPriceAfterPromotion3.Deposit = i3.PromotionHome.Deposit;
        //                                                    TotalPriceAfterPromotion3.Description = i3.PromotionHome.Description;
        //                                                    TotalPriceAfterPromotion3.Dinner = i3.PromotionHome.Dinner;
        //                                                    TotalPriceAfterPromotion3.EarlyDay = i3.PromotionHome.EarlyDay;
        //                                                    TotalPriceAfterPromotion3.Lunch = i3.PromotionHome.Lunch;
        //                                                    TotalPriceAfterPromotion3.Name = i3.PromotionHome.Name;
        //                                                    TotalPriceAfterPromotion3.NightForFreeNight = i3.PromotionHome.NightForFreeNight;
        //                                                    TotalPriceAfterPromotion3.Note = i3.PromotionHome.Note;
        //                                                    TotalPriceAfterPromotion3.PlaneRateId = i3.PromotionHome.PlaneRateId;
        //                                                    TotalPriceAfterPromotion3.PolicyContent = i3.PromotionHome.PolicyContent;
        //                                                    TotalPriceAfterPromotion3.PolicyId = i3.PromotionHome.PolicyId;
        //                                                    TotalPriceAfterPromotion3.Price = i3.PromotionHome.Price;
        //                                                    TotalPriceAfterPromotion3.PriceExchangeRate = i3.PromotionHome.PriceExchangeRate;
        //                                                    TotalPriceAfterPromotion3.PromotionId = i3.PromotionHome.PromotionId;
        //                                                    TotalPriceAfterPromotion3.PromotionName = i3.PromotionHome.PromotionName;
        //                                                    TotalPriceAfterPromotion3.TotalPriceAfterPromotionExchangeRate = i3.PromotionHome.TotalPriceAfterPromotionExchangeRate;
        //                                                    TotalPriceAfterPromotion3.TypePromotion = i3.PromotionHome.TypePromotion;
        //                                                    Promotion_Home.Add(TotalPriceAfterPromotion3);
        //                                                }
        //                                                if (PriceStuff.Count >= 5)
        //                                                {

        //                                                    foreach (var i4 in PriceStuff[4].Model_PriceAndDayofWeed)
        //                                                    {
        //                                                        Promotion_Home TotalPriceAfterPromotion4 = new Promotion_Home();
        //                                                        TotalPriceAfterPromotion4.TotalPriceAfterPromotion = TotalPriceAfterPromotion3.TotalPriceAfterPromotion + i4.TotalPriceAfterPromotionFriday + i4.TotalPriceAfterPromotionMonday + i4.TotalPriceAfterPromotionSaturday + i4.TotalPriceAfterPromotionSunday
        //                                                        + i4.TotalPriceAfterPromotionThursday + i4.TotalPriceAfterPromotionTuesday + i4.TotalPriceAfterPromotionWednesday;
        //                                                        TotalPriceAfterPromotion4.TotalPrice = TotalPriceAfterPromotion3.TotalPrice + i4.PriceFriday + i4.PriceMonday + i4.PriceSaturday + i4.PriceSunday
        //                                                             + i4.PriceThursday + i4.PriceTuesday + i4.PriceWednesday;
        //                                                        if (PriceStuff.Count == 5)
        //                                                        {
        //                                                            TotalPriceAfterPromotion4.AmountRate = i4.PromotionHome.AmountRate;
        //                                                            TotalPriceAfterPromotion4.Breakfast = i4.PromotionHome.Breakfast;
        //                                                            TotalPriceAfterPromotion4.ChooseRoom = i4.PromotionHome.ChooseRoom;
        //                                                            TotalPriceAfterPromotion4.DayInHouse = i4.PromotionHome.DayInHouse;
        //                                                            TotalPriceAfterPromotion4.Deposit = i4.PromotionHome.Deposit;
        //                                                            TotalPriceAfterPromotion4.Description = i4.PromotionHome.Description;
        //                                                            TotalPriceAfterPromotion4.Dinner = i4.PromotionHome.Dinner;
        //                                                            TotalPriceAfterPromotion4.EarlyDay = i4.PromotionHome.EarlyDay;
        //                                                            TotalPriceAfterPromotion4.Lunch = i4.PromotionHome.Lunch;
        //                                                            TotalPriceAfterPromotion4.Name = i4.PromotionHome.Name;
        //                                                            TotalPriceAfterPromotion4.NightForFreeNight = i4.PromotionHome.NightForFreeNight;
        //                                                            TotalPriceAfterPromotion4.Note = i4.PromotionHome.Note;
        //                                                            TotalPriceAfterPromotion4.PlaneRateId = i4.PromotionHome.PlaneRateId;
        //                                                            TotalPriceAfterPromotion4.PolicyContent = i4.PromotionHome.PolicyContent;
        //                                                            TotalPriceAfterPromotion4.PolicyId = i4.PromotionHome.PolicyId;
        //                                                            TotalPriceAfterPromotion4.Price = i4.PromotionHome.Price;
        //                                                            TotalPriceAfterPromotion4.PriceExchangeRate = i4.PromotionHome.PriceExchangeRate;
        //                                                            TotalPriceAfterPromotion4.PromotionId = i4.PromotionHome.PromotionId;
        //                                                            TotalPriceAfterPromotion4.PromotionName = i4.PromotionHome.PromotionName;
        //                                                            TotalPriceAfterPromotion4.TotalPriceAfterPromotionExchangeRate = i4.PromotionHome.TotalPriceAfterPromotionExchangeRate;
        //                                                            TotalPriceAfterPromotion4.TypePromotion = i4.PromotionHome.TypePromotion;
        //                                                            Promotion_Home.Add(TotalPriceAfterPromotion4);
        //                                                        }
        //                                                        if (PriceStuff.Count >= 6)
        //                                                        {
        //                                                            foreach (var i5 in PriceStuff[5].Model_PriceAndDayofWeed)
        //                                                            {
        //                                                                Promotion_Home TotalPriceAfterPromotion5 = new Promotion_Home();
        //                                                                TotalPriceAfterPromotion5.TotalPriceAfterPromotion = TotalPriceAfterPromotion4.TotalPriceAfterPromotion + i5.TotalPriceAfterPromotionFriday + i5.TotalPriceAfterPromotionMonday + i5.TotalPriceAfterPromotionSaturday + i5.TotalPriceAfterPromotionSunday
        //                                                                + i5.TotalPriceAfterPromotionThursday + i5.TotalPriceAfterPromotionTuesday + i5.TotalPriceAfterPromotionWednesday;
        //                                                                TotalPriceAfterPromotion5.TotalPrice = TotalPriceAfterPromotion4.TotalPrice + i5.PriceFriday + i5.PriceMonday + i5.PriceSaturday + i5.PriceSunday
        //                                                                     + i5.PriceThursday + i5.PriceTuesday + i5.PriceWednesday;
        //                                                                if (PriceStuff.Count == 6)
        //                                                                {
        //                                                                    TotalPriceAfterPromotion5.AmountRate = i5.PromotionHome.AmountRate;
        //                                                                    TotalPriceAfterPromotion5.Breakfast = i5.PromotionHome.Breakfast;
        //                                                                    TotalPriceAfterPromotion5.ChooseRoom = i5.PromotionHome.ChooseRoom;
        //                                                                    TotalPriceAfterPromotion5.DayInHouse = i5.PromotionHome.DayInHouse;
        //                                                                    TotalPriceAfterPromotion5.Deposit = i5.PromotionHome.Deposit;
        //                                                                    TotalPriceAfterPromotion5.Description = i5.PromotionHome.Description;
        //                                                                    TotalPriceAfterPromotion5.Dinner = i5.PromotionHome.Dinner;
        //                                                                    TotalPriceAfterPromotion5.EarlyDay = i5.PromotionHome.EarlyDay;
        //                                                                    TotalPriceAfterPromotion5.Lunch = i5.PromotionHome.Lunch;
        //                                                                    TotalPriceAfterPromotion5.Name = i5.PromotionHome.Name;
        //                                                                    TotalPriceAfterPromotion5.NightForFreeNight = i5.PromotionHome.NightForFreeNight;
        //                                                                    TotalPriceAfterPromotion5.Note = i5.PromotionHome.Note;
        //                                                                    TotalPriceAfterPromotion5.PlaneRateId = i5.PromotionHome.PlaneRateId;
        //                                                                    TotalPriceAfterPromotion5.PolicyContent = i5.PromotionHome.PolicyContent;
        //                                                                    TotalPriceAfterPromotion5.PolicyId = i5.PromotionHome.PolicyId;
        //                                                                    TotalPriceAfterPromotion5.Price = i5.PromotionHome.Price;
        //                                                                    TotalPriceAfterPromotion5.PriceExchangeRate = i5.PromotionHome.PriceExchangeRate;
        //                                                                    TotalPriceAfterPromotion5.PromotionId = i5.PromotionHome.PromotionId;
        //                                                                    TotalPriceAfterPromotion5.PromotionName = i5.PromotionHome.PromotionName;
        //                                                                    TotalPriceAfterPromotion5.TotalPriceAfterPromotionExchangeRate = i5.PromotionHome.TotalPriceAfterPromotionExchangeRate;
        //                                                                    TotalPriceAfterPromotion5.TypePromotion = i5.PromotionHome.TypePromotion;
        //                                                                    Promotion_Home.Add(TotalPriceAfterPromotion5);
        //                                                                }
        //                                                                if (PriceStuff.Count >= 7)
        //                                                                {
        //                                                                    foreach (var i6 in PriceStuff[6].Model_PriceAndDayofWeed)
        //                                                                    {
        //                                                                        Promotion_Home TotalPriceAfterPromotion6 = new Promotion_Home();
        //                                                                        TotalPriceAfterPromotion6.TotalPriceAfterPromotion = TotalPriceAfterPromotion5.TotalPriceAfterPromotion + i6.TotalPriceAfterPromotionFriday + i6.TotalPriceAfterPromotionMonday + i6.TotalPriceAfterPromotionSaturday + i6.TotalPriceAfterPromotionSunday
        //                                                                        + i6.TotalPriceAfterPromotionThursday + i6.TotalPriceAfterPromotionTuesday + i6.TotalPriceAfterPromotionWednesday;
        //                                                                        TotalPriceAfterPromotion6.TotalPrice = TotalPriceAfterPromotion5.TotalPrice + i6.PriceFriday + i6.PriceMonday + i6.PriceSaturday + i6.PriceSunday
        //                                                                             + i6.PriceThursday + i6.PriceTuesday + i6.PriceWednesday;
        //                                                                        if (PriceStuff.Count == 7)
        //                                                                        {
        //                                                                            TotalPriceAfterPromotion6.AmountRate = i6.PromotionHome.AmountRate;
        //                                                                            TotalPriceAfterPromotion6.Breakfast = i6.PromotionHome.Breakfast;
        //                                                                            TotalPriceAfterPromotion6.ChooseRoom = i6.PromotionHome.ChooseRoom;
        //                                                                            TotalPriceAfterPromotion6.DayInHouse = i6.PromotionHome.DayInHouse;
        //                                                                            TotalPriceAfterPromotion6.Deposit = i6.PromotionHome.Deposit;
        //                                                                            TotalPriceAfterPromotion6.Description = i6.PromotionHome.Description;
        //                                                                            TotalPriceAfterPromotion6.Dinner = i6.PromotionHome.Dinner;
        //                                                                            TotalPriceAfterPromotion6.EarlyDay = i6.PromotionHome.EarlyDay;
        //                                                                            TotalPriceAfterPromotion6.Lunch = i6.PromotionHome.Lunch;
        //                                                                            TotalPriceAfterPromotion6.Name = i6.PromotionHome.Name;
        //                                                                            TotalPriceAfterPromotion6.NightForFreeNight = i6.PromotionHome.NightForFreeNight;
        //                                                                            TotalPriceAfterPromotion6.Note = i6.PromotionHome.Note;
        //                                                                            TotalPriceAfterPromotion6.PlaneRateId = i6.PromotionHome.PlaneRateId;
        //                                                                            TotalPriceAfterPromotion6.PolicyContent = i6.PromotionHome.PolicyContent;
        //                                                                            TotalPriceAfterPromotion6.PolicyId = i6.PromotionHome.PolicyId;
        //                                                                            TotalPriceAfterPromotion6.Price = i6.PromotionHome.Price;
        //                                                                            TotalPriceAfterPromotion6.PriceExchangeRate = i6.PromotionHome.PriceExchangeRate;
        //                                                                            TotalPriceAfterPromotion6.PromotionId = i6.PromotionHome.PromotionId;
        //                                                                            TotalPriceAfterPromotion6.PromotionName = i6.PromotionHome.PromotionName;
        //                                                                            TotalPriceAfterPromotion6.TotalPriceAfterPromotionExchangeRate = i6.PromotionHome.TotalPriceAfterPromotionExchangeRate;
        //                                                                            TotalPriceAfterPromotion6.TypePromotion = i6.PromotionHome.TypePromotion;
        //                                                                            Promotion_Home.Add(TotalPriceAfterPromotion6);
        //                                                                        }

        //                                                                    }
        //                                                                }
        //                                                            }
        //                                                        }

        //                                                    }
        //                                                }

        //                                            }
        //                                        }

        //                                    }
        //                                }
        //                            }
        //                        }

        //                    }

        //                    Promotion_Home.ForEach(z =>
        //                    {
        //                        z.TotalPriceAfterPromotionExchangeRate = ConvertCurrency(z.TotalPriceAfterPromotion, configCurrency.Result);
        //                        x.TotalPriceExchangeRate = ConvertCurrency(z.TotalPrice, configCurrency.Result);
        //                        x.PromotionHome = z; // gán promotion vào roomtype
        //                            x.TotalPrice = z.TotalPrice;
        //                        roomTypesResult.Add((RoomType_Home)x.Clone());
        //                    });

        //                    #endregion
        //                }
        //            }
        //        });

        //        roomTypesResult.OrderBy(x => x.PromotionHome.Price);
        //        var result = roomTypesResult.GroupBy(g => g.PromotionHome.TotalPriceAfterPromotion)
        //           .Select(grp => grp.First())
        //           .ToList();
        //        return JsonConvert.SerializeObject(result);
        //    }
        //}

            

        public JsonResult DetailRoomType(int id)
        {
            using (var connection = DB.ConnectionFactory())
            {
                int LanguageId = (int)Session["languageIdClient"];
                using (var multi = connection.QueryMultiple("RoomType_GetDetail_Client",
                    new
                    {
                        RoomTypeId = id,
                        LanguageId = LanguageId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<string> amenity = multi.Read<string>().ToList();
                    RoomType_Home roomType = multi.Read<RoomType_Home>().SingleOrDefault();
                    List<string> gallary = multi.Read<string>().ToList();
                    if (amenity is null) amenity = new List<string>();
                    if (roomType is null) roomType = new RoomType_Home();
                    if (gallary is null) gallary = new List<string>();
                    return Json(new
                    {
                        amenity = amenity,
                        roomType = roomType,
                        gallary = gallary
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult SaveInfoBooking(List<RoomType_Home> data, ParamsQuery param)
        {
            Session["DataBooking"] = data;
            Session["Params"] = param;
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDataScreenConfirm()
        {
            List<RoomType_Home> data = new List<RoomType_Home>();
            List<RoomType_Home> dataNotRepeat = new List<RoomType_Home>();
            ParamsQuery param = new ParamsQuery();

            if (Session["DataBooking"] != null)
                data = (List<RoomType_Home>)Session["DataBooking"];
            if (Session["Params"] != null)
                param = (ParamsQuery)Session["Params"];
            data.ForEach(x =>
            {
                if (dataNotRepeat.FindIndex(y => y.RoomTypeId == x.RoomTypeId) < 0)
                    dataNotRepeat.Add(x);
            });
            using (var connection = DB.ConnectionFactory())
            {
                ConfigCurrency configCurrency = connection.QuerySingleOrDefault<ConfigCurrency>("ConfigCurrency_GetExchangeRate",
                   new
                   {
                       HodelCode = param.hotelCode,
                       CurrencyCode = param.currency
                   }, commandType: CommandType.StoredProcedure);
                if (configCurrency.AutoCalculator)
                {
                    configCurrency.Result = DataHelper.CurrencyConvertor(param.currency);
                }
                // tính lại giá chuyển đổi theo loại tiền tệ
                if (param.currency != "VND")
                {
                    data.ForEach(x =>
                    {
                        x.TotalPriceExchangeRate = ConvertCurrency(x.TotalPrice, configCurrency.Result);
                        x.PromotionHome.TotalPriceAfterPromotionExchangeRate = ConvertCurrency(x.PromotionHome.TotalPriceAfterPromotion, configCurrency.Result);
                    });
                }

                List<Extrabed_Home> extrabeds = new List<Extrabed_Home>();
                List<Service_Home> services = connection.Query<Service_Home>("Service_GetBuyOnline",
                    new
                    {
                        HotelCode = param.hotelCode,
                        LanguageCode = param.lang
                    }, commandType: CommandType.StoredProcedure).ToList();
                List<TaxFee> taxFees = connection.Query<TaxFee>("TaxFee_GetBuyOnline",
                    new
                    {
                        HotelCode = param.hotelCode
                    }, commandType: CommandType.StoredProcedure).ToList();

                if (extrabeds is null) extrabeds = new List<Extrabed_Home>();
                if (services is null) services = new List<Service_Home>();
                if (taxFees is null) taxFees = new List<TaxFee>();



                // danh sách giường phụ lấy theo loại phòng. Yêu cầu danh sách loại phòng không được trùng lặp dữ liệu
                dataNotRepeat.ForEach(x =>
                {
                    List<Extrabed_Home> extrabedsClone = connection.Query<Extrabed_Home>("Extrabed_GetByRoomType",
                        new
                        {
                            RoomTypeId = x.RoomTypeId,
                            LanguageCode = param.lang
                        }, commandType: CommandType.StoredProcedure).ToList();
                    if (extrabedsClone is null) extrabedsClone = new List<Extrabed_Home>();
                    extrabeds.AddRange(extrabedsClone);
                });
                float vat = taxFees.Find(x => x.TypeTaxFee).Amount;
                float serviceCharge = taxFees.Find(x => !x.TypeTaxFee).Amount;
                Session["taxFees"] = (vat + serviceCharge);
                param.tax = vat;
                param.serviceCharge = serviceCharge;
                param.resultAmountForVoucher = 0;
                Session["Params"] = param;
                extrabeds.ForEach(x =>
                {
                    x.NumberChoose = 0;
                    x.PriceExchangeRate = ConvertCurrency(x.Price, configCurrency.Result);
                });
                services.ForEach(x =>
                {
                    x.NumberChoose = 0;
                    x.PriceExchangeRate = ConvertCurrency(x.Price, configCurrency.Result);
                });
                return Json(JsonConvert.SerializeObject(new
                {
                    data = data,
                    param = param,
                    services = services,
                    extrabeds = extrabeds,
                    vat = vat,
                    serviceCharge = serviceCharge
                }), JsonRequestBehavior.AllowGet);
            }
        }
        // voucher
        public JsonResult GetAmountForVoucher(string voucher, string hotelCode, List<Service_Home> services)
        {
            List<RoomType_Home> data = new List<RoomType_Home>();
            ParamsQuery param = new ParamsQuery();

            if (Session["DataBooking"] != null)
                data = (List<RoomType_Home>)Session["DataBooking"];
            if (Session["Params"] != null)
                param = (ParamsQuery)Session["Params"];
            using (var connection = DB.ConnectionFactory())
            {
                ConfigCurrency configCurrency = connection.QuerySingleOrDefault<ConfigCurrency>("ConfigCurrency_GetExchangeRate",
                   new
                   {
                       HodelCode = param.hotelCode,
                       CurrencyCode = param.currency
                   }, commandType: CommandType.StoredProcedure);
                if (configCurrency.AutoCalculator)
                {
                    configCurrency.Result = DataHelper.CurrencyConvertor(param.currency);
                }
                // tính lại giá chuyển đổi theo loại tiền tệ
                if (param.currency != "VND")
                {
                    data.ForEach(x =>
                    {
                        x.TotalPriceExchangeRate = ConvertCurrency(x.TotalPrice, configCurrency.Result);
                        x.PromotionHome.TotalPriceAfterPromotionExchangeRate = ConvertCurrency(x.PromotionHome.TotalPriceAfterPromotion, configCurrency.Result);
                    });
                }

                using (var multi = connection.QueryMultiple("Voucher_CheckAcceptDeposit",
                    new
                    {
                        HotelCode = hotelCode,
                        VoucherCode = voucher,
                        Date = DatetimeHelper.DateTimeUTCNow()
                    }, commandType: CommandType.StoredProcedure))
                {
                    double amountForVoucher = 0;
                    int accept = multi.Read<int>().SingleOrDefault();
                    List<int> roomIdForDiscountByVoucher = new List<int>();
                    Voucher_Home voucherResult = new Voucher_Home();
                    if (accept == 1)
                    {
                        voucherResult = multi.Read<Voucher_Home>().SingleOrDefault();
                        List<int> roomtypeAccept = multi.Read<int>().ToList();
                        List<int> serviceAccept = multi.Read<int>().ToList();
                        if (roomtypeAccept is null) roomtypeAccept = new List<int>();
                        if (serviceAccept is null) serviceAccept = new List<int>();
                        if (services is null) services = new List<Service_Home>();
                        if (voucherResult.DiscountForService)
                        {
                            services.ForEach(x =>
                            {
                                if (serviceAccept.FindIndex(y => y == x.ServiceId) >= 0)
                                {
                                    if (param.currency == "VND")
                                        amountForVoucher += x.Price * x.NumberChoose * voucherResult.AmountForService / 100;
                                    else
                                        amountForVoucher += x.PriceExchangeRate * x.NumberChoose * voucherResult.AmountForService / 100;
                                }
                            });
                        }
                        if (voucherResult.DiscountForRoom)
                        {
                            data.ForEach(x =>
                            {
                                // kiểm tra voucher có cho phép loại phòng này không
                                if (roomtypeAccept.FindIndex(y => y == x.RoomTypeId) >= 0)
                                {
                                    // tính tổng thể các giá sau promotion
                                    if (x.PromotionHome.ChooseRoom > 0)
                                    {
                                        if (param.currency == "VND")
                                            amountForVoucher += x.PromotionHome.TotalPriceAfterPromotion * x.PromotionHome.ChooseRoom * voucherResult.AmountForRoom / 100;
                                        else
                                            amountForVoucher += x.PromotionHome.TotalPriceAfterPromotionExchangeRate * x.PromotionHome.ChooseRoom * voucherResult.AmountForRoom / 100;
                                        roomIdForDiscountByVoucher.Add(x.RoomTypeId);
                                    }
                                }
                            });
                        }
                    }
                    double resultAmountForVoucher = param.currency == "VND" ? Math.Round(amountForVoucher, 0) : Math.Round(amountForVoucher, 2);
                    return Json(new
                    {
                        accept = accept,
                        amountForVoucher = resultAmountForVoucher,
                        roomIdForDiscountByVoucher = roomIdForDiscountByVoucher,
                        amountForRoom = voucherResult.AmountForRoom
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        // post booking
        public JsonResult Post(Guest_Home guest, List<Extrabed_Home> extrabeds, List<Service_Home> services, Voucher_Home voucher)
        {
            if (extrabeds is null) extrabeds = new List<Extrabed_Home>();
            if (services is null) services = new List<Service_Home>();
            if (guest is null) guest = new Guest_Home();

            DateTime datetimeNow = DatetimeHelper.DateTimeUTCNow();
            List<RoomType_Home> data = new List<RoomType_Home>();
            ParamsQuery param = new ParamsQuery();

            if (Session["DataBooking"] != null)
                data = (List<RoomType_Home>)Session["DataBooking"];
            if (Session["Params"] != null)
                param = (ParamsQuery)Session["Params"];
            int resultPost = 0; // 1: VTC, 0: Offline, 2:OnePay, 3:Paypal
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                int hotelId = connection.QuerySingleOrDefault<int>("Hotel_GetHotelId",
                    new
                    {
                        HotelCode = param.hotelCode
                    }, commandType: CommandType.StoredProcedure);
                using (var transaction = connection.BeginTransaction())
                {
                    #region add new guest for booking
                    int guestId = connection.QuerySingleOrDefault<int>("Guest_Post",
                        new
                        {
                            HotelId = hotelId,
                            FirstName = guest.FirstName,
                            SurName = guest.SurName,
                            Photo = "",
                            TypeguestId = 0,
                            ZIPCode = "",
                            Company = "",
                            Gender = -1,
                            Dob = "",
                            Region = "",
                            Country = guest.Country,
                            IdentityCart = "",
                            Passport = "",
                            CreditCard = "",
                            DoIssueCreditCard = "",
                            CVC = DataHelper.Encrypt(""),
                            Phone = guest.Phone,
                            Fax = "",
                            Email = guest.Email,
                            Address = guest.Address,
                            Note = guest.Note,
                            DateCreate = datetimeNow,
                            CreateBySource = "BookingEngine"
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    #endregion

                    #region post reservation
                    int reservationId = connection.QuerySingleOrDefault<int>("Reservation_Post",
                        new
                        {
                            HotelId = hotelId,
                            GuestId = guestId,
                            CompanyId = 0,
                            TypeReservation = 0,
                            ReminiscentName = "",
                            Color = "#80FF80",
                            Adult = param.adults,
                            Children = param.child,
                            Voucher = voucher.VoucherCode is null ? "" : voucher.VoucherCode,
                            BookingSource = 2,
                            ArrivalFlightDate = guest.ArrivalFlightDate,
                            ArrivalFlightTime = guest.ArrivalFlightTime,
                            DepartureFlightDate = "",
                            DepartureFlightTime = "",
                            CheckIn = param.fromDate.ToString("yyyy/MM/dd 14:00:00"),
                            CheckOut = param.toDate.ToString("yyyy/MM/dd 12:00:00"),
                            PaymentType = guest.TypePaymentMethod,
                            Deposit = 0,
                            CreateDate = datetimeNow
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    Session["reservationId"] = reservationId;
                    if (guest.TypePaymentMethod == 13 || guest.TypePaymentMethod == 14 || guest.TypePaymentMethod == 18)
                    {
                        float totalDeposit = 0;
                        float taxFees = 0;
                        if (Session["taxFees"] != null)
                        {
                            taxFees = (float)Session["taxFees"];
                        }
                        data.ForEach(x =>
                        {
                            if (x.PromotionHome.ChooseRoom > 0)
                            {
                                totalDeposit += x.PromotionHome.ChooseRoom * x.PromotionHome.TotalPriceAfterPromotion * x.PromotionHome.AmountRate / 100;
                            }
                        });
                        totalDeposit = totalDeposit * (100 + taxFees) / 100;
                        connection.Execute("ReservationDeposit_Post",
                            new
                            {
                                ReservationId = reservationId,
                                Deposit = (long)Math.Round(totalDeposit, 0)
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                        Session["dataPayOnline"] = new DataSavePayonline_Home(reservationId, (long)totalDeposit, guest.FirstName, guest.SurName, guest.Email);
                        if (totalDeposit > 0)
                        {
                            if (guest.TypePaymentMethod == 13)
                                resultPost = 1;
                            else if (guest.TypePaymentMethod == 14)
                                resultPost = 2;
                            else if (guest.TypePaymentMethod == 18)
                                resultPost = 3;
                        }
                    }
                    #endregion

                    #region post booking
                    // tách danh sách từ 1 loại phòng chọn nhiều thành nhiều phòng được chọn ( 1-n = n)
                    List<RoomType_Home> dataHorizontal = SplitRoomTypes(data);
                    // tự động gán số người, dịch vụ, extrabed cho danh sách phòng
                    AutoDiscountVoucherForRoomTypes(dataHorizontal, services, voucher.VoucherCode, param.hotelCode); // tính lại giá phòng, dịch vụ khi có voucher

                    AutoAsignPeopleToRoomTypes(dataHorizontal, param.adults, param.child);
                    AutoAsignExtrabedToRoomTypes(dataHorizontal, extrabeds);
                    AutoAsignServiceToRoomTypes(dataHorizontal, services);

                    dataHorizontal.ForEach(x =>
                    {
                        // tính số tiền trả trước
                        // Tất cả số tiền đã trả của các hình thức thanh toán ban đầu đều = 0. Nếu thanh toán trực tuyến
                        // THì sẽ cập nhật lại số tiền đã thanh toán tại method đã thanh toán.
                        double depositMoney = Math.Round(x.PromotionHome.TotalPriceAfterPromotion * x.PromotionHome.AmountRate / 100, 0);
                        #region post booking
                        int bookingId = connection.QuerySingleOrDefault<int>("Booking_Post",
                        new
                        {
                            TypeBooking = 0,
                            ReservationId = reservationId,
                            RoomTypeId = x.RoomTypeId,
                            RoomId = -1,
                            Adult = x.AdultBook,
                            Children = x.ChildrenBook,
                            ArrivalDate = param.fromDate.ToString("yyyy/MM/dd 14:00:00"),
                            DepartureDate = param.toDate.ToString("yyyy/MM/dd 12:00:00"),
                            GuestId = guestId,
                            TypeReservation = 0,
                            PaymentType = guest.TypePaymentMethod,
                            UserCreate = "System BookingEngine",
                            CreateDate = datetimeNow,
                            Prepaid = 0,
                            Discount = 0,
                            DepositPecent = -1,
                            DepositMonney = depositMoney,
                            IncludeVATAndServiceCharge = true
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                        if (x.ExtrabedHome is null)
                            x.ExtrabedHome = new List<Extrabed_Home>();
                        x.ExtrabedHome.ForEach(y =>
                        {
                            if (y.NumberChoose > 0)
                            {
                                connection.Execute("Booking_Extrabed_Post",
                                    new
                                    {
                                        BookingId = bookingId,
                                        ExtrabedId = y.ExtrabedId,
                                        Number = y.NumberChoose,
                                        Price = Math.Round(y.Price, 0),
                                        DateCreate = datetimeNow
                                    }, commandType: CommandType.StoredProcedure,
                                    transaction: transaction);
                            }
                        });

                        if (x.RateAvailabilities is null)
                            x.RateAvailabilities = new List<RateAvailability>();
                        x.RateAvailabilities.ForEach(y =>
                        {
                            connection.Execute("BookingPrice_Post",
                                new
                                {
                                    BookingId = bookingId,
                                    Date = y.Date,
                                    Price = Math.Round(y.PriceForAddClient, 0)
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                        if (x.ServiceHomes is null)
                            x.ServiceHomes = new List<Service_Home>();
                        x.ServiceHomes.ForEach(y =>
                        {
                            if (y.NumberChoose > 0)
                            {
                                connection.Execute("BookingService_Post",
                                    new
                                    {
                                        BookingId = bookingId,
                                        ServiceId = y.ServiceId,
                                        Number = y.NumberChoose,
                                        Price = Math.Round(y.PriceForAddClient, 0),
                                        DateCreate = datetimeNow
                                    }, commandType: CommandType.StoredProcedure,
                                    transaction: transaction);
                            }
                        });
                        connection.Execute("CardBooking_Put",
                            new
                            {
                                BookingId = bookingId,
                                Name = DataHelper.Encrypt(guest.Name),
                                Number = DataHelper.Encrypt(guest.Number),
                                Code = DataHelper.Encrypt(guest.Code),
                                ExpirationMonth = guest.ExpirationMonth,
                                ExpirationYear = guest.ExpirationYear
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                        if (guest.TypePaymentMethod == 13 || guest.TypePaymentMethod == 14 || guest.TypePaymentMethod == 18)
                        {
                            connection.Execute("ReservationDepositBooking_Post",
                                new
                                {
                                    ReservationId = reservationId,
                                    BookingId = bookingId,
                                    Paid = depositMoney
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        }
                        #endregion

                    });
                    #endregion

                    transaction.Commit();
                    AllDataBook_Home allDataBook = new AllDataBook_Home()
                    {
                        reservationId = reservationId,
                        data = dataHorizontal,
                        dataDefault = data,
                        extrabeds = extrabeds,
                        guest = guest,
                        param = param,
                        services = services,
                        voucher = voucher
                    };
                    Session["allDataBook"] = allDataBook;
                    Thread thread = new Thread(SendEmail);
                    thread.Start(allDataBook);
                }
            }
            string url = "";
            switch (resultPost)
            {
                case 0:
                    url = "/Home/BookSuccess";
                    break;
                case 1:
                    url = "/Home/SubmitInvoidVTCPay";
                    break;
                case 2:
                    url = "/Home/SubmitInvoidVTCPay";
                    break;
                case 3:
                    url = "/Home/SubmitInvoidPaypal";
                    break;
                default:
                    url = "/Home/BookError";
                    break;
            }
            return Json(url, JsonRequestBehavior.AllowGet);
        }
        private List<RoomType_Home> SplitRoomTypes(List<RoomType_Home> roomTypes)
        {
            if (roomTypes is null) roomTypes = new List<RoomType_Home>();
            List<RoomType_Home> roomTypesClone = new List<RoomType_Home>();
            roomTypes.ForEach(x =>
            {
                for (int i = 0; i < x.PromotionHome.ChooseRoom; i++)
                {
                    roomTypesClone.Add(JsonConvert.DeserializeObject<RoomType_Home>(JsonConvert.SerializeObject(x)));
                }
            });
            return roomTypesClone;
        }
        private void AutoDiscountVoucherForRoomTypes(List<RoomType_Home> roomTypes, List<Service_Home> services, string voucher, string hotelCode)
        {
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Voucher_CheckAcceptDeposit",
                    new
                    {
                        HotelCode = hotelCode,
                        VoucherCode = voucher,
                        Date = DatetimeHelper.DateTimeUTCNow()
                    }, commandType: CommandType.StoredProcedure))
                {
                    int accept = multi.Read<int>().SingleOrDefault();
                    if (accept == 1)
                    {
                        Voucher_Home voucherResult = multi.Read<Voucher_Home>().SingleOrDefault();
                        List<int> roomtypeAccept = multi.Read<int>().ToList();
                        List<int> serviceAccept = multi.Read<int>().ToList();
                        if (roomtypeAccept is null) roomtypeAccept = new List<int>();
                        if (serviceAccept is null) serviceAccept = new List<int>();
                        if (services is null) services = new List<Service_Home>();
                        if (voucherResult.DiscountForService)
                        {
                            services.ForEach(x =>
                            {
                                if (serviceAccept.FindIndex(y => y == x.ServiceId) >= 0)
                                {
                                    x.PriceForAddClient = x.Price * (100 - voucherResult.AmountForService) / 100;
                                }
                                else
                                {
                                    x.PriceForAddClient = x.Price;
                                }
                            });
                        }
                        if (voucherResult.DiscountForRoom)
                        {
                            roomTypes.ForEach(x =>
                            {
                                // kiểm tra voucher có cho phép loại phòng này không
                                if (roomtypeAccept.FindIndex(y => y == x.RoomTypeId) >= 0)
                                {
                                    // tính tổng thể các giá sau promotion
                                    if (x.PromotionHome.ChooseRoom > 0)
                                    {
                                        x.RateAvailabilities.ForEach(z =>
                                        {
                                            float total = z.Price * (100 - x.PromotionHome.Deposit) / 100 + x.PromotionHome.Price;
                                            z.PriceForAddClient = total * (100 - voucherResult.AmountForRoom) / 100;
                                        });
                                    }
                                    else
                                    {
                                        x.RateAvailabilities.ForEach(z =>
                                        {
                                            z.PriceForAddClient = (z.Price * (100 - x.PromotionHome.Deposit) / 100 + x.PromotionHome.Price);
                                        });
                                    }
                                }
                                else
                                {
                                    x.RateAvailabilities.ForEach(z =>
                                    {
                                        z.PriceForAddClient = (z.Price * (100 - x.PromotionHome.Deposit) / 100 + x.PromotionHome.Price);
                                    });
                                }
                            });
                        }
                        else
                        {
                            roomTypes.ForEach(x =>
                            {
                                x.RateAvailabilities.ForEach(z =>
                                {
                                    z.PriceForAddClient = z.Price * (100 - x.PromotionHome.Deposit) / 100 + x.PromotionHome.Price;
                                });
                            });
                        }
                    }
                    else
                    {
                        roomTypes.ForEach(x =>
                        {
                            x.RateAvailabilities.ForEach(z =>
                            {
                                z.PriceForAddClient = z.Price * (100 - x.PromotionHome.Deposit) / 100 + x.PromotionHome.Price;
                            });
                        });
                        services.ForEach(x =>
                        {
                            x.PriceForAddClient = x.Price;
                        });
                    }
                }
            }
        }
        private void AutoAsignPeopleToRoomTypes(List<RoomType_Home> roomTypes, int totalAdult, int totalChild)
        {
            if (roomTypes is null) roomTypes = new List<RoomType_Home>();
            int sumMaxChild = roomTypes.Sum(x => x.Children);
            if (totalChild > sumMaxChild)
            {
                totalAdult += totalChild - sumMaxChild;
                totalChild = sumMaxChild;
            }
            roomTypes.OrderByDescending(x => x.Adult);
            roomTypes.ForEach(x =>
            {
                if (totalChild > 0)
                {
                    if (totalChild > x.Children)
                    {
                        x.ChildrenBook = x.Children;
                        totalChild -= x.Children;
                    }
                    else
                    {
                        x.ChildrenBook = totalChild;
                        totalChild = 0;
                    }
                }
            });
            roomTypes.ForEach(x =>
            {
                if (totalAdult > 0)
                {
                    if (totalAdult > x.Adult)
                    {
                        x.AdultBook = x.Adult;
                        totalAdult -= x.Adult;
                    }
                    else
                    {
                        x.AdultBook = totalAdult;
                        totalAdult = 0;
                    }
                }
            });
        }
        private void AutoAsignExtrabedToRoomTypes(List<RoomType_Home> roomTypes, List<Extrabed_Home> extrabeds)
        {
            if (roomTypes is null) roomTypes = new List<RoomType_Home>();
            if (extrabeds is null) extrabeds = new List<Extrabed_Home>();
            if (roomTypes.Count > 0)
            {
                roomTypes[0].ExtrabedHome = extrabeds;
            }
        }
        private void AutoAsignServiceToRoomTypes(List<RoomType_Home> roomTypes, List<Service_Home> services)
        {
            if (roomTypes is null) roomTypes = new List<RoomType_Home>();
            if (services is null) services = new List<Service_Home>();
            if (roomTypes.Count > 0)
            {
                roomTypes[0].ServiceHomes = services;
            }

        }
        /// <summary>
        /// chuyển đổi tiền tệ.( 1USD = 23.000VND, => 56.000VND = 2USD)
        /// </summary>
        /// <param name="price">giá muốn chuyển đổi</param>
        /// <param name="init">tỷ giá hối đoái</param>
        /// <returns></returns>
        private float ConvertCurrency(double price, float init)
        {
            return (float)Math.Round(price / init, 2);
        }
        #endregion
        // vòng lặp cho ngày
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date < thru.Date; day = day.AddDays(1))
                yield return day;
        }
        // payment online
        #region Pay VTC
        public ActionResult SubmitInvoidVTCPay()
        {
            if (Session["dataPayOnline"] is null)
                return View("Error");
            ParamsQuery param = new ParamsQuery();
            DataSavePayonline_Home dataSavePayonline = new DataSavePayonline_Home();
            if (Session["Params"] != null)
                param = (ParamsQuery)Session["Params"];
            dataSavePayonline = (DataSavePayonline_Home)Session["dataPayOnline"];
            ConfigVTCPay configVTCPay = new ConfigVTCPay();
            using (var connection = DB.ConnectionFactory())
            {
                configVTCPay = connection.Query<ConfigVTCPay>("ConfigVTCPay_GetByHotelCode",
                    new
                    {
                        HotelCode = param.hotelCode
                    }, commandType: CommandType.StoredProcedure).SingleOrDefault();
                if (configVTCPay is null) configVTCPay = new ConfigVTCPay();
                configVTCPay.ReceiverAccount = DataHelper.Decrypt(configVTCPay.ReceiverAccount);
                configVTCPay.WebSite = DataHelper.Decrypt(configVTCPay.WebSite);
                configVTCPay.SecurityKey = DataHelper.Decrypt(configVTCPay.SecurityKey);
                configVTCPay.WebsiteId = DataHelper.Decrypt(configVTCPay.WebsiteId);
            }
            string WebSite = configVTCPay.WebSite;
            string currency = "VND";
            string reference_number = dataSavePayonline.ReservationId.ToString();
            string language = "vi";
            string url_return = WebSite + "/Home/MessageVTCPay";
            string bill_to_email = dataSavePayonline.Email is null ? "" : dataSavePayonline.Email;
            string bill_to_phone = "";
            string bill_to_address = "";
            string Security_Key = configVTCPay.SecurityKey;
            Dictionary<string, object> paramQueryList = new Dictionary<string, object>()
                {
                    {"website_id",  configVTCPay.WebsiteId},
                    {"amount",dataSavePayonline.Deposit}, //dataSavePayonline.Deposit
                    {"currency",currency}, //VND or USD
                    {"receiver_account", configVTCPay.ReceiverAccount}, //Tài khoản nhận
                    {"reference_number",reference_number},
                    {"transaction_type","sale"},
                    {"language",language},//vn or vi
                    {"url_return",url_return},// link tra ve
                    {"bill_to_email",bill_to_email},// email thanh toan cua khach hang
                    {"bill_to_phone",bill_to_phone},// so dien thoai thanh toan cua khach hang
                    {"bill_to_address",bill_to_address},// dia chi thanh toan cua khach hang
                    {"bill_to_surname",dataSavePayonline.SurName},// họ/ first name
                    {"bill_to_forename",dataSavePayonline.FirstName} // tên/last name        
                };

            string plaintext = string.Empty;
            string listparam = string.Empty;

            String[] sortedKeys = paramQueryList.Keys.ToArray();
            Array.Sort(sortedKeys);

            foreach (String key in sortedKeys)
            {
                plaintext += string.Format("{0}{1}", plaintext.Length > 0 ? "|" : string.Empty, paramQueryList[key]);
                if (new string[] { "url_return", "bill_to_surname", "bill_to_forename", "bill_to_address", "bill_to_address_city" }.Contains(key))
                    listparam += string.Format("{0}={1}&", key, Server.UrlEncode(paramQueryList[key].ToString()));
                else
                    listparam += string.Format("{0}={1}&", key, paramQueryList[key].ToString());
            }

            string textSign = string.Format("{0}|{1}", plaintext, Security_Key);
            string signature = SHA256encrypt(textSign);

            listparam = string.Format("{0}signature={1}", listparam, signature);

            string urlRedirect = string.Format("{0}?{1}", "https://vtcpay.vn/bank-gateway/checkout.html", listparam);//http://alpha1.vtcpay.vn/portalgateway/checkout.html

            Response.Redirect(urlRedirect, false);
            return View("Error");
        }
        [HttpGet]
        public ActionResult MessageVTCPay(string reference_number, string message = "", string status = "")
        {
            int state = 0;
            if (status.Trim() == "7" || status.Trim() == "1")
            {
                message = "Transaction was paid successfully";
                state = 1;
            }
            else
            {
                message = "Transaction was not paid successful";
                state = 3;
            }
            ViewBag.message = message;
            ViewBag.state = state;
            if (state == 1)
            {
                using (var connection = DB.ConnectionFactory())
                {
                    connection.Execute("UpdateReservationWhenPayOnline",
                        new
                        {
                            ReservationId = reference_number
                        }, commandType: CommandType.StoredProcedure);
                    return RedirectToAction("BookSuccess");
                }
            }
            return RedirectToAction("BookError");
        }
        private static string SHA256encrypt(string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256hasher = new SHA256Managed();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(phrase));
            return byteArrayToString(hashedDataBytes);
        }
        private static string byteArrayToString(byte[] inputArray)
        {
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
            {
                output.Append(inputArray[i].ToString("X2"));
            }
            return output.ToString();
        }
        #endregion
        #region Paypal
        public class DataSaveForPayPal
        {
            public Guest_Home guest { get; set; }
            public List<Extrabed_Home> extrabeds { get; set; }
            public List<Service_Home> services { get; set; }
            public Voucher_Home voucher { get; set; }
            public DataSaveForPayPal()
            {
                guest = new Guest_Home();
                voucher = new Voucher_Home();
            }
        }
        public JsonResult SaveDataForPayPal(Guest_Home guest, List<Extrabed_Home> extrabeds, List<Service_Home> services, Voucher_Home voucher)
        {
            Session["DataSavaPayPal"] = new DataSaveForPayPal()
            {
                extrabeds = extrabeds,
                guest = guest,
                services = services,
                voucher = voucher
            };
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        private PaymentPaypal GetConnectPaypal(string token, PaymentPaypal data)
        {
            var client = new RestClient("https://api.paypal.com/v1/payments/payment");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Postman-Token", "4d016f2e-cc83-4536-803e-d6245c0c5553");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("PayPal-Partner-Attribution-Id", "IITTechnology_STP_VN");
            request.AddParameter("undefined", JsonConvert.SerializeObject(data), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<PaymentPaypal>(response.Content);
        }
        private string PostBookingWhenPayByPayPal()
        {
            string currency = (string)Session["currency"];
            if (currency == "VND" || currency == "JPY")
                currency = "USD";
            // Lấy dữ liệu đã lưu từ client
            DataSaveForPayPal dataSaveForPayPal = (DataSaveForPayPal)Session["DataSavaPayPal"];
            if (dataSaveForPayPal is null) dataSaveForPayPal = new DataSaveForPayPal();
            Guest_Home guest = dataSaveForPayPal.guest;
            List<Extrabed_Home> extrabeds = dataSaveForPayPal.extrabeds;
            List<Service_Home> services = dataSaveForPayPal.services;
            Voucher_Home voucher = dataSaveForPayPal.voucher;
            if (extrabeds is null) extrabeds = new List<Extrabed_Home>();
            if (services is null) services = new List<Service_Home>();
            if (guest is null) guest = new Guest_Home();
            guest.TypePaymentMethod = 18;
            DateTime datetimeNow = DatetimeHelper.DateTimeUTCNow();
            List<RoomType_Home> data = new List<RoomType_Home>();
            ParamsQuery param = new ParamsQuery();
            // lấy danh sách đặt phòng
            if (Session["DataBooking"] != null)
                data = (List<RoomType_Home>)Session["DataBooking"];
            if (Session["Params"] != null)
                param = (ParamsQuery)Session["Params"];
            double totalDeposit = 0;
            double totalPriceRoom = 0;
            int reservationId = 0;
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                int hotelId = connection.QuerySingleOrDefault<int>("Hotel_GetHotelId",
                    new
                    {
                        HotelCode = param.hotelCode
                    }, commandType: CommandType.StoredProcedure);
                using (var transaction = connection.BeginTransaction())
                {
                    #region add new guest for booking
                    int guestId = connection.QuerySingleOrDefault<int>("Guest_Post",
                        new
                        {
                            HotelId = hotelId,
                            FirstName = guest.FirstName,
                            SurName = guest.SurName,
                            Photo = "",
                            TypeguestId = 0,
                            ZIPCode = "",
                            Company = "",
                            Gender = -1,
                            Dob = "",
                            Region = "",
                            Country = guest.Country,
                            IdentityCart = "",
                            Passport = "",
                            CreditCard = "",
                            DoIssueCreditCard = "",
                            CVC = DataHelper.Encrypt(""),
                            Phone = guest.Phone,
                            Fax = "",
                            Email = guest.Email,
                            Address = guest.Address,
                            Note = guest.Note,
                            DateCreate = datetimeNow,
                            CreateBySource = "BookingEngine"
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    #endregion
                    #region post reservation
                    reservationId = connection.QuerySingleOrDefault<int>("Reservation_Post",
                        new
                        {
                            HotelId = hotelId,
                            GuestId = guestId,
                            CompanyId = 0,
                            TypeReservation = 0,
                            ReminiscentName = "",
                            Color = "#80FF80",
                            Adult = param.adults,
                            Children = param.child,
                            Voucher = voucher.VoucherCode is null ? "" : voucher.VoucherCode,
                            BookingSource = 2,
                            ArrivalFlightDate = guest.ArrivalFlightDate,
                            ArrivalFlightTime = guest.ArrivalFlightTime,
                            DepartureFlightDate = "",
                            DepartureFlightTime = "",
                            CheckIn = param.fromDate.ToString("yyyy/MM/dd 14:00:00"),
                            CheckOut = param.toDate.ToString("yyyy/MM/dd 12:00:00"),
                            PaymentType = guest.TypePaymentMethod,
                            Deposit = 0,
                            CreateDate = datetimeNow
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    Session["reservationId"] = reservationId;
                    // tính toán số tiền trả trước
                    float taxFees = 0;
                    if (Session["taxFees"] != null)
                    {
                        taxFees = (float)Session["taxFees"];
                    }
                    data.ForEach(x =>
                    {
                        if (x.PromotionHome.ChooseRoom > 0)
                        {
                            totalDeposit += x.PromotionHome.ChooseRoom * x.PromotionHome.TotalPriceAfterPromotion * x.PromotionHome.AmountRate / 100;
                        }
                    });
                    totalDeposit = totalDeposit * (100 + taxFees) / 100;
                    connection.Execute("ReservationDeposit_Post",
                        new
                        {
                            ReservationId = reservationId,
                            Deposit = (long)Math.Round(totalDeposit, 0)
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    #endregion
                    #region post booking
                    // tách danh sách từ 1 loại phòng chọn nhiều thành nhiều phòng được chọn ( 1-n = n)
                    List<RoomType_Home> dataHorizontal = SplitRoomTypes(data);
                    // tự động gán số người, dịch vụ, extrabed cho danh sách phòng
                    AutoDiscountVoucherForRoomTypes(dataHorizontal, services, voucher.VoucherCode, param.hotelCode); // tính lại giá phòng, dịch vụ khi có voucher

                    AutoAsignPeopleToRoomTypes(dataHorizontal, param.adults, param.child);
                    AutoAsignExtrabedToRoomTypes(dataHorizontal, extrabeds);
                    AutoAsignServiceToRoomTypes(dataHorizontal, services);

                    dataHorizontal.ForEach(x =>
                    {
                        // tính số tiền trả trước
                        // Tất cả số tiền đã trả của các hình thức thanh toán ban đầu đều = 0. Nếu thanh toán trực tuyến
                        // THì sẽ cập nhật lại số tiền đã thanh toán tại method đã thanh toán.
                        double depositMoney = Math.Round(x.PromotionHome.TotalPriceAfterPromotion * x.PromotionHome.AmountRate * (100 + taxFees) / 10000, 0);
                        #region post booking
                        int bookingId = connection.QuerySingleOrDefault<int>("Booking_Post",
                        new
                        {
                            TypeBooking = 0,
                            ReservationId = reservationId,
                            RoomTypeId = x.RoomTypeId,
                            RoomId = -1,
                            Adult = x.AdultBook,
                            Children = x.ChildrenBook,
                            ArrivalDate = param.fromDate.ToString("yyyy/MM/dd 14:00:00"),
                            DepartureDate = param.toDate.ToString("yyyy/MM/dd 12:00:00"),
                            GuestId = guestId,
                            TypeReservation = 0,
                            PaymentType = guest.TypePaymentMethod,
                            UserCreate = "System BookingEngine",
                            CreateDate = datetimeNow,
                            Prepaid = 0,
                            Discount = 0,
                            DepositPecent = -1,
                            DepositMonney = Math.Round(depositMoney, 0),
                            IncludeVATAndServiceCharge = true
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                        if (x.ExtrabedHome is null)
                            x.ExtrabedHome = new List<Extrabed_Home>();
                        x.ExtrabedHome.ForEach(y =>
                        {
                            if (y.NumberChoose > 0)
                            {
                                connection.Execute("Booking_Extrabed_Post",
                                    new
                                    {
                                        BookingId = bookingId,
                                        ExtrabedId = y.ExtrabedId,
                                        Number = y.NumberChoose,
                                        Price = Math.Round(y.Price, 0),
                                        DateCreate = datetimeNow
                                    }, commandType: CommandType.StoredProcedure,
                                    transaction: transaction);
                            }
                        });

                        if (x.RateAvailabilities is null)
                            x.RateAvailabilities = new List<RateAvailability>();
                        x.RateAvailabilities.ForEach(y =>
                        {
                            connection.Execute("BookingPrice_Post",
                                new
                                {
                                    BookingId = bookingId,
                                    Date = y.Date,
                                    Price = Math.Round(y.PriceForAddClient, 0)
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                        if (x.ServiceHomes is null)
                            x.ServiceHomes = new List<Service_Home>();
                        x.ServiceHomes.ForEach(y =>
                        {
                            if (y.NumberChoose > 0)
                            {
                                connection.Execute("BookingService_Post",
                                    new
                                    {
                                        BookingId = bookingId,
                                        ServiceId = y.ServiceId,
                                        Number = y.NumberChoose,
                                        Price = Math.Round(y.PriceForAddClient, 0),
                                        DateCreate = datetimeNow
                                    }, commandType: CommandType.StoredProcedure,
                                    transaction: transaction);
                            }
                        });
                        // lưu giá trị deposit của booking khi thanh toán PayPal, khi thanh toán PayPal thành công sẽ tự
                        // động cập nhật giá trị trả trước vào booking
                        connection.Execute("ReservationDepositBooking_Post",
                                new
                                {
                                    ReservationId = reservationId,
                                    BookingId = bookingId,
                                    Paid = depositMoney
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        #endregion

                    });
                    #endregion
                    transaction.Commit();
                    AllDataBook_Home allDataBook = new AllDataBook_Home()
                    {
                        reservationId = reservationId,
                        data = dataHorizontal,
                        dataDefault = data,
                        extrabeds = extrabeds,
                        guest = guest,
                        param = param,
                        services = services,
                        voucher = voucher
                    };
                    Session["allDataBook"] = allDataBook;
                    Thread thread = new Thread(SendEmail);
                    thread.Start(allDataBook);

                    ConfigCurrency configCurrency = new ConfigCurrency();
                    configCurrency = connection.QuerySingleOrDefault<ConfigCurrency>("ConfigCurrency_GetExchangeRate",
                           new
                           {
                               HodelCode = param.hotelCode,
                               CurrencyCode = currency
                           }, commandType: CommandType.StoredProcedure);
                    totalDeposit = Math.Round(ConvertCurrency(totalDeposit, configCurrency.Result), 2);
                    allDataBook.data.ForEach(x =>
                    {
                        totalPriceRoom += x.PromotionHome.TotalPriceAfterPromotion;
                    });
                    /// chuyeenr tien te
                    totalPriceRoom = Math.Round(ConvertCurrency(totalPriceRoom, configCurrency.Result), 2);
                }
            }
            return reservationId + "," + totalDeposit + "," + totalPriceRoom;
        }
        public JsonResult CreatePayment()
        {
            string currency = (string)Session["currency"];
            if (currency == "VND" || currency == "JPY")
                currency = "USD";
            Hotel hotel = (Hotel)Session["HotelForBooking"];
            List<RoomType_Home> data = new List<RoomType_Home>();
            if (Session["DataBooking"] != null)
                data = (List<RoomType_Home>)Session["DataBooking"];
            List<string> descriptionItemPostPayPal = new List<string>();
            data.ForEach(x =>
            {
                descriptionItemPostPayPal.Add(x.PromotionHome.ChooseRoom + " " + x.RoomTypeName);
            });
            using (var connection = DB.ConnectionFactory())
            {
                string merchant_id = connection.QuerySingleOrDefault<string>("Hotel_GetMerchantId",
                    new
                    {
                        HotelId = hotel.HotelId
                    }, commandType: CommandType.StoredProcedure);
                if (merchant_id != "")
                {
                    string[] resultPostBookingForPayPal = PostBookingWhenPayByPayPal().Split(',');
                    string invoice_number = resultPostBookingForPayPal[0];
                    double totalDeposit = double.Parse(resultPostBookingForPayPal[1]);
                    double totalPriceRoom = double.Parse(resultPostBookingForPayPal[2]);
                    List<Item> items = new List<Item>();
                    items.Add(new Item()
                    {
                        currency = currency,
                        description = string.Join(",", descriptionItemPostPayPal.ToArray()),
                        name = "Book with " + hotel.Name,
                        price = totalDeposit,
                        quantity = 1,
                        //sku = 1,
                        tax = 0
                    });
                    string url = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority;
                    PaymentPaypal dataPaymentPayPal = new PaymentPaypal()
                    {
                        intent = "sale",
                        payer = new Payer()
                        {
                            payment_method = "paypal"
                        },
                        application_context = new Application_context()
                        {
                            shipping_preference = "NO_SHIPPING",
                            user_action = "commit"
                        },
                        transactions = new List<Transactions>()
                {
                    new Transactions()
                    {
                        amount = new Amount()
                        {
                            total = totalDeposit,
                            currency =  currency
                        },
                        payee= new Payee()
                        {
                            merchant_id = merchant_id //VDT3C4BXVAV2L
                        },
                        custom = invoice_number,
                        invoice_number = invoice_number,
                        payment_options = new Payment_options()
                        {
                            allowed_payment_method = "INSTANT_FUNDING_SOURCE"
                        },
                        item_list = new Item_list()
                        {
                            items = items
                        }
                    }
                },
                        redirect_urls = new Redirect_urls()
                        {
                            return_url = url + "/Home/ExecutePayment",
                            cancel_url = url + "/Home/BookError"
                        }
                    };
                    Auth auth = DataHelper.GetTokenKey();
                    PaymentPaypal paymentPaypal = GetConnectPaypal(auth.access_token, dataPaymentPayPal);
                    Session["token"] = auth.access_token;
                    Session["invoice_number"] = invoice_number;
                    return Json(paymentPaypal, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }
        private string excutePayment(string invoice_number, string paymentId, string payerID, string token)
        {
            var client = new RestClient("https://api.paypal.com/v1/payments/payment/" + paymentId + "/execute");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Postman-Token", "92dba6b9-f6a0-4797-9347-a4932af253cb");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("PayPal-Client-Metadata-Id", invoice_number);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("PayPal-Partner-Attribution-Id", "IITTechnology_STP_VN");
            request.AddParameter("undefined", "{\n\t\"payer_id\":\"" + payerID + "\"\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
        public JsonResult ExecutePayment(string paymentId, string payerID)
        {
            string invoice_number = (string)Session["invoice_number"];
            string resultExcute = excutePayment(invoice_number, paymentId, payerID, (string)Session["token"]);
            PaymentPaypalResult result = JsonConvert.DeserializeObject<PaymentPaypalResult>(resultExcute);
            result.AllDataJson = resultExcute;
            string state = " ";
            string message = " ";
            try
            {
                if (result.transactions[0].related_resources[0].sale.state == "completed")
                {
                    state = "COMPLETED";
                    using (var connection = DB.ConnectionFactory())
                    {
                        connection.Execute("UpdateReservationWhenPayOnline",
                            new
                            {
                                ReservationId = result.transactions[0].invoice_number
                            }, commandType: CommandType.StoredProcedure);
                        connection.Execute("ReservationLogPayPal_Log",
                            new
                            {
                                ReservationId = result.transactions[0].invoice_number,
                                LogContent = result.AllDataJson,
                                DateLog = DatetimeHelper.DateTimeUTCNow()
                            }, commandType: CommandType.StoredProcedure);
                        AllDataBook_Home allDataBook = (AllDataBook_Home)Session["allDataBook"];
                        //allDataBook.Reference_Number = paymentId;
                        Thread thread = new Thread(SendEmailPayOnline);
                        thread.Start(allDataBook);
                    }
                }
            }
            catch (Exception ex)
            {
                state = "";
                message = resultExcute;
            }

            return Json(new
            {
                state = state,
                result = message
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region send Mail booking
        private void SendEmailPayOnline(object obj)
        {
            if (obj is null) return;
            AllDataBook_Home allDataBook = obj as AllDataBook_Home;
            ConfigEmail configEmail = new ConfigEmail();
            TemplateEmail template = new TemplateEmail();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    Hotel hotel = connection.QuerySingleOrDefault<Hotel>("Hotel_GetByCode",
                    new
                    {
                        HotelCode = allDataBook.param.hotelCode
                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);
                    configEmail = connection.QuerySingleOrDefault<ConfigEmail>("ConfigEmail_Get",
                        new
                        {
                            HotelId = hotel.HotelId
                        }, commandType: System.Data.CommandType.StoredProcedure,
                    transaction: transaction);
                    if (configEmail is null) configEmail = new ConfigEmail();
                    configEmail.Email = DataHelper.Decrypt(configEmail.Email);
                    configEmail.EmailReceive = DataHelper.Decrypt(configEmail.EmailReceive);
                    configEmail.Password = DataHelper.Decrypt(configEmail.Password);

                    template = connection.QuerySingleOrDefault<TemplateEmail>("TemplateEmail_Get",
                       new
                       {
                           TypeEmailId = 1,
                           HotelId = hotel.HotelId
                       },
                       commandType: System.Data.CommandType.StoredProcedure,
                    transaction: transaction);
                    if (template is null) template = new TemplateEmail();
                    allDataBook.data.ForEach(x =>
                    {
                        connection.Execute("RateAvailability_ReductionNumber",
                            new
                            {
                                RoomTypeId = x.RoomTypeId,
                                FromDate = allDataBook.param.fromDate,
                                ToDate = allDataBook.param.toDate
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    ReservationEmailSent reservationEmailSent = connection.QuerySingleOrDefault<ReservationEmailSent>("ReservationEmailSent_GetByReservationId",
                        new
                        {
                            ReservationId = allDataBook.reservationId
                        }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);
                    connection.Execute("ReservationEmailSent_Sent",
                        new
                        {
                            ReservationId = allDataBook.reservationId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                    transaction.Commit();
                    if (reservationEmailSent != null)
                    {
                        MailHelper.SendMailGuest(configEmail, allDataBook.guest.Email, reservationEmailSent.Subject + " " + allDataBook.Reference_Number, reservationEmailSent.Content,
                                       template.CC.Split(',').ToList(), template.BCC.Split(',').ToList());
                    }
                }
            }
        }
        private void SendEmail(object obj)
        {
            if (obj is null) return;
            AllDataBook_Home allDataBook = obj as AllDataBook_Home;
            TemplateEmail template = new TemplateEmail();
            ConfigEmail configEmail = new ConfigEmail();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                Hotel hotel = connection.QuerySingleOrDefault<Hotel>("Hotel_GetByCode",
                    new
                    {
                        HotelCode = allDataBook.param.hotelCode
                    }, commandType: CommandType.StoredProcedure);
                template = connection.QuerySingleOrDefault<TemplateEmail>("TemplateEmail_Get",
                    new
                    {
                        TypeEmailId = 1,
                        HotelId = hotel.HotelId
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
                if (template is null) template = new TemplateEmail();
                template.CheckNull();
                configEmail = connection.QuerySingleOrDefault<ConfigEmail>("ConfigEmail_Get",
                    new
                    {
                        HotelId = hotel.HotelId
                    }, commandType: System.Data.CommandType.StoredProcedure);
                if (configEmail is null) configEmail = new ConfigEmail();

                configEmail.Email = DataHelper.Decrypt(configEmail.Email);
                configEmail.EmailReceive = DataHelper.Decrypt(configEmail.EmailReceive);
                configEmail.Password = DataHelper.Decrypt(configEmail.Password);

                #region replace content
                template.Content = template.Content.Replace("[HotelName]", hotel.Name);
                template.Content = template.Content.Replace("[HotelEmail]", hotel.Email);
                template.Content = template.Content.Replace("[HotelPhone]", hotel.Phone);
                template.Content = template.Content.Replace("[BookingId]", allDataBook.reservationId.ToString());
                template.Content = template.Content.Replace("[FirstName]", allDataBook.guest.FirstName);
                template.Content = template.Content.Replace("[SurName]", allDataBook.guest.SurName);
                template.Content = template.Content.Replace("[GuestEmail]", allDataBook.guest.Email);
                template.Content = template.Content.Replace("[GuestPhone]", allDataBook.guest.Phone);

                StringBuilder roomBookAndPolicyOfRoom = new StringBuilder();
                StringBuilder extrabedInfor = new StringBuilder();
                StringBuilder services = new StringBuilder();
                int tempRound = allDataBook.param.currency == "VND" ? 0 : 2;
                double totalPriceRoom = 0;
                double totalPriceExtrabed = 0;
                double totalPriceService = 0;
                double totalPriceVoucher = 0;
                double totalPrice = 0;
                double totalDeposit = 0;
                double totalVAT = 0;
                allDataBook.data.ForEach(x =>
                {
                    roomBookAndPolicyOfRoom.Append("<p><b>" + x.RoomTypeName + "</b>" + x.PromotionHome.PolicyContent + "</p>");
                    if (allDataBook.param.currency == "VND")
                        totalPriceRoom += x.PromotionHome.TotalPriceAfterPromotion;
                    else
                        totalPriceRoom += x.PromotionHome.TotalPriceAfterPromotionExchangeRate;
                });
                allDataBook.extrabeds.ForEach(x =>
                {
                    extrabedInfor.Append("<p><b>" + x.ExtrabedName + "</b>" + " (" + x.RoomTypeName + ")" + "(" + x.NumberChoose + " x " + x.Price + ") " + "</p>");
                    if (allDataBook.param.currency == "VND")
                        totalPriceExtrabed += x.Price * x.NumberChoose;
                    else
                        totalPriceExtrabed += x.PriceExchangeRate * x.NumberChoose;
                });
                allDataBook.services.ForEach(x =>
                {
                    services.Append("<p><b>" + x.ServiceName + "</b>" + " (" + x.NumberChoose + " x " + x.Price + ")" + "</p>");
                    if (allDataBook.param.currency == "VND")
                        totalPriceService += x.Price * x.NumberChoose;
                    else
                        totalPriceService += x.PriceExchangeRate * x.NumberChoose;
                });
                totalPriceVoucher = Math.Round(allDataBook.voucher.amountForVoucher, tempRound);
                totalPrice = totalPriceRoom + totalPriceExtrabed + totalPriceService - totalPriceVoucher;
                totalVAT = Math.Round(totalPrice * (allDataBook.param.tax + allDataBook.param.serviceCharge + (allDataBook.param.tax * allDataBook.param.serviceCharge / 100)) / 100, tempRound);
                #region cal deposit when has voucher

                using (var multi = connection.QueryMultiple("Voucher_CheckAcceptDeposit",
                    new
                    {
                        HotelCode = allDataBook.param.hotelCode,
                        VoucherCode = allDataBook.voucher.VoucherCode,
                        Date = DatetimeHelper.DateTimeUTCNow()
                    }, commandType: CommandType.StoredProcedure))
                {
                    int accept = multi.Read<int>().SingleOrDefault();
                    Voucher_Home voucherResult = new Voucher_Home();
                    if (accept == 1)
                    {
                        voucherResult = multi.Read<Voucher_Home>().SingleOrDefault();
                        List<int> roomtypeAccept = multi.Read<int>().ToList();
                        List<int> serviceAccept = multi.Read<int>().ToList();
                        if (roomtypeAccept is null) roomtypeAccept = new List<int>();
                        if (serviceAccept is null) serviceAccept = new List<int>();
                        if (voucherResult.DiscountForRoom)
                        {
                            allDataBook.data.ForEach(x =>
                            {
                                if (roomtypeAccept.FindIndex(y => y == x.RoomTypeId) >= 0)
                                {
                                    if (allDataBook.param.currency == "VND")
                                        totalDeposit += x.PromotionHome.TotalPriceAfterPromotion * (100 - voucherResult.AmountForRoom) * x.PromotionHome.AmountRate / 10000;
                                    else
                                        totalDeposit += x.PromotionHome.TotalPriceAfterPromotionExchangeRate * (100 - voucherResult.AmountForRoom) * x.PromotionHome.AmountRate / 10000;
                                }
                                else
                                {
                                    if (allDataBook.param.currency == "VND")
                                        totalDeposit += x.PromotionHome.TotalPriceAfterPromotion * x.PromotionHome.AmountRate / 100;
                                    else
                                        totalDeposit += x.PromotionHome.TotalPriceAfterPromotionExchangeRate * x.PromotionHome.AmountRate / 100;
                                }
                            });
                        }
                        else
                        {
                            allDataBook.data.ForEach(x =>
                            {
                                if (allDataBook.param.currency == "VND")
                                    totalDeposit += x.PromotionHome.TotalPriceAfterPromotion * x.PromotionHome.AmountRate / 100;
                                else
                                    totalDeposit += x.PromotionHome.TotalPriceAfterPromotionExchangeRate * x.PromotionHome.AmountRate / 100;
                            });
                        }
                    }
                    else
                    {
                        allDataBook.data.ForEach(x =>
                        {
                            if (allDataBook.param.currency == "VND")
                                totalDeposit += x.PromotionHome.TotalPriceAfterPromotion * x.PromotionHome.AmountRate / 100;
                            else
                                totalDeposit += x.PromotionHome.TotalPriceAfterPromotionExchangeRate * x.PromotionHome.AmountRate / 100;
                        });
                    }
                }
                #endregion
                totalDeposit = Math.Round(totalDeposit * (100 + (allDataBook.param.tax + allDataBook.param.serviceCharge + (allDataBook.param.tax * allDataBook.param.serviceCharge) / 100)) / 100, tempRound);
                if (allDataBook.guest.TypePaymentMethod == 11 || allDataBook.guest.TypePaymentMethod == 15)
                    totalDeposit = 0;
                template.Content = template.Content.Replace("[Room]", roomBookAndPolicyOfRoom.ToString());
                template.Content = template.Content.Replace("[PriceRoom]", String.Format("{0:n}", Math.Round(totalPriceRoom, tempRound)));
                template.Content = template.Content.Replace("[unitprice]", allDataBook.param.currency);
                template.Content = template.Content.Replace("[PriceExtrabed]", String.Format("{0:n}", Math.Round(totalPriceExtrabed, tempRound)));
                template.Content = template.Content.Replace("[ExtrabedInfor]", extrabedInfor.ToString());
                template.Content = template.Content.Replace("[Service]", services.ToString());
                template.Content = template.Content.Replace("[PriceService]", String.Format("{0:n}", Math.Round(totalPriceService, tempRound)));
                template.Content = template.Content.Replace("[Voucher]", allDataBook.voucher.VoucherCode);
                template.Content = template.Content.Replace("[PriceVoucher]", String.Format("{0:n}", totalPriceVoucher));
                template.Content = template.Content.Replace("[Checkin]", allDataBook.param.fromDate.ToString("dd/MM/yyyy"));
                template.Content = template.Content.Replace("[Chechout]", allDataBook.param.toDate.ToString("dd/MM/yyyy"));
                template.Content = template.Content.Replace("[Adults]", allDataBook.param.adults.ToString());
                template.Content = template.Content.Replace("[Childrent]", allDataBook.param.child.ToString());
                template.Content = template.Content.Replace("[ArrivalFlightNumber]", allDataBook.guest.ArrivalFlightDate);
                template.Content = template.Content.Replace("[ArrivalTimeFlight]", allDataBook.guest.ArrivalFlightTime);
                template.Content = template.Content.Replace("[OrtherRequest]", allDataBook.guest.Note);
                template.Content = template.Content.Replace("[TotalPrice]", String.Format("{0:n}", Math.Round(totalPrice, tempRound)));
                template.Content = template.Content.Replace("[TotalVAT]", String.Format("{0:n}", totalVAT));
                template.Content = template.Content.Replace("[Deposit]", String.Format("{0:n}", totalDeposit));
                template.Content = template.Content.Replace("[Balance]", String.Format("{0:n}", Math.Round(totalPrice + totalVAT - totalDeposit, tempRound)));
                #endregion
                using (var transaction = connection.BeginTransaction())
                {
                    bool status = false;
                    if (allDataBook.guest.TypePaymentMethod == 13 || allDataBook.guest.TypePaymentMethod == 14 || allDataBook.guest.TypePaymentMethod == 18)
                    {
                        allDataBook.data.ForEach(x =>
                        {
                            connection.Execute("RateAvailability_AddNumber",
                                new
                                {
                                    RoomTypeId = x.RoomTypeId,
                                    FromDate = allDataBook.param.fromDate,
                                    ToDate = allDataBook.param.toDate
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
                    if (allDataBook.guest.TypePaymentMethod != 13 && allDataBook.guest.TypePaymentMethod != 14 && allDataBook.guest.TypePaymentMethod != 18)
                    {
                        status = true;
                        if (allDataBook.voucher.VoucherCode != null && allDataBook.voucher.VoucherCode != "")
                        {
                            connection.Execute("Voucher_ReductionNumber",
                                new
                                {
                                    HotelId = hotel.HotelId,
                                    VoucherCode = allDataBook.voucher.VoucherCode
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        }
                    }
                    connection.Execute("ReservationEmailSent_Post",
                    new
                    {
                        ReservationId = allDataBook.reservationId,
                        Subject = template.Subject,
                        Email = allDataBook.guest.Email,
                        Content = template.Content,
                        Date = DatetimeHelper.DateTimeUTCNow(),
                        Source = "BE",
                        Status = status
                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);
                    transaction.Commit();
                }
            }
            MailHelper.SendMailToHotel(configEmail, template.Content, allDataBook.guest.TypePaymentMethodName);
            if (allDataBook.guest.TypePaymentMethod != 13 && allDataBook.guest.TypePaymentMethod != 14 && allDataBook.guest.TypePaymentMethod != 18)
            {
                MailHelper.SendMailGuest(configEmail, allDataBook.guest.Email, template.Subject, template.Content,
                                template.CC.Split(',').ToList(), template.BCC.Split(',').ToList());
            }

        }
        //public static string[] keyTemplate = new string[] {
        //    "[HotelName]","[HotelEmail]","[HotelPhone]","[BookingId]","[FirstName]","[SurName]","[GuestEmail]",
        //    "[GuestPhone]","[Room]","[PriceRoom]","[unitprice]","[PriceExtrabed]","[ExtrabedInfor]","[Service]",
        //    "[PriceService]","[Voucher]","[PriceVoucher]","[Checkin]","[Chechout]","[Adults]","[Childrent]","[ArrivalFlightNumber]",
        //    "[ArrivalTimeFlight]","[OrtherRequest]","[TotalPrice]","TotalVAT","[Deposit]","[Balance]"
        //};
        #endregion

        #region Invoice
        public ActionResult InvoiceInvalid()
        {
            return View();
        }
        public ActionResult InvoicePaySuccess()
        {
            return View();
        }
        public ActionResult Invoice(int code, string email, string lang = "vi")
        {
            using (var connection = DB.ConnectionFactory())
            {
                Invoice invoice = connection.QuerySingleOrDefault<Invoice>("Invoice_DetailGetByCodeEmail",
                    new
                    {
                        InvoiceId = code,
                        Email = email
                    }, commandType: CommandType.StoredProcedure);
                if (invoice is null)
                    return RedirectToAction("InvoiceInvalid");
                Session["invoice"] = invoice;
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting_GetByKey",
                      new
                      {
                          Key = lang,
                          screenId = 61
                      }, commandType: CommandType.StoredProcedure).ToList();
                Session["HotelForBooking"] = connection.QuerySingleOrDefault<Hotel>("Hotel_GetHighLightByHotelId",
                           new
                           {
                               HotelId = invoice.HotelId
                           },
                          commandType: CommandType.StoredProcedure);
                Session["transitions"] = transitions;
                List<PaymentMethod_Home> paymentMethods = connection.Query<PaymentMethod_Home>("PaymentMethod_GetBuyOnlineGetByHotelId",
                   new
                   {
                       HotelId = invoice.HotelId,
                       LanguageCode = lang
                   }, commandType: CommandType.StoredProcedure).ToList();
                if (paymentMethods is null) paymentMethods = new List<PaymentMethod_Home>();
                //paymentMethods.RemoveAll(x => x.ConfigPaymentMethodId == 13 || x.ConfigPaymentMethodId == 14);
                Transition transition = new Transition(transitions);
                paymentMethods.ForEach(x =>
                {
                    switch (x.ConfigPaymentMethodId)
                    {
                        case 11:
                            x.Name = transition.Translate(502, x.Name);
                            break;
                        case 13:
                            x.Name = transition.Translate(2346, x.Name);
                            break;
                        case 14:
                            x.Name = transition.Translate(2347, x.Name);
                            break;
                        case 15:
                            x.Name = transition.Translate(2348, x.Name);
                            break;
                        case 16:
                            x.Name = transition.Translate(2349, x.Name);
                            break;
                        case 17:
                            x.Name = transition.Translate(2350, x.Name);
                            break;
                    }
                });
                Session["paymentMethods"] = paymentMethods;

                return View();
            }
        }
        public JsonResult PayInvoice(Guest_Home guest)
        {
            int resultPost = 0;
            string url = "";
            Invoice invoice = (Invoice)Session["invoice"];
            if (invoice is null)
                resultPost = -1;
            else
            {
                using (var connection = DB.ConnectionFactory())
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        if (guest.TypePaymentMethod == 13 || guest.TypePaymentMethod == 14)
                        {
                            Session["dataInvoicePay"] = invoice.TotalAmount;
                        }
                        else
                        {
                            connection.Execute("Invoice_FeedBack",
                                new
                                {
                                    InvoiceId = invoice.InvoiceId,
                                    NameOnCard = DataHelper.Encrypt(guest.Name),
                                    CardNumber = DataHelper.Encrypt(guest.Number),
                                    SecurityCode = DataHelper.Encrypt(guest.Code),
                                    ExprireMonth = guest.ExpirationMonth,
                                    ExprireYear = guest.ExpirationYear
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            transaction.Commit();
                        }
                    }
                }
            }
            switch (resultPost)
            {
                case 0:
                    url = "/Home/InvoicePaySuccess";
                    break;
                case 1:
                    url = "/Home/SubmitInvoidVTCPay";
                    break;
                case 2:
                    url = "/Home/SubmitInvoidVTCPay";
                    break;
                default:
                    url = "/Home/InvoiceInvalid";
                    break;
            }
            return Json(url, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult TestButtonPaypal()
        {
            return View();
        }
    }
}