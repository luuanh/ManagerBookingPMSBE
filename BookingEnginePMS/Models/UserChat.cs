using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class UserChat
    {
        public int UserChatId { get; set; }
        public int HotelId { get; set; }
        public string FirstUser { get; set; }
        public string SecondUser { get; set; }

        public List<UserChatMessage> UserChatMessages { get; set; }
        //
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string UserSend { get; set; }
        public bool Read { get; set; } // tin nhắn đã đọc chưa
    }
}