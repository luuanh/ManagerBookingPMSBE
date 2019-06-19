using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Extrabed
    {
        public int ExtrabedId { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        // field default for screen show list

        public string ExtrabedName { get; set; }
        public string Description { get; set; }
        public List<ExtrabedLanguage> ExtrabedLanguages { get; set; }
    }
}