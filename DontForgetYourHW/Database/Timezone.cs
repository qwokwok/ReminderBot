using System.ComponentModel.DataAnnotations;

namespace DontForgetYourHW.Database
{
    public class Timezone
    {
        [Key]
        public long TimezoneId { get; set; }
        public long UserId { get; set; }
        public string Abbrevlation { get; set; }
        public string TimezoneString { get; set; }
    }
}
