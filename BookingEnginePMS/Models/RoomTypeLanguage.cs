using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class RoomTypeLanguage
    {
        public int RoomTypeLanguageId { get; set; }
        public int RoomTypeId { get; set; }
        public int LanguageId { get; set; }
        public string ExtrabedOption { get; set; }
        public string DescriptionBed { get; set; }
        public string DescriptionView { get; set; }
        public string Note { get; set; }

    }
}