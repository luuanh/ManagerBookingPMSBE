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
    public class ExtrabedController : SercurityController
    {
        // GET: Admin/Extrabed
        public ActionResult Index()
        {
            if (!CheckSecurity(39))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 39
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
                using (var multi = connection.QueryMultiple("Extrabed_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        LanguageId = LanguageId,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Extrabed> extrabeds = multi.Read<Extrabed>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        extrabeds = extrabeds,
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
                using (var multi = connection.QueryMultiple("Extrabed_Detail_Full",
                    new
                    {
                        ExtrabedId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    Extrabed extrabed = multi.Read<Extrabed>().SingleOrDefault();
                    if (extrabed is null)
                        extrabed = new Extrabed();
                    List<ExtrabedLanguage> extrabedLanguages = multi.Read<ExtrabedLanguage>().ToList();

                    if (extrabedLanguages is null)
                        extrabedLanguages = new List<ExtrabedLanguage>();

                    List<int> languageId = multi.Read<int>().ToList();
                    if (languageId != null)
                        languageId.ForEach(x =>
                        {
                            if (extrabedLanguages.FindIndex(y => y.LanguageId == x) < 0)
                            {
                                extrabedLanguages.Add(new ExtrabedLanguage()
                                {
                                    ExtrabedId = extrabed.ExtrabedId,
                                    LanguageId = x,
                                    ExtrabedName = "",
                                    Description = "",
                                    ExtrabedLanguageId = -1
                                });
                            }
                        });
                    extrabed.ExtrabedLanguages = extrabedLanguages;
                    return Json(extrabed, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Post(Extrabed extrabed)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    int extrabedId = connection.QuerySingleOrDefault<int>("Extrabed_Post",
                    new
                    {
                        HotelId = HotelId,
                        Price = extrabed.Price,
                        Image = extrabed.Image
                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);

                    if (extrabed.ExtrabedLanguages != null)
                    {
                        extrabed.ExtrabedLanguages.ForEach(x =>
                        {
                            connection.Execute("ExtrabedLanguage_Post",
                                new
                                {
                                    ExtrabedId = extrabedId,
                                    LanguageId = x.LanguageId,
                                    ExtrabedName = x.ExtrabedName,
                                    Description = x.Description
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(Extrabed extrabed)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // update extrabed
                    connection.Execute("Extrabed_Put",
                        new
                        {
                            ExtrabedId = extrabed.ExtrabedId,
                            Price = extrabed.Price,
                            Image = extrabed.Image
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    // update extrabed includes extrabedname,description
                    if (extrabed.ExtrabedLanguages != null)
                        extrabed.ExtrabedLanguages.ForEach(x =>
                        {
                            if (x.ExtrabedLanguageId < 0)
                            {
                                connection.Execute("ExtrabedLanguage_Post",
                                new
                                {
                                    ExtrabedId = extrabed.ExtrabedId,
                                    LanguageId = x.LanguageId,
                                    ExtrabedName = x.ExtrabedName,
                                    Description = x.Description
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            }
                            else
                            {
                                connection.Execute("ExtrabedLanguage_Put",
                                new
                                {
                                    ExtrabedId = extrabed.ExtrabedId,
                                    LanguageId = x.LanguageId,
                                    ExtrabedName = x.ExtrabedName,
                                    Description = x.Description
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            }
                        });
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
                connection.Execute("Extrabed_Delete",
                    new
                    {
                        ExtrabedId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAll()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Extrabed> extrabeds = connection.Query<Extrabed>("Extrabed_Get_Sample",
                    new
                    {
                        HotelId = HotelId,
                        LanguageId = LanguageId
                    }, commandType: CommandType.StoredProcedure).ToList();
                return Json(extrabeds, JsonRequestBehavior.AllowGet);
            }
        }
    }
}