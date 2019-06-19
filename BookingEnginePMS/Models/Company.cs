using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public int GroupGuestId { get; set; }
        public int SourceId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string TaxCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactUsName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }

    }
}