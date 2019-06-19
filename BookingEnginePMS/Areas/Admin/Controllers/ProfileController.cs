using BookingEnginePMS.Helper;
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
    public class ProfileController : SercurityController
    {
        // GET: Admin/Profile
        public ActionResult Index()
        {
            if (!CheckSecurity())
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
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
        public JsonResult Detail()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            User user = (User)Session["User"];
            using (var connection = DB.ConnectionFactory())
            {
                User userP = connection.QuerySingleOrDefault<User>("User_Profile",
                    new
                    {
                        UserName = user.UserName
                    }, commandType: CommandType.StoredProcedure);
                return Json(userP, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(User user)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactory())
            {
                User userP = connection.QuerySingleOrDefault<User>("User_Put",
                    new
                    {
                        UserName = user.UserName,
                        Photo = user.Photo,
                        Email = user.Email,
                        FullName = user.FullName
                    }, commandType: CommandType.StoredProcedure);
                User userAfterChange = (User)Session["User"];
                userAfterChange.Photo = user.Photo;
                Session["User"] = userAfterChange;
                return Json(userP, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ChangePassword(User user)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            if (user.NewPassword != user.ConfirmPassword)
                return Json(-1, JsonRequestBehavior.AllowGet);
            string oldPass = DataHelper.CreateMD5(user.UserName + user.Password + DataHelper.key);
            string newPass = DataHelper.CreateMD5(user.UserName + user.NewPassword + DataHelper.key);
            using (var connection = DB.ConnectionFactory())
            {
                int result = connection.QuerySingleOrDefault<int>("User_ChangePassword",
                    new
                    {
                        UserName = user.UserName,
                        OldPassword = oldPass,
                        NewPassword = newPass
                    }, commandType: CommandType.StoredProcedure);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}