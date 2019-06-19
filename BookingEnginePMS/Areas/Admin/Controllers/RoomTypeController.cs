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
    public class RoomTypeController : SercurityController
    {
        // GET: Admin/RoomType
        public ActionResult Index()
        {
            if (!CheckSecurity(38))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<Extrabed> extrabeds = connection.Query<Extrabed>("Extrabed_Get_Sample",
                    new
                    {
                        HotelId = HotelId,
                        LanguageId = LanguageId
                    }, commandType: CommandType.StoredProcedure).ToList();
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 38
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                ViewData["extrabeds"] = extrabeds;
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
                using (var multi = connection.QueryMultiple("RoomType_Get",
                    new
                    {
                        keySearch = keySearch,
                        pageNumber = pageNumber,
                        pageSize = pageSize,
                        HotelId = HotelId
                    }, commandType: CommandType.StoredProcedure))
                {
                    List<RoomType> roomTypes = multi.Read<RoomType>().ToList();
                    int totalRecord = multi.Read<int>().SingleOrDefault();
                    return Json(new
                    {
                        roomTypes = roomTypes,
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
                using (var multi = connection.QueryMultiple("RoomType_Detail_Full",
                    new
                    {
                        RoomTypeId = id
                    }, commandType: CommandType.StoredProcedure))
                {
                    RoomType roomType = multi.Read<RoomType>().SingleOrDefault();
                    if (roomType is null)
                        roomType = new RoomType();
                    List<RoomTypeLanguage> roomTypeLanguages = multi.Read<RoomTypeLanguage>().ToList();

                    if (roomTypeLanguages is null)
                        roomTypeLanguages = new List<RoomTypeLanguage>();

                    List<RoomTypeGallery> roomTypeGalleries = multi.Read<RoomTypeGallery>().ToList();

                    if (roomTypeGalleries is null)
                        roomTypeGalleries = new List<RoomTypeGallery>();

                    List<int> languageId = multi.Read<int>().ToList();
                    if (languageId != null)
                        languageId.ForEach(x =>
                        {
                            if (roomTypeLanguages.FindIndex(y => y.LanguageId == x) < 0)
                            {
                                roomTypeLanguages.Add(new RoomTypeLanguage()
                                {
                                    RoomTypeId = roomType.RoomTypeId,
                                    LanguageId = x,
                                    DescriptionBed = "",
                                    DescriptionView = "",
                                    ExtrabedOption = "",
                                    Note = "",
                                    RoomTypeLanguageId = -1
                                });
                            }
                        });

                    List<int> roomTypeExtrabedIds = multi.Read<int>().ToList();
                    if (roomTypeExtrabedIds is null)
                        roomTypeExtrabedIds = new List<int>();

                    roomType.RoomTypeLanguages = roomTypeLanguages;
                    roomType.RoomTypeGalleries = roomTypeGalleries;
                    roomType.RoomTypeExtrabedIds = roomTypeExtrabedIds;
                    return Json(roomType, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Post(RoomType roomType)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    int roomtypeId = connection.QuerySingleOrDefault<int>("RoomType_Post",
                    new
                    {
                        HotelId = HotelId,
                        RoomTypeName = roomType.RoomTypeName,
                        RoomTypeCode = roomType.RoomTypeCode,
                        MaxPeople = roomType.MaxPeople,
                        Adult = roomType.Adult,
                        Children = roomType.Children,
                        HasExtrabed = roomType.HasExtrabed,
                        Image = roomType.Image,
                        Size = roomType.Size,
                        Index = roomType.Index

                    }, commandType: CommandType.StoredProcedure,
                    transaction: transaction);
                    if (!roomType.HasExtrabed)
                    {
                        roomType.RoomTypeExtrabedIds = new List<int>();
                    }
                    if (roomType.RoomTypeLanguages != null)
                    {
                        roomType.RoomTypeLanguages.ForEach(x =>
                        {
                            connection.Execute("RoomTypeLanguages_Post",
                                new
                                {
                                    RoomTypeId = roomtypeId,
                                    LanguageId = x.LanguageId,
                                    ExtrabedOption = x.ExtrabedOption,
                                    DescriptionBed = x.DescriptionBed,
                                    DescriptionView = x.DescriptionView,
                                    Note = x.Note

                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
                    if (roomType.RoomTypeGalleries != null)
                    {
                        roomType.RoomTypeGalleries.ForEach(x =>
                        {
                            connection.Execute("RoomTypeGalleries_Post",
                                new
                                {
                                    RoomTypeId = roomtypeId,
                                    Image = x.Image

                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
                    if (roomType.RoomTypeExtrabedIds != null && roomType.RoomTypeExtrabedIds.Count > 0)
                    {
                        roomType.RoomTypeExtrabedIds.ForEach(x =>
                        {
                            connection.Execute("RoomTypeExtrabed_Post",
                                new
                                {
                                    RoomTypeId = roomtypeId,
                                    ExtrabedId = x
                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });
                    }
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(RoomType roomType)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // update roomtype
                    connection.Execute("RoomType_Put",
                        new
                        {
                            RoomTypeId = roomType.RoomTypeId,
                            RoomTypeName = roomType.RoomTypeName,
                            RoomTypeCode = roomType.RoomTypeCode,
                            MaxPeople = roomType.MaxPeople,
                            Adult = roomType.Adult,
                            Children = roomType.Children,
                            HasExtrabed = roomType.HasExtrabed,
                            Image = roomType.Image,
                            Size = roomType.Size,
                            Index = roomType.Index
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if (!roomType.HasExtrabed)
                    {
                        roomType.RoomTypeExtrabedIds = new List<int>();
                    }
                    // update roomtype includes note,view..
                    if (roomType.RoomTypeLanguages != null)
                        roomType.RoomTypeLanguages.ForEach(x =>
                        {
                            if (x.RoomTypeLanguageId < 0)
                            {
                                connection.Execute("RoomTypeLanguages_Post",
                                new
                                {
                                    RoomTypeId = roomType.RoomTypeId,
                                    LanguageId = x.LanguageId,
                                    ExtrabedOption = x.ExtrabedOption,
                                    DescriptionBed = x.DescriptionBed,
                                    DescriptionView = x.DescriptionView,
                                    Note = x.Note

                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            }
                            else
                            {
                                connection.Execute("RoomTypeLanguages_Put",
                                new
                                {
                                    RoomTypeId = roomType.RoomTypeId,
                                    LanguageId = x.LanguageId,
                                    ExtrabedOption = x.ExtrabedOption,
                                    DescriptionBed = x.DescriptionBed,
                                    DescriptionView = x.DescriptionView,
                                    Note = x.Note

                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            }
                        });
                    // update gallery
                    connection.Execute("RoomTypeGallery_Delete_Full",
                        new
                        {
                            RoomTypeId = roomType.RoomTypeId
                        },
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if (roomType.RoomTypeGalleries != null)
                        roomType.RoomTypeGalleries.ForEach(x =>
                        {
                            connection.Execute("RoomTypeGalleries_Post",
                                new
                                {
                                    RoomTypeId = roomType.RoomTypeId,
                                    Image = x.Image

                                }, commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                        });

                    connection.Execute("RoomTypeExtrabed_Delete_Full",
                        new
                        {
                            RoomTypeId = roomType.RoomTypeId
                        }, commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if (roomType.RoomTypeExtrabedIds != null && roomType.RoomTypeExtrabedIds.Count > 0)
                    {
                        roomType.RoomTypeExtrabedIds.ForEach(x =>
                        {
                            connection.Execute("RoomTypeExtrabed_Post",
                                new
                                {
                                    RoomTypeId = roomType.RoomTypeId,
                                    ExtrabedId = x
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
                connection.Execute("RoomType_Delete",
                    new
                    {
                        RoomTypeId = id
                    }, commandType: CommandType.StoredProcedure);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        //API sample
        public JsonResult GetRoomType(string userName, string password, int HotelId, string keySearch = "", int pageNumber = 1, int pageSize = 1000)
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
                            message = "account or password is incorrect",
                            data = ""
                        }), JsonRequestBehavior.AllowGet);
                    }
                    using (var multi = connection.QueryMultiple("RoomType_Get",
                        new
                        {
                            keySearch = keySearch,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                            HotelId = HotelId
                        }, commandType: CommandType.StoredProcedure))
                    {
                        List<RoomType> roomTypes = multi.Read<RoomType>().ToList();
                        int totalRecord = multi.Read<int>().SingleOrDefault();
                        return Json(JsonConvert.SerializeObject(new
                        {
                            status = 1,
                            message = "success",
                            data = new
                            {
                                roomTypes = roomTypes,
                                totalRecord = totalRecord
                            }
                        }), JsonRequestBehavior.AllowGet);
                    }
                }
                catch(Exception e)
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
    }
}