using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class VoucherRoomType
    {
        public int VoucherServiceId { get; set; }
        public int VoucherId { get; set; }
        public int RoomTypeId { get; set; }

    }
}