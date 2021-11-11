using RestSharp;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DontForgetYourHW.Helpers
{
    public class RitNews
    {
        public async Task<List<string>> PostRitNews()
        {
            var actualLink = "";
            string homeLink = "https://www.rit.edu/news";

            RestClient rest = new RestClient(homeLink);
            var repsonse = rest.Execute(new RestRequest(""));
            var content = repsonse.Content;
            var pattern = @"about=";

            MatchCollection matches = Regex.Matches(content, pattern);
            int count = 0;
            List<string> linkList = new List<string>();
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                var index = groups[0].Index;
                var firstLine = content.Substring(index + pattern.Length, 200);
                var stopAtClass = firstLine.IndexOf("class=");
                var newLine = firstLine.Substring(1, stopAtClass - 3);
                actualLink = "https://www.rit.edu" + newLine;
                linkList.Add(actualLink);
                count++;
                if (count >= 7)
                    break;
            }
            return await Task.FromResult(linkList);
        }
    }
}
