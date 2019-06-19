using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Language
    {
        public int LanguageId { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string Ensign { get; set; }
        public int Index { get; set; }
        public bool Active { get; set; }

    }
}