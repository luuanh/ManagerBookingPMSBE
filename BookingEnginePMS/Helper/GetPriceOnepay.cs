﻿using System;
using System.Linq;
using System.Xml.Linq;

namespace BookingEnginePMS.Helper
{
    public static class GetPriceOnepay
    {
        public static double USDToVND()
        {
            var vnd = "21150";
            try
            {
                var load = XDocument.Load(@"http://www.vietcombank.com.vn/ExchangeRates/ExrateXML.aspx");
                var xElement = load.Element("ExrateList");
                if (xElement != null)
                {
                    var usds = xElement.Elements("Exrate");
                    foreach (var element in usds.Where(element => element.Attribute("CurrencyCode").Value == "USD"))
                    {
                        vnd = element.Attribute("Sell").Value;
                    }
                }
            }
            catch (Exception)
            {
                vnd = "21150";
            }
            return double.Parse(vnd);
        }
    }
}
