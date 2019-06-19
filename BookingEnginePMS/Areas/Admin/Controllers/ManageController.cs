using BookingEnginePMS.Helper;
using BookingEnginePMS.Models;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class API_GroupHotel
    {
        public string GroupHotelName { get; set; }
        public string Code { get; set; }
        public Hotel Hotel { get; set; }
        public User User { get; set; }
    }
    public class ManageController : SercurityController
    {
        // GET: Admin/Manage
        public ActionResult Index()
        {
            if (!CheckSecurity())
                return Redirect("/Admin/Login/Index");
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 1
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }

        public ActionResult Account()
        {
            if (!CheckSecurity(52))
                return Redirect("/Admin/Login/Index");
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 52
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
                using (var multi = connection.QueryMultiple("User_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<User> users = multi.Read<User>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(JsonConvert.SerializeObject(new
                    {
                        users = users,
                        totalRecord = totalRecord
                    }), JsonRequestBehavior.AllowGet);
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
                using (var multi = connection.QueryMultiple("User_Detail_Full",
                    new
                    {
                        HotelId = HotelId,
                        UserId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    User user = multi.Read<User>().SingleOrDefault();
                    List<Screen> screens = multi.Read<Screen>().ToList();
                    List<Hotel> hotels = multi.Read<Hotel>().ToList();

                    List<Hotel> allHotels = multi.Read<Hotel>().ToList();
                    List<Screen> allScreens = multi.Read<Screen>().ToList();
                    allHotels.ForEach(x =>
                    {
                        x.Check = hotels.FindIndex(y => y.HotelId == x.HotelId && y.Check) >= 0;
                    });
                    allScreens.ForEach(x =>
                    {
                        x.Check = screens.FindIndex(y => y.ScreenId == x.ScreenId && y.Check) >= 0;
                    });
                    return Json(new
                    {
                        user = user,
                        screens = allScreens,
                        hotels = allHotels
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Post(User user, List<Screen> screens, List<Hotel> hotels)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            if (hotels is null) hotels = new List<Hotel>();
            if (screens is null) screens = new List<Screen>();
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    int userId = connection.QuerySingleOrDefault<int>("User_Post",
                        new
                        {
                            UserName = user.UserName,
                            Password = DataHelper.CreateMD5(user.UserName + user.Password + DataHelper.key),
                            Photo = user.Photo,
                            Email = user.Email,
                            FullName = user.FullName,
                            CreateDate = DatetimeHelper.DateTimeUTCNow()
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if (userId < 0)
                        return Json(-1, JsonRequestBehavior.AllowGet);
                    hotels.ForEach(x =>
                    {

                        connection.Execute("UserHotel_Post",
                            new
                            {
                                UserId = userId,
                                HotelId = x.HotelId,
                                Access = x.Check
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    screens.ForEach(x =>
                    {
                        connection.Execute("UserRole_Post",
                            new
                            {
                                UserId = userId,
                                RoleId = x.ScreenId,
                                Access = x.Check
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(User user, List<Screen> screens, List<Hotel> hotels)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    string pass = "";
                    if (user.Password != null && user.Password != "")
                        pass = DataHelper.CreateMD5(user.UserName + user.Password + DataHelper.key);
                    connection.Execute("User_Put_Full",
                        new
                        {
                            UserName = user.UserName,
                            Photo = user.Photo,
                            Email = user.Email,
                            FullName = user.FullName,
                            Password = pass
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    connection.Execute("User_DeleteHotel_Roles",
                        new
                        {
                            UserId = user.UserId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                    hotels.ForEach(x =>
                    {

                        connection.Execute("UserHotel_Post",
                            new
                            {
                                UserId = user.UserId,
                                HotelId = x.HotelId,
                                Access = x.Check
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    screens.ForEach(x =>
                    {
                        connection.Execute("UserRole_Post",
                            new
                            {
                                UserId = user.UserId,
                                RoleId = x.ScreenId,
                                Access = x.Check
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
                connection.Execute("User_Delete",
                    new
                    {
                        UserId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetHotel()
        {
            using (var connection = DB.ConnectionFactory())
            {
                int HotelId = (int)Session["HotelId"];
                List<Hotel> hotels = connection.Query<Hotel>("Hotel_GetAll",
                    new
                    {
                        HotelId = HotelId
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                return Json(hotels, JsonRequestBehavior.AllowGet);
            }
        }
        // API add group hotel
        /// <summary>
        /// Error: 
        /// -1 : params không hợp lệ
        /// 0 : mã nhóm khách sạn đã tồn tại
        /// -2 : mã khách sạn đã tồn tại
        /// -3 : tài khoản đã tồn tại
        /// </summary>
        /// 
        public List<int> ScreenForPMS = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 18, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 38, 39, 40, 42, 43, 45, 47, 48, 49, 52, 53, 58, 59, 60, 62, 63, 64 }; // PMS
        public List<int> ScreenForBE = new List<int>() { 1, 4, 13, 14, 15, 16, 17, 18, 20, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 35, 37, 38, 39, 40, 41, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 60, 61, 62 }; // BE
        public JsonResult PostGroupHotel(API_GroupHotel data)
        {
            if (data.Hotel is null || data.User is null ||
                data.GroupHotelName is null || data.GroupHotelName == "" ||
                data.Code is null || data.Code == "")
                return Json(-1, JsonRequestBehavior.AllowGet);
            DateTime dateTimeNow = DatetimeHelper.DateTimeUTCNow();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    #region add group company
                    int groupHotelId = connection.QuerySingleOrDefault<int>("GroupHotel_Post",
                         new
                         {
                             Code = data.Code,
                             GroupHotelName = data.GroupHotelName
                         }, commandType: CommandType.StoredProcedure,
                         transaction: transaction);
                    if (groupHotelId < 0)
                    {
                        transaction.Dispose();
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    #endregion
                    #region add hotel
                    int hotelId = connection.QuerySingleOrDefault<int>("Hotel_Post_HasGroupHotelId",
                        new
                        {
                            GroupHotelId = groupHotelId,
                            Name = data.Hotel.Name,
                            Code = data.Hotel.Code,
                            Logo = data.Hotel.Logo,
                            Phone = data.Hotel.Phone,
                            Fax = data.Hotel.Fax,
                            Email = data.Hotel.Email,
                            Hotline = data.Hotel.Hotline,
                            Address = data.Hotel.Address,
                            Website = data.Hotel.Website,
                            Facebook = data.Hotel.Facebook,
                            Skyper = data.Hotel.Skyper,
                            Google = data.Hotel.Google,
                            Youtobe = data.Hotel.Youtobe,
                            HighLight = 1,
                            TypeSoftware = data.Hotel.TypeSoftware,
                            CreateDate = dateTimeNow,
                            TypePaymentHotel = data.Hotel.TypePaymentHotel,
                            CommissionRate = data.Hotel.CommissionRate,
                            TimeExtended = data.Hotel.TimeExtended
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                    if (hotelId < 0)
                    {
                        transaction.Dispose();
                        return Json(-2, JsonRequestBehavior.AllowGet);
                    }
                    #endregion
                    #region add user
                    int userId = connection.QuerySingleOrDefault<int>("User_Post_MasterRole",
                        new
                        {
                            UserName = data.User.UserName,
                            Password = data.User.Password,//DataHelper.CreateMD5(data.User.UserName + data.User.Password + DataHelper.key),
                            Photo = data.User.Photo,
                            Email = data.User.Email,
                            FullName = data.User.FullName,
                            CreateDate = DatetimeHelper.DateTimeUTCNow()
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if (userId < 0)
                    {
                        transaction.Dispose();
                        return Json(-3, JsonRequestBehavior.AllowGet);
                    }
                    #endregion
                    #region add userHotel
                    connection.Execute("UserHotel_Post",
                           new
                           {
                               UserId = userId,
                               HotelId = hotelId,
                               Access = 1
                           }, commandType: CommandType.StoredProcedure,
                           transaction: transaction);
                    #endregion
                    // nếu screens = null thì add toàn bộ role các screen
                    // ngược lại add theo danh sách screens
                    switch (data.Hotel.TypeSoftware)
                    {
                        case 3:
                            connection.Execute("UserHotel_AddRoleFull",
                            new
                            {
                                UserId = userId,
                                Access = 1
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                            break;
                        case 1:
                            ScreenForPMS.ForEach(x =>
                            {
                                connection.Execute("UserRole_Post",
                                    new
                                    {
                                        UserId = userId,
                                        RoleId = x,
                                        Access = 1
                                    }, commandType: CommandType.StoredProcedure,
                                    transaction: transaction);
                            });
                            break;
                        case 2:
                            ScreenForBE.ForEach(x =>
                            {
                                connection.Execute("UserRole_Post",
                                    new
                                    {
                                        UserId = userId,
                                        RoleId = x,
                                        Access = 1
                                    }, commandType: CommandType.StoredProcedure,
                                    transaction: transaction);
                            });
                            break;
                    }
                    transaction.Commit();
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}