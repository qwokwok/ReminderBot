using System;
using System.Collections.Generic;
using System.Text;

namespace DontForgetYourHW.Models
{
    public class Level
    {
        public int Lvl { get; set; }
        public DateTime Date { get; set; }
        public bool IsAdded { get; set; }

        public Level(int _lvl)
        {
            Lvl = _lvl;
            IsAdded = false;
        }
    }
}
