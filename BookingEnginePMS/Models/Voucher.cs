using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Voucher
    {
        public int VoucherId { get; set; }
        public int HotelId { get; set; }
        public string VoucherCode { get; set; }
        public string VoucherName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool DiscountForService { get; set; }
        public float AmountForService { get; set; }
        public bool DiscountForRoom { get; set; }
        public float AmountForRoom { get; set; }
        public int Number { get; set; }
        public List<VoucherLanguage> VoucherLanguages { get; set; }
        public List<int> VoucherRoomTypes { get; set; }
        public List<int> VoucherServices { get; set; }
    }
}