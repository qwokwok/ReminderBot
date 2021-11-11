using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DontForgetYourHW.Helpers.RIT
{
    public class Covid
    {
        public static bool IsScheduledRange()
        {
            TimeSpan tstart = new TimeSpan(6, 30, 0);
            TimeSpan tend = new TimeSpan(6, 35, 0);

            TimeSpan dt = DateTime.Now.TimeOfDay;

            if ((dt > tstart) && (dt < tend))
                return true;

            else
                return false;
        }

        public static string GetAlertLevel()
        {
            RestClient rest = new RestClient("https://www.rit.edu/ready/dashboard");
            var repsonse = rest.Execute(new RestRequest(""));
            var content = repsonse.Content;
            var pattern = $"https://www.rit.edu/ready/rit-covid-19-alert-levels#";

            var displayColor = "";

            MatchCollection matches = Regex.Matches(content, pattern);
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                var start = groups[0].Index;
                var body = content.Substring(start + 52, 20);
                var end = body.IndexOf("\">");
                var color = body.Substring(0, end);

                var link = "https://cdn.discordapp.com/attachments/743475559807582339/";

                switch (color)
                {
                    case "green": displayColor = link + "752617832453505024/green.png"; break;
                    case "yellow": displayColor = link + "752617935612280902/yellow.png"; break;
                    case "orange": displayColor = link + "752617953501118585/orange.png"; break;
                    case "red": displayColor = link + "752617968151560212/red.png"; break;
                }
                break;
            }
            return displayColor;
        }
    }
}
