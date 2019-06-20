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
    public class StatisticController : SercurityController
    {
        // GET: Statistic
        public ActionResult Registration()
        {
            if (!CheckSecurity())
                return Redirect("/Login/Index");
            return View();
        }
        public ActionResult HotelCancelBooking()
        {
            if (!CheckSecurity())
                return Redirect("/Login/Index");
            return View();
        }

        public JsonResult getSituation(int type, int month, int year)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                List<Hotel> hotels = connection.Query<Hotel>("ManageSystem_Hotel_GetSituation",
                    new
                    {
                        type = type,
                        month = month,
                        year = year
                    },
                    commandType: System.Data.CommandType.StoredProcedure).ToList();

                int numberTest = hotels.FindAll(x => x.Status == 1).Count;
                int numberReal = hotels.FindAll(x => x.Status == 2).Count;
                int numberOverdue = hotels.FindAll(x => x.Status == 3).Count;
                int numberLooked = hotels.FindAll(x => x.Status == 4).Count;
                List<int> datas = new List<int>() { numberTest, numberReal, numberOverdue, numberLooked };
                return Json(datas, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getUse(int type, int month, int year)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                List<Hotel> hotels = connection.Query<Hotel>("ManageSystem_Hotel_GetUse",
                       new
                       {
                           type = type,
                           month = month,
                           year = year
                       },
                       commandType: System.Data.CommandType.StoredProcedure).ToList();
                int numberPMS = hotels.FindAll(x => x.TypeSoftware == 1).Count;
                int numberBE = hotels.FindAll(x => x.TypeSoftware == 2).Count;
                int numberPMSBE = hotels.FindAll(x => x.TypeSoftware == 3).Count;
                List<int> datas = new List<int>() { numberPMS, numberBE, numberPMSBE };
                return Json(datas, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getRegistration(int type, int month, int year)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                List<Hotel> hotels = connection.Query<Hotel>("ManageSystem_Hotel_GetRegistration",
                       new
                       {
                           type = type,
                           month = month,
                           year = year
                       },
                       commandType: System.Data.CommandType.StoredProcedure).ToList();
                int numberPMS = hotels.FindAll(x => x.TypeSoftware == 1).Count;
                int numberBE = hotels.FindAll(x => x.TypeSoftware == 2).Count;
                int numberPMSBE = hotels.FindAll(x => x.TypeSoftware == 3).Count;
                List<int> datas = new List<int>() { numberPMS, numberBE, numberPMSBE };
                return Json(datas, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getRegistrationSituation(int month, int year)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                List<Hotel> hotels = connection.Query<Hotel>("ManageSystem_Hotel_GetRegistrationSituation",
                       new
                       {
                           month = month,
                           year = year
                       },
                       commandType: System.Data.CommandType.StoredProcedure).ToList();
                List<Hotel> hpms = hotels.FindAll(x => x.TypeSoftware == 1);
                List<Hotel> hbe = hotels.FindAll(x => x.TypeSoftware == 2);
                List<Hotel> hpmsbe = hotels.FindAll(x => x.TypeSoftware == 3);
                int rangeDate = (new DateTime(year, month + 1, 1) - new DateTime(year, month, 1)).Days;
                List<string> lables = new List<string>();
                int[] pms = new int[rangeDate];
                int[] be = new int[rangeDate];
                int[] pmsbe = new int[rangeDate];
                for (int i = 1; i <= rangeDate; i++)
                {
                    lables.Add("Ngày " + i);
                    pms[i - 1] = hpms.FindAll(x => x.DayStartUse.Day == i).Count;
                    be[i - 1] = hbe.FindAll(x => x.DayStartUse.Day == i).Count;
                    pmsbe[i - 1] = hpmsbe.FindAll(x => x.DayStartUse.Day == i).Count;
                }
                return Json(new
                {
                    lables = lables,
                    pms = pms,
                    be = be,
                    pmsbe = pmsbe
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getAllRegistration(int month, int year)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                List<Hotel> hotels = connection.Query<Hotel>("ManageSystem_Hotel_GetRegistrationSituation",
                       new
                       {
                           month = month,
                           year = year
                       },
                       commandType: System.Data.CommandType.StoredProcedure).ToList();
                int rangeDate = (new DateTime(year, month + 1, 1) - new DateTime(year, month, 1)).Days;
                List<string> lables = new List<string>();
                int[] data = new int[rangeDate];
                for (int i = 1; i <= rangeDate; i++)
                {
                    lables.Add("Ngày " + i);
                    data[i - 1] = hotels.FindAll(x => x.DayStartUse.Day == i).Count;
                }
                return Json(new
                {
                    lables = lables,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBookingCancel(int hotelId, int type)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                List<Booking_Reservation> booking_Reservations = connection.Query<Booking_Reservation>("ManageSystem_Booking_GetCancel",
                    new
                    {
                        HotelId = hotelId,
                        Status = type
                    }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                if (booking_Reservations is null) booking_Reservations = new List<Booking_Reservation>();
                booking_Reservations.ForEach(y =>
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
                        {
                            taxFeesForBooking = connection.QuerySingleOrDefault<float>("ManageSystem_TaxFee_GetByHotelFromBooking",
                                new
                                {
                                    BookingId = y.BookingId
                                }, commandType: CommandType.StoredProcedure);
                        }

                        float total = (y.TotalRoom + y.TotalExtrabed + y.TotalService);
                        float discount = (float)Math.Round(total * y.Discount / 100, 0);
                        y.Total = (float)Math.Round((total - discount) * (100 + taxFeesForBooking) / 100, 0);
                    }
                });
                return Json(JsonConvert.SerializeObject(booking_Reservations), JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult GetBookingCheckOut(int hotelId, int type)
        //{
        //    if (!CheckSecurity())
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    using (var connection = DB.ConnectionFactoryBEPMS())
        //    {
        //        List
        //    }
        //}
    }
}