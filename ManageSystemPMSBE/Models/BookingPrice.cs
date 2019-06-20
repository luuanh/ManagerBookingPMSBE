using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystemPMSBE.Models
{
    public class BookingPrice
    {
        public int BookingPriceId { get; set; }
        public int BookingId { get; set; }
        public DateTime Date { get; set; }
        public float Price { get; set; }
        public bool Paid { get; set; }
        public float ApplyVAT { get; set; }
        public float ApplyServiceCharge { get; set; }
        //
        public int Number { get; set; }
        public BookingPrice()
        {
            Price = 0;
            Number = 0;
        }
    }
}