using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystemPMSBE.Models
{
    public class RenewalRegistration
    {
        public int RenewalRegistrationId { get; set; }
        public int LanguageId { get; set; }
        public string Template { get; set; }
    }
}