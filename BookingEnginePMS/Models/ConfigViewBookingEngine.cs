using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class ConfigViewBookingEngine
    {
        public int ConfigViewBookingEngineId { get; set; }
        public int HotelId { get; set; }
        public string Lang { get; set; }
        public string Currency { get; set; }
    }
}