using Dapper;
using ManageSystemPMSBE.DTCore;
using ManageSystemPMSBE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ManageSystemPMSBE.Controllers
{
    public class HomeController : SercurityController
    {
      

        public ActionResult Dashboard()
        {
          
            if (!CheckSecurity())
                return Redirect("/Login/Index");
         
            return View();
        }
        public JsonResult GetDashboard()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                using (var multi = connection.QueryMultiple("ManageSystem_Dashboard", commandType: CommandType.StoredProcedure))
                {
                    int totalHotel = multi.Read<int>().SingleOrDefault();
                    int totalHotelUsePMS = multi.Read<int>().SingleOrDefault();
                    int totalHotelUseBE = multi.Read<int>().SingleOrDefault();
                    int totalHotelUseBEPMS = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        totalHotel = totalHotel,
                        totalHotelUsePMS = totalHotelUsePMS,
                        totalHotelUseBE = totalHotelUseBE,
                        totalHotelUseBEPMS = totalHotelUseBEPMS
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GetDetailHotelById(int id)
        {
            if (!CheckSecurity())
                return Redirect("/Login/Index");
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                Hotel hotel = new Hotel();
                hotel = connection.QuerySingleOrDefault<Hotel>("ManageSystem_Hotel_GetById", new
                {
                    id = id
                }, commandType: CommandType.StoredProcedure);
                ViewData["hotel"] = hotel;
               
                return View(hotel);
                     

            }
        }
        [HttpPost]
        public ActionResult UpdateHotel(Hotel model)
        {
            if (!CheckSecurity())
                return Redirect("/Login/Index");
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                Hotel hotel = new Hotel();
                hotel = connection.QuerySingleOrDefault<Hotel>("ManageSystem_Hotel_GetById", new
                {
                    id = model.HotelId
                }, commandType: CommandType.StoredProcedure);

               
                DateTime dateNow = DateTime.Now;
                DateTime date = new DateTime();
                date = dateNow.AddMonths((int)(hotel.TimeExtended));
                if (hotel != null)
                {
                    //hotel.Status = model.Status;
                    //hotel.TimeExtended = model.TimeExtended;
                    connection.Execute("ManageSystem_UpdateHotel", new { HotelId = model.HotelId, Status = model.Status, TimeExtended = model.TimeExtended, DayStartUse = date },commandType:CommandType.StoredProcedure);

                }

              

                TempData["Messages"] = "Successful";
                return Redirect("/Home/Dashboard");


            }

        }
        //[HttpPut]
        //public ActionResult DeleteHotel(int HotelId)
        //{
        //    if (!CheckSecurity())
        //        return Redirect("/Login/Index");
        //    using (var connection = DB.ConnectionFactoryBEPMS())
        //    {
        //        try
        //        {
        //            Hotel hotel = new Hotel();
        //            hotel = connection.QuerySingleOrDefault<Hotel>("ManageSystem_Hotel_GetById", new
        //            {
        //                id = HotelId
        //            }, commandType: CommandType.StoredProcedure);

        //            if (hotel != null)
        //            {
        //                //hotel.Status = model.Status;
        //                //hotel.TimeExtended = model.TimeExtended;
        //                connection.Execute("ManageSystem_DeleteHotel", new { HotelId = HotelId }, commandType: CommandType.StoredProcedure);

        //            }




        //            return Redirect("/Home/Dashboard");

        //        }
        //        catch
        //        {
        //            return View("/Home/Dashboard");
        //        }



        //    }

        //}

    }
}