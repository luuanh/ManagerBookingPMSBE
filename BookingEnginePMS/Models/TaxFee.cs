using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class TaxFee
    {
        public int TaxFeeId { get; set; }
        public float Amount { get; set; }
        public string Description { get; set; }
        public bool TypeTaxFee { get; set; }
    }
}