using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DontForgetYourHW.Database;
using DontForgetYourHW.Helpers.Notifications.Anime;
using DontForgetYourHW.Helpers.Notifications.Mangas;
using DontForgetYourHW.Helpers.RIT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DontForgetYourHW.Helpers
{
    public class Reminder
    {
        #region Fields

        private readonly CommandService _service;
        private readonly DiscordSocketClient _client;
        private const ulong GI_DEAF_CHANNEL = 785398582316498985;
        private const ulong HOBBY_CHANNEL = 785398582316498985;
        private Discord.Color RandomColor { get; set; } = new Color(Filtering.X(), Filtering.X(), Filtering.X());

        #endregion Fields

        #region Constructors

        public Reminder(CommandService service, DiscordSocketClient client)
        {
            _client = client;
            _service = service;
        }

        #endregion Constructors

        #region Handlers

        public async void DoTasks(object sender, ElapsedEventArgs e)
        {
            Program._interval.Stop();
            //await CheckHomework();
            await CheckManga();
            await CheckAnime();
            await PostRitNews();
            //await GetCases();
            await CheckResin();
            await CheckArtifact();
            await CheckCrystal();
            await CheckDomain();
            Program._interval.Start();
        }

        private async Task CheckDomain()
        {
            var list = await Program._db.Domain.ToListAsync();
            list = list.OrderBy(x => x.UserId).ToList();

            var canSave = false;
            var canMention = true;
            long currentlyId = 0;

            foreach (var item in list)
            {
                var hasReminder = false;

                if (item.Due < DateTime.Now)
                    hasReminder = true;

                if (hasReminder)
                {
                    var channel = _client.GetChannel((ulong)item.GuildChannelId) as IMessageChannel;

                    if (currentlyId == item.UserId)
                        canMention = false;

                    else
                        canMention = true;

                    if (canMention)
                        await channel.SendMessageAsync($"<@{item.UserId}>");

                    var sb = new StringBuilder();
                    currentlyId = item.UserId;

                    var embed = new EmbedBuilder();

                    if (item.Name == "Gold" ||
                    item.Name == "Freedom" ||
                    item.Name == "Ballad" ||
                    item.Name == "Prosperity" ||
                    item.Name == "Diligence" ||
                    item.Name == "Resistance")
                        sb.AppendLine($"It is now obtainable for farming\nto stock {item.Material}.");
                    else
                        sb.AppendLine($"It is now obtainable for farming\nto upgrade {item.Name}.");

                    var ea = new EmbedAuthorBuilder();
                    ea.WithName($"{item.Username}#{item.Discriminator}");
                    ea.WithIconUrl(item.AvatarUrl);
                    embed.WithTitle(item.Material);
                    embed.WithAuthor(ea);
                    embed.WithThumbnailUrl(item.MaterialImage);
                    embed.WithColor(RandomColor);
                    embed.Description = sb.ToString();
                    await channel.SendMessageAsync(null, false, embed.Build()).ConfigureAwait(false);

                    Program._db.Domain.Remove(item);
                    canSave = true;
                }
            }

            if(canSave)
                await Program._db.SaveChangesAsync().ConfigureAwait(false);
        }

        #endregion Handlers

        #region Methods

        public async Task PostRitNews()
        {
            var rit = new RitNews();
            var ritLink = await rit.PostRitNews();
            ritLink.Reverse();
            var listFromDB = await Program._db.Link.ToListAsync();
            var weweChannel = _client.GetChannel(743004274232918050) as IMessageChannel;

            var checkList = new List<string>();

            var hasAny = false;
            foreach (var item in listFromDB)
            {
                foreach (var link in ritLink)
                {
                    if (link.ToLower() == item.Address.ToLower())
                    {
                        if (!checkList.Contains(link))
                        {
                            checkList.Add(link);
                            hasAny = true;
                        }
                    }
                }
                if (checkList.Count >= 7)
                    break;
            }

            if (hasAny)
            {
                foreach (var item in checkList)
                {
                    if (ritLink.Contains(item))
                        ritLink.Remove(item);
                }
            }

            foreach (var item in ritLink)
            {
                await weweChannel.SendMessageAsync(item);
                await Program._db.Link.AddAsync(new Link() { Address = item });
            }

            await Program._db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task CheckHomework()
        {
            var botsmanagementChannel = _client.GetChannel(743004274232918050) as IMessageChannel;

            var list = await Program._db.Homework.ToListAsync();
            foreach (var item in list)
            {
                if (DateTime.Now > (DateTime)item.Due)
                {
                    item.IsCurrent = false;
                    item.IsRemindEnabled = false;
                    Program._db.Homework.Update(item);
                }

                if (item.IsRemindEnabled == true
                    && item.IsCurrent == true)
                {
                    var due = (DateTime)item.Due;
                    var now = DateTime.Now;

                    var howClose = due - now;
                    var hour = Math.Abs(howClose.Hours);
                    var min = Math.Abs(howClose.Minutes);

                    var timerString = "";
                    if (hour > 0)
                        timerString = $"{hour}hr ";

                    timerString += $"{min}min";

                    // to do - improve logic for reminding repeatly if user allows it
                    if (howClose.TotalHours <= 6)
                    {
                        await botsmanagementChannel.SendMessageAsync($"<@{item.UserId}> You have [{item.Abbreviation}] `{item.Name}` is dued in {timerString}. You better finish it.");
                        item.IsRemindEnabled = false;
                        Program._db.Homework.Update(item);
                    }
                    // ---
                }
            }
            await Program._db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task CheckManga()
        {
            var weweChannel = _client.GetChannel(HOBBY_CHANNEL) as IMessageChannel;

            var listFromDB = await Program._db.Manga.ToListAsync();

            var userHasNotifys = true;

            if (listFromDB.Count == 0)
                userHasNotifys = false;

            var isUpdating = false;

            if (userHasNotifys)
            {
                foreach (var item in listFromDB)
                {
                    var values = new Tuple<string, string, string>("", "", "");
                    switch (item.Source)
                    {
                        case Source.WebToons:
                            values = Webtoons.GetLinkFromWebToons(item.NameLink, item.LatestEpisode);
                            break;

                        case Source.MangaFox:
                            break;
                    }

                    var title = values.Item3;
                    var episode = values.Item2;
                    var link = values.Item1;

                    if (link != "")
                    {
                        var mentions = "";
                        var userList = listFromDB.Where(x => x.NameLink == item.NameLink).ToList();
                        foreach (var user in userList)
                        {
                            user.LatestEpisode = Convert.ToInt32(episode);
                            mentions += $"<@{user.UserId}> ";
                            Program._db.Manga.Update(user);
                        }

                        isUpdating = true;
                        await weweChannel.SendMessageAsync($"At last, " +
                            $"**{title}** is here. {mentions}\n{link}");
                    }
                }
                if (isUpdating)
                    await Program._db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private async Task CheckAnime()
        {
            var botsmanagementChannel = _client.GetChannel(HOBBY_CHANNEL) as IMessageChannel;

            var isSuccess = false;
            var list = await Program._db.Anime.ToListAsync();

            foreach (var item in list)
            {
                var tuples = FourAnime.GetLastEpisode(item.NameLink);
                var episode = tuples.Item1;
                if (episode != item.LatestEpisode.ToString() &&
                    episode != "" && tuples.Item2 != "")
                {
                    var userList = list.Where(x => x.Name == item.Name).ToList();
                    var mentions = "";

                    foreach (var user in userList)
                    {
                        mentions += $"<@{user.UserId}> ";

                        user.LatestEpisode = episode;
                        Program._db.Anime.Update(user);
                    }
                    isSuccess = true;

                    var embed = new EmbedBuilder();
                    var sb = new StringBuilder();
                    sb.AppendLine($"Episode {episode} is now available\n" +
                        $"on 4anime.");
                    embed.WithColor(RandomColor);
                    embed.WithTitle(item.Name);
                    embed.WithUrl($"{item.EpisodeLink}{episode}");
                    embed.WithThumbnailUrl("https://cdn.discordapp.com/attachments/743004274232918050/784422063939125318/4anime.png");
                    embed.WithImageUrl(item.Image);
                    embed.Description = sb.ToString();

                    await botsmanagementChannel.SendMessageAsync($"{mentions}").ConfigureAwait(false);
                    await botsmanagementChannel.SendMessageAsync(null, false, embed.Build()).ConfigureAwait(false);

                    await Task.Delay(200);
                }
            }

            if (isSuccess)
                await Program._db.SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task GetCases()
        {
            var wChannel = _client.GetChannel(743004274232918050) as IMessageChannel;
            string urlAddress = "https://www.rit.edu/ready/dashboard";

            var isInScheduledRange = Covid.IsScheduledRange();

            if (!isInScheduledRange)
                return;

            var colorAlert = Covid.GetAlertLevel();
            await wChannel.SendMessageAsync(colorAlert);

            using (WebClient client = new WebClient())
            {
                string code = client.DownloadString(urlAddress);
                string strStart = $"card-header position-relative  font-weight-normal\">";
                string strEnd = "</p>";

                int start = 0;
                int end = 0;

                start = code.IndexOf(strStart, 0);
                end = code.IndexOf(strEnd, start + strStart.Length);

                string result = "New Positive Cases from Past 14 Days for Students --> " + code.Substring(start + strStart.Length, end - (start + strStart.Length)).Trim();

                await wChannel.SendMessageAsync(result);
            }
        }

        private async Task CheckResin()
        {
            var wChannel = _client.GetChannel(GI_DEAF_CHANNEL) as IMessageChannel;
            var list = await Program._db.Resin.ToListAsync();

            foreach (var item in list)
            {
                if(item.IsRemindEnabled == true)
                {
                    var hasReminder = false;

                    var mins = ((DateTime)item.Due - DateTime.Now).TotalMinutes;
                    var resin = 160 - (mins/8);
                    if (resin > 160)
                        resin = 160;

                    var sb = new StringBuilder();

                    if (item.ReminderAt == 5)
                    {
                        if(((DateTime)item.Due - DateTime.Now).TotalMinutes < 8)
                        {
                            item.IsRemindEnabled = false;
                            item.ReminderAt = 1;
                            hasReminder = true;
                        }
                    }

                    if (hasReminder)
                    {
                        await wChannel.SendMessageAsync($"<@{item.UserId}>");

                        if (resin < 160)
                            sb.AppendLine($"Your resin has just replenished to **({Math.Floor(resin)}/160)**");

                        var embed = new EmbedBuilder();

                        var ea = new EmbedAuthorBuilder();
                        ea.WithName($"{item.Username}#{item.Discriminator}");
                        ea.WithIconUrl(item.AvatarUrl);
                        embed.WithAuthor(ea);
                        embed.WithThumbnailUrl("https://cdn.discordapp.com/attachments/743004274232918050/783703328647282698/resin.png");
                        embed.WithColor(RandomColor);
                        embed.Description = sb.ToString();
                        await wChannel.SendMessageAsync(null, false, embed.Build()).ConfigureAwait(false);

                    }
                }
            }
            await Program._db.SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task CheckArtifact()
        {
            var wChannel = _client.GetChannel(GI_DEAF_CHANNEL) as IMessageChannel;
            var list = await Program._db.Artifact.ToListAsync();

            foreach (var item in list)
            {
                if (item.IsRemindEnabled == true)
                {
                    var hasReminder = false;
                    var sb = new StringBuilder();

                    if (item.Due < DateTime.Now)
                        hasReminder = true;

                    if (hasReminder)
                    {
                        await wChannel.SendMessageAsync($"<@{item.UserId}>");

                        sb.AppendLine($"New artifacts have just respawned.");

                        var timezoneUsers = await Program._db.Timezone.ToListAsync().ConfigureAwait(false);
                        var tzUser = timezoneUsers.Where(x => x.UserId == (long)item.UserId).ToList();

                        Database.Timezone nameTimezone = new Database.Timezone();
                        if (tzUser.Count != 0)
                        {
                            foreach (var itemm in tzUser)
                            {
                                nameTimezone = itemm;
                                break;
                            }
                            /////
                            var time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, nameTimezone.TimezoneString);
                            var tzi = TimeZoneInfo.FindSystemTimeZoneById(nameTimezone.TimezoneString);
                            bool isDaylight = tzi.IsDaylightSavingTime(DateTime.Now);
                            /////

                            if (isDaylight == true)
                            {
                                var abb = nameTimezone.Abbrevlation.ToUpper();
                                var full = abb.Substring(0, 1) + "DT";
                                sb.AppendLine($"\nAt {time:MMMM dd h:mm tt} {abb}");
                            }
                            else
                                sb.AppendLine($"\nAt {time:MMMM dd h:mm tt} {nameTimezone.Abbrevlation}");
                        }
                        else
                            sb.AppendLine($"\nAt {DateTime.Now:MMMM dd h:mm tt} EST");

                        var embed = new EmbedBuilder();

                        var ea = new EmbedAuthorBuilder();
                        ea.WithName($"{item.Username}#{item.Discriminator}");
                        ea.WithIconUrl(item.AvatarUrl);
                        embed.WithAuthor(ea);
                        embed.WithThumbnailUrl("https://cdn.discordapp.com/attachments/743004274232918050/783661312344588298/artifact.png");
                        embed.WithColor(RandomColor);
                        embed.Description = sb.ToString();
                        await wChannel.SendMessageAsync(null, false, embed.Build()).ConfigureAwait(false);

                        Program._db.Artifact.Remove(item);
                    }
                }
            }
            await Program._db.SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task CheckCrystal()
        {
            var wChannel = _client.GetChannel(GI_DEAF_CHANNEL) as IMessageChannel;
            var list = await Program._db.Crystal.ToListAsync();

            foreach (var item in list)
            {
                if (item.IsRemindEnabled == true)
                {
                    var hasReminder = false;
                    var sb = new StringBuilder();

                    if (item.Due < DateTime.Now)
                        hasReminder = true;

                    if (hasReminder)
                    {
                        await wChannel.SendMessageAsync($"<@{item.UserId}>");

                        sb.AppendLine($"\nNew crystals have just respawned.");

                        var timezoneUsers = await Program._db.Timezone.ToListAsync().ConfigureAwait(false);
                        var tzUser = timezoneUsers.Where(x => x.UserId == (long)item.UserId).ToList();

                        Database.Timezone nameTimezone = new Database.Timezone();
                        if (tzUser.Count != 0)
                        {
                            foreach (var itemm in tzUser)
                            {
                                nameTimezone = itemm;
                                break;
                            }
                            /////
                            var time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, nameTimezone.TimezoneString);
                            var tzi = TimeZoneInfo.FindSystemTimeZoneById(nameTimezone.TimezoneString);
                            bool isDaylight = tzi.IsDaylightSavingTime(DateTime.Now);
                            /////

                            if (isDaylight == true)
                            {
                                var abb = nameTimezone.Abbrevlation.ToUpper();
                                var full = abb.Substring(0, 1) + "DT";
                                sb.AppendLine($"\nAt {time:MMMM dd h:mm tt} {abb}");
                            }
                            else
                                sb.AppendLine($"\nAt {time:MMMM dd h:mm tt} {nameTimezone.Abbrevlation}");
                        }
                        else
                            sb.AppendLine($"\nAt {DateTime.Now:MMMM dd h:mm tt} EST");

                        var embed = new EmbedBuilder();

                        var ea = new EmbedAuthorBuilder();
                        ea.WithName($"{item.Username}#{item.Discriminator}");
                        ea.WithIconUrl(item.AvatarUrl);
                        embed.WithAuthor(ea);
                        embed.WithThumbnailUrl("https://cdn.discordapp.com/attachments/743004274232918050/783665974142435338/crystal.png");
                        embed.WithColor(RandomColor);
                        embed.Description = sb.ToString();
                        await wChannel.SendMessageAsync(null, false, embed.Build()).ConfigureAwait(false);

                        Program._db.Crystal.Remove(item);
                    }
                }
            }
            await Program._db.SaveChangesAsync().ConfigureAwait(false);
        }
        #endregion Methods
    }
}