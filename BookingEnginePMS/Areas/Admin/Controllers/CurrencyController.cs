using BookingEnginePMS.Helper;
using BookingEnginePMS.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class CurrencyController : SercurityController
    {
        public ActionResult Index()
        {
            if (!CheckSecurity(44))
                return Redirect("/Admin/Login/Index");
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 44
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public JsonResult GetData(string typeCurrency)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                ConfigCurrency currency = connection.QuerySingleOrDefault<ConfigCurrency>("Currency_GetData",
                     new
                     {
                         HotelId = HotelId,
                         CurrencyCode = typeCurrency
                     },
                     commandType: System.Data.CommandType.StoredProcedure);
                if (currency is null) currency = new ConfigCurrency(typeCurrency);
                return Json(currency, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Get()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                List<Currency> currencies = connection.Query<Currency>("Currency_Get",
                    commandType: System.Data.CommandType.StoredProcedure).ToList();
                return Json(currencies, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(ConfigCurrency currency)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];

            using (var connection = DB.ConnectionFactory())
            {
                if (currency.ConfigCurrencyId < 0)
                    connection.Execute("ConfigCurrency_Post",
                        new
                        {
                            HotelId = HotelId,
                            CurrencyCode = currency.CurrencyCode,
                            Result = Math.Round(currency.Result, 6),
                            AutoCalculator = currency.AutoCalculator
                        }, commandType: CommandType.StoredProcedure);
                else
                {
                    connection.Execute("ConfigCurrency_Put",
                        new
                        {
                            ConfigCurrencyId = currency.ConfigCurrencyId,
                            CurrencyCode = currency.CurrencyCode,
                            Result = Math.Round(currency.Result, 6),
                            AutoCalculator = currency.AutoCalculator
                        }, commandType: CommandType.StoredProcedure);
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetValueConvertCurrentFromBank(string typeCurrency)
        {
            typeCurrency = typeCurrency.Trim();
            float result = DataHelper.CurrencyConvertor(typeCurrency);
            return Json(Math.Round(result, 6), JsonRequestBehavior.AllowGet);
        }
    }
}