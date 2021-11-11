using System;
using System.ComponentModel.DataAnnotations;

namespace DontForgetYourHW.Database.GenshinImpact
{
    public class Artifact
    {
        [Key]
        public long ArtifactId { get; set; }
        public long UserId { get; set; }
        public string AvatarUrl { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public DateTime? Due { get; set; }
        public long GuildChannelId { get; set; }
        public bool IsRemindEnabled { get; set; }

        public Artifact()
        {
            //setting default value
            IsRemindEnabled = true;
        }
    }
}
