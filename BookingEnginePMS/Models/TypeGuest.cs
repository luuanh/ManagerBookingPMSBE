using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class TypeGuest
    {
        public int TypeGuestId { get; set; }
        public string TypeGuestName { get; set; }
        public float MinAmount { get; set; }
        public float Discount { get; set; }
        public int Level { get; set; }
    }
}