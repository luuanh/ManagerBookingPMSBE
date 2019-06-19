using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Guest
    {
        public int GuestId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Photo { get; set; }
        public int TypeGuestId { get; set; }
        public string ZIPCode { get; set; }
        public string Company { get; set; }
        public int Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string IdentityCart { get; set; }
        public string Passport { get; set; }
        public string CreditCard { get; set; }
        public string DoIssueCreditCard { get; set; }
        public string CVC { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public DateTime DateCreate { get; set; }
        public string CreateBySource { get; set; }
        public float TotalPaid { get; set; }
        public float Discount { get; set; }
        //
        public string TypeGuestName { get; set; }
    }
}