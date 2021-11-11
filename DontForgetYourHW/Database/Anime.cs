using System.ComponentModel.DataAnnotations;

namespace DontForgetYourHW.Database
{
    public class Anime
    {
        [Key]
        public long AnimeId { get; set; }
        public Source Source { get; set; }
        public string SourceLink { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public string Name { get; set; }
        public string NameLink { get; set; }
        public string EpisodeLink { get; set; }
        public string LatestEpisode { get; set; }
        public string Image { get; set; }
        public long GuildChannelId { get; set; }
    }
}
