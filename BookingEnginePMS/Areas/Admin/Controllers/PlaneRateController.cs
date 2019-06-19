using BookingEnginePMS.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class PlaneRateController : SercurityController
    {
        // GET: Admin/PlaneRate
        public ActionResult Index()
        {
            if (!CheckSecurity(17))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 17
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Get(string keySearch, int pageNumber, int pageSize)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("PlaneRate_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        HotelId = HotelId,
                        LanguageId = LanguageId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<PlaneRate> planeRates = multi.Read<PlaneRate>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        planeRates = planeRates,
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
                using (var multi = connection.QueryMultiple("PlaneRate_Detail_Full",
                    new
                    {
                        PlaneRateId = id,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    PlaneRate plane = multi.Read<PlaneRate>().SingleOrDefault();
                    List<PlaneRateRoomType> planeRateRoomTypes = multi.Read<PlaneRateRoomType>().ToList();
                    List<RoomType> roomTypes = multi.Read<RoomType>().ToList();

                    List<int> languages = multi.Read<int>().ToList();
                    List<PlaneRateLanguage> planeRateLanguages = multi.Read<PlaneRateLanguage>().ToList();

                    if (plane is null) plane = new PlaneRate();
                    if (planeRateRoomTypes is null) planeRateRoomTypes = new List<PlaneRateRoomType>();
                    if (languages is null) languages = new List<int>();
                    if (planeRateLanguages is null) planeRateLanguages = new List<PlaneRateLanguage>();

                    List<PlaneRateRoomType> planeRateRoomTypesResult = new List<PlaneRateRoomType>();
                    List<PlaneRateLanguage> planeRateLanguagesResult = new List<PlaneRateLanguage>();
                    roomTypes.ForEach(x =>
                    {
                        PlaneRateRoomType planeRateRoomType = new PlaneRateRoomType()
                        {
                            RoomTypeId = x.RoomTypeId,
                            RoomTypeName = x.RoomTypeName,
                            Checked = planeRateRoomTypes.FindIndex(y => y.RoomTypeId == x.RoomTypeId && y.PlaneRateId == id) >= 0
                        };
                        planeRateRoomTypesResult.Add(planeRateRoomType);
                    });
                    languages.ForEach(x =>
                    {
                        PlaneRateLanguage planeRateLanguage = planeRateLanguages.Find(y => y.LanguageId == x);
                        if (planeRateLanguage is null)
                            planeRateLanguagesResult.Add(new PlaneRateLanguage()
                            {
                                LanguageId = x,
                                Name = ""
                            });
                        else
                            planeRateLanguagesResult.Add(planeRateLanguage);
                    });

                    plane.PlaneRateRoomTypes = planeRateRoomTypesResult;
                    plane.PlaneRateLanguages = planeRateLanguagesResult;
                    return Json(plane, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Post(PlaneRate planeRate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    int planerateId = connection.QuerySingleOrDefault<int>("PlaneRate_Post",
                    new
                    {
                        HotelId = HotelId,
                        Price = planeRate.Price,
                        Breakfast = planeRate.Breakfast,
                        Lunch = planeRate.Lunch,
                        Dinner = planeRate.Dinner

                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);

                    if (planeRate.PlaneRateRoomTypes != null)
                    {
                        planeRate.PlaneRateRoomTypes.ForEach(x =>
                        {
                            if (x.Checked)
                            {
                                connection.Execute("PlaneRateRoomTypes_Post",
                                new
                                {
                                    PlaneRateId = planerateId,
                                    RoomTypeId = x.RoomTypeId
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            }
                        });
                    }
                    if (planeRate.PlaneRateLanguages is null) planeRate.PlaneRateLanguages = new List<PlaneRateLanguage>();
                    planeRate.PlaneRateLanguages.ForEach(x =>
                    {
                        connection.Execute("PlaneRateLanguage_Post",
                            new
                            {
                                PlaneRateId = planerateId,
                                LanguageId = x.LanguageId,
                                Name = x.Name
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(PlaneRate planeRate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // update amenity
                    connection.Execute("PlaneRate_Put",
                        new
                        {
                            PlaneRateId = planeRate.PlaneRateId,
                            Price = planeRate.Price,
                            Breakfast = planeRate.Breakfast,
                            Lunch = planeRate.Lunch,
                            Dinner = planeRate.Dinner

                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    // update amanity includes amenityname
                    connection.Execute("PlaneRateRoomType_Delete_Full",
                        new
                        {
                            PlaneRateId = planeRate.PlaneRateId
                        },
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if (planeRate.PlaneRateRoomTypes != null)
                    {
                        planeRate.PlaneRateRoomTypes.ForEach(x =>
                        {
                            if (x.Checked)
                            {
                                connection.Execute("PlaneRateRoomTypes_Post",
                                new
                                {
                                    PlaneRateId = planeRate.PlaneRateId,
                                    RoomTypeId = x.RoomTypeId
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            }
                        });
                    }
                    if (planeRate.PlaneRateLanguages is null) planeRate.PlaneRateLanguages = new List<PlaneRateLanguage>();
                    planeRate.PlaneRateLanguages.ForEach(x =>
                    {
                        connection.Execute("PlaneRateLanguage_Post",
                            new
                            {
                                PlaneRateId = planeRate.PlaneRateId,
                                LanguageId = x.LanguageId,
                                Name = x.Name
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
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
                connection.Execute("PlaneRate_Delete",
                    new
                    {
                        PlaneRateId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}