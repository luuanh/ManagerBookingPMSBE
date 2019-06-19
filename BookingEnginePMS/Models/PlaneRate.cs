using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class PlaneRate
    {
        public int PlaneRateId { get; set; }
        public float Price { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        public List<PlaneRateRoomType> PlaneRateRoomTypes { get; set; }
        public List<PlaneRateLanguage> PlaneRateLanguages { get; set; }
        //
        public string Name { get; set; }
    }
}