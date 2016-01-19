using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BIIS.Identities
{
    /// <summary>
    /// Class, that represents .biis page
    /// </summary>
    public class BiisPage
    {
        private const string ButtonRegexPattern = @"<biisButton\s+name=""(?<name>\S+)""\s+onClick=""(?<onClick>\S+)\s*,\s*(?<onClickTarget>\S+)"">(?<text>.+)</biisButton>";
        private const string LabelRegexPattern = @"<biisLabel\s+name=""(?<name>\S+)"">(?<text>\S*)</biisLabel>";

        /// <summary>
        /// Parsed to html page
        /// </summary>
        public string HtmlPage { get; }
        /// <summary>
        /// Controls, that page holds
        /// </summary>
        public List<BiisControl> Controls { get; } = new List<BiisControl>();
        /// <summary>
        /// Action, button on page, hold
        /// </summary>
        public List<ButtonDelegate> ButtonDelegates { get; } = new List<ButtonDelegate>();
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="page">Text of .biis page</param>
        /// <param name="buttonDelegates">Actions, on which buttons will subscribe</param>
        public BiisPage(string page, params ButtonDelegate[] buttonDelegates)
        {
            // add actions 

            ButtonDelegates.AddRange(buttonDelegates);

            // Find all elements and parse .biis to .html
            var htmlPage = string.Empty;
            for (Match match = Regex.Match(page, ButtonRegexPattern); match.Success; match = match.NextMatch())
            {
                var button = new BiisButton
                {
                    Name = match.Groups["name"].Value,
                    Text = match.Groups["text"].Value
                };

                foreach (var buttonDelegate in ButtonDelegates)
                {
                    var delegateName = buttonDelegate.Method.Name;
                    if (delegateName != match.Groups["onClick"].Value) continue;

                    button.ButtonOnClickDelegate = buttonDelegate;
                    button.OnClickTarget = match.Groups["onClickTarget"].Value;
                }

                htmlPage = Regex.Replace(page, ButtonRegexPattern, $"<button name=\"{button.Name}\" type=\"submit\">{button.Text}</button>");
                Controls.Add(button);    
            }

            for(Match match = Regex.Match(page, LabelRegexPattern); match.Success; match = match.NextMatch())
            {
                var label = new BiisLabel
                {
                    Name = match.Groups["name"].Value,
                    Text = match.Groups["text"].Value
                };
                Controls.Add(label);

                htmlPage = Regex.Replace(htmlPage, $@"<biisLabel\s+name=""{label.Name}"">(?<text>.*)</biisLabel>", $"<label name=\"{label.Name}\">{label.Text}</label>");
            }

            HtmlPage = htmlPage;
        }

        /// <summary>
        /// Returns .html representation, after POST request
        /// </summary>
        /// <returns></returns>
        public string GetHtmlAfterPost()
        {
            var htmlPage = HtmlPage;
            // find all buttons
            foreach (var button in Controls.OfType<BiisButton>())
            {
                // find label, which belongs to this button
                var target = Controls.OfType<BiisLabel>().Single(i => i.Name == button.OnClickTarget);
                // execute action
                button.ButtonOnClickDelegate(target);
                // update page
                htmlPage = Regex.Replace(htmlPage, $@"<label\s+name=""{target.Name}"">\S*</label>", $@"<label name=""{target.Name}\"">{target.Text}</label>");
            }

            return htmlPage;
        }
    }
}