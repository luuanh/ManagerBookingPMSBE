using BookingEnginePMS.Helper;
using BookingEnginePMS.Models;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    #region class helper
    public class Promotion_RateAvailability
    {
        public int PromotionId { get; set; }
        public string PromotionName { get; set; }
        public float Deposit { get; set; }
        public float Price { get; set; }
        public List<float> EndPrices { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
    public class RoomType_RateAvailability
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public string RoomTypeCode { get; set; }
        public string Image { get; set; }
        public List<RateAvailability> RateAvailabilities { get; set; }
        public List<Promotion_RateAvailability> Promotions { get; set; }
    }
    public class ParamsChangeRoomSell
    {
        public int RoomTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Number { get; set; }
        public float Price { get; set; }
        public bool AllDay { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    }
    #endregion
    public class RateAvailabilityController : SercurityController
    {
        // GET: Admin/RateAvailability
        public ActionResult Index()
        {
            if (!CheckSecurity(13))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 13
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Get(DateTime fromDate, DateTime toDate, int typeSearch = -1)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            if (typeSearch > 0)
                toDate = fromDate.AddDays(typeSearch);
            using (var connection = DB.ConnectionFactory())
            {
                List<RoomType_RateAvailability> roomTypes = connection.Query<RoomType_RateAvailability>("RoomType_Get_Full",
                    new
                    {
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (roomTypes is null)
                    roomTypes = new List<RoomType_RateAvailability>();
                roomTypes.ForEach(x =>
                {
                    List<RateAvailability> rateAvailabilities = connection.Query<RateAvailability>("RateAvailability_Get_By_RoomType",
                        new
                        {
                            RoomTypeId = x.RoomTypeId,
                            FromDate = fromDate,
                            ToDate = toDate
                        }, commandType: CommandType.StoredProcedure).ToList();
                    if (rateAvailabilities is null)
                        rateAvailabilities = new List<RateAvailability>();
                    List<Promotion_RateAvailability> promotions = connection.Query<Promotion_RateAvailability>("RateAvailability_Get_Promotion_By_RoomType",
                        new
                        {
                            RoomTypeId = x.RoomTypeId
                        }, commandType: CommandType.StoredProcedure).ToList();
                    if (promotions is null)
                        promotions = new List<Promotion_RateAvailability>();
                    x.RateAvailabilities = rateAvailabilities;
                    x.Promotions = promotions;
                    x.Promotions.ForEach(y =>
                    {
                        y.EndPrices = new List<float>();
                    });
                });
                roomTypes.ForEach(x =>
                {
                    List<RateAvailability> rateAvailabilities = new List<RateAvailability>();
                    for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
                    {
                        RateAvailability rateAvailability = x.RateAvailabilities.Find(y => y.Date == date);
                        if (rateAvailability is null)
                            rateAvailability = new RateAvailability()
                            {
                                Date = date,
                                Init = false,
                                Number = 0,
                                Price = 0,
                                RoomTypeId = x.RoomTypeId,
                                Status = -1
                            };
                        else
                            rateAvailability.Init = true;
                        rateAvailability.DayofWeek = date.DayOfWeek.ToString();
                        rateAvailabilities.Add(rateAvailability);
                        x.Promotions.ForEach(y =>
                        {
                            float endPrice = rateAvailability.Price * (100 - y.Deposit) / 100 + y.Price;
                            if (endPrice < 0)
                                endPrice = 0;
                            y.EndPrices.Add((float)Math.Round(endPrice, 0));
                        });
                    }
                    x.RateAvailabilities = rateAvailabilities;
                });
                return Json(JsonConvert.SerializeObject(roomTypes), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetOnlyRoomType(DateTime fromDate, DateTime toDate, int roomtypeId)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                RoomType_RateAvailability roomType = connection.QuerySingleOrDefault<RoomType_RateAvailability>("RoomType_Get_FullOnlyOneRoomType",
                    new
                    {
                        RoomtypeId = roomtypeId
                    }, commandType: CommandType.StoredProcedure);
                if (roomType is null)
                    roomType = new RoomType_RateAvailability();
                List<RateAvailability> rateAvailabilities = connection.Query<RateAvailability>("RateAvailability_Get_By_RoomType",
                        new
                        {
                            RoomTypeId = roomType.RoomTypeId,
                            FromDate = fromDate,
                            ToDate = toDate
                        }, commandType: CommandType.StoredProcedure).ToList();
                if (rateAvailabilities is null)
                    rateAvailabilities = new List<RateAvailability>();
                List<Promotion_RateAvailability> promotions = connection.Query<Promotion_RateAvailability>("RateAvailability_Get_Promotion_By_RoomType",
                    new
                    {
                        RoomTypeId = roomType.RoomTypeId
                    }, commandType: CommandType.StoredProcedure).ToList();
                if (promotions is null)
                    promotions = new List<Promotion_RateAvailability>();
                roomType.RateAvailabilities = rateAvailabilities;
                roomType.Promotions = promotions;
                roomType.Promotions.ForEach(y =>
                {
                    y.EndPrices = new List<float>();
                });
                List<RateAvailability> rateAvailabilitiesTemp = new List<RateAvailability>();
                for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
                {
                    RateAvailability rateAvailability = roomType.RateAvailabilities.Find(y => y.Date == date);
                    if (rateAvailability is null)
                        rateAvailability = new RateAvailability()
                        {
                            Date = date,
                            Init = false,
                            Number = 0,
                            Price = 0,
                            RoomTypeId = roomType.RoomTypeId,
                            Status = -1
                        };
                    else
                        rateAvailability.Init = true;
                    rateAvailability.DayofWeek = date.DayOfWeek.ToString();
                    rateAvailabilitiesTemp.Add(rateAvailability);
                    roomType.Promotions.ForEach(y =>
                    {
                        float endPrice = rateAvailability.Price * (100 - y.Deposit) / 100 + y.Price;
                        if (endPrice < 0)
                            endPrice = 0;
                        y.EndPrices.Add((float)Math.Round(endPrice, 0));
                    });
                }
                roomType.RateAvailabilities = rateAvailabilitiesTemp;
                return Json(JsonConvert.SerializeObject(roomType), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PostChangeStatus(int roomTypeId, DateTime date, int status)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                connection.Execute("RateAvailability_Post_ChangeStatus",
                    new
                    {
                        RoomTypeId = roomTypeId,
                        Date = date,
                        Status = status
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PostNumberRoomSell(ParamsChangeRoomSell paramsChangeRoomSell)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                if (paramsChangeRoomSell.AllDay)
                {
                    for (DateTime date = paramsChangeRoomSell.FromDate; date <= paramsChangeRoomSell.ToDate; date = date.AddDays(1))
                    {
                        connection.Execute("RateAvailability_Post_NumberRoomSell",
                            new
                            {
                                RoomTypeId = paramsChangeRoomSell.RoomTypeId,
                                Date = date,
                                Number = paramsChangeRoomSell.Number
                            }, commandType: CommandType.StoredProcedure);
                    }
                }
                else
                {
                    List<string> dayOfWeek = new List<string>();
                    if (paramsChangeRoomSell.Monday)
                        dayOfWeek.Add("Monday");
                    if (paramsChangeRoomSell.Tuesday)
                        dayOfWeek.Add("Tuesday");
                    if (paramsChangeRoomSell.Wednesday)
                        dayOfWeek.Add("Wednesday");
                    if (paramsChangeRoomSell.Thursday)
                        dayOfWeek.Add("Thursday");
                    if (paramsChangeRoomSell.Friday)
                        dayOfWeek.Add("Friday");
                    if (paramsChangeRoomSell.Saturday)
                        dayOfWeek.Add("Saturday");
                    if (paramsChangeRoomSell.Sunday)
                        dayOfWeek.Add("Sunday");
                    for (DateTime date = paramsChangeRoomSell.FromDate; date <= paramsChangeRoomSell.ToDate; date = date.AddDays(1))
                    {
                        if (dayOfWeek.Contains(date.DayOfWeek.ToString()))
                        {
                            connection.Execute("RateAvailability_Post_NumberRoomSell",
                            new
                            {
                                RoomTypeId = paramsChangeRoomSell.RoomTypeId,
                                Date = date,
                                Number = paramsChangeRoomSell.Number
                            }, commandType: CommandType.StoredProcedure);
                        }
                    }
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PostPriceRoomSell(ParamsChangeRoomSell paramsChangeRoomSell)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                if (paramsChangeRoomSell.AllDay)
                {
                    for (DateTime date = paramsChangeRoomSell.FromDate; date <= paramsChangeRoomSell.ToDate; date = date.AddDays(1))
                    {
                        connection.Execute("RateAvailability_Post_PriceRoomSell",
                            new
                            {
                                RoomTypeId = paramsChangeRoomSell.RoomTypeId,
                                Date = date,
                                Price = Math.Round(paramsChangeRoomSell.Price + 0.001, 0)
                            }, commandType: CommandType.StoredProcedure);
                    }
                }
                else
                {
                    List<string> dayOfWeek = new List<string>();
                    if (paramsChangeRoomSell.Monday)
                        dayOfWeek.Add("Monday");
                    if (paramsChangeRoomSell.Tuesday)
                        dayOfWeek.Add("Tuesday");
                    if (paramsChangeRoomSell.Wednesday)
                        dayOfWeek.Add("Wednesday");
                    if (paramsChangeRoomSell.Thursday)
                        dayOfWeek.Add("Thursday");
                    if (paramsChangeRoomSell.Friday)
                        dayOfWeek.Add("Friday");
                    if (paramsChangeRoomSell.Saturday)
                        dayOfWeek.Add("Saturday");
                    if (paramsChangeRoomSell.Sunday)
                        dayOfWeek.Add("Sunday");
                    for (DateTime date = paramsChangeRoomSell.FromDate; date <= paramsChangeRoomSell.ToDate; date = date.AddDays(1))
                    {
                        if (dayOfWeek.Contains(date.DayOfWeek.ToString()))
                        {
                            connection.Execute("RateAvailability_Post_PriceRoomSell",
                            new
                            {
                                RoomTypeId = paramsChangeRoomSell.RoomTypeId,
                                Date = date,
                                Price = Math.Round(paramsChangeRoomSell.Price + 0.001, 0)
                            }, commandType: CommandType.StoredProcedure);
                        }
                    }
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CloseMultiRoom(List<int> roomIds, DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (roomIds is null) roomIds = new List<int>();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    roomIds.ForEach(x =>
                    {
                        connection.Execute("RateAvailability_CloseRoomRangeDate",
                            new
                            {
                                RoomTypeId = x,
                                FromDate = fromDate,
                                ToDate = toDate
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult OpenMultiRoom(List<int> roomIds, DateTime fromDate, DateTime toDate)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            if (roomIds is null) roomIds = new List<int>();
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    roomIds.ForEach(x =>
                    {
                        connection.Execute("RateAvailability_OpenRoomRangeDate",
                            new
                            {
                                RoomTypeId = x,
                                FromDate = fromDate,
                                ToDate = toDate
                            }, commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }

        //API sample
        public JsonResult GetRateAvailability(string userName,string password,int HotelId,DateTime fromDate, DateTime toDate, int typeSearch = -1)
        {
            if (typeSearch > 0)
                toDate = fromDate.AddDays(typeSearch);
            using (var connection = DB.ConnectionFactory())
            {
                try
                {
                    string pass = DataHelper.Decrypt(password);
                    int resultCheckAccept = connection.QuerySingleOrDefault<int>("User_Check_AllowAccess_API",
                           new
                           {
                               Username = userName,
                               Password = DataHelper.CreateMD5(userName + pass + DataHelper.key),
                               HotelId = HotelId
                           }, commandType: System.Data.CommandType.StoredProcedure);
                    if (resultCheckAccept < 0)
                    {
                        return Json(JsonConvert.SerializeObject(new
                        {
                            status = -1,
                            message = "account or password is incorrect",
                            data = ""
                        }), JsonRequestBehavior.AllowGet);
                    }
                    List<RoomType_RateAvailability> roomTypes = connection.Query<RoomType_RateAvailability>("RoomType_Get_Full",
                        new
                        {
                            HotelId = HotelId
                        }, commandType: CommandType.StoredProcedure).ToList();
                    if (roomTypes is null)
                        roomTypes = new List<RoomType_RateAvailability>();
                    roomTypes.ForEach(x =>
                    {
                        List<RateAvailability> rateAvailabilities = connection.Query<RateAvailability>("RateAvailability_Get_By_RoomType",
                            new
                            {
                                RoomTypeId = x.RoomTypeId,
                                FromDate = fromDate,
                                ToDate = toDate
                            }, commandType: CommandType.StoredProcedure).ToList();
                        if (rateAvailabilities is null)
                            rateAvailabilities = new List<RateAvailability>();
                        List<Promotion_RateAvailability> promotions = connection.Query<Promotion_RateAvailability>("RateAvailability_Get_Promotion_By_RoomType",
                            new
                            {
                                RoomTypeId = x.RoomTypeId
                            }, commandType: CommandType.StoredProcedure).ToList();
                        if (promotions is null)
                            promotions = new List<Promotion_RateAvailability>();
                        x.RateAvailabilities = rateAvailabilities;
                        x.Promotions = promotions;
                        x.Promotions.ForEach(y =>
                        {
                            y.EndPrices = new List<float>();
                        });
                    });
                    roomTypes.ForEach(x =>
                    {
                        List<RateAvailability> rateAvailabilities = new List<RateAvailability>();
                        for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
                        {
                            RateAvailability rateAvailability = x.RateAvailabilities.Find(y => y.Date == date);
                            if (rateAvailability is null)
                                rateAvailability = new RateAvailability()
                                {
                                    Date = date,
                                    Init = false,
                                    Number = 0,
                                    Price = 0,
                                    RoomTypeId = x.RoomTypeId,
                                    Status = -1
                                };
                            else
                                rateAvailability.Init = true;
                            rateAvailability.DayofWeek = date.DayOfWeek.ToString();
                            rateAvailabilities.Add(rateAvailability);
                            x.Promotions.ForEach(y =>
                            {
                                float endPrice = rateAvailability.Price * (100 - y.Deposit) / 100 + y.Price;
                                if (endPrice < 0)
                                    endPrice = 0;
                                y.EndPrices.Add((float)Math.Round(endPrice, 0));
                            });
                        }
                        x.RateAvailabilities = rateAvailabilities;
                    });
                    return Json(JsonConvert.SerializeObject(new
                    {
                        status = 1,
                        message = "Success",
                        data = roomTypes
                    }), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(JsonConvert.SerializeObject(new
                    {
                        status = -1,
                        message = e.Message,
                        data = ""
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult PostChangeStatusRateAvailability(string userName, string password, int HotelId,int roomTypeId, DateTime date, int status)
        {
            using (var connection = DB.ConnectionFactory())
            {
                try
                {
                    string pass = DataHelper.Decrypt(password);
                    int resultCheckAccept = connection.QuerySingleOrDefault<int>("User_Check_AllowAccess_API",
                           new
                           {
                               Username = userName,
                               Password = DataHelper.CreateMD5(userName + pass + DataHelper.key),
                               HotelId = HotelId
                           }, commandType: System.Data.CommandType.StoredProcedure);
                    if (resultCheckAccept < 0)
                    {
                        return Json(JsonConvert.SerializeObject(new
                        {
                            status = -1,
                            message = "account or password is incorrect"
                        }), JsonRequestBehavior.AllowGet);
                    }
                    connection.Open();
                    connection.Execute("RateAvailability_Post_ChangeStatus",
                        new
                        {
                            RoomTypeId = roomTypeId,
                            Date = date,
                            Status = status
                        }, commandType: CommandType.StoredProcedure);
                    return Json(JsonConvert.SerializeObject(new
                    {
                        status = 1,
                        message = "success"
                    }), JsonRequestBehavior.AllowGet);
                }
                catch(Exception e)
                {
                    return Json(JsonConvert.SerializeObject(new
                    {
                        status = -1,
                        message = e.Message
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult PostNumberRoomSellRateAvailability(string userName, string password, int HotelId, ParamsChangeRoomSell paramsChangeRoomSell)
        {
            using (var connection = DB.ConnectionFactory())
            {
                try
                {
                    string pass = DataHelper.Decrypt(password);
                    int resultCheckAccept = connection.QuerySingleOrDefault<int>("User_Check_AllowAccess_API",
                           new
                           {
                               Username = userName,
                               Password = DataHelper.CreateMD5(userName + pass + DataHelper.key),
                               HotelId = HotelId
                           }, commandType: System.Data.CommandType.StoredProcedure);
                    if (resultCheckAccept < 0)
                    {
                        return Json(JsonConvert.SerializeObject(new
                        {
                            status = -1,
                            message = "account or password is incorrect"
                        }), JsonRequestBehavior.AllowGet);
                    }
                    connection.Open();
                    if (paramsChangeRoomSell.AllDay)
                    {
                        for (DateTime date = paramsChangeRoomSell.FromDate; date <= paramsChangeRoomSell.ToDate; date = date.AddDays(1))
                        {
                            connection.Execute("RateAvailability_Post_NumberRoomSell",
                                new
                                {
                                    RoomTypeId = paramsChangeRoomSell.RoomTypeId,
                                    Date = date,
                                    Number = paramsChangeRoomSell.Number
                                }, commandType: CommandType.StoredProcedure);
                        }
                    }
                    else
                    {
                        List<string> dayOfWeek = new List<string>();
                        if (paramsChangeRoomSell.Monday)
                            dayOfWeek.Add("Monday");
                        if (paramsChangeRoomSell.Tuesday)
                            dayOfWeek.Add("Tuesday");
                        if (paramsChangeRoomSell.Wednesday)
                            dayOfWeek.Add("Wednesday");
                        if (paramsChangeRoomSell.Thursday)
                            dayOfWeek.Add("Thursday");
                        if (paramsChangeRoomSell.Friday)
                            dayOfWeek.Add("Friday");
                        if (paramsChangeRoomSell.Saturday)
                            dayOfWeek.Add("Saturday");
                        if (paramsChangeRoomSell.Sunday)
                            dayOfWeek.Add("Sunday");
                        for (DateTime date = paramsChangeRoomSell.FromDate; date <= paramsChangeRoomSell.ToDate; date = date.AddDays(1))
                        {
                            if (dayOfWeek.Contains(date.DayOfWeek.ToString()))
                            {
                                connection.Execute("RateAvailability_Post_NumberRoomSell",
                                new
                                {
                                    RoomTypeId = paramsChangeRoomSell.RoomTypeId,
                                    Date = date,
                                    Number = paramsChangeRoomSell.Number
                                }, commandType: CommandType.StoredProcedure);
                            }
                        }
                    }
                    return Json(JsonConvert.SerializeObject(new
                    {
                        status = 1,
                        message = "success"
                    }), JsonRequestBehavior.AllowGet);
                }
                catch(Exception e)
                {
                    return Json(JsonConvert.SerializeObject(new
                    {
                        status = -1,
                        message = e.Message
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult PostPriceRoomSellRateAvailability(string userName, string password, int HotelId, ParamsChangeRoomSell paramsChangeRoomSell)
        {
            using (var connection = DB.ConnectionFactory())
            {
                try
                {
                    string pass = DataHelper.Decrypt(password);
                    int resultCheckAccept = connection.QuerySingleOrDefault<int>("User_Check_AllowAccess_API",
                           new
                           {
                               Username = userName,
                               Password = DataHelper.CreateMD5(userName + pass + DataHelper.key),
                               HotelId = HotelId
                           }, commandType: System.Data.CommandType.StoredProcedure);
                    if (resultCheckAccept < 0)
                    {
                        return Json(JsonConvert.SerializeObject(new
                        {
                            status = -1,
                            message = "account or password is incorrect"
                        }), JsonRequestBehavior.AllowGet);
                    }
                    connection.Open();
                    if (paramsChangeRoomSell.AllDay)
                    {
                        for (DateTime date = paramsChangeRoomSell.FromDate; date <= paramsChangeRoomSell.ToDate; date = date.AddDays(1))
                        {
                            connection.Execute("RateAvailability_Post_PriceRoomSell",
                            new
                            {
                                RoomTypeId = paramsChangeRoomSell.RoomTypeId,
                                Date = date,
                                Price = Math.Round(paramsChangeRoomSell.Price + 0.001, 0)
                            }, commandType: CommandType.StoredProcedure);
                        }
                    }
                    else
                    {
                        List<string> dayOfWeek = new List<string>();
                        if (paramsChangeRoomSell.Monday)
                            dayOfWeek.Add("Monday");
                        if (paramsChangeRoomSell.Tuesday)
                            dayOfWeek.Add("Tuesday");
                        if (paramsChangeRoomSell.Wednesday)
                            dayOfWeek.Add("Wednesday");
                        if (paramsChangeRoomSell.Thursday)
                            dayOfWeek.Add("Thursday");
                        if (paramsChangeRoomSell.Friday)
                            dayOfWeek.Add("Friday");
                        if (paramsChangeRoomSell.Saturday)
                            dayOfWeek.Add("Saturday");
                        if (paramsChangeRoomSell.Sunday)
                            dayOfWeek.Add("Sunday");
                        for (DateTime date = paramsChangeRoomSell.FromDate; date <= paramsChangeRoomSell.ToDate; date = date.AddDays(1))
                        {
                            if (dayOfWeek.Contains(date.DayOfWeek.ToString()))
                            {
                                connection.Execute("RateAvailability_Post_PriceRoomSell",
                            new
                            {
                                RoomTypeId = paramsChangeRoomSell.RoomTypeId,
                                Date = date,
                                Price = Math.Round(paramsChangeRoomSell.Price + 0.001, 0)
                            }, commandType: CommandType.StoredProcedure);
                            }
                        }
                    }
                    return Json(JsonConvert.SerializeObject(new
                    {
                        status = 1,
                        message = "success"
                    }), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(JsonConvert.SerializeObject(new
                    {
                        status = -1,
                        message = e.Message
                    }), JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}