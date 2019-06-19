using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class ExtrabedLanguage
    {
        public int ExtrabedLanguageId { get; set; }
        public int ExtrabedId { get; set; }
        public int LanguageId { get; set; }
        public string ExtrabedName { get; set; }
        public string Description { get; set; }

    }
}