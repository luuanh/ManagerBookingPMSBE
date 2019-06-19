using BookingEnginePMS.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class AmenityController : SercurityController
    {
        // GET: Admin/Amenity
        public ActionResult Index()
        {
            if (!CheckSecurity(37))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 37
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public JsonResult Get(string keySearch, int pageNumber, int pageSize)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Amenity_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        LanguageId = LanguageId,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Amenity> amenities = multi.Read<Amenity>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        amenities = amenities,
                        totalRecord = totalRecord
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Detail(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Amenity_Detail_Full",
                    new
                    {
                        AmenityId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    Amenity amenity = multi.Read<Amenity>().SingleOrDefault();
                    if (amenity is null)
                        amenity = new Amenity();
                    List<AmenityLanguage> amenityLanguages = multi.Read<AmenityLanguage>().ToList();

                    if (amenityLanguages is null)
                        amenityLanguages = new List<AmenityLanguage>();

                    List<int> languageId = multi.Read<int>().ToList();
                    if (languageId != null)
                        languageId.ForEach(x =>
                        {
                            if (amenityLanguages.FindIndex(y => y.LanguageId == x) < 0)
                            {
                                amenityLanguages.Add(new AmenityLanguage()
                                {
                                    AmenityId = amenity.AmenityId,
                                    LanguageId = x,
                                    AmenityName = "",
                                    AmenityLanguageId = -1
                                });
                            }
                        });
                    amenity.AmenityLanguages = amenityLanguages;
                    return Json(amenity, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Post(Amenity amenity)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    int amenityId = connection.QuerySingleOrDefault<int>("Amenity_Post",
                    new
                    {
                        HotelId = HotelId,
                        Index = amenity.Index,
                        Note = amenity.Note
                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);

                    if (amenity.AmenityLanguages != null)
                    {
                        amenity.AmenityLanguages.ForEach(x =>
                        {
                            connection.Execute("AmenityLanguage_Post",
                                new
                                {
                                    AmenityId = amenityId,
                                    LanguageId = x.LanguageId,
                                    AmenityName = x.AmenityName
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(Amenity amenity)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // update amenity
                    connection.Execute("Amenity_Put",
                        new
                        {
                            AmenityId = amenity.AmenityId,
                            Index = amenity.Index,
                            Note = amenity.Note
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    // update amanity includes amenityname
                    if (amenity.AmenityLanguages != null)
                        amenity.AmenityLanguages.ForEach(x =>
                        {
                            if (x.AmenityLanguageId < 0)
                            {
                                connection.Execute("AmenityLanguage_Post",
                                new
                                {
                                    AmenityId = amenity.AmenityId,
                                    LanguageId = x.LanguageId,
                                    AmenityName = x.AmenityName
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            }
                            else
                            {
                                connection.Execute("AmenityLanguage_Put",
                                new
                                {
                                    AmenityId = amenity.AmenityId,
                                    LanguageId = x.LanguageId,
                                    AmenityName = x.AmenityName
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            }
                        });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Delete(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("Amenity_Delete",
                    new
                    {
                        AmenityId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}