using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class ReservationNote
    {
        public int ReservationNoteId { get; set; }
        public int ReservationId { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserName { get; set; }

    }
}