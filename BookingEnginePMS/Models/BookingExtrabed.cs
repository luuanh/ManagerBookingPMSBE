using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class BookingExtrabed
    {
        public int BookingExtrabedId { get; set; }
        public int BookingId { get; set; }
        public int ExtrabedId { get; set; }
        public int Number { get; set; }
        [UIHint("Currency")]
        public float Price { get; set; }
        public bool Paid { get; set; }
        public DateTime DateCreate { get; set; }
        public float ApplyVAT { get; set; }
        public float ApplyServiceCharge { get; set; }
        //
        public string ExtrabedName { get; set; }
        // dung trong view pay
        public bool check { get; set; }
    }
}