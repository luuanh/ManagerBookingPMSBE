using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class AmenityLanguage
    {
        public int AmenityLanguageId { get; set; }
        public int AmenityId { get; set; }
        public int LanguageId { get; set; }
        public string AmenityName { get; set; }

    }
}