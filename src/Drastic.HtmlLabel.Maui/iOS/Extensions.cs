// <copyright file="Extensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Foundation;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Internals;
using UIKit;
using NativeFont = UIKit.UIFont;

namespace Drastic.HtmlLabel.Maui
{
    internal static class ColorExtensions
    {
        internal static bool IsEqualToColor(this UIColor self, UIColor otherColor)
        {
            nfloat r;
            nfloat g;
            nfloat b;
            nfloat a;

            self.GetRGBA(out r, out g, out b, out a);
            nfloat r2;
            nfloat g2;
            nfloat b2;
            nfloat a2;

            otherColor.GetRGBA(out r2, out g2, out b2, out a2);

            return r == r2 && g == g2 && b == b2 && a == a2;
        }
    }

    internal static class FontExtensions
    {
        private static readonly string defaultFontName = NativeFont.SystemFontOfSize(12).Name;

/* プロジェクト 'Drastic.HtmlLabel.Maui(net7.0-maccatalyst)' からのマージされていない変更
前:
        internal static bool IsBold(NativeFont font)
後:
        private static readonly Dictionary<ToNativeFontFontKey, NativeFont> ToUiFont = new Dictionary<ToNativeFontFontKey, NativeFont>();

        public static NativeFont ToUIFont(this Microsoft.Maui.Font self) => self.ToNativeFont();

        internal static bool IsBold(NativeFont font)
*/

        private static readonly Dictionary<ToNativeFontFontKey, NativeFont> ToUiFont = new Dictionary<ToNativeFontFontKey, NativeFont>();

        public static NativeFont ToUIFont(this Microsoft.Maui.Font self) => self.ToNativeFont();

        internal static bool IsBold(NativeFont font)
        {
            UIFontDescriptor fontDescriptor = font.FontDescriptor;
            UIFontDescriptorSymbolicTraits traits = fontDescriptor.SymbolicTraits;
            return traits.HasFlag(UIFontDescriptorSymbolicTraits.Bold);
        }

        internal static NativeFont Bold(this NativeFont font)
        {
            UIFontDescriptor fontDescriptor = font.FontDescriptor;
            UIFontDescriptorSymbolicTraits traits = fontDescriptor.SymbolicTraits;
            traits = traits | UIFontDescriptorSymbolicTraits.Bold;
            UIFontDescriptor boldFontDescriptor = fontDescriptor.CreateWithTraits(traits);
            return NativeFont.FromDescriptor(boldFontDescriptor, font.PointSize);
        }


        internal static NativeFont Italic(this NativeFont self)
        {
            UIFontDescriptor fontDescriptor = self.FontDescriptor;
            UIFontDescriptorSymbolicTraits traits = fontDescriptor.SymbolicTraits;
            traits = traits | UIFontDescriptorSymbolicTraits.Italic;
            UIFontDescriptor boldFontDescriptor = fontDescriptor.CreateWithTraits(traits);
            return NativeFont.FromDescriptor(boldFontDescriptor, self.PointSize);
        }

        internal static NativeFont WithTraitsOfFont(this NativeFont self, NativeFont font)
        {
            UIFontDescriptor fontDescriptor = self.FontDescriptor;
            UIFontDescriptorSymbolicTraits traits = fontDescriptor.SymbolicTraits;
            traits = traits | font.FontDescriptor.SymbolicTraits;
            UIFontDescriptor boldFontDescriptor = fontDescriptor.CreateWithTraits(traits);
            return NativeFont.FromDescriptor(boldFontDescriptor, self.PointSize);
        }

        internal static NativeFont ToUIFont(this IFontElement element) => element.ToNativeFont();

        internal static string CleanseFontName(string fontName)
        {
            var fontFile = FontFile.FromString(fontName);

            return fontFile.PostScriptName;
        }

