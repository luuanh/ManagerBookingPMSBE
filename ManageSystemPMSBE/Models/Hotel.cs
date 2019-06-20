using System;

namespace ManageSystemPMSBE.Models
{
    public class Hotel
    {
        public int HotelId { get; set; }
        public int GroupHotelId { get; set; }
        public string Name { get; set; }
        public string GroupCode { get; set; }
        public string Code { get; set; }
        public string Logo { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Hotline { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Skyper { get; set; }
        public string Google { get; set; }
        public string Youtobe { get; set; }
        public int Status { get; set; }
        public int HighLight { get; set; }
        public int TypeSoftware { get; set; }
        public DateTime CreateDate { get; set; }
        public int TypePaymentHotel { get; set; }
        public float CommissionRate { get; set; }
        public DateTime DayStartUse { get; set; }
        public float TimeExtended { get; set; }

        public string GetTypePayment()
        {
            switch (TypePaymentHotel)
            {
                case 1:
                    return "Tháng";
                case 2:
                    return "Phần trăm";
                default:
                    return "";
            }
        }
        public string GetTypeSofware()
        {
            switch (TypeSoftware)
            {
                case 1:
                    return "PMS";
                case 2:
                    return "BookingEngine";
                case 3:
                    return "BookingEngine + PMS";
                default:
                    return "";
            }
        }
        public string GetStatus()
        {
            switch (Status)
            {
                case 1:
                    return "Dùng Thử";
                case 2:
                    return "Dùng Thật";
                case 3:
                    return "Hết Hạn";
                case 4:
                    return "Đã Khóa";
                default:
                    return "";
            }
        }
    }
    public class Hotel_Revanue
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public int NumberBooking { get; set; }
        public int NumberInvoice { get; set; }
        public int NumberOrder { get; set; }
        public int NumberOrderMoved { get; set; }
        public int NumberService { get; set; }
        public int NumberExtrabed { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double Total { get; set; }
    }
}