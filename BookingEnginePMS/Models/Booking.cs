using System;
using System.Collections.Generic;

namespace BookingEnginePMS.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int TypeBooking { get; set; }
        public int ReservationId { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomId { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public int GuestId { get; set; }
        public int PaymentType { get; set; }
        public float DepositPecent { get; set; }
        public float DepositMonney { get; set; }
        public float PrePaid { get; set; }
        public float Discount { get; set; }
        public float Paid { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime NoshowDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime CancelDate { get; set; }
        public string UserCreate { get; set; }
        public string UserBook { get; set; }
        public string UserCheckin { get; set; }
        public string UserNoshow { get; set; }
        public string UserCheckout { get; set; }
        public string UserCancel { get; set; }
        public bool IsDebt { get; set; }
        public int EndPay { get; set; }
        public string Note { get; set; }
        public string StayTime { get; set; }
        public bool IncludeVATAndServiceCharge { get; set; }

        public List<BookingPrice> BookingPrices { get; set; }
        public List<BookingExtrabed> BookingExtrabeds { get; set; }
        public VisaBooking VisaBookings { get; set; }
        public List<InvoiceBooking> InvoiceBookings { get; set; }
        public List<BookingService> BookingServices { get; set; }
        //
        public Guest Guest { get; set; }
        public CardBooking CardBooking { get; set; }
        public string RoomTypeName { get; set; }
        public string RoomCode { get; set; }
        //
        public float TotalAmount { get; set; }
        //
        public int AdultChoose { get; set; }
        public int ChildrenChoose { get; set; }


    }
}