using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class CashHistory
    {
        public int CashHistoryId { get; set; }
        public int HotelId { get; set; }
        public int CashDrawerId { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime DateClosed { get; set; }
        public float StartBalance { get; set; }
        public float DrawerBalance { get; set; }
        public float CashDrop { get; set; }
        public string NoteOpen { get; set; }
        public string NoteClose { get; set; }
        public string UserSession { get; set; }
        public bool Status { get; set; }

        //
        public string Name { get; set; }
        public bool Deleted { get; set; }
    }
}