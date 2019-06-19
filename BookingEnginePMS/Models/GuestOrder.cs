using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class GuestOrder
    {
        public int GuestOrderId { get; set; }
        public string GuestOrderCode { get; set; }
        public int GuestId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Paid { get; set; }
        public string Cashier { get; set; }
        public string CreateBy { get; set; }
        //
        public string GuestName { get; set; }
        public float Total { get; set; }
    }
}