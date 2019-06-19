using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Active { get; set; }
        public string Token { get; set; }
        public List<int> Roles { get; set; }

        //
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        // notification
        public bool Access { get; set; }
    }
}