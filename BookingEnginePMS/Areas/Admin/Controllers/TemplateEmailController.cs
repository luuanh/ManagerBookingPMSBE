using BookingEnginePMS.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class TemplateEmailController : SercurityController
    {
        // GET: Admin/TemplateEmail
        public static string[] keyTemplate = new string[] {
            "[HotelName]","[HotelEmail]","[HotelPhone]","[BookingId]","[FirstName]","[SurName]","[GuestEmail]",
            "[GuestPhone]","[Room]","[PriceRoom]","[unitprice]","[PriceExtrabed]","[ExtrabedInfor]","[Service]",
            "[PriceService]","[Voucher]","[PriceVoucher]","[Checkin]","[Chechout]","[Adults]","[Childrent]","[ArrivalFlightNumber]",
            "[ArrivalTimeFlight]","[OrtherRequest]","[TotalPrice]","TotalVAT","[Deposit]","[Balance]"
        };
        public ActionResult Index()
        {
            if (!CheckSecurity(51))
                return Redirect("/Admin/Login/Index");
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 51
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                Session["keyTemplate"] = keyTemplate.ToList();
            }
            return View();
        }
        public JsonResult Get(int typeEmailId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                TemplateEmail template = connection.QuerySingleOrDefault<TemplateEmail>("TemplateEmail_Get",
                    new
                    {
                        TypeEmailId = typeEmailId,
                        HotelId = HotelId
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
                if (template is null) template = new TemplateEmail(typeEmailId, HotelId);
                return Json(template, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(TemplateEmail template)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("TemplateEmail_Put",
                    new
                    {
                        HotelId = HotelId,
                        TypeEmailId = template.TypeEmailId,
                        CC = template.CC,
                        BCC = template.BCC,
                        Subject = template.Subject,
                        Content = template.Content
                    }, commandType: CommandType.StoredProcedure);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTypeTemplate()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                List<TypeEmail> currencies = connection.Query<TypeEmail>("TypeEmail_Get",
                    commandType: System.Data.CommandType.StoredProcedure).ToList();
                return Json(currencies, JsonRequestBehavior.AllowGet);
            }
        }
    }
}