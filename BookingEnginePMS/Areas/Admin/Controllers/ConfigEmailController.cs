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
    public class ConfigEmailController : SercurityController
    {
        // GET: Admin/ConfigEmail
        public ActionResult Index()
        {
            if (!CheckSecurity(50))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 50
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Get()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                ConfigEmail configEmail = connection.QuerySingleOrDefault<ConfigEmail>("ConfigEmail_Get",
                    new
                    {
                        HotelId = HotelId
                    }, commandType: System.Data.CommandType.StoredProcedure);
                if (configEmail is null) configEmail = new ConfigEmail(HotelId);
                configEmail.Email = DataHelper.Decrypt(configEmail.Email);
                configEmail.EmailReceive = DataHelper.Decrypt(configEmail.EmailReceive);
                configEmail.Password = DataHelper.Decrypt(configEmail.Password);
                return Json(configEmail, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(ConfigEmail configEmail)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("ConfigEmail_Put",
                    new
                    {
                        HotelId = HotelId,
                        Email = DataHelper.Encrypt(configEmail.Email),
                        Password = DataHelper.Encrypt(configEmail.Password),
                        EmailReceive = DataHelper.Encrypt(configEmail.EmailReceive),
                        SubjectOffline = configEmail.SubjectOffline,
                        SubjectOnline = configEmail.SubjectOffline
                    }, commandType: System.Data.CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}