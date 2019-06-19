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
    public class ServiceCategoryController : SercurityController
    {
        // GET: Admin/ServiceCategory
        public ActionResult Index()
        {
            if (!CheckSecurity(48))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 48
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Get(string keySearch, int pageNumber, int pageSize)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("ServiceCategory_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        LanguageId = LanguageId,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<ServiceCategory> serviceCategories = multi.Read<ServiceCategory>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        serviceCategories = serviceCategories,
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
                using (var multi = connection.QueryMultiple("ServiceCategory_Detail_Full",
                    new
                    {
                        ServiceCategoryId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    ServiceCategory serviceCategory = multi.Read<ServiceCategory>().SingleOrDefault();
                    if (serviceCategory is null)
                        serviceCategory = new ServiceCategory();
                    List<ServiceCategoryLanguage> serviceCategoryLanguages = multi.Read<ServiceCategoryLanguage>().ToList();

                    if (serviceCategoryLanguages is null)
                        serviceCategoryLanguages = new List<ServiceCategoryLanguage>();

                    List<int> languageId = multi.Read<int>().ToList();
                    if (languageId != null)
                        languageId.ForEach(x =>
                        {
                            if (serviceCategoryLanguages.FindIndex(y => y.LanguageId == x) < 0)
                            {
                                serviceCategoryLanguages.Add(new ServiceCategoryLanguage()
                                {
                                    ServiceCategoryId = serviceCategory.ServiceCategoryId,
                                    LanguageId = x,
                                    ServiceCategoryName = "",
                                    ServiceCategoryLanguageId = -1
                                });
                            }
                        });
                    serviceCategory.ServiceCategoryLanguages = serviceCategoryLanguages;
                    return Json(serviceCategory, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Post(ServiceCategory serviceCategory)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    int serviceCategoryId = connection.QuerySingleOrDefault<int>("ServiceCategory_Post",
                    new
                    {
                        HotelId = HotelId,
                        Code = serviceCategory.Code,
                        Image = serviceCategory.Image,
                        Index = serviceCategory.Index
                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);

                    if (serviceCategory.ServiceCategoryLanguages != null)
                    {
                        serviceCategory.ServiceCategoryLanguages.ForEach(x =>
                        {
                            connection.Execute("ServiceCategoryLanguages_Post",
                                new
                                {
                                    LanguageId = x.LanguageId,
                                    ServiceCategoryId = serviceCategoryId,
                                    ServiceCategoryName = x.ServiceCategoryName
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(ServiceCategory serviceCategory)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // update extrabed
                    connection.Execute("ServiceCategory_Put",
                        new
                        {
                            ServiceCategoryId = serviceCategory.ServiceCategoryId,
                            Code = serviceCategory.Code,
                            Image = serviceCategory.Image,
                            Index = serviceCategory.Index
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    // update extrabed includes extrabedname,description
                    connection.Execute("ServiceCategoryLanguage_Delete_Full",
                        new
                        {
                            ServiceCategoryId = serviceCategory.ServiceCategoryId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if (serviceCategory.ServiceCategoryLanguages != null)
                    {
                        serviceCategory.ServiceCategoryLanguages.ForEach(x =>
                        {
                            connection.Execute("ServiceCategoryLanguages_Post",
                                new
                                {
                                    LanguageId = x.LanguageId,
                                    ServiceCategoryId = serviceCategory.ServiceCategoryId,
                                    ServiceCategoryName = x.ServiceCategoryName
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
                connection.Execute("ServiceCategory_Delete",
                    new
                    {
                        ServiceCategoryId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetFull()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<ServiceCategory> serviceCategories = connection.Query<ServiceCategory>("ServiceCategory_GetFull",
                         new
                         {
                             HotelId = HotelId,
                             LanguageId = LanguageId
                         }, commandType: CommandType.StoredProcedure).ToList();
                return Json(serviceCategories, JsonRequestBehavior.AllowGet);
            }
        }
    }
}