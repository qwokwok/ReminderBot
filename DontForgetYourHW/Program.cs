using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DontForgetYourHW.Database;
using DontForgetYourHW.Helpers;
using DontForgetYourHW.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Timezone = DontForgetYourHW.Models.Timezone;

namespace DontForgetYourHW
{
    class Program
    {
        #region Fields
        public static Context _db;
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        public static readonly System.Timers.Timer _interval = new System.Timers.Timer(1 * 60 * 1000);

        private string _token = "";
        public static List<AdventureRank> AdventureRanks { get; set; }
        public static List<Domain> Materials { get; set; }

        public static List<Timezone> Timezones = new List<Timezone>();
        #endregion

        static void Main(string[] args)
        {
            ReadJsons();
            AddTimezones();
            new Program().RunBotAsync().GetAwaiter().GetResult();
        }

        #region Startup
        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddDbContext<Context>()    
                .BuildServiceProvider();

            _db = _services.GetRequiredService<Context>();

            _client.Log += Client_Log;

            await _client.SetGameAsync("k commands for help", 
                "https://www.twitch.tv/enviosity", 
                ActivityType.Streaming);

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, _token);

            await _client.StartAsync();

            StartTimer(_commands, _client);

            await Task.Delay(-1);
        }

        private Task Client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot)
                return;

            int argPos = 0;
            if(message.HasStringPrefix("k ", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }
        #endregion

        #region Methods
        public void StartTimer(CommandService service, DiscordSocketClient client)
        {
            var reminder = new Reminder(service, client);
            _interval.Elapsed += reminder.DoTasks;
            _interval.Start();
        }

        public static void ReadJsons()
        {
            var list = new List<AdventureRank>();

            using (var reader = new StreamReader("Rank.json"))
            {
                var json = reader.ReadToEnd();

                list = JsonConvert.DeserializeObject<List<AdventureRank>>(json);

                // if a value is null, it will return true
            }
            AdventureRanks = list;

            var dlist = new List<Domain>();

            using (var reader = new StreamReader("Domain.json"))
            {
                var json = reader.ReadToEnd();

                dlist = JsonConvert.DeserializeObject<List<Domain>>(json);

                // if a value is null, it will return true
            }
            Materials = dlist;
        }

        public static void AddTimezones()
        {
            Timezones.Add(new Timezone("jst", "Tokyo Standard Time", false));
            Timezones.Add(new Timezone("kst", "Korea Standard Time", false));
            Timezones.Add(new Timezone("aest", "E. Australia Standard Time", false));
            Timezones.Add(new Timezone("awst", "W. Australia Standard Time", false));
            Timezones.Add(new Timezone("acst", "Aus Central W. Standard Time", false));
            Timezones.Add(new Timezone("aft", "Afghanistan Standard Time", false));
            Timezones.Add(new Timezone("ast", "Atlantic Standard Time", false));
            Timezones.Add(new Timezone("cat", "W. Central Africa Standard Time", false));
            Timezones.Add(new Timezone("wat", "W. Central Africa Standard Time", false));
            Timezones.Add(new Timezone("eat", "E. Africa Standard Time", false));
            Timezones.Add(new Timezone("cet", "Central European Standard Time", false));
            Timezones.Add(new Timezone("eet", "E. Europe Standard Time", false));
            Timezones.Add(new Timezone("wet", "W. Europe Standard Time", false));
            Timezones.Add(new Timezone("pst", "Pacific Standard Time", true));
            Timezones.Add(new Timezone("pdt", "Pacific Standard Time", true));
            Timezones.Add(new Timezone("msk", "Mountain Standard Time", true));
            Timezones.Add(new Timezone("mst", "Mountain Standard Time", true));
            Timezones.Add(new Timezone("cst", "Central Standard Time", true));
            Timezones.Add(new Timezone("cdt", "Central Standard Time", true));
            Timezones.Add(new Timezone("est", "Eastern Standard Time", true));
            Timezones.Add(new Timezone("edt", "Eastern Standard Time", true));
            Timezones.Add(new Timezone("akst", "Alaskan Standard Time", true));
            Timezones.Add(new Timezone("akdt", "Alaskan Standard Time", true));
            Timezones.Add(new Timezone("mst", "Mountain Standard Time", true));
            Timezones.Add(new Timezone("mdt", "Mountain Standard Time", true));
            Timezones.Add(new Timezone("hst", "Hawaiian Standard Time", true));
            Timezones.Add(new Timezone("hdt", "Hawaiian Standard Time", true));
        }
        #endregion
    }
}
