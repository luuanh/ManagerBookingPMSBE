using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class TransitionLanguage
    {
        public int TransitionLanguageId { get; set; }
        public int TransitionId { get; set; }
        public int LanguageId { get; set; }
        public string Result { get; set; }

        //
        public string Key { get; set; }
        public int ScreenId { get; set; }
    }
}