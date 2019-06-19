using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Amenity
    {
        public int AmenityId { get; set; } 
        public int Index { get; set; }
        public string Note { get; set; }
        // AmenityName default for screen show list
        public string AmenityName { get; set; }
        public List<AmenityLanguage> AmenityLanguages { get; set; }
    }
}