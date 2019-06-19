using System.Collections.Generic;

namespace BookingEnginePMS.Models
{
    public class Hotel
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
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
        public bool HighLight { get; set; }
        public int TypeSoftware { get; set; }
        public int TypePaymentHotel { get; set; }
        public float CommissionRate { get; set; }
        public int TimeExtended { get; set; }
        public List<HotelLanguage> hotelLanguages { get; set; }
        //
        public string Terms { get; set; }
        public string InforAccount { get; set; }
        public string Note { get; set; }
        public bool Check { get; set; }
        public string GroupHotelName { get; set; }
        public Hotel() {
            Name = "";
            Code = "";
            HotelId = -1;
            Logo = "";
            Phone = "";
            Fax = "";
            Email = "";
        }
    }
}