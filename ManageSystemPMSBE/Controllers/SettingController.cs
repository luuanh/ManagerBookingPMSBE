using Dapper;
using ManageSystemPMSBE.DTCore;
using ManageSystemPMSBE.Models;
using System.Data;
using System.Web.Mvc;

namespace ManageSystemPMSBE.Controllers
{
    public class SettingController : SercurityController
    {
        // GET: Setting
        public ActionResult RenewalRegistration()
        {
            if (!CheckSecurity())
                return Redirect("/Login/Index");
            return View();
        }
        public ActionResult UpdateSystem()
        {
            if (!CheckSecurity())
                return Redirect("/Login/Index");
            return View();
        }



        public JsonResult GetRenewalRegistration(int languageId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                RenewalRegistration renewalRegistrations = connection.QuerySingleOrDefault<RenewalRegistration>("RenewalRegistration_Detail_Full",
                    new
                    {
                        LanguageId = languageId
                    }, commandType: CommandType.StoredProcedure);
                if (renewalRegistrations is null)
                {
                    renewalRegistrations = new RenewalRegistration()
                    {
                        LanguageId = languageId,
                        Template = ""
                    };
                }
                return Json(renewalRegistrations, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PutRenewalRegistration(RenewalRegistration renewalRegistration)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                connection.Execute("RenewalRegistration_Put",
                    new
                    {
                        LanguageId = renewalRegistration.LanguageId,
                        Template = renewalRegistration.Template
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetUpdateSystem(int languageId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                NotificationSystemLanguage notificationSystem = connection.QuerySingleOrDefault<NotificationSystemLanguage>("NotificationSystemLanguage_Detail",
                    new
                    {
                        LanguageId = languageId
                    }, commandType: CommandType.StoredProcedure);
                if (notificationSystem is null)
                {
                    notificationSystem = new NotificationSystemLanguage()
                    {
                        LanguageId = languageId,
                        Content = "",
                        Status = false
                    };
                }
                return Json(notificationSystem, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PutUpdateSystem(NotificationSystemLanguage notificationSystem)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactoryBEPMS())
            {
                connection.Execute("NotificationSystemLanguage_Put",
                    new
                    {
                        LanguageId = notificationSystem.LanguageId,
                        Content = notificationSystem.Content,
                        Status = notificationSystem.Status
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}