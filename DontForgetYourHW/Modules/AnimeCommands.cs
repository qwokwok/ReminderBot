using Discord;
using Discord.Commands;
using DontForgetYourHW.Database;
using DontForgetYourHW.Helpers;
using DontForgetYourHW.Helpers.Notifications.Anime;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontForgetYourHW.Modules
{
    public class AnimeCommands : ModuleBase<SocketCommandContext>
    {
        Discord.Color RandomColor { get; set; } = new Color(Filtering.X(), Filtering.X(), Filtering.X());
        private const string ANIME_URL = "https://cdn.discordapp.com/attachments/743004274232918050/784422063939125318/4anime.png";

        #region Add Anime Helper - Done
        [Command("notifyanime")]
        public async Task NotifyAnimeHelper()
        {
            var user = Context.User;
            var sb = new StringBuilder();
            var list = await Program._db.Anime.ToListAsync();
            var animeList = list.Where(x => x.UserId == (long)user.Id)
                .OrderBy(y => y.Name)
                .ToList();

            sb.AppendLine($"To add new anime to your list, you can use a command like below");
            sb.AppendLine($"Example: `qq notifyanime \"one piece\"`");
            sb.AppendLine($"Note: It only works for anime that is on currently airing\n");

            sb.AppendLine("Your collection");
            sb.AppendLine("```\n");
            foreach (var item in animeList)
                sb.AppendLine($"{item.Name}");

            if (animeList.Count == 0)
                sb.AppendLine("No Anime");

            sb.AppendLine("\n```");

            var embed = new EmbedBuilder();
            embed.Title = "Your Anime List";
            embed.WithColor(RandomColor);
            embed.WithThumbnailUrl(ANIME_URL);
            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Add Anime - Done
        [Command("notifyanime")]
        public async Task NotifyAnime(string _name)
        {
            var user = Context.User;
            var sb = new StringBuilder();
            var list = await Program._db.Anime.ToListAsync();
            var animeList = list.Where(x => x.UserId == (long)user.Id).ToList();
            var hasAnime = false;

            var embed = new EmbedBuilder();
            embed.WithAuthor(user);

            foreach (var item in animeList)
            {
                if (item.Name.ToLower() == _name.ToLower())
                {
                    hasAnime = true;
                    sb.AppendLine($"`{item.Name}` is already on your list.");
                    break;
                }
            }

            var isSuccess = false;
            if (!hasAnime)
            {
                var tuples = FourAnime.CheckIfHave(_name);
                if (tuples.Item1 != "")
                {
                    var episodeTuples = FourAnime.GetLastEpisode(tuples.Item1);

                    if (episodeTuples.Item1 != "" || episodeTuples.Item2 != "")
                    {
                        var anime = new Anime()
                        {
                            LatestEpisode = episodeTuples.Item1,
                            Name = tuples.Item2,
                            NameLink = tuples.Item1,
                            SourceLink = "https://4anime.to/",
                            UserId = (long)user.Id,
                            Username = user.Username,
                            Discriminator = user.Discriminator,
                            EpisodeLink = episodeTuples.Item2,
                            Image = "https://4anime.to/" + tuples.Item3
                        };
                        await Program._db.Anime.AddAsync(anime).ConfigureAwait(false);
                        await Program._db.SaveChangesAsync().ConfigureAwait(false);
                        sb.AppendLine("Added successfully.");
                        isSuccess = true;
                        embed.WithFooter($"{tuples.Item2}");
                        embed.WithImageUrl("https://4anime.to/" + tuples.Item3);
                    }
                }
                else
                    sb.AppendLine($"{_name} cannot be found on `4Anime`'s currently airing.");
            }

            if (isSuccess)
            {
                var showlist = await Program._db.Anime.ToListAsync();
                var showUserList = showlist.Where(x => x.UserId == (long)user.Id)
                   .OrderBy(y => y.Name)
                   .ToList();


                sb.AppendLine($"You now have {showUserList.Count} anime.");
            //    sb.AppendLine("Your anime list");
            //    sb.AppendLine("```");
            //    foreach (var item in showUserList)
            //        sb.AppendLine($"{item.Name}");
            //
            //    if (showUserList.Count == 0)
            //        sb.AppendLine("No Anime");
            //
            //    sb.AppendLine("```");
            }

            embed.Title = "Searching Anime";
            embed.WithAuthor(user);
            embed.WithThumbnailUrl(ANIME_URL);
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Remove Anime Helper - Done
        [Command("remove-anime")]
        public async Task RemoveAnimeHelper()
        {
            var user = Context.User;
            var sb = new StringBuilder();
            var list = await Program._db.Anime.ToListAsync();
            var animeList = list.Where(x => x.UserId == (long)user.Id)
                .OrderBy(y => y.Name)
                .ToList();

            sb.AppendLine($"Try a command below to remove anime from the list.");
            sb.AppendLine($"Example: `qq remove-anime \"one piece\"`\n");

            sb.AppendLine("Your collection");
            sb.AppendLine("```\n");
            foreach (var item in animeList)
                sb.AppendLine($"{item.Name}");

            if (animeList.Count == 0)
                sb.AppendLine("No Anime");

            sb.AppendLine("\n```");

            var embed = new EmbedBuilder();
            embed.Title = "Removing Anime";
            embed.WithAuthor(user);
            embed.WithColor(RandomColor);
            embed.WithThumbnailUrl(ANIME_URL);
            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Remove Anime - Done
        [Command("remove-anime")]
        public async Task RemoveAnime(string _name)
        {
            var user = Context.User;
            var sb = new StringBuilder();
            var list = await Program._db.Anime.ToListAsync();
            var animeList = list.Where(x => x.UserId == (long)user.Id)
                .OrderBy(y => y.Name)
                .ToList();

            var isFound = false;
            foreach (var item in animeList)
            {
                if (item.Name.ToLower() == _name.ToLower())
                {
                    Program._db.Anime.Remove(item);
                    animeList.Remove(item);
                    sb.AppendLine($"`{item.Name}` is removed.\n");
                    isFound = true;
                    break;
                }
            }

            if (!isFound)
                sb.AppendLine($"`{_name}` isn't on your list.\n");

            else
                await Program._db.SaveChangesAsync().ConfigureAwait(false);

            sb.AppendLine("Your collection");
            sb.AppendLine("```\n");
            foreach (var item in animeList)
                sb.AppendLine($"{item.Name}");

            if (animeList.Count == 0)
                sb.AppendLine("No Anime");

            sb.AppendLine("\n```");

            var embed = new EmbedBuilder();
            embed.Title = "Removing Anime";
            embed.WithColor(RandomColor);
            embed.WithThumbnailUrl(ANIME_URL);
            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion
    }
}
