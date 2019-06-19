using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class GuestOrderService
    {
        public int GuestOrderServiceId { get; set; }
        public int GuestOrderId { get; set; }
        public int ServiceId { get; set; }
        public int Number { get; set; }
        public float Price { get; set; }
        //
        public string ServiceName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}