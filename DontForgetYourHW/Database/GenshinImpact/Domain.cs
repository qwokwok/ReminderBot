using System;
using System.ComponentModel.DataAnnotations;

namespace DontForgetYourHW.Database.GenshinImpact
{
    public class Domain
    {
        [Key]
        public long DomainId { get; set; }
        public long UserId { get; set; }
        public string AvatarUrl { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public long GuildChannelId { get; set; }
        public DateTime? Due { get; set; }
        public string Name { get; set; }
        public string Material { get; set; }
        public string MaterialImage { get; set; }
        public string Tag { get; set; }
    }
}
