using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Models
{
    public class Transition
    {
        public int TransitionId { get; set; }
        public string Key { get; set; }
        public int ScreenId { get; set; }
        public List<TransitionLanguage> transitionLanguages { get; set; }
        public Transition(List<TransitionLanguage> transitions)
        {
            if (transitions is null) transitions = new List<TransitionLanguage>();
            transitionLanguages = transitions;
        }
        /// <summary>
        /// Dịch
        /// </summary>
        /// <param name="transitionId">key</param>
        /// <param name="key">giá trị mặc định</param>
        /// <returns></returns>
        public string Translate(int transitionId, string key)
        {
            TransitionLanguage transition = transitionLanguages.Find(x => x.TransitionId == transitionId);
            return transition == null ? key : transition.Result;
        }
    }
}