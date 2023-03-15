﻿using System.Text.RegularExpressions;

namespace Drastic.HtmlLabel.Maui
{
    internal static class StringExtensions
    {
        public static string ReplaceTag(this string html, string oldTagRegex, string newTag) =>
            Regex.Replace(html, @"(<\s*\/?\s*)" + oldTagRegex + @"((\s+[\w\-\,\.\(\)\=""\:\;]*)*>)", "$1" + newTag + "$2");
    }
}