        private static NativeFont ToNativeFont1(string family, float size, FontAttributes attributes)
        {
            var bold = (attributes & FontAttributes.Bold) != 0;
            var italic = (attributes & FontAttributes.Italic) != 0;

            if (family != null && family != defaultFontName)
            {
                try
                {
                    NativeFont result = null;
                    if (NativeFont.FamilyNames.Contains(family))
                    {
                        var descriptor = new UIFontDescriptor().CreateWithFamily(family);

                        if (bold || italic)
                        {
                            var traits = (UIFontDescriptorSymbolicTraits)0;
                            if (bold)
                            {
                                traits = traits | UIFontDescriptorSymbolicTraits.Bold;
                            }

                            if (italic)
                            {
                                traits = traits | UIFontDescriptorSymbolicTraits.Italic;
                            }

                            descriptor = descriptor.CreateWithTraits(traits);
                            result = NativeFont.FromDescriptor(descriptor, size);
                            if (result != null)
                            {
                                return result;
                            }
                        }
                    }

                    var cleansedFont = CleanseFontName(family);
                    result = NativeFont.FromName(cleansedFont, size);
                    if (family.StartsWith(".SFUI", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var fontWeight = family.Split('-').LastOrDefault();

                        if (!string.IsNullOrWhiteSpace(fontWeight) && Enum.TryParse<UIFontWeight>(fontWeight, true, out var uIFontWeight))
                        {
                            result = NativeFont.SystemFontOfSize(size, uIFontWeight);
                            return result;
                        }

                        result = NativeFont.SystemFontOfSize(size, UIFontWeight.Regular);
                        return result;
                    }

                    if (result == null)
                    {
                        result = NativeFont.FromName(family, size);
                    }

                    if (result != null)
                    {
                        return result;
                    }
                }
                catch
                {
                    Debug.WriteLine("Could not load font named: {0}", family);
                }
            }

            if (bold && italic)
            {
                var defaultFont = NativeFont.SystemFontOfSize(size);

                var descriptor = defaultFont.FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold | UIFontDescriptorSymbolicTraits.Italic);
                return NativeFont.FromDescriptor(descriptor, 0);
            }

            if (italic)
            {
                return NativeFont.ItalicSystemFontOfSize(size);
            }

            if (bold)
            {
                return NativeFont.BoldSystemFontOfSize(size);
            }

            return NativeFont.SystemFontOfSize(size);
        }

        internal static bool IsDefault(this Span self)
        {
            return self.FontFamily == null && self.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(Label), true) &&
                    self.FontAttributes == FontAttributes.None;
        }

        private static NativeFont ToNativeFont(this IFontElement element)
        {
            var fontFamily = element.FontFamily;
            var fontSize = (float)element.FontSize;
            var fontAttributes = element.FontAttributes;
            return ToNativeFont(fontFamily, fontSize, fontAttributes, ToNativeFont1);
        }

        private static NativeFont ToNativeFont(this Microsoft.Maui.Font self)
        {
            var size = (float)self.Size;

            var fontAttributes = self.GetFontAttributes();

            return ToNativeFont(self.Family, size, fontAttributes, ToNativeFont1);
        }

        private static NativeFont ToNativeFont(string family, float size, FontAttributes attributes, Func<string, float, FontAttributes, NativeFont> factory)
        {
            var key = new ToNativeFontFontKey(family, size, attributes);

            lock (ToUiFont)
            {
                NativeFont value;
                if (ToUiFont.TryGetValue(key, out value))
                {
                    return value;
                }
            }

            var generatedValue = factory(family, size, attributes);

            lock (ToUiFont)
            {
                NativeFont value;
                if (!ToUiFont.TryGetValue(key, out value))
                {
                    ToUiFont.Add(key, value = generatedValue);
                }

                return value;
            }
        }

        private struct ToNativeFontFontKey
        {
#pragma warning disable 0414 // these are not called explicitly, but they are used to establish uniqueness. allow it!
            private string family;

            internal ToNativeFontFontKey(string family, float size, FontAttributes attributes)
            {
                this.family = family;
                this.size = size;
                this.attributes = attributes;
            }

