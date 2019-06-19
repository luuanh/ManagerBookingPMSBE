using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class PlaneRateRoomType
    {
        public int PlaneRateRoomTypeId { get; set; }
        public int PlaneRateId { get; set; }
        public int RoomTypeId { get; set; }

        // field show for screen list
        public string RoomTypeName { get; set; }
        public bool Checked { get; set; }
    }
}