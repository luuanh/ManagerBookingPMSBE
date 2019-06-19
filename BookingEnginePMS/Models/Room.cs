using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomCode { get; set; }
        public int Floor { get; set; }
        public int Status { get; set; }

        // field for screen show list
        public string RoomTypeCode { get; set; }

    }
}