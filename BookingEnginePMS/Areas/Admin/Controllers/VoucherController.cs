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
    public class VoucherController : SercurityController
    {
        // GET: Admin/Voucher
        public ActionResult Index()
        {
            if (!CheckSecurity(18))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Service> services = connection.Query<Service>("Service_GetFull",
                    new
                    {
                        LanguageId = LanguageId,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure).ToList();
                List<RoomType> roomTypes = connection.Query<RoomType>("RoomType_Get_Full",
                    new
                    {
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure).ToList();
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 18
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                ViewData["services"] = services;
                ViewData["roomTypes"] = roomTypes;
            }
            return View();
        }
        public JsonResult Get(string keySearch, int pageNumber, int pageSize)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("Voucher_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        LanguageId = LanguageId,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<Voucher> vouchers = multi.Read<Voucher>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(JsonConvert.SerializeObject(new
                    {
                        vouchers = vouchers,
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
                using (var multi = connection.QueryMultiple("Voucher_Detail_Full",
                    new
                    {
                        VoucherId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    Voucher voucher = multi.Read<Voucher>().SingleOrDefault();
                    if (voucher is null)
                        voucher = new Voucher();
                    List<VoucherLanguage> voucherLanguages = multi.Read<VoucherLanguage>().ToList();

                    if (voucherLanguages is null)
                        voucherLanguages = new List<VoucherLanguage>();

                    List<int> languageId = multi.Read<int>().ToList();
                    if (languageId != null)
                        languageId.ForEach(x =>
                        {
                            if (voucherLanguages.FindIndex(y => y.LanguageId == x) < 0)
                            {
                                voucherLanguages.Add(new VoucherLanguage()
                                {
                                    LanguageId = x,
                                    Description = ""
                                });
                            }
                        });

                    List<int> voucherRoomTypes = multi.Read<int>().ToList();
                    if (voucherRoomTypes is null)
                        voucherRoomTypes = new List<int>();

                    List<int> voucherServices = multi.Read<int>().ToList();
                    if (voucherServices is null)
                        voucherServices = new List<int>();

                    voucher.VoucherLanguages = voucherLanguages;
                    voucher.VoucherRoomTypes = voucherRoomTypes;
                    voucher.VoucherServices = voucherServices;
                    return Json(JsonConvert.SerializeObject(voucher), JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Post(Voucher voucher)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    if (!voucher.DiscountForRoom)
                    {
                        voucher.AmountForRoom = 0;
                        voucher.VoucherRoomTypes = new List<int>();
                    }
                    if (!voucher.DiscountForService)
                    {
                        voucher.AmountForService = 0;
                        voucher.VoucherServices = new List<int>();
                    }
                    int voucherId = connection.QuerySingleOrDefault<int>("Voucher_Post",
                    new
                    {
                        HotelId = HotelId,
                        VoucherCode = voucher.VoucherCode,
                        VoucherName = voucher.VoucherName,
                        FromDate = voucher.FromDate,
                        ToDate = voucher.ToDate,
                        DiscountForService = voucher.DiscountForService,
                        AmountForService = voucher.AmountForService,
                        DiscountForRoom = voucher.DiscountForRoom,
                        AmountForRoom = voucher.AmountForRoom,
                        Number = voucher.Number

                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);
                    if (voucher.VoucherLanguages != null)
                    {
                        voucher.VoucherLanguages.ForEach(x =>
                        {
                            connection.Execute("VoucherLanguages_Post",
                                new
                                {
                                    VoucherId = voucherId,
                                    LanguageId = x.LanguageId,
                                    Description = x.Description

                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
                    if (voucher.VoucherRoomTypes != null)
                    {
                        voucher.VoucherRoomTypes.ForEach(x =>
                        {
                            connection.Execute("VoucherRoomTypes_Post",
                                new
                                {
                                    VoucherId = voucherId,
                                    RoomTypeId = x
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
                    if (voucher.VoucherServices != null)
                    {
                        voucher.VoucherServices.ForEach(x =>
                        {
                            connection.Execute("VoucherServices_Post",
                                new
                                {
                                    VoucherId = voucherId,
                                    ServiceId = x
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(Voucher voucher)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // update voucher
                    if (!voucher.DiscountForRoom)
                    {
                        voucher.AmountForRoom = 0;
                        voucher.VoucherRoomTypes = new List<int>();
                    }
                    if (!voucher.DiscountForService)
                    {
                        voucher.AmountForService = 0;
                        voucher.VoucherServices = new List<int>();
                    }
                    connection.Execute("Voucher_Put",
                        new
                        {
                            VoucherId = voucher.VoucherId,
                            VoucherCode = voucher.VoucherCode,
                            VoucherName = voucher.VoucherName,
                            FromDate = voucher.FromDate,
                            ToDate = voucher.ToDate,
                            DiscountForService = voucher.DiscountForService,
                            AmountForService = voucher.AmountForService,
                            DiscountForRoom = voucher.DiscountForRoom,
                            AmountForRoom = voucher.AmountForRoom,
                            Number = voucher.Number
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    connection.Execute("VoucherLanguages_Delete_Full",
                        new
                        {
                            VoucherId = voucher.VoucherId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if (voucher.VoucherLanguages != null)
                    {
                        voucher.VoucherLanguages.ForEach(x =>
                        {
                            connection.Execute("VoucherLanguages_Post",
                                new
                                {
                                    VoucherId = voucher.VoucherId,
                                    LanguageId = x.LanguageId,
                                    Description = x.Description

                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
                    connection.Execute("VoucherRoomTypes_Delete_Full",
                        new
                        {
                            VoucherId = voucher.VoucherId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                    if (voucher.VoucherRoomTypes != null)
                    {
                        voucher.VoucherRoomTypes.ForEach(x =>
                        {
                            connection.Execute("VoucherRoomTypes_Post",
                                new
                                {
                                    VoucherId = voucher.VoucherId,
                                    RoomTypeId = x
                                },commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }

                    connection.Execute("VoucherServices_Delete_Full",
                        new
                        {
                            VoucherId = voucher.VoucherId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);

                    if (voucher.VoucherServices != null)
                    {
                        voucher.VoucherServices.ForEach(x =>
                        {
                            connection.Execute("VoucherServices_Post",
                                new
                                {
                                    VoucherId = voucher.VoucherId,
                                    ServiceId = x
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
                connection.Execute("Voucher_Delete",
                    new
                    {
                        VoucherId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}