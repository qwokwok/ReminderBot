using System.ComponentModel.DataAnnotations;

namespace DontForgetYourHW.Database
{
    public partial class Link
    {
        [Key]
        public long Id { get; set; }
        public string Address { get; set; }
    }
}
