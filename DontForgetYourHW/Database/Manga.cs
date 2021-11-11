using System.ComponentModel.DataAnnotations;

namespace DontForgetYourHW.Database
{
    public class Manga
    {
        [Key]
        public long MangaId { get; set; }
        public Source Source { get; set; }
        public string SourceLink { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public string Name { get; set; }
        public string NameLink { get; set; }
        public int LatestEpisode { get; set; }
        public long GuildChannelId { get; set; }
    }

    public enum Source
    {
        WebToons,
        MangaFox,
    }
}
