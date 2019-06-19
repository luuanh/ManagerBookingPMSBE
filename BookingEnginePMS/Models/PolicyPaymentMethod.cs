namespace BookingEnginePMS.Models
{
    public class PolicyPaymentMethod
    {
        public int PolicyPaymentMethodId { get; set; }
        public int HotelId { get; set; }
        public int LanguageId { get; set; }
        public int ConfigPaymentMethodId { get; set; }
        public string Policy { get; set; }

    }
}