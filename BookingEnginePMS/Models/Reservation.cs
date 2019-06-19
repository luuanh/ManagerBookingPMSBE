using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int HotelId { get; set; }
        public int GuestId { get; set; }
        public int CompanyId { get; set; }
        public int TypeReservation { get; set; }
        public string ReminiscentName { get; set; }
        public string Color { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public string Voucher { get; set; }
        public string BookingSource { get; set; }
        public string ArrivalFlightDate { get; set; }
        public string ArrivalFlightTime { get; set; }
        public string DepartureFlightDate { get; set; }
        public string DepartureFlightTime { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int PaymentType { get; set; }
        public float DepositPecent { get; set; }
        public float Deposit { get; set; }
        public float Amount { get; set; }
        public int Status { get; set; }
        public string NoteForBill { get; set; }
        public DateTime CreateDate { get; set; }
        public float ExcessCash { get; set; }
        public List<Booking> Bookings { get; set; }
        public List<ReservationService> ReservationServices { get; set; }
        //
        public bool IncludeTaxFee { get; set; } // Thông tin đặt phòng có tính thuế hay không
        public string Note { get; set; }
        public Guest Guest { get; set; }
        public Company Company { get; set; }
        public int Day { get; set; }
    }
}