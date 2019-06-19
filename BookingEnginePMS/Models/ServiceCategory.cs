using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class ServiceCategory
    {
        public int ServiceCategoryId { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public int Index { get; set; }
        public List<ServiceCategoryLanguage> ServiceCategoryLanguages { get; set; }
        //
        public string ServiceCategoryName { get; set; }
    }
}