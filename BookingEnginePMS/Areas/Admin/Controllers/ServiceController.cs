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
    public class ServiceController : SercurityController
    {
        // GET: Admin/Service
        public ActionResult Index()
        {
            if (!CheckSecurity(49))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 49
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Get(string keySearch, int serviceCategoryId, int pageNumber, int pageSize)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Service_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        LanguageId = LanguageId,
                        HotelId = HotelId,
                        serviceCategoryId = serviceCategoryId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Service> services = multi.Read<Service>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        services = services,
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
                using (var multi = connection.QueryMultiple("Service_Detail_Full",
                    new
                    {
                        ServiceId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    Service service = multi.Read<Service>().SingleOrDefault();
                    if (service is null)
                        service = new Service();
                    List<ServiceLanguage> serviceLanguages = multi.Read<ServiceLanguage>().ToList();
                    if (serviceLanguages is null)
                        serviceLanguages = new List<ServiceLanguage>();

                    List<int> languageId = multi.Read<int>().ToList();
                    if (languageId != null)
                        languageId.ForEach(x =>
                        {
                            if (serviceLanguages.FindIndex(y => y.LanguageId == x) < 0)
                            {
                                serviceLanguages.Add(new ServiceLanguage()
                                {
                                    LanguageId = x,
                                    ServiceName = "",
                                    Description = ""
                                });
                            }
                        });
                    service.ServiceLanguages = serviceLanguages;
                    return Json(service, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Post(Service service)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    int serviceId = connection.QuerySingleOrDefault<int>("Service_Post",
                    new
                    {
                        HotelId = HotelId,
                        ServiceCategoryId = service.ServiceCategoryId,
                        ServiceCode = service.ServiceCode,
                        Photo = service.Photo,
                        Index = service.Index,
                        BuyOnline = service.BuyOnline,
                        Price = service.Price

                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);

                    if (service.ServiceLanguages != null)
                    {
                        service.ServiceLanguages.ForEach(x =>
                        {
                            connection.Execute("ServiceLanguages_Post",
                                new
                                {
                                    ServiceId = serviceId,
                                    LanguageId = x.LanguageId,
                                    ServiceName = x.ServiceName,
                                    Description = x.Description,
                                    Policy = x.Policy
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(Service service)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.QuerySingleOrDefault<int>("Service_Put",
                    new
                    {
                        ServiceId = service.ServiceId,
                        ServiceCategoryId = service.ServiceCategoryId,
                        ServiceCode = service.ServiceCode,
                        Photo = service.Photo,
                        Index = service.Index,
                        BuyOnline = service.BuyOnline,
                        Price = service.Price

                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);
                    connection.Execute("ServiceLanguages_Delete_Full",
                        new
                        {
                            ServiceId = service.ServiceId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if (service.ServiceLanguages != null)
                    {
                        service.ServiceLanguages.ForEach(x =>
                        {
                            connection.Execute("ServiceLanguages_Post",
                                new
                                {
                                    ServiceId = service.ServiceId,
                                    LanguageId = x.LanguageId,
                                    ServiceName = x.ServiceName,
                                    Description = x.Description,
                                    Policy = x.Policy
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
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
                connection.Execute("Service_Delete",
                    new
                    {
                        ServiceId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAll(int serviceCategoryId,string voucher)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Service> services = connection.Query<Service>("Service_GetAll_By_ServiceCategoryId",
                    new
                    {
                        LanguageId = LanguageId,
                        ServiceCategoryId = serviceCategoryId,
                        VoucherCode = voucher,
                        Date = DatetimeHelper.DateTimeUTCNow()
                    },
                     commandType: CommandType.StoredProcedure).ToList();
                return Json(services, JsonRequestBehavior.AllowGet);
            }
        }
    }
}