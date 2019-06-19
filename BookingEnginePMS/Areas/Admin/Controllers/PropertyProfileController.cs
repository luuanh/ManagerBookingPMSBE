using BookingEnginePMS.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class PropertyProfileController : SercurityController
    {
        // GET: Admin/PropertyProfile
        public ActionResult Index()
        {
            if (!CheckSecurity(35))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 35
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public ActionResult Chain()
        {
            if (!CheckSecurity(35))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 35
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public JsonResult Get(string keySearch, int pageNumber, int pageSize)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                // lấy danh sách khachsh sạn theo group có mã từ HotelId
                using (var multi = connection.QueryMultiple("Hotel_Get",
                    new
                    {
                        HotelId = HotelId,
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Hotel> hotels = multi.Read<Hotel>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        hotels = hotels,
                        totalRecord = totalRecord
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Detail(int id = -1)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (id < 0 && Session["HotelId"] != null)
                id = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Hotel_Detail_Full",
                    new
                    {
                        HotelId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    Hotel hotel = multi.Read<Hotel>().SingleOrDefault();
                    if (hotel is null)
                        hotel = new Hotel();
                    List<HotelLanguage> hotelLanguage = multi.Read<HotelLanguage>().ToList();
                    if (hotelLanguage is null)
                        hotelLanguage = new List<HotelLanguage>();
                    List<int> languageId = multi.Read<int>().ToList();
                    if (languageId != null)
                        languageId.ForEach(x =>
                        {
                            if (hotelLanguage.FindIndex(y => y.LanguageId == x) < 0)
                            {
                                hotelLanguage.Add(new HotelLanguage()
                                {
                                    LanguageId = x,
                                    InforAccount = "",
                                    Note = "",
                                    Terms = "",
                                    HotelLanguageId = -1
                                });
                            }
                        });
                    hotel.hotelLanguages = hotelLanguage;
                    return Json(hotel, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Post(Hotel hotel)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            User user = (User)Session["User"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // insert hotel
                    int hotelId = connection.QuerySingleOrDefault<int>("Hotel_Post",
                    new
                    {
                        HotelIdDefault = HotelId,
                        Name = hotel.Name,
                        Code = hotel.Code,
                        Logo = hotel.Logo,
                        Phone = hotel.Phone,
                        Fax = hotel.Fax,
                        Email = hotel.Email,
                        Hotline = hotel.Hotline,
                        Address = hotel.Address,
                        Website = hotel.Website,
                        Facebook = hotel.Facebook,
                        Skyper = hotel.Skyper,
                        Google = hotel.Google,
                        Youtobe = hotel.Youtobe,
                        HighLight = hotel.HighLight
                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);
                    if (hotelId < 0)
                    {
                        transaction.Dispose();
                        return Json(-1, JsonRequestBehavior.AllowGet);
                    }
                    // insert hotellanguage includes terms, note..
                    if (hotel.hotelLanguages != null)
                    {
                        hotel.hotelLanguages.ForEach(x =>
                        {
                            connection.Execute("HotelLanguage_Post",
                            new
                            {
                                HotelId = hotelId,
                                LanguageId = x.LanguageId,
                                Terms = x.Terms,
                                InforAccount = x.InforAccount,
                                Note = x.Note
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                        });
                    }
                    // update user accept hotel
                    connection.Execute("UserHotel_Post_ByUserName",
                        new
                        {
                            UserName = user.UserName,
                            HotelId = hotelId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(Hotel hotel)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // update hotel
                    int result = connection.QuerySingleOrDefault<int>("Hotel_Put",
                         new
                         {
                             HotelId = hotel.HotelId,
                             Name = hotel.Name,
                             Code = hotel.Code,
                             Logo = hotel.Logo,
                             Phone = hotel.Phone,
                             Fax = hotel.Fax,
                             Email = hotel.Email,
                             Hotline = hotel.Hotline,
                             Address = hotel.Address,
                             Website = hotel.Website,
                             Facebook = hotel.Facebook,
                             Skyper = hotel.Skyper,
                             Google = hotel.Google,
                             Youtobe = hotel.Youtobe,
                             HighLight = hotel.HighLight
                         }, commandType: CommandType.StoredProcedure,
                         transaction: transaction);
                    if (result < 0)
                    {
                        transaction.Dispose();
                        return Json(-1, JsonRequestBehavior.AllowGet);
                    }
                    // update hotellanguage includes terms, note..
                    if (hotel.hotelLanguages != null)
                        hotel.hotelLanguages.ForEach(x =>
                        {
                            if (x.HotelLanguageId < 0)
                            {
                                connection.Execute("HotelLanguage_Post",
                                new
                                {
                                    HotelId = hotel.HotelId,
                                    LanguageId = x.LanguageId,
                                    Terms = x.Terms,
                                    InforAccount = x.InforAccount,
                                    Note = x.Note
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            }
                            else
                            {
                                connection.Execute("HotelLanguage_Put",
                                new
                                {
                                    HotelId = hotel.HotelId,
                                    LanguageId = x.LanguageId,
                                    Terms = x.Terms,
                                    InforAccount = x.InforAccount,
                                    Note = x.Note
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
                connection.Execute("Language_Delete",
                    new
                    {
                        LanguageId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}