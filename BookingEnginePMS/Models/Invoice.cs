using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int HotelId { get; set; }
        public string InvoiceCode { get; set; }
        public string Title { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public float TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime DateInvoice { get; set; }
        public DateTime ArrivalDate { get; set; }
        public bool Status { get; set; }
        public bool FeedBack { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string SecurityCode { get; set; }
        public int ExprireMonth { get; set; }
        public int ExprireYear { get; set; }
        public string UserCreate { get; set; }
    }
}