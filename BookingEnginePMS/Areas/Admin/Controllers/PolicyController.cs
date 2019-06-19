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
    public class PolicyController : SercurityController
    {
        // GET: Admin/Policy
        public ActionResult Index()
        {
            if (!CheckSecurity(46))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 46
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
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Policy_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        LanguageId = LanguageId,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Policy> policies = multi.Read<Policy>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        policies = policies,
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
                using (var multi = connection.QueryMultiple("Policy_Detail_Full",
                    new
                    {
                        PolicyId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    Policy policy = multi.Read<Policy>().SingleOrDefault();
                    if (policy is null)
                        policy = new Policy();
                    List<PolicyLanguage> policyLanguages = multi.Read<PolicyLanguage>().ToList();

                    if (policyLanguages is null)
                        policyLanguages = new List<PolicyLanguage>();

                    List<int> languages = multi.Read<int>().ToList();
                    if (languages != null)
                        languages.ForEach(x =>
                        {
                            if (policyLanguages.FindIndex(y => y.LanguageId == x) < 0)
                            {
                                policyLanguages.Add(new PolicyLanguage()
                                {
                                    PolicyLanguageId = -1,
                                    LanguageId = x,
                                    Content = "",
                                    PolicyName = "",
                                    PolicyId = policy.PolicyId
                                });
                            }
                        });
                    policy.PolicyLanguages = policyLanguages;
                    return Json(policy, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Post(Policy policy)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    int policyId = connection.QuerySingleOrDefault<int>("Policy_Post",
                    new
                    {
                        HotelId = HotelId,
                        Index = policy.Index,
                        RequirePrice = policy.RequirePrice
                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);

                    if (policy.PolicyLanguages != null)
                    {
                        policy.PolicyLanguages.ForEach(x =>
                        {
                            connection.Execute("PolicyLanguage_Post",
                                new
                                {
                                    PolicyId = policyId,
                                    LanguageId = x.LanguageId,
                                    PolicyName = x.PolicyName,
                                    Content = x.Content
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(Policy policy)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // update policy
                    connection.Execute("Policy_Put",
                        new
                        {
                            PolicyId = policy.PolicyId,
                            Index = policy.Index,
                            RequirePrice = policy.RequirePrice
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    // update amanity includes content, name
                    connection.Execute("PolicyLanguage_Delete_Full",
                        new
                        {
                            PolicyId = policy.PolicyId
                        },
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if (policy.PolicyLanguages != null)
                    {
                        policy.PolicyLanguages.ForEach(x =>
                        {
                            connection.Execute("PolicyLanguage_Post",
                                new
                                {
                                    PolicyId = policy.PolicyId,
                                    LanguageId = x.LanguageId,
                                    PolicyName = x.PolicyName,
                                    Content = x.Content
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
                connection.Execute("Policy_Delete",
                    new
                    {
                        PolicyId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}