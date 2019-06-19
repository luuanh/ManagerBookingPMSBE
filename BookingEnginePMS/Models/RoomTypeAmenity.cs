using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class RoomTypeAmenity
    {
        public int RoomTypeAmenityId { get; set; }
        public int RoomTypeId { get; set; }
        public int AmenityId { get; set; }

    }
}