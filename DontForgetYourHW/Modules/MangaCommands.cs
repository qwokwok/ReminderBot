using Discord;
using Discord.Commands;
using DontForgetYourHW.Database;
using DontForgetYourHW.Helpers;
using DontForgetYourHW.Helpers.Notifications.Mangas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontForgetYourHW.Modules
{
    public class MangaCommands : ModuleBase<SocketCommandContext>
    {
        private Discord.Color RandomColor { get; set; } = new Color(Filtering.X(), Filtering.X(), Filtering.X());

        private const string WEBTOON_URL = "https://cdn.discordapp.com/attachments/743004274232918050/784684940058165248/webtoon.png";

        #region Notify Helper - Done
        [Command("notifymanga")]
        public async Task NotifyWithWebToons()
        {
            var user = Context.User;
            var sb = new StringBuilder();

            //sb.AppendLine("Enter a number of which manga source you like to be notifed from?\n");
            //sb.AppendLine("```yaml\n1 = Webtoons");
            //sb.AppendLine("2 = Manga Fox - working on it```\n");

            sb.AppendLine("try this format below\n");
            //sb.AppendLine("Format:\n`qq.notifymanga NUMBER \"TITLE\"`\n");
            sb.AppendLine("Example:\n`qq notifymanga \"Tower of God\"`");

            var embed = new EmbedBuilder();
            embed.Title = "Adding Manga";
            embed.WithColor(RandomColor);
            embed.WithAuthor(user);
            embed.WithThumbnailUrl(WEBTOON_URL);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Notify with Weebtoons - Done
        [Command("notifymanga")]
        public async Task NotifyWithWebToons(string _name)
        {
            var user = Context.User;
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var isValid = true;
            var list = await Program._db.Manga.ToListAsync();
            var mangaList = list.Where(x => x.UserId == (long)user.Id).ToList();

            if (_name == "")
            {
                sb.Append("You haven't entered a manga name");
                isValid = false;
            }

            if (isValid)
            {
                var alreadyHas = false;
                foreach (var item in mangaList)
                {
                    if(item.Name == _name &&
                        item.Source == Source.WebToons)
                    {
                        alreadyHas = true;
                        break;
                    }
                }

                if(!alreadyHas)
                {
                    var link = Webtoons.CheckIfHave(_name);

                    if (link != "")
                    {
                        sb.AppendLine("Manga is found. You will be notified with the latest episode shortly to ensure it's working properly.\n");

                        var manga = new Manga()
                        {
                            Name = _name,
                            Source = Source.WebToons,
                            SourceLink = "https://www.webtoons.com/en/genre",
                            UserId = (long)user.Id,
                            Username = user.Username,
                            Discriminator = user.Discriminator,
                            NameLink = link,
                            LatestEpisode = 1
                        };

                        await Program._db.Manga.AddAsync(manga);
                        await Program._db.SaveChangesAsync();
                    }
                    else
                    {
                        embed.WithImageUrl("https://cdn.discordapp.com/attachments/743475559807582339/744704102122127439/example.png");
                        sb.AppendLine("Manga cannot be found. Make sure to enter manga with either uppercase or lowercase" +
                            " matching to a title from the source below.");
                        sb.AppendLine("https://www.webtoons.com/en/genre");

                        sb.AppendLine("\nNot working:\n`qq notifymanga \"tower of god\"`");
                        sb.AppendLine("\nWorking:\n`qq notifymanga \"Tower of God\"`\n");
                    }
                }
                else
                    sb.AppendLine($"`{_name}` is already on your list.\n");
            }

            var showlist = await Program._db.Manga.ToListAsync();
            var showUserList = showlist.Where(x => x.UserId == (long)user.Id)
                .OrderBy(y => y.Name)
                .ToList();

            sb.AppendLine("Manga collection");
            sb.AppendLine("```");

            foreach (var item in showUserList)
                sb.AppendLine($"{item.Name}");

            if (showUserList.Count == 0)
                sb.AppendLine("No Manga");

            sb.AppendLine("```");

            embed.Title = "Searching Manga";
            embed.WithColor(RandomColor);
            embed.WithThumbnailUrl(WEBTOON_URL);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Remove Manga Helper - Done
        [Command("remove-manga")]
        public async Task RemoveMangaHelper()
        {
            var user = Context.User;
            var sb = new StringBuilder();
            var list = await Program._db.Manga.ToListAsync();
            var mangaList = list.Where(x => x.UserId == (long)user.Id)
                .OrderBy(y => y.Name)
                .ToList();

            sb.AppendLine($"You can remove manga by using a command below");
            sb.AppendLine($"Example: `qq remove-manga \"tower of god\"`\n");

            sb.AppendLine("Your collection");
            sb.AppendLine("```\n");
            foreach (var item in mangaList)
                sb.AppendLine($"{item.Name}");

            if (mangaList.Count == 0)
                sb.AppendLine("No Manga");

            sb.AppendLine("\n```");

            var embed = new EmbedBuilder();
            embed.Title = "Removing Manga";
            embed.WithColor(RandomColor);
            embed.WithThumbnailUrl(WEBTOON_URL);
            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Remove Manga - Done
        [Command("remove-manga")]
        public async Task RemoveManga(string _name)
        {
            var user = Context.User;
            var sb = new StringBuilder();
            var list = await Program._db.Manga.ToListAsync();
            var mangaList = list.Where(x => x.UserId == (long)user.Id)
                .OrderBy(y => y.Name)
                .ToList();

            var isFound = false;
            foreach (var item in mangaList)
            {
                if (item.Name.ToLower() == _name.ToLower())
                {
                    Program._db.Manga.Remove(item);
                    mangaList.Remove(item);
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
            foreach (var item in mangaList)
                sb.AppendLine($"{item.Name}");

            if(mangaList.Count == 0)
                sb.AppendLine("No Manga");

            sb.AppendLine("\n```");


            var embed = new EmbedBuilder();
            embed.Title = "Removing Manga";
            embed.WithColor(RandomColor);
            embed.WithThumbnailUrl(WEBTOON_URL);
            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Weebtoons Manga List - Done
        //[Command("manga")]
        //public async Task WebToonsList()
        //{
        //    var sb = new StringBuilder();
        //    var mangaList = Webtoons.GetList();

        //    foreach (var item in mangaList)
        //        sb.AppendLine($"{item.Title}");

        //    var embed = new EmbedBuilder();
        //    embed.Title = "Which manga would you like to be notified?";
        //    embed.WithColor(RandomColor);
        //    embed.Description = sb.ToString();

        //    await ReplyAsync(null, false, embed.Build());
        //}
        #endregion
    }
}