            private float size;
            private FontAttributes attributes;
#pragma warning restore 0414
        }
    }

    internal static class AttributedStringExtensions
    {
        internal static void SetLineHeight(this NSMutableAttributedString mutableHtmlString, HtmlLabel element)
        {
            if (element.LineHeight < 0)
            {
                return;
            }

            using (var lineHeightStyle = new NSMutableParagraphStyle { LineHeightMultiple = (nfloat)element.LineHeight })
            {
                mutableHtmlString.AddAttribute(UIStringAttributeKey.ParagraphStyle, lineHeightStyle, new NSRange(0, mutableHtmlString.Length));
            }
        }

        internal static void SetLinksStyles(this NSMutableAttributedString mutableHtmlString, HtmlLabel element)
        {
            UIStringAttributes linkAttributes = null;

            if (!element.UnderlineText)
            {
                linkAttributes ??= new UIStringAttributes();
                linkAttributes.UnderlineStyle = NSUnderlineStyle.None;
            }
            if (!element.LinkColor.IsDefault())
            {
                linkAttributes ??= new UIStringAttributes();
                linkAttributes.ForegroundColor = element.LinkColor.ToUIColor();
            }

            mutableHtmlString.EnumerateAttribute(UIStringAttributeKey.Link, new NSRange(0, mutableHtmlString.Length), NSAttributedStringEnumeration.LongestEffectiveRangeNotRequired,
                (NSObject value, NSRange range, ref bool stop) =>
                {
                    if (value != null && value is NSUrl url)
                    {
                        // Applies the style
                        if (linkAttributes != null)
                        {
                            mutableHtmlString.AddAttributes(linkAttributes, range);
                        }
                    }
                });
        }

        internal static NSMutableAttributedString RemoveTrailingNewLines(this NSAttributedString htmlString)
        {
            var count = 0;
            for (int i = 1; i <= htmlString.Length; i++)
            {
                if (htmlString.Substring(htmlString.Length - i, 1).Value != "\n")
                {
                    break;
                }

                count++;
            }

            if (count > 0)
            {
                htmlString = htmlString.Substring(0, htmlString.Length - count);
            }

            return new NSMutableAttributedString(htmlString);
        }

        internal static NSMutableAttributedString AddCharacterSpacing(this NSAttributedString attributedString, string text, double characterSpacing)
        {
            if (attributedString == null && characterSpacing == 0)
            {
                return null;
            }

            NSMutableAttributedString mutableAttributedString = attributedString as NSMutableAttributedString;
            if (attributedString == null || attributedString.Length == 0)
            {
                mutableAttributedString = text == null ? new NSMutableAttributedString() : new NSMutableAttributedString(text);
            }
            else
            {
                mutableAttributedString = new NSMutableAttributedString(attributedString);
            }

            AddKerningAdjustment(mutableAttributedString, text, characterSpacing);

            return mutableAttributedString;
        }

        internal static bool HasCharacterAdjustment(this NSMutableAttributedString mutableAttributedString)
        {
            if (mutableAttributedString == null)
            {
                return false;
            }

            NSRange removalRange;
            var attributes = mutableAttributedString.GetAttributes(0, out removalRange);

            for (uint i = 0; i < attributes.Count; i++)
            {
                if (attributes.Keys[i] is NSString nSString && nSString == UIStringAttributeKey.KerningAdjustment)
                {
                    return true;
                }
            }

            return false;
        }

        internal static void AddKerningAdjustment(NSMutableAttributedString mutableAttributedString, string text, double characterSpacing)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (characterSpacing == 0 && !mutableAttributedString.HasCharacterAdjustment())
                {
                    return;
                }

                mutableAttributedString.AddAttribute(
                    UIStringAttributeKey.KerningAdjustment,
                    NSObject.FromObject(characterSpacing), new NSRange(0, text.Length - 1));
            }
        }

        internal static bool IsHorizontal(this Button.ButtonContentLayout layout) =>
            layout.Position == Button.ButtonContentLayout.ImagePosition.Left ||
            layout.Position == Button.ButtonContentLayout.ImagePosition.Right;
    }

    internal static class AlignmentExtensions
    {
        internal static UITextAlignment ToNativeTextAlignment(this TextAlignment alignment, EffectiveFlowDirection flowDirection)
        {
            var isLtr = flowDirection != EffectiveFlowDirection.RightToLeft;
            switch (alignment)
            {
                case TextAlignment.Center:
                    return UITextAlignment.Center;
                case TextAlignment.End:
                    if (isLtr)
                    {
                        return UITextAlignment.Right;
                    }
                    else
                    {
                        return UITextAlignment.Left;
                    }

                default:
                    if (isLtr)
                    {
                        return UITextAlignment.Left;
                    }
                    else
                    {
                        return UITextAlignment.Right;
                    }
            }
        }

        internal static UIControlContentVerticalAlignment ToNativeTextAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return UIControlContentVerticalAlignment.Center;
                case TextAlignment.End:
                    return UIControlContentVerticalAlignment.Bottom;
                case TextAlignment.Start:
                    return UIControlContentVerticalAlignment.Top;
                default:
                    return UIControlContentVerticalAlignment.Top;
            }
        }
    }
}