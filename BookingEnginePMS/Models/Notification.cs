using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int HotelId { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserCreate { get; set; }
        public List<UserNotification> UserNotifications { get; set; }

        //
        public bool Status { get; set; }
        public bool AllowDelete { get; set; }
    }
}