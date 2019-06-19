using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class VisaBooking
    {
        public int VisaBookingId { get; set; }
        public int BookingId { get; set; }
        public string SerialNo { get; set; }
        public string VisaNo { get; set; }
        public string VisaDate { get; set; }
        public string VisaIssuePlace { get; set; }
        public string ArrivalFrom { get; set; }
        public string ArrivalTrasportation { get; set; }
        public string VisaExpiryDate { get; set; }
        public string DateOfArrivalIn { get; set; }
        public string TimeOfArrivalIn { get; set; }
        public string PurposeOfVisit { get; set; }
        public string GoingTo { get; set; }
        public string DepartTransportation { get; set; }
        public string ProposedDuration { get; set; }
        public string VisaType { get; set; }

    }
}