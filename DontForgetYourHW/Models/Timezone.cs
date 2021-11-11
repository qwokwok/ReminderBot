using System;
using System.Collections.Generic;
using System.Text;

namespace DontForgetYourHW.Models
{
    public class Timezone
    {
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public bool HasDaylight { get; set; }

        public Timezone(string _abb, string _name, bool _hasDaylight)
        {
            Abbreviation = _abb;
            Name = _name;
            HasDaylight = _hasDaylight;
        }
    }
}
