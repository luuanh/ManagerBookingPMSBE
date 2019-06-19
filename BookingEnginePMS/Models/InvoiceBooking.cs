using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class InvoiceBooking
    {
        public int InvoiceBookingId { get; set; }
        public int BookingId { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string JsonData { get; set; }
        public string UserCreate { get; set; }
        //
        public Guest Guest { get; set; }
        public Hotel Hotel { get; set; }
        public Booking Booking { get; set; }
    }
}