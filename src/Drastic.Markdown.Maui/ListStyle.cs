// <copyright file="ListStyle.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Markdig.Syntax;

namespace Drastic.Markdown
{
    public enum ListStyleType
    {
        Square,
        Circle,
        None,
        Symbol,
        Decimal,
        Custom,
    }


    public class ListStyle
    {
        public float Indentation { get; set; } = 10;

        public float? Spacing { get; set; }

        public float ItemsVerticalSpacing { get; set; } = 10;

        public Thickness ListMargin { get; set; } = new Thickness(0);

        public ListStyleType BulletStyleType { get; set; }

        public int BulletSize { get; set; } = 4;

        public float? BulletFontSize { get; set; }

        public float? BulletLineHeight { get; set; }

        public Color? BulletColor { get; set; }

        public FontAttributes BulletFontAttributes { get; set; } = FontAttributes.None;

        public LayoutOptions BulletVerticalOptions { get; set; }

        public LayoutOptions ItemVerticalOptions { get; set; }

        public string? Symbol { get; set; }

        public Func<int, ListBlock, ListItemBlock, View>? CustomCallback { get; set; }
    }
}
