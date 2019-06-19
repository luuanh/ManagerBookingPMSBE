using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class TransactionDebt
    {
        public int TransactionDebtId { get; set; }
        public int ReservationDebtId { get; set; }
        public float Pay { get; set; }
        public DateTime Date { get; set; }
        public string UserChange { get; set; }

    }
}