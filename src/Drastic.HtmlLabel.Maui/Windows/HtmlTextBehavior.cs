// <copyright file="HtmlTextBehavior.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using Drastic.HtmlLabel.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Span = Microsoft.UI.Xaml.Documents.Span;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]

namespace Drastic.HtmlLabel.Maui
{
    internal class HtmlTextBehavior : Behavior<TextBlock>
    {
        // All the supported tags
        internal const string elementA = "A";
        internal const string elementB = "B";
        internal const string elementBr = "BR";
        internal const string elementEm = "EM";
        internal const string elementI = "I";
        internal const string elementP = "P";
        internal const string elementStrong = "STRONG";
        internal const string elementU = "U";
        internal const string elementUl = "UL";
        internal const string elementLi = "LI";
        internal const string elementDiv = "DIV";
        private readonly HtmlLabel label;

        public HtmlTextBehavior(HtmlLabel label)
        {
            this.label = label;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Loaded += this.OnAssociatedObjectLoaded;
            this.AssociatedObject.LayoutUpdated += this.OnAssociatedObjectLayoutUpdated;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Loaded -= this.OnAssociatedObjectLoaded;
            this.AssociatedObject.LayoutUpdated -= this.OnAssociatedObjectLayoutUpdated;
        }

        private static void ParseText(XElement element, InlineCollection inlines, HtmlLabel label)
        {
            if (element == null)
            {
                return;
            }

            InlineCollection currentInlines = inlines;
            var elementName = element.Name.ToString().ToUpperInvariant();
            switch (elementName)
            {
                case elementA:
                    var link = new Hyperlink();
                    XAttribute href = element.Attribute("href");
                    var unescapedUri = Uri.UnescapeDataString(href?.Value);
                    if (href != null)
                    {
                        try
                        {
                            link.NavigateUri = new Uri(unescapedUri);
                        }
                        catch (FormatException)
                        { /* href is not valid */
                        }
                    }

                    link.Click += (sender, e) =>
                    {
                        sender.NavigateUri = null;
                        RendererHelper.HandleUriClick(label, unescapedUri);
                    };
                    if (label.LinkColor.IsNotDefault())
                    {
                        link.Foreground = label.LinkColor.ToPlatform();
                    }

                    if (!label.UnderlineText)
                    {
                        link.UnderlineStyle = UnderlineStyle.None;
                    }

                    inlines.Add(link);
                    currentInlines = link.Inlines;
                    break;
                case elementB:
                case elementStrong:
                    var bold = new Bold();
                    inlines.Add(bold);
                    currentInlines = bold.Inlines;
                    break;
                case elementI:
                case elementEm:
                    var italic = new Italic();
                    inlines.Add(italic);
                    currentInlines = italic.Inlines;
                    break;
                case elementU:
                    var underline = new Underline();
                    inlines.Add(underline);
                    currentInlines = underline.Inlines;
                    break;
                case elementBr:
                    inlines.Add(new LineBreak());
                    break;
                case elementP:
                    // Add two line breaks, one for the current text and the second for the gap.
                    if (AddLineBreakIfNeeded(inlines))
                    {
                        inlines.Add(new LineBreak());
                    }

                    var paragraphSpan = new Span();
                    inlines.Add(paragraphSpan);
                    currentInlines = paragraphSpan.Inlines;
                    break;
                case elementLi:
                    inlines.Add(new LineBreak());
                    inlines.Add(new Run { Text = " • " });
                    break;
                case elementUl:
                case elementDiv:
                    _ = AddLineBreakIfNeeded(inlines);
                    var divSpan = new Span();
                    inlines.Add(divSpan);
                    currentInlines = divSpan.Inlines;
                    break;
            }

            foreach (XNode node in element.Nodes())
            {
                if (node is XText textElement)
                {
                    currentInlines.Add(new Run { Text = textElement.Value });
                }
                else
                {
                    ParseText(node as XElement, currentInlines, label);
                }
            }

            // Add newlines for paragraph tags
            if (elementName == "ElementP")
            {
                currentInlines.Add(new LineBreak());
            }
        }

        private void OnAssociatedObjectLayoutUpdated(object sender, object o) => this.UpdateText();

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.UpdateText();
            this.AssociatedObject.Loaded -= this.OnAssociatedObjectLoaded;
        }

        private void UpdateText()
        {
            if (this.AssociatedObject == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.AssociatedObject.Text))
            {
                return;
            }

            var text = this.AssociatedObject.Text;

            // Just incase we are not given text with elements.
            var modifiedText = $"<div>{text}</div>";

            var linkRegex = new Regex(@"<a\s+href=""(.+?)\""");

            MatchCollection matches = linkRegex.Matches(modifiedText);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    for (var i = 1; i < match.Groups.Count; i++)
                    {
                        Group group = match.Groups[i];
                        var escapedUri = Uri.EscapeDataString(group.Value);
                        modifiedText = modifiedText.Replace(group.Value, escapedUri, StringComparison.InvariantCulture);
                    }
                }

                System.Diagnostics.Debug.WriteLine(@$"ERROR: ${matches}");
            }

            modifiedText = Regex.Replace(modifiedText, "<br>", "<br></br>", RegexOptions.IgnoreCase)
                .Replace("\n", string.Empty, StringComparison.OrdinalIgnoreCase) // KWI-FIX Enters resulted in multiple lines
                .Replace("&nbsp;", "&#160;", StringComparison.OrdinalIgnoreCase); // KWI-FIX &nbsp; is not supported by the UWP TextBlock

            // reset the text because we will add to it.
            this.AssociatedObject.Inlines.Clear();

            var element = XElement.Parse(modifiedText);
            ParseText(element, this.AssociatedObject.Inlines, this.label);

            this.AssociatedObject.LayoutUpdated -= this.OnAssociatedObjectLayoutUpdated;
            this.AssociatedObject.Loaded -= this.OnAssociatedObjectLoaded;
        }

        private static bool AddLineBreakIfNeeded(InlineCollection inlines)
        {
            if (inlines.Count <= 0)
            {
                return false;
            }

            Inline lastInline = inlines[inlines.Count - 1];
            while (lastInline is Span)
            {
                var span = (Span)lastInline;
                if (span.Inlines.Count > 0)
                {
                    lastInline = span.Inlines[span.Inlines.Count - 1];
                }
            }

            if (lastInline is LineBreak)
            {
                return false;
            }

            inlines.Add(new LineBreak());
            return true;
        }
    }
}