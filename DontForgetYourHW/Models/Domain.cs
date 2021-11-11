using System;
using System.Collections.Generic;
using System.Text;

namespace DontForgetYourHW.Models
{
    public class Domain
    {
        public string Name { get; set; }
        public string Material { get; set; }
        public string NameImage { get; set; }
        public string MaterialImage { get; set; }
        public string Dungeon { get; set; }
        public List<string> Tag { get; set; }
        public List<int> Days { get; set; }
    }
}
