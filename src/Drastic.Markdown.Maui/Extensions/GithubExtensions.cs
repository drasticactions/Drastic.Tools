// <copyright file="GithubExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Text.RegularExpressions;

#nullable disable

namespace Drastic.Markdown
{
    /// <summary>
    /// A set of helper extensions for parsing Github common urls.
    /// </summary>
    public static class GithubExtensions
    {
        private const string GithubReadmeUrl = "https://raw.githubusercontent.com/{0}/{1}/{2}/README.md";

        private static readonly Regex GithubRepoRegex = new Regex("http(s)?:\\/\\/github.com\\/([a-zA-Z0-9_-]+)\\/([a-zA-Z0-9_-]+)\\/((blob|tree)\\/([a-zA-Z0-9_-]+))?");

        public static bool TryExtractGithubRawMarkdownUrl(string url, out string readmeUrl)
        {
            var match = GithubRepoRegex.Match(url);
            if (match.Success)
            {
                var user = match.Groups[2].Value;
                var repo = match.Groups[3].Value;
                var branch = match.Groups.Count > 6 ? match.Groups[6].Value : "master";
                readmeUrl = string.Format(GithubReadmeUrl, user, repo, branch);
                return true;
            }

            readmeUrl = null;
            return false;
        }
    }
}
