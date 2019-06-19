using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class UserChatMessage
    {
        public int UserChatMessageId { get; set; }
        public int UserChatId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string UserSend { get; set; }

    }
}