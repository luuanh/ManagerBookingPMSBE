using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class UserLogin
    {
        public int UserLoginId { get; set; }
        public int UserId { get; set; }
        public string UserAgent { get; set; }
        public string Ip { get; set; }
        public DateTime LastestLogin { get; set; }

    }
}