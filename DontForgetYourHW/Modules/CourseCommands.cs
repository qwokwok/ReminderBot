using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DontForgetYourHW.Database;
using DontForgetYourHW.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontForgetYourHW.Modules
{
    public class CourseCommands : ModuleBase<SocketCommandContext>
    {
        Discord.Color RandomColor { get; set; } = new Color(Filtering.X(), Filtering.X(), Filtering.X());

        #region Add a course helper - Done
        [Command("addC")]
        public async Task AddCourses()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Use the following format to add a course.\n");
            sb.AppendLine($"Format:\n`qq addC \"COURSE\" \"ABBREVIATION\" \"START-END TIME\" \"DAY(S) OF WEEK\"` \n");
            sb.AppendLine($"Example:\n`qq addC \"Java For Programmer\" \"Java\" \"12:00pm-2:25pm\" \"MTWTHF\"` \n");

            var embed = new EmbedBuilder();
            embed.Title = "Ok what are your courses?";
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Add a course - Done
        [Command("addC")]
        public async Task Add(string _name, string _abb, string _time, string _days)
        {
            var user = Context.User;
            var alreadyHas = false;
            var sb = new StringBuilder();
            var title = " - Failed";
            var valid = true;

            if (_name == "")
            {
                sb.Append($"The course name can't be blank.\n");
                valid = false;
            }

            if (_name.Length > 19)
            {
                sb.Append($"The name can't have more than 19 characters");
                valid = false;
            }

            if (_abb == "")
            {
                sb.Append($"The abbreviation can't be blanked.");
                valid = false;
            }

            if (_abb.Length > 4)
            {
                sb.Append($"The abbreviation can't have more than 4 characters");
                valid = false;
            }

            if (!_time.Contains(":") || !_time.Contains("m") || !_time.Contains("-"))
            {
                sb.Append($"The time you have entered is invalid. Use the following format\n");
                sb.Append($"Example: `12:00pm-2:25pm`\n");
                valid = false;
            }

            if (_days == "")
            {
                sb.AppendLine($"You aren't telling me which day...\n");
                sb.Append($"Example: `MTWTHF`");
                valid = false;
            }

            if (valid)
            {
                //var list = await Program._db.Course.ToListAsync();
                //list = list.Where(x => x.UserId == (long)user.Id).ToList();
                var list = GetCourses(user).Result;
                Filtering._intList = new List<int>();
                var hasDays = Filtering.DaysToString(_days);
                var days = hasDays.Split(" & ");

                var canContinue = true;
                foreach (var item in list)
                {
                    if (canContinue)
                    {
                        if (item.CourseName == _name)
                        {
                            if (item.Days != null)
                            {
                                var xDays = item.Days.Split(" & ");
                                foreach (var currentDay in xDays)
                                {
                                    if (days.Contains(currentDay))
                                    {
                                        alreadyHas = true;
                                        canContinue = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                        break;
                }

                if (alreadyHas)
                    sb.Append("You already mentioned that, shittyhead.");

                if (!alreadyHas)
                {
                    var intList = Filtering._intList;

                    foreach (var item in intList)
                    {
                        var course = new Course();
                        course.UserId = (long)user.Id;
                        course.CourseName = _name;
                        course.Abbreviation = _abb;
                        course.StartTime = Filtering.GetTime(true, _time);
                        course.EndTime = Filtering.GetTime(false, _time);
                        course.Days = Filtering.DaysToString(_days);
                        course.DayIndex = item;
                        await Program._db.Course.AddAsync(course).ConfigureAwait(false);
                    }

                    await Program._db.SaveChangesAsync().ConfigureAwait(false);

                    sb.AppendLine($"Ok i'll keep it in mind\n");
                    title = " - Succeed";

                    //var allList = await Program._db.Course.ToListAsync();
                    //var sortedList = allList.Where(x => x.UserId == (long)user.Id).ToList();
                    var sortedList = GetCourses(user).Result;
                    var temp = sortedList.OrderBy(x => x.DayIndex).ThenBy(y => y.StartTime).ToList();

                    foreach (var item in temp)
                    {
                        var str = Filtering.GetCourseTitleString(item);
                        sb.AppendLine(str);
                    }
                }
            }

            var embed = new EmbedBuilder();
            embed.Title = "Adding course" + title;
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Show courses - Done
        [Command("mycourses")]
        public async Task List()
        {
            var user = Context.User;

            var sb = new StringBuilder();

            //var list = await Program._db.Course.ToListAsync();
            //var newList = list.Where(x => x.UserId == (long)user.Id).ToList();
            var list = GetCourses(user).Result;

            var temp = list.OrderBy(x => x.DayIndex).ThenBy(y => y.StartTime).ToList();

            if (temp.Count == 0)
                sb.AppendLine($"I don't remember you mentioned your courses ¯\\_(ツ)_/¯");

            else
            {
                foreach (var item in temp)
                {
                    var str = Filtering.GetCourseTitleString(item);
                    sb.AppendLine(str);
                }
            }

            var embed = new EmbedBuilder();
            embed.Title = $"{user.Username}'s courses";
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Remove courses Helper - Done
        [Command("removeC")]
        public async Task RemoveHelper()
        {
            var user = Context.User;
            var sb = new StringBuilder();
            sb.AppendLine("Please use the following format below to remove a course.\n");
            sb.AppendLine("Format:\n`qq removeC \"COURSE\"`\n");
            sb.AppendLine("Example:\n`qq removeC \"Java For Programmer\"`");

            var embed = new EmbedBuilder();
            embed.Title = $"Which course would you like to remove?";
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        #region Remove courses - Done
        [Command("removeC")]
        public async Task Remove(string _name)
        {
            var user = Context.User;

            var sb = new StringBuilder();

            var list = GetCourses(user).Result;
            var title = " - Failed";

            if (list.Count == 0)
                sb.Append($"You don't have any courses.\n");

            else
            {
                var found = false;
                var name = "";
                foreach (var item in list)
                {
                    if (_name == item.CourseName)
                    {
                        Program._db.Course.Remove(item);
                        await Program._db.SaveChangesAsync().ConfigureAwait(false);
                        found = true;
                        title = " - Succeed";
                        name = item.CourseName;
                    }
                }

                if (!found)
                    sb.Append($"There is no course you mentioned.\n");

                else
                {
                    sb.AppendLine($"Ok I will forget {name}.\n");

                    var allList = await Program._db.Course.ToListAsync();
                    var sortedList = allList.Where(x => x.UserId == (long)user.Id).ToList();
                    var temp = sortedList.OrderBy(x => x.DayIndex).ThenBy(y => y.StartTime).ToList();

                    foreach (var item in temp)
                    {
                        var str = Filtering.GetCourseTitleString(item);
                        sb.AppendLine(str);
                    }
                }
            }

            var embed = new EmbedBuilder();
            embed.Title = $"Remove a course" + title;
            embed.WithColor(RandomColor);
            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }
        #endregion

        public async Task<List<Course>> GetCourses(SocketUser _user)
        {
            var list = await Program._db.Course.ToListAsync().ConfigureAwait(false);
            list = list.Where(x => x.UserId == (long)_user.Id).ToList();
            return list;
        }
    }
}
