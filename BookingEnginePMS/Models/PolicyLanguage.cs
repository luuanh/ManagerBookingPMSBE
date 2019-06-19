using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class PolicyLanguage
    {
        public int PolicyLanguageId { get; set; }
        public int PolicyId { get; set; }
        public int LanguageId { get; set; }
        public string PolicyName { get; set; }
        public string Content { get; set; }

    }
}