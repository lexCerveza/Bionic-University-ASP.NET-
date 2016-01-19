using System.Text;
using System.Text.RegularExpressions;

namespace Chromefox
{
    public class ContentCutter
    {
        public static string GetContentFromResponse(string response)
        {
            var content = new StringBuilder();
            content.AppendLine(Regex.Match(response, @"HTTP/\d\.\d\W*(?<code>\d*\W*\w*)").Groups["code"].Value);
            
            switch (GetResponseCode(response))
            {
                case 200:
                    content.AppendLine($"Title : {Regex.Match(response, @"<title>(?<title>.*)</title>", RegexOptions.IgnoreCase).Groups["title"].Value}");
                    content.AppendLine($"Content-Length : {Regex.Match(response, @"Content-Length:\W*(?<content_length>\d*)").Groups["content_length"].Value}");
                    break;
                case 302:
                    content.AppendLine($"Location: {Regex.Match(response, @"Location:\W*(?<location>.*)").Groups["location"].Value}");
                    break;
                case 404:
                    content.AppendLine("Not found");
                    break;
                default:
                    content.AppendLine("I don't know how to handle this(");
                    break;
            }

            return content.ToString();
        }

        private static int GetResponseCode(string response)
        {
            int responseCode;
            var responseCodeString = Regex.Match(response, @"HTTP/\d\.\d\W*(?<code>\d*)\W*\w*").Groups["code"].Value;
            int.TryParse(responseCodeString, out responseCode);

            return responseCode;
        }
    }
}