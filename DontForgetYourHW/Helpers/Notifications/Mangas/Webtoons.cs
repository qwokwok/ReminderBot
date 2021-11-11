using RestSharp;
using System;
using System.Text.RegularExpressions;

namespace DontForgetYourHW.Helpers.Notifications.Mangas
{
    public static class Webtoons
    {
        #region Methods
        public static Tuple<string, string, string> GetLinkFromWebToons(string _nameLink, int _latestEpisode)
        {
            RestClient rest = new RestClient(_nameLink);
            var repsonse = rest.Execute(new RestRequest(""));
            var content = repsonse.Content;
            var pattern = $"data-episode-no=\"";

            MatchCollection matches = Regex.Matches(content, pattern);

            var link = "";
            var episode = "";
            var title = "";

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                var index = groups[0].Index;
                var body = content.Substring(index + pattern.Length, 600);
                var endBody = body.IndexOf($"\"");
                episode = body.Substring(0, endBody);

                if (_latestEpisode.ToString() == episode)
                    break;

                var startTitle = body.IndexOf($"subj\"><span>") + 12;
                var titleBody = body.Substring(startTitle, 600 - startTitle);
                var endTitle = titleBody.IndexOf($"</span>");

                title = titleBody.Substring(0, endTitle);

                link = GetLink(body);

                break;
            }
            return new Tuple<string, string, string>(link, episode, title);
        }

        public static string CheckIfHave(string _name)
        {
            RestClient rest = new RestClient("https://www.webtoons.com/en/genre");
            var repsonse = rest.Execute(new RestRequest(""));
            var content = repsonse.Content;
            var pattern = $"<p class=\"subj\">{_name}</p>";

            MatchCollection matches = Regex.Matches(content, pattern);

            var link = "";

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                var index = groups[0].Index;
                var body = content.Substring(index - 400, 400);

                link = GetLink(body);

                if (link != "")
                    break;
            }

            return link;
        }

        public static string GetLink(string _body)
        {
            var startLink = _body.IndexOf($"href=\"") + 6;
            var endLink = _body.IndexOf($"\" class=");
            var link = _body.Substring(startLink, endLink - startLink);
            return link;
        }

        //public static List<Manga> GetList()
        //{
        //    RestClient rest = new RestClient("https://www.webtoons.com/en/genre");
        //    var repsonse = rest.Execute(new RestRequest(""));
        //    var content = repsonse.Content;
        //    var pattern = $"class=\"subj\">";

        //    MatchCollection matches = Regex.Matches(content, pattern);

        //    var list = new List<Manga>();

        //    foreach (Match match in matches)
        //    {
        //        GroupCollection groups = match.Groups;
        //        var index = groups[0].Index;
        //        var body = content.Substring(index + pattern.Length, 300);
        //        var endTitle = body.IndexOf("</p>");
        //        var title = body.Substring(0, endTitle);

        //        var startFavorite = body.IndexOf("num\">") + 5;
        //        var endFavorite = body.IndexOf("</em>");
        //        var favorite = body.Substring(startFavorite, endFavorite - startFavorite);

        //        var indexAuthor = body.IndexOf("author\">") + 8;
        //        var endAuthor = body.IndexOf("</p>");
        //        var author = body.Substring(indexAuthor, title.Length);

        //        var removeTag = author.IndexOf("</p");

        //        if (removeTag != -1)
        //            author = author.Substring(0, removeTag);

        //        var manga = new Manga() { Title = title, Author = author, Favorite = favorite, };

        //        var contains = false;

        //        foreach (var item in list)
        //        {
        //            if (item.Title == title)
        //            {
        //                contains = true;
        //                break;
        //            }
        //        }

        //        if (!contains)
        //            list.Add(manga);
        //    }

        //    return list;
        //}
        #endregion
    }
}
