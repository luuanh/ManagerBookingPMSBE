using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class BookingService
    {
        public int BookingServiceId { get; set; }
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public int Number { get; set; }
        public float Price { get; set; }
        public bool Paid { get; set; }
        public DateTime DatePaid { get; set; }
        public DateTime DateCreate { get; set; }
        public float ApplyVAT { get; set; }
        public float ApplyServiceCharge { get; set; }
        //
        public string Photo { get; set; }
        public string ServiceName { get; set; }
        // dungf trong view pay
        public bool check { get; set; }
    }
}