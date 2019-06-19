using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Policy
    {
        public int PolicyId { get; set; }
        public int Index { get; set; }
        public bool RequirePrice { get; set; }
        public List<PolicyLanguage> PolicyLanguages { get; set; }

        // field for screen list
        public string PolicyName { get; set; }
    }
}