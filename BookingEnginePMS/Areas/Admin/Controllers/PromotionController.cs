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
    public class PromotionController : SercurityController
    {
        // GET: Admin/Promotion
        public ActionResult Index()
        {
            if (!CheckSecurity(14))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 14
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public ActionResult InActive()
        {
            if (!CheckSecurity(14))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 14
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public ActionResult New()
        {
            if (!CheckSecurity(15))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 15
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult GetDataConfigForScreenEdit()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            int LanguageId = (int)Session["LanguageId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Promotion_Get_DataConfigNeedToScreenEdit",
                        new
                        {
                            HotelId = HotelId,
                            LanguageId = LanguageId
                        }, commandType: CommandType.StoredProcedure))
                {
                    List<PlaneRate> planeRates = multi.Read<PlaneRate>().ToList();
                    if (planeRates is null)
                        planeRates = new List<PlaneRate>();

                    List<PlaneRateRoomType> planeRateRoomTypes = multi.Read<PlaneRateRoomType>().ToList();
                    if (planeRateRoomTypes is null)
                        planeRateRoomTypes = new List<PlaneRateRoomType>();
                    planeRates.ForEach(x =>
                    {
                        x.PlaneRateRoomTypes = planeRateRoomTypes.FindAll(y => y.PlaneRateId == x.PlaneRateId);
                    });

                    List<Policy> policies = multi.Read<Policy>().ToList();
                    if (policies is null)
                        policies = new List<Policy>();

                    return Json(new
                    {
                        planeRates = planeRates,
                        policies = policies
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult Edit(int id)
        {
            if (!CheckSecurity(15))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 15
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Post(Promotion promotion)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    if (promotion is null)
                        promotion = new Promotion();
                    int promotionId = connection.QuerySingleOrDefault<int>("Promotion_Post",
                        new
                        {
                            HotelId = HotelId,
                            PlaneRateId = promotion.PlaneRateId,
                            TypePromotion = promotion.TypePromotion,
                            PromotionName = promotion.PromotionName,
                            DayInHouse = promotion.DayInHouse,
                            EarlyDay = promotion.EarlyDay,
                            NightForFreeNight = promotion.NightForFreeNight,
                            Deposit = promotion.Deposit,
                            FromDate = promotion.FromDate,
                            ToDate = promotion.ToDate,
                            Monday = promotion.Monday,
                            Tuesday = promotion.Tuesday,
                            Wednesday = promotion.Wednesday,
                            Thursday = promotion.Thursday,
                            Friday = promotion.Friday,
                            Saturday = promotion.Saturday,
                            Sunday = promotion.Sunday,
                            PolicyId = promotion.PolicyId,
                            AmountRate = promotion.isRequireRate ? promotion.AmountRate : 0
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if (promotion.PromotionRoomTypes is null)
                        promotion.PromotionRoomTypes = new List<PromotionRoomType>();
                    promotion.PromotionRoomTypes.ForEach(x =>
                    {
                        if (x.Checked)
                        {
                            connection.Execute("PromotionRoomType_Post",
                            new
                            {
                                PromotionId = promotionId,
                                RoomTypeId = x.RoomTypeId
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                        }
                    });

                    if (promotion.PromotionLanguages is null)
                        promotion.PromotionLanguages = new List<PromotionLanguage>();
                    promotion.PromotionLanguages.ForEach(x =>
                    {
                        connection.Execute("PromotionLanguage_Post",
                            new
                            {
                                PromotionId = promotionId,
                                LanguageId = x.LanguageId,
                                Description = x.Description,
                                Note = x.Note
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(Promotion promotion)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    if (promotion is null)
                        promotion = new Promotion();
                    connection.Execute("Promotion_Put",
                        new
                        {
                            PromotionId = promotion.PromotionId,
                            PlaneRateId = promotion.PlaneRateId,
                            TypePromotion = promotion.TypePromotion,
                            PromotionName = promotion.PromotionName,
                            DayInHouse = promotion.DayInHouse,
                            EarlyDay = promotion.EarlyDay,
                            NightForFreeNight = promotion.NightForFreeNight,
                            Deposit = promotion.Deposit,
                            FromDate = promotion.FromDate,
                            ToDate = promotion.ToDate,
                            Monday = promotion.Monday,
                            Tuesday = promotion.Tuesday,
                            Wednesday = promotion.Wednesday,
                            Thursday = promotion.Thursday,
                            Friday = promotion.Friday,
                            Saturday = promotion.Saturday,
                            Sunday = promotion.Sunday,
                            PolicyId = promotion.PolicyId,
                            AmountRate = promotion.isRequireRate ? promotion.AmountRate : 0

                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if (promotion.PromotionRoomTypes is null)
                        promotion.PromotionRoomTypes = new List<PromotionRoomType>();
                    connection.Execute("PromotionRoomType_Delete_Full",
                        new
                        {
                            PromotionId = promotion.PromotionId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    promotion.PromotionRoomTypes.ForEach(x =>
                    {
                        if (x.Checked)
                        {
                            connection.Execute("PromotionRoomType_Post",
                            new
                            {
                                PromotionId = promotion.PromotionId,
                                RoomTypeId = x.RoomTypeId
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                        }
                    });

                    if (promotion.PromotionLanguages is null)
                        promotion.PromotionLanguages = new List<PromotionLanguage>();
                    connection.Execute("PromotionLanguage_Delete_Full",
                        new
                        {
                            PromotionId = promotion.PromotionId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    promotion.PromotionLanguages.ForEach(x =>
                    {
                        connection.Execute("PromotionLanguage_Post",
                            new
                            {
                                PromotionId = promotion.PromotionId,
                                LanguageId = x.LanguageId,
                                Description = x.Description,
                                Note = x.Note
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }   
        }
        public JsonResult GetActive()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Promotion_Get",
                    new
                    {
                        HotelId = HotelId,
                        Status = 1
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Promotion> promotion = multi.Read<Promotion>().ToList();
                    if (promotion is null)
                        promotion = new List<Promotion>();
                    List<PromotionRoomType> promotionRoomTypes = multi.Read<PromotionRoomType>().ToList();
                    if (promotionRoomTypes is null)
                        promotionRoomTypes = new List<PromotionRoomType>();
                    promotion.ForEach(x =>
                    {
                        x.PromotionRoomTypes = promotionRoomTypes.FindAll(y => y.PromotionId == x.PromotionId);
                    });
                    return Json(JsonConvert.SerializeObject(promotion), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult GetInActive()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Promotion_Get",
                    new
                    {
                        HotelId = HotelId,
                        Status = 0
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Promotion> promotion = multi.Read<Promotion>().ToList();
                    if (promotion is null)
                        promotion = new List<Promotion>();
                    List<PromotionRoomType> promotionRoomTypes = multi.Read<PromotionRoomType>().ToList();
                    if (promotionRoomTypes is null)
                        promotionRoomTypes = new List<PromotionRoomType>();
                    promotion.ForEach(x =>
                    {
                        x.PromotionRoomTypes = promotionRoomTypes.FindAll(y => y.PromotionId == x.PromotionId);
                    });
                    return Json(JsonConvert.SerializeObject(promotion), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Deactivate(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("Promotion_Update_Status",
                    new
                    {
                        PromotionId = id,
                        Status = 0
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Reactivate(int id)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Execute("Promotion_Update_Status",
                    new
                    {
                        PromotionId = id,
                        Status = 1
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
                connection.Execute("Promotion_Delete",
                    new
                    {
                        PromotionId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Detail(int id)
        {
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Promotion_Detail_Full",
                    new
                    {
                        PromotionId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    Promotion promotion = multi.Read<Promotion>().SingleOrDefault();
                    if (promotion is null)
                        promotion = new Promotion();
                    List<PromotionLanguage> promotionLanguages = multi.Read<PromotionLanguage>().ToList();
                    if (promotionLanguages is null)
                        promotionLanguages = new List<PromotionLanguage>();
                    List<PromotionRoomType> promotionRoomTypes = multi.Read<PromotionRoomType>().ToList();
                    if (promotionRoomTypes is null)
                        promotionRoomTypes = new List<PromotionRoomType>();
                    List<Language> languages = multi.Read<Language>().ToList();
                    if (languages is null)
                        languages = new List<Language>();
                    List<RoomType> roomTypes = multi.Read<RoomType>().ToList();
                    if (roomTypes is null)
                        roomTypes = new List<RoomType>();

                    List<PromotionLanguage> promotionLanguagesResult = new List<PromotionLanguage>();
                    List<PromotionRoomType> promotionRoomTypesResult = new List<PromotionRoomType>();
                    languages.ForEach(x =>
                    {
                        PromotionLanguage pl = promotionLanguages.Find(y => y.LanguageId == x.LanguageId);
                        if (pl is null)
                        {
                            pl = new PromotionLanguage()
                            {
                                PromotionLanguageId = -1,
                                LanguageId = x.LanguageId,
                                Note = "",
                                Description = ""
                            };
                        }
                        promotionLanguagesResult.Add(pl);
                    });
                    roomTypes.ForEach(x =>
                    {
                        PromotionRoomType pr = promotionRoomTypes.Find(y => y.RoomTypeId == x.RoomTypeId);
                        if (pr is null)
                        {
                            pr = new PromotionRoomType()
                            {
                                PromotionRoomTypeId = -1,
                                RoomTypeId = x.RoomTypeId,
                                Checked = false
                            };
                        }
                        else
                            pr.Checked = true;
                        pr.RoomTypeName = x.RoomTypeName;
                        promotionRoomTypesResult.Add(pr);
                    });
                    promotion.PromotionLanguages = promotionLanguagesResult;
                    promotion.PromotionRoomTypes = promotionRoomTypesResult;
                    return Json(JsonConvert.SerializeObject(promotion), JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}