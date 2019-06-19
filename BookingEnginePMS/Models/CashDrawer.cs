using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class CashDrawer
    {
        public int CashDrawerId { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        public int Active { get; set; }
        public int TimesOpened { get; set; }
        public float LastBalance { get; set; }
        public DateTime? LastOpen { get; set; }
        public DateTime? LastClose { get; set; }
        public string LastOpenedBy { get; set; }

    }
}