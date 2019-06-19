using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class VoucherLanguage
    {
        public int VoucherLanguageId { get; set; }
        public int VoucherId { get; set; }
        public int LanguageId { get; set; }
        public string Description { get; set; }

    }
}