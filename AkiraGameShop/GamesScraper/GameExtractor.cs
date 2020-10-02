using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AkiraGameShop
{
    public class GameExtractor
    {
        private static string _html;
        private static List<string> _links;

        public static void Extract(string html, out List<string> titles)
        {
            _html = html;

            //get links
            _links = GetLinks();
            //get titles
            titles = GetTitles();
        }

        private static List<string> GetTitles()
        {
            var titles = new List<string>();
            try
            {
                foreach (var link in _links)
                {
                    //get title
                    var title = link.Split('/')[3]
                        .Replace("-", " ")
                        .ToUpper();

                    titles.Add(title);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return titles;
        }

        private static List<string> GetLinks()
        {
            var skip = true;
            var list = new List<string>();

            var regex = new Regex("href=\"(.*?)\"",
                RegexOptions.Singleline | RegexOptions.CultureInvariant);

            if (regex.IsMatch(_html))
                foreach (Match match in regex.Matches(_html))
                {
                    if (match.Value.Contains("/all-games/"))
                    {
                        skip = false;
                        continue;
                    }

                    if (skip)
                        continue;

                    if (match.Value.Contains("https://steamunlocked.net/"))
                        list.Add(match.Groups[1].Value);
                }

            return list;
        }

        public static string FindLink(string title)
        {
            foreach (var link in _links)
                if (link.Contains(title))
                    return link;

            return "Errore";
        }
    }
}