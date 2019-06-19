using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class PromotionLanguage
    {
        public int PromotionLanguageId { get; set; }
        public int PromotionId { get; set; }
        public int LanguageId { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }

    }
}