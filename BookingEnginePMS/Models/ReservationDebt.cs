using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class ReservationDebt
    {
        public int ReservationDebtId { get; set; }
        public int ReservationId { get; set; }
        public int BookingId { get; set; }
        public float TotalAmount { get; set; }
        public float Paid { get; set; }
        public List<TransactionDebt> TransactionDebts { get; set; }
    }
}