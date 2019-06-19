using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Promotion
    {
        public int PromotionId { get; set; }
        public int PlaneRateId { get; set; }
        public int TypePromotion { get; set; }
        public string PromotionName { get; set; }
        public int DayInHouse { get; set; }
        public int EarlyDay { get; set; }
        public int NightForFreeNight { get; set; }
        public float Deposit { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public int PolicyId { get; set; }
        public float AmountRate { get; set; }
        public bool Status { get; set; }
        public List<PromotionRoomType> PromotionRoomTypes { get; set; }
        public List<PromotionLanguage> PromotionLanguages { get; set; }

        // addition
        public bool isRequireRate { get; set; }
    }
}