namespace BookingEnginePMS.Models
{
    public class ConfigPaymentMethod
    {
        public int ConfigPaymentMethodId { get; set; }
        public string Name { get; set; }
        //
        public int HotelId { get; set; }
        public bool Active { get; set; }
        public bool ActiveInvoice { get; set; }
        public bool RequireCard { get; set; }
        public int Index { get; set; }
    }
}