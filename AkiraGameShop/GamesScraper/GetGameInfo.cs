using System.Text.RegularExpressions;
using System.Web;
using RestSharp;

namespace AkiraGameShop
{
    public class GetGameInfo
    {
        public static byte[] GetImage(string titleGame)
        {
            var client = new RestClient("https://steamunlocked.net");
            var request = new RestRequest($"/?s={titleGame}", Method.GET);

            var sourcePage = client.Execute(request).Content;

            //get link image
            var linkImage = Regex.Match(sourcePage, "src=\"https://steamunlocked.net/wp-content/uploads/(.*?)\"")
                .Groups[1].Value;

            client = new RestClient("https://steamunlocked.net/wp-content/uploads");
            request = new RestRequest(linkImage, Method.GET);

            //return bytes image
            return client.Execute(request).RawBytes;
        }

        public static string GetDescription(string gameTitle)
        {
            gameTitle = GetOriginalTitle(gameTitle);

            var link = GameExtractor.FindLink(gameTitle); //dal titolo ti da il link

            return CheckDescription(link);
        }

        private static string CheckDescription(string link)
        {
            var client = new RestClient(link);
            var request = new RestRequest();

            var sourcePage = client.Get(request).Content.Replace("\"", "'");

            //scrape description
            var description = Regex.Match(sourcePage, "About The Game</h2><p>(.*?)</p>").Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex.Match(sourcePage, @"About The Game</h2><div
id='game_area_description'>(.*?)<br").Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex
                    .Match(sourcePage, "id='game_area_description' class='game_area_description'><p>(.*?)</p>")
                    .Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex
                    .Match(sourcePage, "id='game_area_description' class='game_area_description'>(.*?)</div>").Groups[1]
                    .Value;

            if (string.IsNullOrEmpty(description))
                description = Regex.Match(sourcePage, "id='game_area_description'>(.*?)</div>").Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex.Match(sourcePage,
                    "id='game_area_description' class='game_area_description'>(.*?)<br").Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex.Match(sourcePage, @"<p
class='css-1aq06d4-MarkdownHeading__heading'>(.*?)</p>").Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex
                    .Match(sourcePage, "id='game_area_description' class='game_area_description'>(.*?)<img").Groups[1]
                    .Value;

            if (string.IsNullOrEmpty(description))
                description = Regex.Match(sourcePage, "class='description'><p>(.*?)</p>").Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex.Match(sourcePage, "id='game_area_description'><p>(.*?)</p>").Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex.Match(sourcePage, "class='Markdown-paragraph_fe094d70'>(.*?)</div>")
                    .Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex.Match(sourcePage,
                        "id='game_area_description' class='game_area_description'>(.*?)</span>")
                    .Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex
                    .Match(sourcePage, "id='game_area_description' class='game_area_description'>(.*?)</div>").Groups[1]
                    .Value;

            if (string.IsNullOrEmpty(description))
                description = Regex.Match(sourcePage, @"id='game_area_description' class='game_area_description'><p
                    class='uk-dropcap'>(.*?)</p>").Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex.Match(sourcePage,
                        "id='game_area_description' class='game_area_description'><p>(.*?)</p>")
                    .Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex.Match(sourcePage, "About The Game</h2><p>(.*?)<img").Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex
                    .Match(sourcePage, "id='game_area_description' class='game_area_description'><p>(.*?)<a").Groups[1]
                    .Value;

            if (string.IsNullOrEmpty(description))
                description = Regex.Match(sourcePage, "The Game</h2><p>(.*?)</p>").Groups[1].Value;

            if (string.IsNullOrEmpty(description))
                description = Regex.Match(sourcePage, @"id='game_area_description' class='game_area_description'><ul
                    class='bb_ul'><li>(.*?)<br").Groups[1].Value;

            if (string.IsNullOrEmpty(description)) description = "Descrizione al momento non disponibile.";

            description = HttpUtility.HtmlDecode(description);
            description = Regex.Replace(description, "<.*?>", string.Empty); //rimuove l'html restante dalla descrizione

            return description;
        }
        public static string GetDownloadLink(string link)
        {
            var client = new RestClient(link);
            var request = new RestRequest();

            var sourcePage = client.Get(request).Content;
            return Regex.Match(sourcePage, "class=\"btn-download\" href=\"(.*?)\" target=\"_blank\" rel=\"noopener\">").Groups[1].Value;
            
        }

        public static string GetOriginalTitle(string gameTitle)
        {
            return gameTitle.Replace(" ", "-").ToLower();
        }
    }
}