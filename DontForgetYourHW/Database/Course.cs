using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DontForgetYourHW.Database
{
    [Table("Course")]
    public partial class Course
    {
        [Key]
        public long CourseId { get; set; }
        public long UserId { get; set; }
        public string Abbreviation { get; set; }
        public string CourseName { get; set; }
        public int DayIndex { get; set; }
        public string Days { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public long GuildChannelId { get; set; }
    }
}
