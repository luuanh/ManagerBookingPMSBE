using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class ServiceLanguage
    {
        public int ServiceLanguageId { get; set; }
        public int ServiceId { get; set; }
        public int LanguageId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Policy { get; set; }

    }
}