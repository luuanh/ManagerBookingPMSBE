using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public int ServiceCategoryId { get; set; }
        public string ServiceCode { get; set; }
        public string Photo { get; set; }
        public int Index { get; set; }
        public bool BuyOnline { get; set; }
        public float Price { get; set; }
        public List<ServiceLanguage> ServiceLanguages { get; set; }
        //
        public string ServiceName { get; set; }
        public string Description { get; set; }
    }
}