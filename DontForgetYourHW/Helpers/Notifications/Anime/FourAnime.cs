using RestSharp;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DontForgetYourHW.Helpers.Notifications.Anime
{
    public class FourAnime
    {
        public static Tuple<string, string, string> CheckIfHave(string _name)
        {
            string pattern = $"DIV_4";
            var link = "";
            var title = "";
            var image = "";
            var canContinue = true;
            for (int i = 1; i <= 6; i++)
            {
                var rest = new RestClient("https://4anime.to/browse?sort_order=title+asc&_sft_status=airing&sf_paged=" + i);
                var repsonse = rest.Execute(new RestRequest(""));
                var content = repsonse.Content;
                MatchCollection matches = Regex.Matches(content, pattern);

                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    var index = groups[0].Index;
                    var body = content.Substring(index + 17, 400);
                    var endLink = body.IndexOf("\" ");
                    link = body.Substring(0, endLink);

                    var startImage = body.IndexOf("src=\"") + 5;
                    var imageBody = body.Substring(startImage, 200);
                    var endImage = imageBody.IndexOf("\" alt=");
                    image = imageBody.Substring(0, endImage);

                    var startTitle = body.IndexOf("alt=\"") + 5;
                    var titleBody = body.Substring(startTitle, 400 - startTitle);
                    var endTitle = titleBody.IndexOf("\" id");
                    title = titleBody.Substring(0, endTitle);
                    if (title.ToLower() == _name.ToLower())
                    {
                        canContinue = false;
                        break;
                    }
                }
                if (!canContinue)
                    break;
            }
            if (canContinue)
                link = "";

            var tuples = new Tuple<string, string, string>(link, title, image);
            return tuples;
        }

        public static Tuple<string, string> GetLastEpisode(string _link)
        {
            var pattern = "title=\"\">";
            var episode = "";
            var epLink = "";

            var rest = new RestClient(_link);
            var repsonse = rest.Execute(new RestRequest(""));
            var content = repsonse.Content;
            MatchCollection matches = Regex.Matches(content, pattern);

            if (matches.Count != 0)
            {
                var match = matches.Last();

                GroupCollection groups = match.Groups;
                var index = groups[0].Index;
                var body = content.Substring(index + 9, 400);
                var endEpisode = body.IndexOf("</a");
                episode = body.Substring(0, endEpisode);

                var largerText = content.Substring(index - 100, 100);
                var startEpLink = largerText.IndexOf("href=\"") + 6;
                var epBody = largerText.Substring(startEpLink, 50);
                var endEpBody = epBody.IndexOf("episode-") + 8;
                epLink = epBody.Substring(0, endEpBody);
            }

            return new Tuple<string, string>(episode, epLink);
        }
    }
}
