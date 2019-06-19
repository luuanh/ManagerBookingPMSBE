using BookingEnginePMS.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class LanguageController : SercurityController
    {
        // GET: Admin/Language
        public ActionResult Index()
        {
            //if (!CheckSecurity(56))
            //    return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 56
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public ActionResult Translate()
        {
            //if (!CheckSecurity())
            //    return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 57
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Get(string keySearch, int pageNumber, int pageSize)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Language_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Language> languages = multi.Read<Language>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        languages = languages,
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
                Language language = connection.QuerySingleOrDefault<Language>("Language_Detail",
                    new
                    {
                        LanguageId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(language, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Post(Language language)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("Language_Post",
                    new
                    {
                        Key = language.Key,
                        Title = language.Title,
                        Ensign = language.Ensign,
                        Index = language.Index,
                        Active = language.Active
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(Language language)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("Language_Put",
                    new
                    {
                        LanguageId = language.LanguageId,
                        Key = language.Key,
                        Title = language.Title,
                        Ensign = language.Ensign,
                        Index = language.Index,
                        Active = language.Active
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
                connection.Execute("Language_Delete",
                    new
                    {
                        LanguageId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetActive()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                List<Language> language = connection.Query<Language>("Language_GetActive"
                    , commandType: CommandType.StoredProcedure).ToList();
                return Json(language, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetScreen()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Screen> language = connection.Query<Screen>("Language_GetScreen",
                    new
                    {
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure).ToList();
                return Json(language, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetTranslate(int languageId, int screenId, string keySearch)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("Language_GetTranslate",
                    new
                    {
                        languageId = languageId,
                        screenId = screenId,
                        keySearch = keySearch
                    }
                    , commandType: CommandType.StoredProcedure).ToList();
                return Json(transitions, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PutCellTranlate(TransitionLanguage transition)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            using (var connection = DB.ConnectionFactory())
            {
                if (transition.TransitionLanguageId > 0)
                {
                    connection.Execute("TransitionLanguage_Put",
                      new
                      {
                          TransitionLanguageId = transition.TransitionLanguageId,
                          Result = transition.Result
                      }, commandType: CommandType.StoredProcedure);
                }
                else
                {
                    connection.Execute("TransitionLanguage_Post",
                        new
                        {
                            TransitionId = transition.TransitionId,
                            LanguageId = transition.LanguageId,
                            Result = transition.Result
                        }, commandType: CommandType.StoredProcedure);
                }
                  
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PutAllCellTranlate(List<TransitionLanguage> transitions)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);
            if (transitions is null) transitions = new List<TransitionLanguage>();
            using (var connection = DB.ConnectionFactory())
            {
                transitions.ForEach(x =>
                {

                    connection.Execute("TransitionLanguage_Put",
                        new
                        {
                            TransitionLanguageId = x.TransitionLanguageId,
                            Result = x.Result
                        }, commandType: CommandType.StoredProcedure);
                });
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}