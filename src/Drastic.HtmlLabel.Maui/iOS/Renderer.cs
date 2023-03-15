// <copyright file="Renderer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.HtmlLabel.Maui;
using Foundation;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Drastic.HtmlLabel.Maui;
using Foundation;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]

namespace Drastic.HtmlLabel.Maui
{
    /// <summary>
    /// HtmlLabel Implementation.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class HtmlLabelRenderer : BaseTextViewRenderer<HtmlLabel>
    {
        /// <summary>
        /// Used for registration with dependency service.
        /// </summary>
        public static void Initialize()
        {
        }

        protected override bool NavigateToUrl(NSUrl url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            // Try to handle uri, if it can't be handled, fall back to IOS his own handler.
            return !RendererHelper.HandleUriClick(this.Element, url.AbsoluteString);
        }

        protected override void ProcessText()
        {
            if (string.IsNullOrWhiteSpace(this.Element?.Text))
            {
                this.Control.Text = string.Empty;
                return;
            }

            this.Control.Font = this.Element.ToUIFont();
            if (!this.Element.TextColor.IsDefault())
            {
                this.Control.TextColor = this.Element.TextColor.ToUIColor();
            }

            var linkColor = this.Element.LinkColor;
            if (!linkColor.IsDefault())
            {
                this.Control.TintColor = linkColor.ToUIColor();
            }

            var isRtl = Device.FlowDirection == FlowDirection.RightToLeft;
            var styledHtml = new RendererHelper(this.Element, this.Element.Text, Device.RuntimePlatform, isRtl).ToString();
            this.SetText(styledHtml);
            this.SetNeedsDisplay();
        }

        private void SetText(string html)
        {
            // Create HTML data sting
            var stringType = new NSAttributedStringDocumentAttributes
            {
                DocumentType = NSDocumentType.HTML,
            };
            var nsError = new NSError();

            var htmlData = NSData.FromString(html, NSStringEncoding.Unicode);

            using var htmlString = new NSAttributedString(htmlData, stringType, out _, ref nsError);
            var mutableHtmlString = htmlString.RemoveTrailingNewLines();

            mutableHtmlString.EnumerateAttributes(new NSRange(0, mutableHtmlString.Length), NSAttributedStringEnumeration.None,
                (NSDictionary value, NSRange range, ref bool stop) =>
                {
                    var md = new NSMutableDictionary(value);
                    var font = md[UIStringAttributeKey.Font] as UIFont;

                    if (font != null)
                    {
                        md[UIStringAttributeKey.Font] = this.Control.Font.WithTraitsOfFont(font);
                    }
                    else
                    {
                        md[UIStringAttributeKey.Font] = this.Control.Font;
                    }

                    var foregroundColor = md[UIStringAttributeKey.ForegroundColor] as UIColor;
                    if (foregroundColor == null || foregroundColor.IsEqualToColor(UIColor.Black))
                    {
                        md[UIStringAttributeKey.ForegroundColor] = this.Control.TextColor;
                    }

                    mutableHtmlString.SetAttributes(md, range);
                });

            mutableHtmlString.SetLineHeight(this.Element);
            mutableHtmlString.SetLinksStyles(this.Element);
            this.Control.AttributedText = mutableHtmlString;
        }
    }
}