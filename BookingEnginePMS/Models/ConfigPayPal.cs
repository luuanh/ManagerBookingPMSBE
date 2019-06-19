namespace BookingEnginePMS.Models
{
    public class ConfigPayPal
    {
        public int ConfigPayPalId { get; set; }
        public int HotelId { get; set; }
        public string MerchantId { get; set; }
        public bool EmailConfirm { get; set; }
    }
}