using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class CardBooking
    {
        public int CardBookingId { get; set; }
        public int BookingId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Code { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }

    }
}