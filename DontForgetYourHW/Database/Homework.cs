using System;
using System.ComponentModel.DataAnnotations;

namespace DontForgetYourHW.Database
{
    public partial class Homework
    {
        [Key]
        public long HomeworkId { get; set; }
        public long UserId { get; set; }
        public string CourseName { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public string Instruction { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsRemindEnabled { get; set; }
        public bool IsDone { get; set; }
        public DateTime? Due { get; set; }
        public long GuildChannelId { get; set; }
        public Homework()
        {
            //setting default values
            IsCurrent = true;
            IsDone = false;
            IsRemindEnabled = false;
        }
    }
}
