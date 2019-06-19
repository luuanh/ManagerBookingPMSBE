using BookingEnginePMS.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class TaxFeeController : SercurityController
    {
        // GET: Admin/TaxFee
        public ActionResult Index()
        {
            if (!CheckSecurity(45))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 45
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
            using (var connection = DB.ConnectionFactory())
            {
                List<TaxFee> taxFees = connection.Query<TaxFee>("TaxFee_Get",
                        new
                        {
                            HotelId = HotelId
                        }, commandType: CommandType.StoredProcedure).ToList();
                int totalRecord = taxFees.Count;
                return Json(new
                {
                    taxFees = taxFees,
                    totalRecord = totalRecord
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Detail(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                TaxFee taxFee = connection.QuerySingleOrDefault<TaxFee>("TaxFee_Detail",
                    new
                    {
                        TaxFeeId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(taxFee, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Post(TaxFee taxFee)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("TaxFee_Post",
                    new
                    {
                        HotelId = HotelId,
                        Amount = taxFee.Amount,
                        Description = taxFee.Description,
                        TypeTaxFee = taxFee.TypeTaxFee
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(TaxFee taxFee)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("TaxFee_Put",
                    new
                    {
                        TaxFeeId = taxFee.TaxFeeId,
                        Amount = taxFee.Amount,
                        Description = taxFee.Description,
                        TypeTaxFee = taxFee.TypeTaxFee
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("TaxFee_Delete",
                    new
                    {
                        TaxFeeId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAll()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TaxFee> taxFees = connection.Query<TaxFee>("TaxFee_GetAll",
                    new
                    {
                        HotelId = HotelId
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                return Json(taxFees, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetToalFee_Lastest()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                string[] taxFees = connection.QuerySingleOrDefault<string>("TaxFee_GetTotal_Lastest",
                       new
                       {
                           HotelId = HotelId
                       },
                       commandType: CommandType.StoredProcedure).Split(',');
                float tax = float.Parse(taxFees[0]);
                float fee = float.Parse(taxFees[1]);
                return Json(new float[] { tax, fee }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}