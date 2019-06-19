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
    public class CompanyController : SercurityController
    {
        // GET: Admin/Company
        public ActionResult Index()
        {
            if (!CheckSecurity(12))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 12
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Get(string keySearch = "", int pageNumber = 1, int pageSize = 1000)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Company_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Company> companies = multi.Read<Company>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        companies = companies,
                        totalRecord = totalRecord
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Detail(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                Company company = connection.QuerySingleOrDefault<Company>("Company_Detail",
                    new
                    {
                        CompanyId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(company, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Post(Company company)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("Company_Post",
                    new
                    {
                        HotelId = HotelId,
                        GroupGuestId = company.GroupGuestId,
                        SourceId = company.SourceId,
                        CompanyName = company.CompanyName,
                        CompanyCode = "",
                        TaxCode = company.TaxCode,
                        Phone = company.Phone,
                        Fax = company.Fax,
                        Email = company.Email,
                        Address = company.Address,
                        ContactUsName = company.ContactUsName,
                        ContactPhone = company.ContactPhone,
                        ContactEmail = company.ContactEmail
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(Company company)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("Company_Put",
                    new
                    {
                        CompanyId = company.CompanyId,
                        GroupGuestId = company.GroupGuestId,
                        SourceId = company.SourceId,
                        CompanyName = company.CompanyName,
                        CompanyCode = "",
                        TaxCode = company.TaxCode,
                        Phone = company.Phone,
                        Fax = company.Fax,
                        Email = company.Email,
                        Address = company.Address,
                        ContactUsName = company.ContactUsName,
                        ContactPhone = company.ContactPhone,
                        ContactEmail = company.ContactEmail
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}