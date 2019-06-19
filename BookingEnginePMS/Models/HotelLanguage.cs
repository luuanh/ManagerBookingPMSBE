using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class HotelLanguage
    {
        public int HotelLanguageId { get; set; }
        public int LanguageId { get; set; }
        public string Terms { get; set; }
        public string InforAccount { get; set; }
        public string Note { get; set; }

    }
}