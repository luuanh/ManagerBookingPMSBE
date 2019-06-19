using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class ConfigEmail
    {
        public int ConfigEmailId { get; set; }
        public int HotelId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string EmailReceive { get; set; }
        public string SubjectOffline { get; set; }
        public string SubjectOnline { get; set; }
        public ConfigEmail(int hotelId)
        {
            HotelId = hotelId;
        }
        public ConfigEmail() { }
    }
}