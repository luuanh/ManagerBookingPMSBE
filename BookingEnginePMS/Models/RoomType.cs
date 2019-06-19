using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class RoomType
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public string RoomTypeCode { get; set; }
        public int MaxPeople { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public bool HasExtrabed { get; set; }
        public string Image { get; set; }
        public string Size { get; set; }
        public int Index { get; set; }
        public List<RoomTypeGallery> RoomTypeGalleries { get; set; }
        public List<RoomTypeLanguage> RoomTypeLanguages { get; set; }
        public List<int> RoomTypeExtrabedIds { get; set; }
    }
}