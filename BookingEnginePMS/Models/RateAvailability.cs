using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class RateAvailability
    {
        public int RateAvailabilityId { get; set; }
        public int RoomTypeId { get; set; }
        public float Price { get; set; }
        public int Number { get; set; }
        public int Status { get; set; }
        public DateTime Date { get; set; }
        public bool Init { get; set; }

        //
        public string DayofWeek { get; set; }
        public float PriceForAddClient { get; set; } // giá dùng từ ngoài BE khi tạo booking

        public RateAvailability Clone()
        {
            return (RateAvailability)this.MemberwiseClone();
        }
    }
}