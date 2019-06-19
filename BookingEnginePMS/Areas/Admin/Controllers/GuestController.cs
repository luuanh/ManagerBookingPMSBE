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
    public class Guest_Sample
    {
        public int GuestId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string FullName_V { get; set; }
        public string FullName_E { get; set; }
        public string Email { get; set; }
    }
    public class GuestController : SercurityController
    {
        // GET: Admin/Guest
        public ActionResult Index()
        {
            if (!CheckSecurity(11))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            List<string> countries = ConfigData.GetAllCountry();
            ViewData["countries"] = countries;
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 11
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Get(string keySearch = "", int pageNumber = 1, int pageSize = 1000)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Guest_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Guest> guests = multi.Read<Guest>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        guests = guests,
                        totalRecord = totalRecord
                    }, JsonRequestBehavior.AllowGet);
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
                float taxFees = connection.QuerySingleOrDefault<float>("TaxFee_GetTotal_Lastest",
                    new
                    {
                        HotelId = HotelId
                    },
                    commandType: CommandType.StoredProcedure);
                using (var multi = connection.QueryMultiple("Guest_Detail_Full",
                    new
                    {
                        GuestId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    Guest guest = multi.Read<Guest>().SingleOrDefault();
                    guest.ZIPCode = DataHelper.Decrypt(guest.ZIPCode);
                    guest.IdentityCart = DataHelper.Decrypt(guest.IdentityCart);
                    guest.Passport = DataHelper.Decrypt(guest.Passport);
                    guest.CreditCard = DataHelper.Decrypt(guest.CreditCard);
                    guest.CVC = DataHelper.Decrypt(guest.CVC);
                    List<Reservation_Sample> reservations = multi.Read<Reservation_Sample>().ToList();
                    if (reservations is null)
                        reservations = new List<Reservation_Sample>();
                    float totalPrice = 0;
                    float totalPaid = 0;
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
                            using (var multi2 = connection.QueryMultiple("Booking_Get_Multi_Price",
                                new
                                {
                                    BookingId = y.BookingId
                                }, commandType: CommandType.StoredProcedure))
                            {
                                y.TotalRoom = multi2.Read<float>().SingleOrDefault();
                                y.TotalExtrabed = multi2.Read<float>().SingleOrDefault();
                                y.TotalService = multi2.Read<float>().SingleOrDefault();
                                float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                                float discount = (float)Math.Round(total * y.Discount / 100 + 0.001, 0);
                                y.Total = (float)Math.Round((total - discount) * (100 + taxFees) / 100, 0);
                            }
                        });
                        x.TotalPrice = (float)Math.Round(bookings.Sum(y => y.Total) + 0.001, 0);
                        x.Paid = (float)Math.Round(bookings.Sum(y => y.Paid + y.PrePaid), 0);
                        totalPrice += x.TotalPrice;
                        totalPaid += x.Paid;
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
                    });
                    return Json(JsonConvert.SerializeObject(new
                    {
                        guest = guest,
                        history = reservations,
                        totalPrice = totalPrice,
                        totalPaid = totalPaid
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Post(Guest guest)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.QuerySingleOrDefault<int>("Guest_Post",
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
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(Guest guest)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
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
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("RoomType_Delete",
                    new
                    {
                        RoomTypeId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAll()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Guest_Sample> guests = connection.Query<Guest_Sample>("Guest_GetAll_Sample",
                     new
                     {
                         HotelId = HotelId
                     }, commandType: CommandType.StoredProcedure).ToList();
                return Json(guests, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDiscountForGuest(int guestId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                float discount = connection.QuerySingleOrDefault<float>("Guest_Get_Discount",
                     new
                     {
                         GuestId = guestId
                     }, commandType: CommandType.StoredProcedure);
                return Json(discount, JsonRequestBehavior.AllowGet);
            }
        }
    }
}