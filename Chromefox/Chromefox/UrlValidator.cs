using System.Text.RegularExpressions;

namespace Chromefox
{
    class UrlValidator
    {
        private const string ComPattern = @"\.com";
        private const string RuPattern = @"\.ru";
        private const string UaPattern = @"\.ua";
        private const string EmailPattern = @"\@";
        private const string WwwPattern = @"^www\.";

        public static string ValidateUrl(string url)
        {
            if (Regex.Match(url, EmailPattern).Success)
            {
                return string.Empty;
            }

            if (!(Regex.Match(url, ComPattern).Success ||
                  Regex.Match(url, RuPattern).Success ||
                  Regex.Match(url, UaPattern).Success))
            {
                return string.Empty;
            }

            return !Regex.Match(url, WwwPattern).Success ? $"www.{url}" : url;
        }
    }
}
