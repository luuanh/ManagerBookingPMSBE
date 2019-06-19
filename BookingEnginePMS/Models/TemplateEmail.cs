using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class TemplateEmail
    {
        public int TemplateEmailId { get; set; }
        public int HotelId { get; set; }
        public int TypeEmailId { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public TemplateEmail() {
            CC = "";
            BCC = "";
            Subject = "";
            Content = "";
        }
        public TemplateEmail(int typeEmailId,int hotelId)
        {
            HotelId = hotelId;
            TypeEmailId = typeEmailId;
        }
        public void CheckNull()
        {
            if (CC is null) CC = "";
            if (BCC is null) BCC = "";
            if (Subject is null) Subject = "";
            if (Content is null) Content = "";
        }
    }
}