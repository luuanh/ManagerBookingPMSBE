using BookingEnginePMS.Helper;
using BookingEnginePMS.Models;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class InvoiceController : SercurityController
    {
        // GET: Admin/Invoice
        public ActionResult Index()
        {
            if (!CheckSecurity(20))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 20
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
            }
            return View();
        }
        public JsonResult Get(string keySearch, int pageNumber, int pageSize, int status, int feedback, DateTime startDate, DateTime endDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Invoice_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        status = status,
                        feedback = feedback,
                        startDate = startDate,
                        endDate = endDate,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Invoice> invoices = multi.Read<Invoice>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(JsonConvert.SerializeObject(
                        new
                        {
                            invoices = invoices,
                            totalRecord = totalRecord
                        }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Detail(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                Invoice invoice = connection.QuerySingleOrDefault<Invoice>("Invoice_Detail",
                    new
                    {
                        InvoiceId = id
                    }, commandType: CommandType.StoredProcedure);
                invoice.NameOnCard = DataHelper.Decrypt(invoice.NameOnCard);
                invoice.SecurityCode = DataHelper.Decrypt(invoice.SecurityCode);
                invoice.CardNumber = DataHelper.Decrypt(invoice.CardNumber);
                return Json(JsonConvert.SerializeObject(invoice), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Post(Invoice invoice)
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
                    int invoiceId = connection.QuerySingleOrDefault<int>("Invoice_Post",
                     new
                     {
                         HotelId = HotelId,
                         Title = "",
                         CustomerName = invoice.CustomerName,
                         Phone = invoice.Phone,
                         Email = invoice.Email,
                         TotalAmount = invoice.TotalAmount,
                         CurrencyCode = invoice.CurrencyCode,
                         DateInvoice = DatetimeHelper.DateTimeUTCNow(),
                         ArrivalDate = invoice.ArrivalDate,
                         Description = invoice.Description,
                         UserCreate = user.UserName
                     }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                    string link = ((System.Web.HttpRequestWrapper)Request).Url.Authority + "/Home/Invoice?code=" + invoiceId + "&email=" + invoice.Email;
                    connection.Execute("Invoice_Put_Link",
                        new
                        {
                            InvoiceId = invoiceId,
                            Link = link
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    transaction.Commit();
                }

                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(Invoice invoice)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    string link = ((System.Web.HttpRequestWrapper)Request).Url.Authority + "/Home/Invoice?code=" + invoice.InvoiceId + "&email=" + invoice.Email;
                    connection.Execute("Invoice_Put",
                     new
                     {
                         InvoiceId = invoice.InvoiceId,
                         Title = "",
                         CustomerName = invoice.CustomerName,
                         Phone = invoice.Phone,
                         Email = invoice.Email,
                         TotalAmount = invoice.TotalAmount,
                         CurrencyCode = invoice.CurrencyCode,
                         ArrivalDate = invoice.ArrivalDate,
                         Description = invoice.Description,
                         Link = link
                     }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    transaction.Commit();
                }

                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("Invoice_Delete",
                    new
                    {
                        InvoiceId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Paid(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("Invoice_Paid",
                    new
                    {
                        InvoiceId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ClearCard(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("Invoice_ClearCard",
                    new
                    {
                        InvoiceId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}