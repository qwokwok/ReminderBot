using System;
using System.Collections.Generic;
using System.Text;

namespace DontForgetYourHW.Models
{
    public class Quest
    {
        public int Requirement { get; set; }
        public int Exp { get; set; }
        public string Name { get; set; }
        public Types Type { get; set; }
        public int TotalExp { get; set; }

        public enum Types
        {
            Story,
            Archon,
            World
        }
    }
}
