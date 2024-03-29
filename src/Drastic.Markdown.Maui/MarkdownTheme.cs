﻿// <copyright file="MarkdownTheme.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#nullable disable

namespace Drastic.Markdown
{
    public class MarkdownTheme
    {
        public MarkdownTheme()
        {
            this.Paragraph = new MarkdownStyle
            {
                Attributes = FontAttributes.None,
                FontSize = 12,
            };

            this.Heading1 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                BorderSize = 1,
                FontSize = 26,
            };

            this.Heading2 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                BorderSize = 1,
                FontSize = 22,
            };

            this.Heading3 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                FontSize = 20,
            };

            this.Heading4 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                FontSize = 18,
            };

            this.Heading5 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                FontSize = 16,
            };

            this.Heading6 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                FontSize = 14,
            };

            this.Link = new LinkStyle
            {
                Attributes = FontAttributes.None,
                FontSize = 12,
            };

            this.Code = new MarkdownStyle
            {
                Attributes = FontAttributes.None,
                FontSize = 12,
            };

            this.Quote = new MarkdownStyle
            {
                Attributes = FontAttributes.None,
                BorderSize = 4,
                FontSize = 12,
                BackgroundColor = Color.FromArgb("#808080").MultiplyAlpha(.1f),
            };

            this.Separator = new MarkdownStyle
            {
                BorderSize = 2,
            };

            this.OrderedList = new ListStyle
            {
                BulletStyleType = ListStyleType.Decimal,
                BulletVerticalOptions = LayoutOptions.Start,
                ItemVerticalOptions = LayoutOptions.Start,
            };

            this.UnorderedList = new ListStyle
            {
                BulletStyleType = ListStyleType.Square,
                BulletVerticalOptions = LayoutOptions.Center,
                ItemVerticalOptions = LayoutOptions.Center,
            };

            // Platform specific properties
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    this.Code.FontFamily = "Courier";
                    break;

                case Device.Android:
                    this.Code.FontFamily = "monospace";
                    break;
            }
        }

        public Color BackgroundColor { get; set; }

        public MarkdownStyle Paragraph { get; set; }

        public MarkdownStyle Heading1 { get; set; }

        public MarkdownStyle Heading2 { get; set; }

        public MarkdownStyle Heading3 { get; set; }

        public MarkdownStyle Heading4 { get; set; }

        public MarkdownStyle Heading5 { get; set; }

        public MarkdownStyle Heading6 { get; set; }

        public MarkdownStyle Quote { get; set; }

        public MarkdownStyle Separator { get; set; }

        public LinkStyle Link { get; set; }

        public MarkdownStyle Code { get; set; }

        public ListStyle OrderedList { get; set; }

        public ListStyle UnorderedList { get; set; }

        public float Margin { get; set; } = 10;

        public float VerticalSpacing { get; set; } = 10;

        public bool UseEmojiAndSmileyExtension { get; set; }
    }

    public class LightMarkdownTheme : MarkdownTheme
    {
        public static readonly Color DefaultBackgroundColor = Color.FromHex("#ffffff");

        public LightMarkdownTheme()
        {
            this.BackgroundColor = DefaultBackgroundColor;
            this.Paragraph.ForegroundColor = DefaultTextColor;
            this.Heading1.ForegroundColor = DefaultTextColor;
            this.Heading1.BorderColor = DefaultSeparatorColor;
            this.Heading2.ForegroundColor = DefaultTextColor;
            this.Heading2.BorderColor = DefaultSeparatorColor;
            this.Heading3.ForegroundColor = DefaultTextColor;
            this.Heading4.ForegroundColor = DefaultTextColor;
            this.Heading5.ForegroundColor = DefaultTextColor;
            this.Heading6.ForegroundColor = DefaultTextColor;
            this.Link.ForegroundColor = DefaultAccentColor;
            this.Code.ForegroundColor = DefaultTextColor;
            this.Code.BackgroundColor = DefaultCodeBackground;
            this.Quote.ForegroundColor = DefaultQuoteTextColor;
            this.Quote.BorderColor = DefaultQuoteBorderColor;
            this.Separator.BorderColor = DefaultSeparatorColor;
        }

        public static readonly Color DefaultAccentColor = Color.FromHex("#0366d6");

        public static readonly Color DefaultTextColor = Color.FromHex("#24292e");

        public static readonly Color DefaultCodeBackground = Color.FromHex("#f6f8fa");

        public static readonly Color DefaultSeparatorColor = Color.FromHex("#eaecef");

        public static readonly Color DefaultQuoteTextColor = Color.FromHex("#6a737d");

        public static readonly Color DefaultQuoteBorderColor = Color.FromHex("#dfe2e5");
    }

    public class DarkMarkdownTheme : MarkdownTheme
    {
        public static readonly Color DefaultBackgroundColor = Color.FromHex("#2b303b");

        public DarkMarkdownTheme()
        {
            this.BackgroundColor = DefaultBackgroundColor;
            this.Paragraph.ForegroundColor = DefaultTextColor;
            this.Heading1.ForegroundColor = DefaultTextColor;
            this.Heading1.BorderColor = DefaultSeparatorColor;
            this.Heading2.ForegroundColor = DefaultTextColor;
            this.Heading2.BorderColor = DefaultSeparatorColor;
            this.Heading3.ForegroundColor = DefaultTextColor;
            this.Heading4.ForegroundColor = DefaultTextColor;
            this.Heading5.ForegroundColor = DefaultTextColor;
            this.Heading6.ForegroundColor = DefaultTextColor;
            this.Link.ForegroundColor = DefaultAccentColor;
            this.Code.ForegroundColor = DefaultTextColor;
            this.Code.BackgroundColor = DefaultCodeBackground;
            this.Quote.ForegroundColor = DefaultQuoteTextColor;
            this.Quote.BorderColor = DefaultQuoteBorderColor;
            this.Separator.BorderColor = DefaultSeparatorColor;
        }

        public static readonly Color DefaultAccentColor = Color.FromHex("#d08770");

        public static readonly Color DefaultTextColor = Color.FromHex("#eff1f5");

        public static readonly Color DefaultCodeBackground = Color.FromHex("#4f5b66");

        public static readonly Color DefaultSeparatorColor = Color.FromHex("#65737e");

        public static readonly Color DefaultQuoteTextColor = Color.FromHex("#a7adba");

        public static readonly Color DefaultQuoteBorderColor = Color.FromHex("#a7adba");
    }
}
