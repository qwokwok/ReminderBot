using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DontForgetYourHW.Database;
using DontForgetYourHW.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontForgetYourHW.Modules
{
    public class HomeworkCommands : ModuleBase<SocketCommandContext>
    {
        Discord.Color RandomColor { get; set; } = new Color(Filtering.X(), Filtering.X(), Filtering.X());

        #region Commands
        #region Add Homework Helper - Done
        [Command("newhw")]
        public async Task AddHomework()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Use the following format to add homework.\n");
            sb.AppendLine($"Format:\n `qq newhw \"HW NAME\" \"ABBREVIATION\" \"DUE TIME THEN DATE\"`\n");
            sb.AppendLine($"Example:\n`qq newhw \"Homework #1\" \"java\" \"11:59pm aug 31\"`\n");
            var embed = new EmbedBuilder();
            embed.Title = "What is your homework?";
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region New Homework - Done
        //qq.i have new hw "text" from "Java For Programmer" - Adding your new homework
        [Command("newhw")]
        public async Task AddHomework(string _name, string _abb, string _due)
        {
            var user = Context.User;
            var sb = new StringBuilder();
            var isValid = true;
            var title = "";
            var list = await Program._db.Course.ToListAsync().ConfigureAwait(false);
            var courseList = list.Where(x => x.UserId == (long)user.Id).ToList();

            if (_name == "")
            {
                sb.Append("");
                isValid = false;
                title = " - Failed";
            }
            else if(_name.Length > 18)
            {
                sb.Append("Name of homework is too long. Must be 18 or less characters");
                isValid = false;
                title = " - Failed";
            }

            if (_abb == "")
            {
                sb.Append("");
                isValid = false;
                title = " - Failed";
            }

            if (_due == "")
            {
                sb.Append("");
                isValid = false;
                title = " - Failed";
            }

            if (isValid)
            {
                var hasCourse = false;
                var courseName = "";

                foreach (var item in courseList)
                {
                    if (item.Abbreviation.ToUpper() == _abb.ToUpper())
                    {
                        courseName = item.CourseName;
                        hasCourse = true;
                        break;
                    }
                }

                if (hasCourse)
                {
                    var homeworkList = GetHomework(user).Result;

                    var hasHomework = false;

                    foreach (var item in homeworkList)
                    {
                        if (item.Name.ToLower() == _name.ToLower() && item.Abbreviation.ToUpper() == _abb.ToUpper())
                        {
                            hasHomework = true;
                            break;
                        }
                    }

                    if (hasHomework)
                    {
                        sb.AppendLine("I'm pretty sure you have told me before.");
                        title = " - Failed";
                    }
                    else
                    {
                        var date = DateTime.Parse(_due, new CultureInfo("en-US", true));
                        if (DateTime.Now > date)
                            date = date.AddYears(1);

                        var homework = new Homework();
                        homework.Name = _name;
                        homework.Abbreviation = _abb;
                        homework.CourseName = courseName;
                        homework.Due = date;
                        homework.UserId = (long)user.Id;
                        await Program._db.Homework.AddAsync(homework).ConfigureAwait(false);
                        sb.AppendLine($"Good luck with your `{_name}`.");
                        title = " - Succeed";
                        await Program._db.SaveChangesAsync().ConfigureAwait(false);
                    }
                }
                else
                {
                    sb.Append("It turns out you don't have the course.\n`qq.add course` to add a specific course before adding homework.");
                    title = " - Failed";
                }
            }

            var embed = new EmbedBuilder();
            embed.Title = "Adding Homework" + title;
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Show Currently Homework - Done
        [Command("myhw")]
        public async Task ListHomework()
        {
            var user = Context.User;
            var sb = new StringBuilder();
            var homeworkList = GetHomework(user).Result;
            var currentHomeworkList = homeworkList.Where(x => x.IsCurrent == true).ToList();

            if (currentHomeworkList.Count == 0)
            {
                sb.AppendLine($"It seems you have no homework at the moment.\nNow enjoy as much as you can before" +
                    $" you die from new homework.");
            }
            else
            {
                var orderBy = currentHomeworkList.OrderBy(x => x.Due);
                foreach (var item in orderBy)
                {
                    var str = Filtering.GetHomeworkString(item);
                    sb.AppendLine(str);
                }
            }

            var embed = new EmbedBuilder();
            embed.Title = "Listing Currently Homework";
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Show Homework Record - Done
        //[Command("mytotalhw")]
        //public async Task ListTotalHomework()
        //{
        //    var user = Context.User;
        //    var sb = new StringBuilder();
        //    var homeworkList = GetHomework(user).Result;

        //    var formatString = "";

        //    if (homeworkList.Count == 0)
        //        sb.AppendLine($"You have no homework in record.");

        //    else
        //    {
        //        var orderBy = homeworkList.OrderBy(x => x.Due);
        //        formatString += "```\n";
        //        foreach (var item in orderBy)
        //        {
        //            var str = Filtering.GetHomeworkRecordString(item);
        //            formatString += str;
        //        }
        //        formatString += "```";
        //    }

        //    var embed = new EmbedBuilder();
        //    embed.Title = $"Homework Record ({homeworkList.Count})";
        //    embed.WithColor(RandomColor);
        //    embed.Description = sb.ToString();
        //    await ReplyAsync(null, false, embed.Build());

        //    await Context.Channel.SendMessageAsync(formatString);
        //}
        #endregion

        #region Remove all Homework - Done
        //[Command("removeall")]
        //public async Task RemoveHomework()
        //{
        //    var user = Context.User;
        //    var sb = new StringBuilder();
        //    var homeworkList = GetHomework(user).Result;

        //    foreach (var item in homeworkList)
        //        Program._db.Homework.Remove(item);

        //    await Program._db.SaveChangesAsync().ConfigureAwait(false);

        //    var embed = new EmbedBuilder();
        //    embed.Title = "Removing all hw";
        //    embed.WithColor(RandomColor);
        //    embed.Description = sb.ToString();
        //    await ReplyAsync(null, false, embed.Build());
        //}
        #endregion

        #region Remind Homework Helper - Done
        [Command("remind")]
        public async Task RemindHomeworkHelper()
        {
            var user = Context.User;
            var sb = new StringBuilder();
            sb.AppendLine("Please use the following format below.\n");
            sb.AppendLine("Format:\n`qq remind \"HOMEWORK\" \"ABBREVIATION\"`\n");
            sb.AppendLine("Example:\n`qq remind \"study for quiz\" \"sci\"`\n");

            var homeworkList = GetHomework(user).Result;

            foreach (var item in homeworkList)
            {
                var str = Filtering.GetHomeworkString(item);
                sb.AppendLine(str);
            }

            var embed = new EmbedBuilder();
            embed.Title = "Which homework do you want me to remind about?";
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Remind Homework - Done
        [Command("remind")]
        public async Task RemindHomework(string _name, string _abb)
        {
            var user = Context.User;
            var sb = new StringBuilder();
            var homeworkList = GetHomework(user).Result;

            var isValid = true;
            var message = "";

            if (_name == "")
            {
                isValid = false;
                sb.AppendLine("I don't heard name of the homework.");
            }

            if(_abb == "")
            {
                isValid = false;
                message = "No course mentioned.";
            }

            var isSuccess = false;
            if(isValid)
            {
                var HasHomework = false;
                foreach (var item in homeworkList)
                {
                    if (item.Name.ToLower() == _name.ToLower()
                        && item.Abbreviation.ToLower() == _abb.ToLower())
                    {
                        HasHomework = true;
                        if (item.IsCurrent == true)
                        {
                            item.IsRemindEnabled = true;
                            Program._db.Homework.Update(item);
                            message = "Ok you will be reminded 6 hours before the due time.";
                            isSuccess = true;
                            break;
                        }
                        else
                        {
                            message = "The homework has past the due date...";
                            break;
                        }
                    }
                }
                if (!HasHomework)
                    message = "The homework you mentioned isn't found.";
            }       

            sb.AppendLine($"{message}\n");

            if(isSuccess)
            {
                foreach (var w in homeworkList)
                {
                    var str = Filtering.GetHomeworkString(w);
                    sb.AppendLine(str);
                    await Program._db.SaveChangesAsync().ConfigureAwait(false);
                }
            }

            var embed = new EmbedBuilder();
            embed.Title = "Setting a reminder";
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Remind Homework - Done
        [Command("remind-all")]
        public async Task RemindAll()
        {
            var embed = new EmbedBuilder();
            var user = Context.User;
            embed = SetReminders(user, embed, true).Result;
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Remind Homework - Done
        [Command("stop-remind-all")]
        public async Task StopRemindAll()
        {
            var embed = new EmbedBuilder();
            var user = Context.User;
            embed = SetReminders(user, embed, false).Result;
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Done with Homework Helper - Done
        [Command("donehw")]
        public async Task HomeworkIsDoneHelper()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Please use the following format below.\n");
            sb.AppendLine("Format:\n`qq donehw \"HOMEWORK\" \"ABBREVIATION\"`\n");
            sb.AppendLine("Example:\n`qq donehw \"practice exercise 1\" \"java\"`\n");

            var embed = new EmbedBuilder();
            embed.Title = "Which homework is just finished?";
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Done with Homework - Done
        [Command("donehw")]
        public async Task HomeworkIsDone(string _name, string _abb)
        {
            var user = Context.User;
            var sb = new StringBuilder();
            var homeworkList = GetHomework(user).Result;

            //
            var isValid = true;
            var message = "";

            if(_name == "")
            {
                sb.AppendLine("What, which homework?");
                isValid = false;
            }

            if(_abb == "")
            {
                sb.AppendLine("No course entered");
                isValid = false;
            }
            //

            var HasHomework = false;
            if(isValid)
            {
                foreach (var item in homeworkList)
                {
                    if (item.Name.ToLower() == _name.ToLower() &&
                        item.Abbreviation.ToUpper() == _abb.ToUpper())
                    {
                        item.IsDone = true;
                        HasHomework = true;
                        item.IsCurrent = false;
                        item.IsRemindEnabled = false;
                        Program._db.Homework.Update(item);
                        message = $"Good job. {item.Name.ToLower()} has been marked as completed.";
                        break;
                    }
                }
                await Program._db.SaveChangesAsync().ConfigureAwait(false);
                if (!HasHomework) message = "Can't find the specific homework";
            }
            sb.AppendLine(message);
            var embed = new EmbedBuilder();
            embed.Title = "Marking homework as completed";
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }
        #endregion
        #endregion

        #region Methods
        public async Task<List<Homework>> GetHomework(SocketUser _user)
        {
            var list = await Program._db.Homework.ToListAsync().ConfigureAwait(false);
            var homeworkList = list.Where(x => x.UserId == (long)_user.Id).ToList();
            return homeworkList;
        }

        public async Task<EmbedBuilder> SetReminders(SocketUser _user, EmbedBuilder _embed, bool _isEnabled)
        {
            var sb = new StringBuilder();
            var homeworkList = GetHomework(_user).Result;
            var currentList = homeworkList.Where(x => x.IsCurrent == true).ToList();

            foreach (var item in currentList)
            {
                item.IsRemindEnabled = _isEnabled;
                Program._db.Homework.Update(item);
            }

            if (currentList.Count != 0)
            {
                if (_isEnabled)
                    sb.AppendLine(":rollsafe:\n");
                else
                    sb.AppendLine("All reminders are off now.\n");

                foreach (var w in currentList)
                {
                    var str = Filtering.GetHomeworkString(w);
                    sb.AppendLine(str);
                    await Program._db.SaveChangesAsync().ConfigureAwait(false);
                }
            }

            if(_isEnabled)
                _embed.Title = "Turning all homework reminder on";
            else
                _embed.Title = "Turning all homework reminder off";

            _embed.WithColor(RandomColor);
            _embed.Description = sb.ToString();
            return _embed;
        }
        #endregion
    }
}
