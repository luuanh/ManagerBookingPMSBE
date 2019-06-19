using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class RoomTypePriceHour
    {
        public int RoomTypePriceHourId { get; set; }
        public int RoomTypeId { get; set; }
        public int HourStart { get; set; }
        public float PriceStart { get; set; }
        public int HourNext1 { get; set; }
        public float PriceNext1 { get; set; }
        public int HourNext2 { get; set; }
        public float PriceNext2 { get; set; }
        public int HourNext3 { get; set; }
        public float PriceNext3 { get; set; }
        public RoomTypePriceHour()
        {
            HourStart = 1;
            PriceStart = 0;
            HourNext1 = 1;
            PriceNext1 = 0;
            HourNext2 = 1;
            PriceNext2 = 0;
            HourNext3 = 1;
            PriceNext3 = 0;
        }
    }
}