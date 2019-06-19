using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class ConfigCurrency
    {
        public int ConfigCurrencyId { get; set; }
        public int HotelId { get; set; }
        public string CurrencyCode { get; set; }
        public float Result { get; set; }
        public bool AutoCalculator { get; set; }
        public ConfigCurrency(string currencyCode)
        {
            ConfigCurrencyId = -1;
            Result = 0;
            AutoCalculator = false;
            CurrencyCode = currencyCode;
        }
        public ConfigCurrency() { }
    }
}