using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class ServiceCategoryLanguage
    {
        public int ServiceCategoryLanguageId { get; set; }
        public int LanguageId { get; set; }
        public int ServiceCategoryId { get; set; }
        public string ServiceCategoryName { get; set; }


    }
}