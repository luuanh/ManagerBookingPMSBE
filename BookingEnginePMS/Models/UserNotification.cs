using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class UserNotification
    {
        public int UserNotificationId { get; set; }
        public int UserId { get; set; }
        public int NotificationId { get; set; }
        public bool Status { get; set; }
        public bool Access { get; set; }
        //
        public string UserName { get; set; }
    }
}