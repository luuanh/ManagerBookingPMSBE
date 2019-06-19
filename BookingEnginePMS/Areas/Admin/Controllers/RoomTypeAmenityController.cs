using BookingEnginePMS.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace BookingEnginePMS.Areas.Admin.Controllers
{
    public class RoomTypeForAmenity
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public bool Checked { get; set; }
    }
    public class AmenityRoomType
    {
        public int AmenityId { get; set; }
        public string AmenityName { get; set; }
        public bool AllRoomType { get; set; }
        public List<RoomTypeForAmenity> RoomType { get; set; }
    }
    public class RoomTypeAmenityController : SercurityController
    {
        // GET: Admin/RoomTypeAmenity
        public ActionResult Index()
        {
            if (!CheckSecurity(41))
                return Redirect("/Admin/Login/Index");

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                List<TransitionLanguage> transitions = connection.Query<TransitionLanguage>("TransitionLanguage_Get_Screen_Setting",
                       new
                       {
                           languageId = LanguageId,
                           screenId = 41
                       }, commandType: CommandType.StoredProcedure).ToList();
                Session["transitions"] = transitions;
                return View();
            }
        }
        public JsonResult Get()
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            int LanguageId = (int)Session["LanguageId"];
            int HotelId = (int)Session["HotelId"];
            using (var connection = DB.ConnectionFactory())
            {
                using (var multi = connection.QueryMultiple("RoomTypeAmenity_Get",
                    new
                    {
                        HotelId = HotelId,
                        LanguageId = LanguageId
                    },
                    commandType: CommandType.StoredProcedure))
                {
                    List<Amenity> amenities = multi.Read<Amenity>().ToList();
                    if (amenities is null)
                        amenities = new List<Amenity>();

                    List<RoomTypeAmenity> roomTypeAmenities = multi.Read<RoomTypeAmenity>().ToList();
                    if (roomTypeAmenities is null)
                        roomTypeAmenities = new List<RoomTypeAmenity>();

                    List<RoomType> roomTypes = multi.Read<RoomType>().ToList();
                    if (roomTypes is null)
                        roomTypes = new List<RoomType>();
                    List<AmenityRoomType> amenityRoomTypes = new List<AmenityRoomType>();
                    amenities.ForEach(x =>
                    {
                        AmenityRoomType amenityRoomType = new AmenityRoomType()
                        {
                            AmenityId = x.AmenityId,
                            AmenityName = x.AmenityName,
                            RoomType = new List<RoomTypeForAmenity>()
                        };
                        bool AllRoomType = true;
                        roomTypes.ForEach(y =>
                        {
                            RoomTypeForAmenity roomTypeForAmenity = new RoomTypeForAmenity()
                            {
                                RoomTypeId = y.RoomTypeId,
                                RoomTypeName = y.RoomTypeName
                            };
                            if (roomTypeAmenities.FindIndex(z => z.RoomTypeId == y.RoomTypeId && z.AmenityId == x.AmenityId) >= 0)
                            {
                                roomTypeForAmenity.Checked = true;
                            }
                            else
                            {
                                roomTypeForAmenity.Checked = false;
                                AllRoomType = false;
                            }
                            amenityRoomType.RoomType.Add(roomTypeForAmenity);
                        });
                        amenityRoomType.AllRoomType = AllRoomType;
                        amenityRoomTypes.Add(amenityRoomType);
                    });
                    return Json(amenityRoomTypes, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult Put(List<AmenityRoomType> amenityRoomTypes)
        {
            if (!CheckSecurity())
                return Json("", JsonRequestBehavior.AllowGet);

            using (var connection = DB.ConnectionFactory())
            {
                connection.Open();
                int HotelId = (int)Session["HotelId"];
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute("RoomTypeAmenity_Delete_Full",
                        new
                        {
                            HotelId = HotelId
                        },
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    amenityRoomTypes.ForEach(x =>
                    {
                        if (x.AllRoomType)
                        {
                            x.RoomType.ForEach(y => y.Checked = true);
                        }
                        x.RoomType.ForEach(y =>
                        {
                            if (y.Checked)
                            {
                                connection.Execute("RoomTypeAmenity_Post",
                                    new
                                    {
                                        RoomTypeId = y.RoomTypeId,
                                        AmenityId = x.AmenityId
                                    }, commandType: CommandType.StoredProcedure,
                                    transaction: transaction);
                            }
                        });
                    });
                    transaction.Commit();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}