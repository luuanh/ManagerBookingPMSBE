using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystemPMSBE.Models
{
    public class Booking_Reservation
    {
        public int HotelId { get; set; }
        public int ReservationId { get; set; }
        public int BookingId { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public float Paid { get; set; }
        public float PrePaid { get; set; }
        public int Status { get; set; }
        public string GuestName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int TypeBooking { get; set; }
        public int BookingSource { get; set; }
        public float Discount { get; set; }
        public string UserCreate { get; set; }
        //

        public float TotalRoom { get; set; }
        public float TotalExtrabed { get; set; }
        public float TotalService { get; set; }
        public float Total { get; set; }
        //
        public List<BookingExtrabed> BookingExtrabeds { get; set; }
        public List<BookingService> BookingServices { get; set; }
        public List<BookingPrice> BookingPrices { get; set; }
        // pay debt
        public float PayDebt { get; set; }
        // Hotel
        public string Name { get; set; } // hotel name
        public string Code { get; set; }// hotel code
    }
}