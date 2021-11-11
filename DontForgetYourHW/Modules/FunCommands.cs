using Discord.Commands;
using System.Text;
using DontForgetYourHW.Helpers;
using System.Threading.Tasks;
using Discord;
using System.Net;
using System;
using Discord.WebSocket;
using RestSharp;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace DontForgetYourHW.Modules
{
    public class FunCommands : ModuleBase<SocketCommandContext>
    {
        private Discord.Color RandomColor { get; set; } = new Color(Filtering.X(), Filtering.X(), Filtering.X());

        private const string HELP_URL = "https://cdn.discordapp.com/attachments/743004274232918050/784690224893526046/help.png";

        private readonly CommandService _service;
        private readonly DiscordSocketClient _client;

        public FunCommands(CommandService service, DiscordSocketClient client)
        {
            _client = client;
            _service = service;
        }

        #region Commands List - Not Done

        [Command("commands"), Alias("help")]
        public async Task CommandsList()
        {
            var sb = new StringBuilder();
            //sb.AppendLine($"```yaml\nGeneral```");
            //sb.AppendLine($"`qq.myid`, `qq.bye`, `qq.insult`, `qq.notifymanga`");
            //sb.AppendLine($"`qq.notifyanime`, `qq.remove-manga`, `qq.remove-anime`\n");

            //sb.AppendLine($"```yaml\nAdd and manage courses```");
            //sb.AppendLine($"`qq.addC`, `qq.removeC`, `qq.mycourses`\n");

            //sb.AppendLine($"```yaml\nAdd and manage homework```");
            //sb.AppendLine($"`qq.newhw`, `qq.donehw`, `qq.myhw`, `qq.remind`");
            //sb.AppendLine($"`qq.remind-all`, `qq.stop-remind-all`");

            var embed = new EmbedBuilder();
            embed.WithColor(RandomColor);
            embed.WithImageUrl(HELP_URL);
            embed.Title = "Commands";
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }

        #endregion Commands List - Not Done

        #region What is my ID - Done

        [Command("myid")]
        public async Task WhatIsMyId()
        {
            var user = Context.User;
            var id = user.Id;

            var sb = new StringBuilder();
            sb.AppendLine($"your id is [{id}]");
            sb.AppendLine($"it is ugly, you might want to delete your account.");

            var embed = new EmbedBuilder();
            embed.WithColor(RandomColor);
            embed.ImageUrl = user.GetAvatarUrl();
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }

        #endregion What is my ID - Done

        #region Says Bye - Done

        [Command("bye"), Alias("goodbye", "farewell", "goodnight")]
        public async Task Goodbye()
        {
            await Context.Channel.SendMessageAsync($"ok bai! {Context.Message.Author.Mention}");
        }

        #endregion Says Bye - Done

        #region Example

        [Command("hello")] // <- bot will do a task if an user types "qq.hello"
        public async Task DoStuffs()
        {
            var user = Context.User; // <- get user info whoever typed "qq.hello"
            var sb = new StringBuilder(); // <- create an instance of the stringbuilder to allow us to write strings
            sb.AppendLine($"Hello @<{user.Id}>"); // <- AppendLine has same concept as Console.WriteLine, the bot only uses AppendLine.
            sb.AppendLine($"Your currently status is {user.Status}"); // <-- get user's status
            var embed = new EmbedBuilder(); // <- create a box of comment where we will use some inside
            embed.WithColor(new Color(158, 24, 172)); // <- color on left side of the comment box
            embed.Title = "Title"; // <- a title inside the comment box
            embed.Description = sb.ToString(); // <- we put all string appendline(s) inside the comment box

            await ReplyAsync(null, false, embed.Build()); // <- bot starts to post the comment box

            await Context.Channel.SendMessageAsync($"hello! {Context.Message.Author.Mention}"); // <- a message will be sent that is outside of a comment box
        }

        #endregion

        #region Methods

        [Command("insult")]
        public async Task Insult()
        {
            string urlAddress = "https://insult.mattbas.org/api/insult.html";

            using (WebClient client = new WebClient())
            {
                string code = client.DownloadString(urlAddress);
                string strStart = $"insult\">";
                string strEnd = "</h1>";

                int start = 0;
                int end = 0;

                start = code.IndexOf(strStart, 0);
                end = code.IndexOf(strEnd, start + strStart.Length);

                string result = code.Substring(start + strStart.Length, end - (start + strStart.Length)) + $" -> {Context.Message.Author.Mention}";

                await Context.Channel.SendMessageAsync(result);
            }
        }

        [Command("insult")]
        public async Task InsultTarget(string id)
        {
            string urlAddress = "https://insult.mattbas.org/api/insult.html";

            using (WebClient client = new WebClient())
            {
                string code = client.DownloadString(urlAddress);
                string strStart = $"insult\">";
                string strEnd = "</h1>";

                int start = 0;
                int end = 0;

                start = code.IndexOf(strStart, 0);
                end = code.IndexOf(strEnd, start + strStart.Length);

                string result = code.Substring(start + strStart.Length, end - (start + strStart.Length)) + $", " + id + "!";

                await Context.Channel.SendMessageAsync(result);
            }
        }

        [Command("ql")]
        public async Task PostGoodLuckGif()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(RandomColor);
            embed.WithImageUrl("https://cdn.discordapp.com/attachments/783870382063091753/786952660758167552/tenor.gif");

            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        [Command("give")]
        public async Task ImageOne(string _word)
        {
            var fullWord = _word;
            var link = GetImage(fullWord);

            var embed = new EmbedBuilder();
            embed.WithImageUrl(link);
            //await ReplyAsync(null, false, embed.Build());
            await ReplyAsync(link, false, null);
        }

        private static string GetImage(string _input)
        {
            var result = _input.ToLower();
            if (_input.Contains(" "))
                result = Regex.Replace(_input, " ", "%20", RegexOptions.IgnoreCase);

            var link = "https://www.dogpile.com/serp?qc=images&q=" + result;

            RestClient rest = new RestClient(link);
            var repsonse = rest.Execute(new RestRequest(""));
            var content = repsonse.Content;
            var pattern = $"<div class=\"image\"";

            MatchCollection matches = Regex.Matches(content, pattern);

            var links = new List<string>();
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                var index = groups[0].Index;
                var firstLine = content.Substring(index + pattern.Length, 1000);
                var start = firstLine.IndexOf($"href=\"");
                var end = firstLine.IndexOf($"\" data-thash=");
                var image = firstLine.Substring(start + 6 , end - start - 6);
                links.Add(image);
            }

            var count = links.Count;
            var rand = new Random();
            var number = rand.Next(0, count);

            return links[number];
        }

        [Command("give")]
        public async Task ImageTwo(string _word, string _wordTwo)
        {
            var fullWord = $"{_word} {_wordTwo}";
            var link = GetImage(fullWord);

            var embed = new EmbedBuilder();
            embed.WithImageUrl(link);
            //await ReplyAsync(null, false, embed.Build());
            await ReplyAsync(link, false, null);
        }

        [Command("give")]
        public async Task ImageThree(string _word, string _wordTwo, string _wordThree)
        {
            var fullWord = $"{_word} {_wordTwo} {_wordThree}";
            var link = GetImage(fullWord);

            var embed = new EmbedBuilder();
            embed.WithImageUrl(link);
            //await ReplyAsync(null, false, embed.Build());
            await ReplyAsync(link, false, null);
        }

        [Command("give")]
        public async Task ImageFour(string _word, string _wordTwo, string _wordThree, string _wordFour)
        {
            var fullWord = $"{_word} {_wordTwo} {_wordThree} {_wordFour}";
            var link = GetImage(fullWord);

            var embed = new EmbedBuilder();
            embed.WithImageUrl(link);
            //await ReplyAsync(null, false, embed.Build());
            await ReplyAsync(link, false, null);
        }

        [Command("give")]
        public async Task ImageFive(string _word, string _wordTwo, string _wordThree, string _wordFour, string _wordFive)
        {
            var fullWord = $"{_word} {_wordTwo} {_wordThree} {_wordFour} {_wordFive}";
            var link = GetImage(fullWord);

            var embed = new EmbedBuilder();
            embed.WithImageUrl(link);
            //await ReplyAsync(null, false, embed.Build());
            await ReplyAsync(link, false, null);
        }

        #region Test Purpose
        /// <summary>
        ///     For testing purpose -
        ///     We will be using this command to test if a method inside the command is working for future reference;
        /// </summary>
        /// <returns></returns>
        [Command("test")]
        public async Task Test(string _level, string _exp)
        {
            var user = Context.User;
            var username = user.Username;
            var disc = user.Discriminator;
            var imageUrl = user.GetAvatarUrl(ImageFormat.Auto, 128);
            var userid = user.Id;

            var level = Int32.Parse(_level);
            var exp = Int32.Parse(_exp);

            var embed = new EmbedBuilder();
            var sb = new StringBuilder();
            sb.AppendLine("Testing");
            var ea = new EmbedAuthorBuilder();

            ea.WithName($"{username}#{disc}");
            ea.WithIconUrl(imageUrl);
            embed.WithAuthor(ea);
            embed.WithThumbnailUrl("https://cdn.discordapp.com/attachments/743004274232918050/783657321837756467/resin.png");
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            var sendUser = _client.GetUser(userid);
            await sendUser.SendMessageAsync(null, false, embed.Build()).ConfigureAwait(false);
        }
        #endregion Test Purpose
    }
}