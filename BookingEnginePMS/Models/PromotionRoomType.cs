using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class PromotionRoomType
    {
        public int PromotionRoomTypeId { get; set; }
        public int PromotionId { get; set; }
        public int RoomTypeId { get; set; }

        // field for screen list
        public string RoomTypeName { get; set; }
        public bool Checked { get; set; }
    }
}