using Discord;
using Discord.Commands;
using DontForgetYourHW.Database.GenshinImpact;
using DontForgetYourHW.Helpers;
using DontForgetYourHW.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontForgetYourHW.Modules
{
    public class GenshinImpactCommands : ModuleBase<SocketCommandContext>
    {
        private Discord.Color RandomColor { get; set; } = new Color(Filtering.X(), Filtering.X(), Filtering.X());

        private const int RESIN_CAPACITY = 160;
        private const string ARTIFACT_URL = "https://cdn.discordapp.com/attachments/743004274232918050/783661312344588298/artifact.png";
        private const string RESIN_URL = "https://cdn.discordapp.com/attachments/743004274232918050/783703328647282698/resin.png";
        private const string AR_URL = "https://cdn.discordapp.com/attachments/743004274232918050/783664331916181554/ar.png";
        private const string CRYSTAL_URL = "https://cdn.discordapp.com/attachments/743004274232918050/783665974142435338/crystal.png";
        private const string REP_URL = "https://cdn.discordapp.com/attachments/743004274232918050/783666145496399903/rep.png";
        private const string EXAMPLE_URL = "https://cdn.discordapp.com/attachments/772358312297496656/784395243419336714/unknown.png";
        private const string REP_EXAMPLE_URL = "https://cdn.discordapp.com/attachments/772358312297496656/784408187859763230/Untitled.png";

        private const int FOUR_ZERO_TOTAL = 145375;
        private const int FOUR_FIVE_TOTAL = 207775;
        private const int FIVE_ZERO_TOTAL = 294175;
        private const int FIVE_FIVE_TOTAL = 450175;
        private const int FIVE_SIX_TOTAL = 682525;

        public static List<Quest> Quests { get; set; }

        [Command("when")]
        public async Task WhenHelper()
        {
            var embed = new EmbedBuilder();
            var sb = new StringBuilder();
            embed.WithColor(RandomColor);
            sb.AppendLine("Displaying time when a certain amount of resin is met.\n");
            sb.AppendLine("qq when `CURRENT_AMOUNT_RESIN` `FINAL_AMOUNT_RESIN`");
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }

        [Command("when")]
        public async Task When(string _current, string _goal)
        {
            try
            {
                var current = Int32.Parse(_current);
                var goal = Int32.Parse(_goal);

                var embed = new EmbedBuilder();
                var sb = new StringBuilder();

                embed.WithColor(RandomColor);    

                if (goal > current)
                {
                    var left = goal - current;
                    var totalMinute = left * 8;

                    var reachedTime = DateTime.Now.AddMinutes(totalMinute);

                    var user = Context.User;
                    var list = await Program._db.Timezone.ToListAsync().ConfigureAwait(false);
                    var records = list.Where(x => x.UserId == (long)user.Id).ToList();

                    if(records.Count == 0)
                    {
                        //sb.AppendLine($"{goal} resin will be reached at {reachedTime:h:mm tt} CST");
                        //sb.AppendLine($"{goal} resin is being reached from {current} resin at **{reachedTime:h:mm tt} EST**");

                        sb.AppendLine($"Your resin is hitting {goal} from {current} at **{reachedTime:h:mm tt} EST**");
                    }

                    else
                    {
                        Database.Timezone nameTimezone = new Database.Timezone();

                        foreach (var item in records)
                        {
                            nameTimezone = item;
                            break;
                        }
                        /////
                        var time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(reachedTime, nameTimezone.TimezoneString);
                        var tzi = TimeZoneInfo.FindSystemTimeZoneById(nameTimezone.TimezoneString);
                        bool isDaylight = tzi.IsDaylightSavingTime(DateTime.Now);

                        //sb.AppendLine($"{goal} resin will be reached at {time:h:mm tt} {nameTimezone.Abbrevlation}");
                        //sb.AppendLine($"{goal} resin is being reached from {current} resin at **{time:h:mm tt} {nameTimezone.Abbrevlation}**");

                        sb.AppendLine($"Your resin is reaching {goal} from {current} at **{time:h:mm tt} {nameTimezone.Abbrevlation}**");
                    }
                }
                else
                    sb.AppendLine("The first number cannot be greater than the current number.");

                embed.Description = sb.ToString();

                await ReplyAsync(null, false, embed.Build());
            }
            catch (FormatException)
            {
                await Context.Channel.SendMessageAsync("Please enter valid number");
            }
        }

        #region Character commands
            [Command("razor")]
        public async Task GiveRazor() => await PostCharacter("Razor", Constants.RAZORICONURL, Constants.RAZORIMAGEURL, Constants.ELECTRO, "The design was created by Gobelyn • 12/10/20");

        [Command("beidou")]
        public async Task GiveBeidou() => await PostCharacter("Beidou", Constants.BEIDOUICONURL, Constants.BEIDOUIMAGEURL, Constants.ELECTRO, "The design was created by Gobelyn • 11/18/20");

        [Command("lisa")]
        public async Task GiveLisa() => await PostCharacter("Lisa", Constants.LISAICONURL, Constants.LISAIMAGEURL, Constants.ELECTRO, "Coming soon");

        [Command("keqing")]
        public async Task GiveKeqing() => await PostCharacter("Keqing", Constants.KEQINGICONURL, Constants.KEQINGIMAGEURL, Constants.ELECTRO, "The design was created by Gobelyn • 11/18/20");

        [Command("fischl")]
        public async Task GiveFischl() => await PostCharacter("Fischl", Constants.FISCHLICONURL, Constants.FISCHLIMAGEURL, Constants.ELECTRO, "The design was created by Gobelyn • 12/8/20");

        [Command("jean")]
        public async Task GiveJean() => await PostCharacter("Jean", Constants.JEANICONURL, Constants.JEANIMAGEURL, Constants.ANEMO, "The design was created by Gobelyn • 11/18/20");

        [Command("sucrose")]
        public async Task GiveSucrose() => await PostCharacter("Sucrose", Constants.SUCROSEICONURL, Constants.SUCROSEIMAGEURL, Constants.ANEMO, "The design was created by Gobelyn • 11/29/20");

        [Command("venti")]
        public async Task GiveVenti() => await PostCharacter("Venti", Constants.VENTIICONURL, Constants.VENTIIMAGEURL, Constants.ANEMO, "The design was created by Gobelyn • 11/24/20");

        [Command("xiao")]
        public async Task GiveXiao() => await PostCharacter("Xiao", Constants.XIAOICONURL, Constants.XIAOIMAGEURL, Constants.ANEMO, "Coming soon");

        [Command("zhongli")]
        public async Task GiveZhongi() => await PostCharacter("Zhongli", Constants.ZHONGLIICONURL, Constants.ZHONGLIIMAGEURL, Constants.GEO, "The design was created by Gobelyn • 12/2/20");

        [Command("ning"), Alias("ningguang")]
        public async Task GiveNing() => await PostCharacter("Ningguang", Constants.NINGICONURL, Constants.NINGIMAGEURL, Constants.GEO, "The design was created by Gobelyn  • 11/16/20");

        [Command("noelle")]
        public async Task GiveNoelle() => await PostCharacter("Noelle", Constants.NOELLEICONURL, Constants.NOELLEIMAGEURL, Constants.GEO, "Coming soon");

        [Command("xingqiu")]
        public async Task GiveXingqiu() => await PostCharacter("Xingqiu", Constants.XINGQIUICONURL, Constants.XINGQIUMAGEURL, Constants.HYDRO, "The design was created by Gobelyn • 11/18/20");

        [Command("barbara"), Alias("barb")]
        public async Task GiveBarbara() => await PostCharacter("Barbara", Constants.BARBARAICONURL, Constants.BARBARAMAGEURL, Constants.HYDRO, "Coming soon");

        [Command("mona")]
        public async Task GiveMona() => await PostCharacter("Mona", Constants.MONAICONURL, Constants.MONAMAGEURL, Constants.HYDRO, "Coming soon");

        [Command("childe"), Alias("tartaglia")]
        public async Task GiveChilde() => await PostCharacter("Tartaglia", Constants.CHILDEICONURL, Constants.CHILDEMAGEURL, Constants.HYDRO, "The design was created by Gobelyn • 11/18/20");

        [Command("diona")]
        public async Task GiveDiona() => await PostCharacter("Diona", Constants.DIONAICONURL, Constants.DIONAIMAGEURL, Constants.CYRO, "The design was created by Gobelyn • 11/18/20");

        [Command("qiqi")]
        public async Task GiveQiqi() => await PostCharacter("Qiqi", Constants.QIQIICONURL, Constants.QIQIIMAGEURL, Constants.CYRO, "Coming soon");

        [Command("kaeya")]
        public async Task GiveKaeya() => await PostCharacter("Kaeya", Constants.KAEYAICONURL, Constants.KAEYAIMAGEURL, Constants.CYRO, "Coming soon");

        [Command("chongyun")]
        public async Task GiveChongyun() => await PostCharacter("Chongyun", Constants.CHONGYUNICONURL, Constants.CHONGYUNIMAGEURL, Constants.CYRO, "Coming soon");

        [Command("ganyu")]
        public async Task GiveGanyu() => await PostCharacter("Ganyu", Constants.GANYUICONURL, Constants.GANYUIMAGEURL, Constants.CYRO, "Coming soon");

        [Command("diluc")]
        public async Task GiveDiluc() => await PostCharacter("Diluc", Constants.DILUCICONURL, Constants.DILUCIMAGEURL, Constants.PYRO, "The design was created by Gobelyn • 11/20/20");

        [Command("xiangling")]
        public async Task GiveXiangling() => await PostCharacter("Xiangling", Constants.XIANGLINGICONURL, Constants.XIANGLINGIMAGEURL, Constants.PYRO, "The design was created by Gobelyn • 11/18/20");

        [Command("bennett")]
        public async Task GiveBennett() => await PostCharacter("Bennett", Constants.BENNETTICONURL, Constants.BENNETIMAGEURL, Constants.PYRO, "The design was created by Gobelyn • 11/18/20");

        [Command("klee")]
        public async Task GiveKlee() => await PostCharacter("Klee", Constants.KLEEICONURL, Constants.KLEEIMAGEURL, Constants.PYRO, "The design was created by Gobelyn • 11/18/20");

        [Command("xinyan")]
        public async Task GiveXinyan() => await PostCharacter("Xinyan", Constants.XINYANICONURL, Constants.XINYANIMAGEURL, Constants.PYRO, "Coming soon");

        [Command("amber")]
        public async Task GiveAmber() => await PostCharacter("Amber", Constants.AMBERICONURL, Constants.AMBERIMAGEURL, Constants.PYRO, "Coming soon");
        #endregion

        #region Domain Reminder commands
        [Command("remindme")]
        public async Task RemindMe(string _first)
        {
            var word = _first;
            Models.Domain nameItem = GetDomain(word);

            await SetDomainReminder(nameItem);
        }

        [Command("remindme")]
        public async Task RemindMeTwo(string _first, string _second)
        {
            var word = $"{_first} {_second}";
            Models.Domain nameItem = GetDomain(word);
            await SetDomainReminder(nameItem);
        }

        [Command("remindme")]
        public async Task RemindMeThree(string _first, string _second, string _third)
        {
            var word = $"{_first} {_second} {_third}";
            Models.Domain nameItem = GetDomain(word);
            await SetDomainReminder(nameItem);
        }

        [Command("remindme")]
        public async Task RemindMeFour(string _first, string _second, string _third, string _four)
        {
            var word = $"{_first} {_second} {_third} {_four}";
            Models.Domain nameItem = GetDomain(word);
            await SetDomainReminder(nameItem);
        }

        [Command("remindme")]
        public async Task RemindMeFive(string _first, string _second, string _third, string _four, string _five)
        {
            var word = $"{_first} {_second} {_third} {_four} {_five}";
            Models.Domain nameItem = GetDomain(word);
            await SetDomainReminder(nameItem);
        }
        #endregion

        [Command("timezone")]
        public async Task ChangeTimezone(string _zone)
        {
            var timezone = CheckTimezone(_zone.ToLower());
            var user = Context.User;
            var list = await Program._db.Timezone.ToListAsync().ConfigureAwait(false);
            var records = list.Where(x => x.UserId == (long)user.Id).ToList();
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            embed.WithAuthor(user);
            embed.WithTitle("Changing Timezone");
            embed.WithColor(RandomColor);

            if (timezone == null)
            {
                sb.AppendLine("The timezone you entered is invalid.\n");
                sb.AppendLine("https://www.timeanddate.com/time/zones/");
                sb.AppendLine("~Here's the list of timezones above~");
                sb.AppendLine("Make sure to enter with an abbrevlation.");
            }

            else
            {
                /////
                var time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, timezone.Name);
                var tzi = TimeZoneInfo.FindSystemTimeZoneById(timezone.Name);
                bool isDaylight = tzi.IsDaylightSavingTime(DateTime.Now);
                /////

                if (isDaylight == true)
                {
                    var abb = _zone.ToUpper();
                    var full = abb.Substring(0, 1) + "DT";
                    sb.AppendLine($"Your timezone is now **{time:h:mm tt} {full}**.");
                }
                else
                    sb.AppendLine($"Your timezone is now **{time:h:mm tt} {_zone.ToUpper()}**.");

                if (records.Count == 0)
                {
                    var tz = new Database.Timezone
                    {
                        UserId = (long)user.Id,
                        Abbrevlation = _zone.ToUpper(),
                        TimezoneString = timezone.Name
                    };
                    await Program._db.Timezone.AddAsync(tz).ConfigureAwait(false);
                }
                else
                {
                    foreach (var item in records)
                    {
                        if (item.UserId == (long)user.Id)
                        {
                            item.Abbrevlation = _zone.ToUpper();
                            item.TimezoneString = timezone.Name;
                        }
                    }
                }
                await Program._db.SaveChangesAsync().ConfigureAwait(false);
            }

            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("resin")]
        public async Task UpdateResin(string _amount)
        {
            var user = Context.User;
            var list = await Program._db.Resin.ToListAsync().ConfigureAwait(false);
            var records = list.Where(x => x.UserId == (long)user.Id).ToList();
            var timezoneUsers = await Program._db.Timezone.ToListAsync().ConfigureAwait(false);
            var tzUser = timezoneUsers.Where(x => x.UserId == (long)user.Id).ToList();

            var isValid = false;

            try
            {
                int number = Int32.Parse(_amount);

                if (number <= 158 && number >= 0)
                    isValid = true;

                if (isValid)
                {
                    var sb = new StringBuilder();
                    int left = RESIN_CAPACITY - number;
                    int minutes = left * 8;
                    var when = DateTime.Now.AddMinutes(minutes);
                    var day = GetDay(when);

                    sb.AppendLine($"Your resin is now **({number}/160)**");

                    Database.Timezone nameTimezone = new Database.Timezone();
                    if(tzUser.Count != 0)
                    {
                        foreach (var item in tzUser)
                        {
                            nameTimezone = item;
                            break;
                        }
                        /////
                        var time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(when, nameTimezone.TimezoneString);
                        var tzi = TimeZoneInfo.FindSystemTimeZoneById(nameTimezone.TimezoneString);
                        bool isDaylight = tzi.IsDaylightSavingTime(DateTime.Now);
                        /////

                        if (isDaylight == true)
                        {
                            var abb = nameTimezone.Abbrevlation.ToUpper();
                            var full = abb.Substring(0, 1) + "DT";
                            sb.AppendLine($"\nIt will be fully replenished at **{time:h:mm tt} {abb}** {day}.");
                        }
                        else
                            sb.AppendLine($"\nIt will be fully replenished at **{time:h:mm tt} {nameTimezone.Abbrevlation}** {day}.");
                    }
                    else
                    {
                        sb.AppendLine($"\nIt will be fully replenished at **{when:h:mm tt} EST** {day}.");
                    }

                    var line = "";
                    int reminderAt = 0;
                    if (number < 159)
                    {
                        line = $"\nYou will be notified at 1 resin away from the capacity.";
                        reminderAt = 5;
                    }

                    if (records.Count == 0)
                    {
                        var resin = new Resin
                        {
                            Due = DateTime.Now.AddMinutes(minutes),
                            UserId = (long)user.Id,
                            Username = user.Username,
                            Discriminator = user.Discriminator,
                            AvatarUrl = user.GetAvatarUrl(ImageFormat.Auto, 128),
                            ReminderAt = reminderAt
                        };
                        await Program._db.Resin.AddAsync(resin).ConfigureAwait(false);
                    }
                    else
                    {
                        foreach (var item in records)
                        {
                            if (item.UserId == (long)user.Id)
                            {
                                item.Due = DateTime.Now.AddMinutes(minutes);
                                item.IsRemindEnabled = true;
                                item.ReminderAt = reminderAt;
                            }
                        }
                    }
                    await Program._db.SaveChangesAsync().ConfigureAwait(false);

                    var embed = new EmbedBuilder();
                    embed.WithAuthor(user);
                    embed.WithThumbnailUrl(RESIN_URL);
                    embed.WithColor(RandomColor);
                    embed.WithFooter(line);
                    embed.Description = sb.ToString();

                    await ReplyAsync(null, false, embed.Build());
                }
                else
                    await Context.Channel.SendMessageAsync("Enter amount between 0-158");
            }
            catch (FormatException)
            {
                await Context.Channel.SendMessageAsync("Please enter valid number");
            }
        }

        [Command("resin")]
        public async Task CheckResin()
        {
            var user = Context.User;
            var list = await Program._db.Resin.ToListAsync().ConfigureAwait(false);
            var records = list.Where(x => x.UserId == (long)user.Id).ToList();

            var timezoneUsers = await Program._db.Timezone.ToListAsync().ConfigureAwait(false);
            var tzUser = timezoneUsers.Where(x => x.UserId == (long)user.Id).ToList();

            DateTime date = DateTime.Now;
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();

            if (records.Count == 0)
            {
                sb.AppendLine($"You haven't told me your resin amount.");
                sb.AppendLine($"\nIf you would like me to notify you, try below");
                sb.AppendLine($"\n`qq resin AMOUNT`");
            }
            else
            {
                foreach (var item in records)
                {
                    if (item.UserId == (long)user.Id)
                    {
                        date = (DateTime)item.Due;
                        break;
                    }
                }
                var leftTime = date - DateTime.Now;
                var minute = leftTime.TotalMinutes;
                var resins = RESIN_CAPACITY - (minute / 8);

                if (resins > RESIN_CAPACITY)
                    resins = RESIN_CAPACITY;

                sb.AppendLine($"You have **({Math.Floor(resins)}/160)** resin at the moment.");

                var day = GetDay(date);
                if (date > DateTime.Now)
                {
                    Database.Timezone nameTimezone = new Database.Timezone();
                    if (tzUser.Count != 0)
                    {
                        foreach (var item in tzUser)
                        {
                            nameTimezone = item;
                            break;
                        }
                        /////
                        var time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, nameTimezone.TimezoneString);
                        var tzi = TimeZoneInfo.FindSystemTimeZoneById(nameTimezone.TimezoneString);
                        bool isDaylight = tzi.IsDaylightSavingTime(DateTime.Now);
                        /////

                        if (isDaylight == true)
                        {
                            var abb = nameTimezone.Abbrevlation.ToUpper();
                            var full = abb.Substring(0, 1) + "DT";
                            sb.AppendLine($"\nIt will be fully replenished at **{time:h:mm tt} {abb}** {day}.");
                        }
                        else
                            sb.AppendLine($"\nIt will be fully replenished at **{time:h:mm tt} {nameTimezone.Abbrevlation}** {day}.");
                    }
                    else
                    {
                        sb.AppendLine($"\nIt will be fully replenished at **{date:h:mm tt} EST** {day}.");
                    }
                }

                else
                {
                    ////////////////////
                    var wasted = DateTime.Now - date;
                    var hour = wasted.Hours;
                    var min = wasted.Minutes;

                    var timerString = "";
                    if (hour > 0)
                        timerString = $"{hour} hour(s) ";

                    timerString += $"{min} minute(s)";
                    ////////////////////
                    var wastedResin = Math.Floor(wasted.TotalMinutes / 8);
                    sb.AppendLine($"\nYour resin has been fully replenished for {timerString},\nyou have wasted **{wastedResin}** resin. You have lost {Math.Floor(wastedResin * .833333333333333333333)} primogem worth of the resin.");

                    embed.WithFooter("Considering 50 primogems = 60 resin in ratio.");
                }
            }

            embed.WithAuthor(user);
            embed.WithColor(RandomColor);
            embed.WithThumbnailUrl(RESIN_URL);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }

        [Command("girep")]
        public async Task CheckRepHelper()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Enter your rep level and rep exp");
            sb.AppendLine($"`qq girep LEVEL EXP`");
            var embed = new EmbedBuilder();
            embed.WithTitle($"Reputation");
            embed.WithThumbnailUrl(REP_URL);
            embed.WithImageUrl(REP_EXAMPLE_URL);
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }

        [Command("girep")]
        public async Task CheckRep(string _level, string _exp)
        {
            try
            {
                var level = Int32.Parse(_level);
                var exp = Int32.Parse(_exp);
                var isValid = false;

                if (level > 0 && level < 8)
                    isValid = true;

                if (isValid)
                {
                    var capacity = 0;
                    var totalExp = 0;

                    switch (level)
                    {
                        case 1: capacity = 400; break;
                        case 2: capacity = 420; totalExp = 400; break;
                        case 3: capacity = 440; totalExp = 820; break;
                        case 4: capacity = 460; totalExp = 1260; break;
                        case 5: capacity = 480; totalExp = 1720; break;
                        case 6: capacity = 500; totalExp = 2200; break;
                        case 7: capacity = 520; totalExp = 2700; break;
                    }

                    if (capacity > exp && exp > 0)
                    {
                        var left = 3220 - (totalExp + exp);
                        decimal weeks = left / 420;

                        var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                        var due = monday.AddDays(7 * (1 + (double)Math.Floor(weeks)));

                        var sb = new StringBuilder();
                        sb.AppendLine($"Experience:\t{_exp}/{capacity}");
                        sb.AppendLine($"\nAssuming you are completing every request and every bounty with the most" +
                                        $" possible exp reward every week, your reputation will be Lv. 8 on {due:MMMM dd}.");
                        var embed = new EmbedBuilder();
                        embed.WithTitle($"Reputation Level: {_level}");
                        embed.WithThumbnailUrl(REP_URL);
                        embed.WithColor(RandomColor);
                        embed.Description = sb.ToString();

                        await ReplyAsync(null, false, embed.Build());
                    }
                    else
                        await Context.Channel.SendMessageAsync("Please enter valid exp amount");
                }
            }
            catch (FormatException)
            {
                await Context.Channel.SendMessageAsync("Please enter valid number");
            }
        }

        [Command("artifactreset")]
        public async Task ResetArtifact()
        {
            var user = Context.User;
            var list = await Program._db.Artifact.ToListAsync().ConfigureAwait(false);
            var records = list.Where(x => x.UserId == (long)user.Id).ToList();
            var nextDay = DateTime.Now.AddDays(1);

            var timezoneUsers = await Program._db.Timezone.ToListAsync().ConfigureAwait(false);
            var tzUser = timezoneUsers.Where(x => x.UserId == (long)user.Id).ToList();

            if (records.Count == 0)
            {
                var artifact = new Artifact
                {
                    Due = nextDay,
                    UserId = (long)user.Id,
                    Username = user.Username,
                    Discriminator = user.Discriminator,
                    AvatarUrl = user.GetAvatarUrl(ImageFormat.Auto, 128)
                };
                await Program._db.Artifact.AddAsync(artifact).ConfigureAwait(false);
            }
            else
            {
                foreach (var item in records)
                {
                    if (item.UserId == (long)user.Id)
                    {
                        item.Due = nextDay;
                        item.IsRemindEnabled = true;
                    }
                }
            }
            await Program._db.SaveChangesAsync().ConfigureAwait(false);

            var sb = new StringBuilder();

            Database.Timezone nameTimezone = new Database.Timezone();
            if (tzUser.Count != 0)
            {
                foreach (var item in tzUser)
                {
                    nameTimezone = item;
                    break;
                }
                /////
                var time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(nextDay, nameTimezone.TimezoneString);
                var tzi = TimeZoneInfo.FindSystemTimeZoneById(nameTimezone.TimezoneString);
                bool isDaylight = tzi.IsDaylightSavingTime(DateTime.Now);
                /////

                if (isDaylight == true)
                {
                    var abb = nameTimezone.Abbrevlation.ToUpper();
                    var full = abb.Substring(0, 1) + "DT";
                    sb.AppendLine($"New artifacts will be respawned at {time:h:mm tt} {abb} tomorrow.");
                }
                else
                    sb.AppendLine($"New artifacts will be respawned at {time:h:mm tt} {nameTimezone.Abbrevlation} tomorrow.");
            }
            else
                sb.AppendLine($"New artifacts will be respawned at {nextDay:h:mm tt} EST tomorrow.");

            var embed = new EmbedBuilder();
            embed.WithTitle($"Farming artifacts");
            embed.WithAuthor(user);
            embed.WithThumbnailUrl(ARTIFACT_URL);
            embed.WithFooter("You will be notified via channel once the artifacts are respawned.");
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }

        [Command("artifact")]
        public async Task Artifact()
        {
            var user = Context.User;
            var list = await Program._db.Artifact.ToListAsync().ConfigureAwait(false);
            var records = list.Where(x => x.UserId == (long)user.Id).ToList();

            var timezoneUsers = await Program._db.Timezone.ToListAsync().ConfigureAwait(false);
            var tzUser = timezoneUsers.Where(x => x.UserId == (long)user.Id).ToList();

            var sb = new StringBuilder();
            var embed = new EmbedBuilder();

            if (records.Count != 0)
            {
                ////////////////////
                var date = (DateTime)records[0].Due - DateTime.Now;

                var hour = date.Hours;
                var min = date.Minutes;

                var timerString = "";
                if (hour > 0)
                    timerString = $"{hour} hour(s) ";

                timerString += $"{min} minute(s)";
                ////////////////////

                sb.AppendLine($"Your artifacts are respawning in {timerString}.");
            }
            else
                sb.AppendLine("Your artifacts are now ready to be looted,\notherwise use `qq artifactreset` to reset.");

            embed.WithTitle($"Farming artifacts");
            embed.WithAuthor(user);
            embed.WithThumbnailUrl(ARTIFACT_URL);
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }

        [Command("crystalreset")]
        public async Task ResetCrystal()
        {
            var user = Context.User;
            var list = await Program._db.Crystal.ToListAsync().ConfigureAwait(false);
            var records = list.Where(x => x.UserId == (long)user.Id).ToList();
            var nextDay = DateTime.Now.AddDays(3);

            var timezoneUsers = await Program._db.Timezone.ToListAsync().ConfigureAwait(false);
            var tzUser = timezoneUsers.Where(x => x.UserId == (long)user.Id).ToList();

            //////////////////////

            if (records.Count == 0)
            {
                var crystal = new Crystal
                {
                    Due = nextDay,
                    UserId = (long)user.Id,
                    Username = user.Username,
                    Discriminator = user.Discriminator,
                    AvatarUrl = user.GetAvatarUrl(ImageFormat.Auto, 128)
                };
                await Program._db.Crystal.AddAsync(crystal).ConfigureAwait(false);
            }
            else
            {
                foreach (var item in records)
                {
                    if (item.UserId == (long)user.Id)
                    {
                        item.Due = nextDay;
                        item.IsRemindEnabled = true;
                    }
                }
            }
            await Program._db.SaveChangesAsync().ConfigureAwait(false);

            var sb = new StringBuilder();

            Database.Timezone nameTimezone = new Database.Timezone();
            if (tzUser.Count != 0)
            {
                foreach (var item in tzUser)
                {
                    nameTimezone = item;
                    break;
                }
                /////
                var time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(nextDay, nameTimezone.TimezoneString);
                var tzi = TimeZoneInfo.FindSystemTimeZoneById(nameTimezone.TimezoneString);
                bool isDaylight = tzi.IsDaylightSavingTime(DateTime.Now);
                /////

                if (isDaylight == true)
                {
                    var abb = nameTimezone.Abbrevlation.ToUpper();
                    var full = abb.Substring(0, 1) + "DT";
                    sb.AppendLine($"Crystals will be respawned at {time:h:mm tt} {abb} {time:MMMM dd}.");
                }
                else
                    sb.AppendLine($"Crystals will be respawned at {time:h:mm tt} {nameTimezone.Abbrevlation} {time:MMMM dd}.");
            }
            else
                sb.AppendLine($"Crystals will be respawned at {nextDay:h:mm tt} EST on {nextDay:MMMM dd}.");

            var embed = new EmbedBuilder();
            embed.WithTitle($"Farming crystals");
            embed.WithAuthor(user);
            embed.WithThumbnailUrl(CRYSTAL_URL);
            embed.WithFooter("You will be notified via channel once the crystals are respawned.");
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }

        [Command("crystal")]
        public async Task Crystal()
        {
            var user = Context.User;
            var list = await Program._db.Crystal.ToListAsync().ConfigureAwait(false);
            var records = list.Where(x => x.UserId == (long)user.Id).ToList();

            var timezoneUsers = await Program._db.Timezone.ToListAsync().ConfigureAwait(false);
            var tzUser = timezoneUsers.Where(x => x.UserId == (long)user.Id).ToList();

            var sb = new StringBuilder();
            var embed = new EmbedBuilder();

            if (records.Count != 0)
            {
                ////////////////////
                var date = (DateTime)records[0].Due - DateTime.Now;

                var day = date.Days;
                var hour = date.Hours;
                var min = date.Minutes;

                var timerString = "";

                if (day > 0)
                    timerString = $"**{day}** day(s) ";

                if (hour > 0)
                    timerString += $"**{hour}** hour(s) ";

                timerString += $"**{min}** minute(s)";
                ////////////////////

                sb.AppendLine($"Your crystals are respawning in\n{timerString}.");
            }
            else
                sb.AppendLine("Your crystals are now ready to be looted,\notherwise use `qq crystalreset` to reset.");

            embed.WithTitle($"Farming crystals");
            embed.WithAuthor(user);
            embed.WithThumbnailUrl(CRYSTAL_URL);
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }

        [Command("ar")]
        public async Task Arank(string _level, string _exp)
        {
            var user = Context.User;
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();

            try
            {
                var level = Int32.Parse(_level);
                var exp = Int32.Parse(_exp);
                var isValid = false;

                if (level > 0 && level < 56)
                    isValid = true;

                if (isValid)
                {
                    var capacity = 0;
                    var totalExp = 0;

                    foreach (var item in Program.AdventureRanks)
                    {
                        if(item.Level == level)
                        {
                            capacity = item.Capacity;
                            totalExp = item.Total;
                            break;
                        }
                    }

                    if(capacity < exp || exp < 0)
                    {
                        sb.AppendLine($"Experience amount you entered isn't in {level}'s progression.");

                        embed.WithTitle($"Adventure Rank {level}");
                        embed.WithAuthor(user);
                        embed.WithThumbnailUrl(AR_URL);
                        embed.WithColor(RandomColor);
                        embed.Description = sb.ToString();

                        await ReplyAsync(null, false, embed.Build());
                    }
                    else
                    {
                        ReadJsons();

                        var daily = 0;
                        var today = DateTime.Now;
                        var days = 0;
                        int[] array = new int[0];
                        int levels = 0;
                        List<Level> LevelList = new List<Level>();

                        if (totalExp + exp < 28100) // 20
                        {
                            levels = 7;
                            array = new int[7] { 20, 30, 35, 40, 45, 50, 55 };
                        }
                        else if (totalExp + exp < 72150) // 30
                        {
                            levels = 6;
                            array = new int[6] { 30, 35, 40, 45, 50, 55 };
                        }

                        else if (totalExp + exp < 105150) // 35
                        {
                            levels = 5;
                            array = new int[5] { 35, 40, 45, 50, 55 };
                        }
                        else if (totalExp + exp < 145375) // 40
                        {
                            levels = 4;
                            array = new int[4] { 40, 45, 50, 55 };
                        }
                        else if (totalExp + exp < 207775) // 45
                        {
                            levels = 3;
                            array = new int[3] { 45, 50, 55 };
                        }
                        else if (totalExp + exp < 294175) // 50
                        {
                            levels = 2;
                            array = new int[2] { 50, 55 };
                        }

                        else if (totalExp + exp < 450175) // 50
                        {
                            levels = 1;
                            array = new int[1] { 55 };
                        }

                        int count = 0;

                        for (int i = 0; i < levels; i++)
                        {
                            LevelList.Add(new Level(array[count]));
                            count++;
                        }

                        totalExp += 2400;
                        Quests.RemoveAll(x => x.TotalExp < totalExp);

                        while (totalExp + exp < FIVE_SIX_TOTAL)
                        {
                            foreach (var item in Quests)
                            {
                                if(totalExp > item.TotalExp)
                                    totalExp += item.Exp;
                            }

                            Quests.RemoveAll(x => x.TotalExp < totalExp);

                            if (totalExp + exp < 10525) // before 12
                                daily = 0;

                            else if (totalExp + exp < 17825) // before 16
                                daily = 175;

                            else if (totalExp + exp < 46375) // before 25
                                daily = 200;

                            else if (totalExp + exp < 105150) // before 35
                                daily = 225;

                            else
                                daily = 250;

                            totalExp += (daily * 4) + 500 + 900;
                            days++;

                            foreach (var item in LevelList)
                            {
                                if (item.Lvl == 20 && item.IsAdded == false && totalExp > 28100)
                                {
                                    item.IsAdded = true;
                                    item.Date = DateTime.Now.AddDays(days);
                                }
                                else if (item.Lvl == 30 && item.IsAdded == false && totalExp > 72150)
                                {
                                    item.IsAdded = true;
                                    item.Date = DateTime.Now.AddDays(days);
                                }
                                else if (item.Lvl == 35 && item.IsAdded == false && totalExp > 105150)
                                {
                                    item.IsAdded = true;
                                    item.Date = DateTime.Now.AddDays(days);
                                }
                                else if (item.Lvl == 40 && item.IsAdded == false && totalExp > 145375)
                                {
                                    item.IsAdded = true;
                                    item.Date = DateTime.Now.AddDays(days);
                                }
                                else if (item.Lvl == 45 && item.IsAdded == false && totalExp > 207775)
                                {
                                    item.IsAdded = true;
                                    item.Date = DateTime.Now.AddDays(days);
                                }
                                else if (item.Lvl == 50 && item.IsAdded == false && totalExp > 294175)
                                {
                                    item.IsAdded = true;
                                    item.Date = DateTime.Now.AddDays(days);
                                }
                                else if (item.Lvl == 55 && item.IsAdded == false && totalExp > 450175)
                                {
                                    item.IsAdded = true;
                                    item.Date = DateTime.Now.AddDays(days);
                                }
                            }
                        }

                        var when = today.AddDays(days);

                        sb.AppendLine($"{exp}/{capacity} Adventure EXP\n");
                        sb.AppendLine($"Assuming you are doing daily commissions and using all resins every day including exp recieved from all of the locked story, archon, and world quests prior to chapter 2 ahead of your currently AR.\n");

                        foreach (var item in LevelList)
                            sb.AppendLine($"You will reach Adventure rank {item.Lvl} on {item.Date:MMM dd, yyyy}");

                        sb.AppendLine($"You will reach Adventure rank 56 on {when:MMM dd, yyyy}");
                        sb.AppendLine($"You will reach Adventure rank 57 on Unknown");
                        sb.AppendLine($"You will reach Adventure rank 58 on Unknown");
                        sb.AppendLine($"You will reach Adventure rank 59 on Unknown");
                        sb.AppendLine($"You will reach Adventure rank 60 on Unknown");

                        embed.WithTitle($"Adventure Rank {level}");
                        embed.WithAuthor(user);
                        embed.WithThumbnailUrl(AR_URL);
                        embed.WithColor(RandomColor);
                        embed.WithFooter("Please note that this isn't accurate since we do not have data from upcoming contents.");
                        embed.Description = sb.ToString();

                        await ReplyAsync(null, false, embed.Build());
                    }
                }
            }
            catch (FormatException)
            {
                await Context.Channel.SendMessageAsync("Please enter valid numbers");
            }
        }

        [Command("ar")]
        public async Task ArankHelper()
        {
            var user = Context.User;
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            sb.AppendLine("Enter your AR rank and AR exp.");
            sb.AppendLine("`qq ar RANK EXP`");
            embed.WithTitle($"Adventure Rank");
            embed.WithAuthor(user);
            embed.WithThumbnailUrl(AR_URL);
            embed.WithImageUrl(EXAMPLE_URL);
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }

        #region Methods
        private Timezone CheckTimezone(string _zone)
        {
            Timezone tz = null;
            foreach (var item in Program.Timezones)
            {
                if (item.Abbreviation == _zone.ToLower())
                {
                    tz = item;
                    break;
                }
            }

            return tz;
        }

        private string GetDay(DateTime _when)
        {
            var day = "";
            var days = _when - DateTime.Today;
            if (days.TotalDays >= 1)
                return day = "tomorrow";

            else
                return day = "today";
        }

        private async Task SetDomainReminder(Models.Domain nameItem)
        {
            var user = Context.User;
            var channelId = GetSpecificChannel(Context.Guild);

            var sb = new StringBuilder();
            var embed = new EmbedBuilder();

            var canSave = false;

            embed.WithTitle($"{nameItem.Material}");
            embed.WithAuthor(user);
            embed.WithThumbnailUrl(nameItem.MaterialImage);
            embed.WithColor(RandomColor);

            var date = DateTime.Now;

            if (nameItem.Name != null)
            {
                var today = DateTime.Now;
                if (today.Hour > 0 && today.Hour < 4)
                    today.AddDays(-1);

                var day = today.DayOfWeek;

                int number = (int)day;
                var days = 0;
                var continueLoop = true;
                while (continueLoop)
                {
                    number++;
                    days++;
                    foreach (var item in nameItem.Days)
                    {
                        if (number == item)
                        {
                            continueLoop = false;
                            break;
                        }
                    }

                    if (number >= 7)
                        number = 0;
                }

                date = DateTime.Today.AddDays(days).AddHours(4);
                if(days == 1)
                    sb.AppendLine($"The material will be obtainable tomorrow ({date:ddd})\nby clearing **{nameItem.Dungeon}**.");
                else
                    sb.AppendLine($"The material will be obtainable in the next {days} days ({date:ddd})\nby clearing **{nameItem.Dungeon}**.");

                if( nameItem.Name == "Gold" ||
                    nameItem.Name == "Freedom" ||
                    nameItem.Name == "Ballad" ||
                    nameItem.Name == "Prosperity" ||
                    nameItem.Name == "Diligence" ||
                    nameItem.Name == "Resistance")
                    sb.AppendLine($"\nI'm assuming you want to stock {nameItem.Name} talents up.");
                else
                    sb.AppendLine($"\nIt's for {nameItem.Name} upgrade.");

                embed.WithFooter("You will be reminded via channel when the day comes.");
                canSave = true;
            }
            else
            {
                sb.AppendLine($"Sorry, I'm not sure what you're talking about");
                embed.WithFooter("Try again with a different keyword.");
            }

            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());

            if (canSave)
            {
                var list = await Program._db.Domain.ToListAsync().ConfigureAwait(false);
                var records = list.Where(x => x.UserId == (long)user.Id).ToList();

                if(records.Count == 0)
                    await SaveDomainReminder(nameItem, user, channelId, date).ConfigureAwait(false);

                else
                {
                    var canAdd = true;

                    foreach (var record in records)
                    {
                        if (record.Name == nameItem.Name)
                            canAdd = false;
                    }

                    if (canAdd)
                        await SaveDomainReminder(nameItem, user, channelId, date).ConfigureAwait(false);
                }
            }
        }

        private static async Task SaveDomainReminder(Models.Domain nameItem, Discord.WebSocket.SocketUser user, long channelId, DateTime date)
        {
            var domain = new Database.GenshinImpact.Domain
            {
                UserId = (long)user.Id,
                Username = user.Username,
                Discriminator = user.Discriminator,
                AvatarUrl = user.GetAvatarUrl(ImageFormat.Auto, 128),
                Name = nameItem.Name,
                Due = date,
                GuildChannelId = channelId,
                Material = nameItem.Material,
                MaterialImage = nameItem.MaterialImage
            };
            await Program._db.Domain.AddAsync(domain).ConfigureAwait(false);
        }

        private long GetSpecificChannel(Discord.WebSocket.SocketGuild guild)
        {
            long numberId = 0;

            switch (guild.Id)
            {
                case Constants.GIFORDEAFSERVER: numberId = Constants.GIFORDEAFCHANNEL; break;
                case Constants.MYSERVER: numberId = Constants.MYCHANNEL; break;
                case Constants.VOIDSERVER: numberId = Constants.VOIDCHANNEL; break;
            }
            return numberId;
        }

        private Models.Domain GetDomain(string word)
        {
            Models.Domain nameItem = new Models.Domain();
            var endloop = false;
            foreach (var item in Program.Materials)
            {
                if (endloop)
                    break;

                foreach (var tag in item.Tag)
                {
                    if (word.ToLower() == tag.ToLower())
                    {
                        nameItem = item;
                        endloop = true;
                        break;
                    }
                }
            }
            return nameItem;
        }

        public static void ReadJsons()
        {
            var list = new List<Quest>();

            using (var reader = new StreamReader("Quest.json"))
            {
                var json = reader.ReadToEnd();

                list = JsonConvert.DeserializeObject<List<Quest>>(json);

                // if a value is null, it will return true
            }
            Quests = list;
        }

        private async Task PostCharacter(string _name, string _iconUrl, string _imageUrl, Discord.Color _color, string _footer)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var ea = new EmbedAuthorBuilder();
            var fo = new EmbedFooterBuilder();

            fo.WithIconUrl("https://cdn.discordapp.com/attachments/772358312297496656/787762489587925022/twitter.png");
            fo.WithText(_footer);
            ea.WithName(_name);
            ea.WithIconUrl(_iconUrl);

            //embed.WithTitle("Ice Wizard Stats");
            //sb.AppendLine("[Click me](https://genshin-impact.fandom.com/wiki/Diluc)");
            //embed.AddField("HP", "665", true);
            //embed.AddField("DPS", "42", true);
            //embed.AddField("Hit Speed", "1.5sec", true);
            //embed.AddField("SlowDown", "35%", true);
            //embed.AddField("AOE", "63", true);

            embed.WithAuthor(ea);
            embed.WithImageUrl(_imageUrl);
            embed.WithColor(_color);
            embed.WithFooter(fo);
            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion
    }
}
