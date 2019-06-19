using System;

namespace BookingEnginePMS.Models
{
    public class ReservationEmailSent
    {
        public int ReservationEmailSentId { get; set; }
        public int ReservationId { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }

    }
}