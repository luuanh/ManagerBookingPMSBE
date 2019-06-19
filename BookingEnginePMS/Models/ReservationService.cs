using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class ReservationService
    {
        public int ReservationServiceId { get; set; }
        public int ReservationId { get; set; }
        public int ServiceId { get; set; }
        public int Number { get; set; }
        public float Price { get; set; }
        public DateTime DateUser { get; set; }
        //
        public int RoomTypeId { get; set; }
        public int Index { get; set; } // filed in view step 3
        public string RoomTypeIdIndex { get; set; }
        public string ServiceName { get; set; }
    }
}