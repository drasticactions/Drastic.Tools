// <copyright file="Renderer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Linq;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Widget;
using Drastic.HtmlLabel.Maui;
using Java.Lang;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]

namespace Drastic.HtmlLabel.Maui
{
    /// <summary>
    /// HtmlLable Implementation.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class HtmlLabelRenderer : LabelRenderer
    {
        private const string tagUlRegex = "[uU][lL]";
        private const string tagOlRegex = "[oO][lL]";
        private const string tagLiRegex = "[lL][iI]";

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlLabelRenderer"/> class.
        /// Create an instance of the renderer.
        /// </summary>
        /// <param name="context"></param>
        public HtmlLabelRenderer(Android.Content.Context context)
            : base(context)
        {
        }

        /// <summary>
        /// Used for registration with dependency service.
        /// </summary>
        public static void Initialize()
        {
        }

        /// <inheritdoc />
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e == null || this.Element == null)
            {
                return;
            }

            try
            {
                this.ProcessText();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"            ERROR: ", ex.Message);
            }
        }

        /// <inheritdoc />
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e != null && RendererHelper.RequireProcess(e.PropertyName))
            {
                try
                {
                    this.ProcessText();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(@"            ERROR: ", ex.Message);
                }
            }
        }

        private static ISpanned RemoveTrailingNewLines(ICharSequence text)
        {
            var builder = new SpannableStringBuilder(text);

            var count = 0;
            for (int i = 1; i <= text.Length(); i++)
            {
                if (!'\n'.Equals(text.CharAt(text.Length() - i)))
                {
                    break;
                }

                count++;
            }

            if (count > 0)
            {
                _ = builder.Delete(text.Length() - count, text.Length());
            }

            return builder;
        }

        private void ProcessText()
        {
            if (this.Control == null || this.Element == null)
            {
                return;
            }

            Color linkColor = ((HtmlLabel)this.Element).LinkColor;
            if (!linkColor.IsDefault())
            {
                this.Control.SetLinkTextColor(linkColor.ToAndroid());
            }

            this.Control.SetIncludeFontPadding(false);
            var isRtl = Device.FlowDirection == FlowDirection.RightToLeft;
            var styledHtml = new RendererHelper(this.Element, this.Element.Text, Device.RuntimePlatform, isRtl).ToString();
            /*
             * Android's TextView doesn't support lists.
             * List tags must be replaces with custom tags,
             * that it will be renderer by a custom tag handler.
             */
            styledHtml = styledHtml
                ?.ReplaceTag(tagUlRegex, ListTagHandler.TagUl)
                ?.ReplaceTag(tagOlRegex, ListTagHandler.TagOl)
                ?.ReplaceTag(tagLiRegex, ListTagHandler.TagLi);

            if (styledHtml != null)
            {
                this.SetText(this.Control, styledHtml);
            }
        }

        private void SetText(TextView control, string html)
        {
            var htmlLabel = (HtmlLabel)this.Element;

            // Set the type of content and the custom tag list handler
            using var listTagHandler = new ListTagHandler(htmlLabel.AndroidListIndent); // KWI-FIX: added AndroidListIndent parameter
            var imageGetter = new UrlImageParser(this.Control);
            FromHtmlOptions fromHtmlOptions = htmlLabel.AndroidLegacyMode ? FromHtmlOptions.ModeLegacy : FromHtmlOptions.ModeCompact;
            ISpanned sequence = Build.VERSION.SdkInt >= BuildVersionCodes.N ?
                Html.FromHtml(html, fromHtmlOptions, imageGetter, listTagHandler) :
                Html.FromHtml(html, imageGetter, listTagHandler);
            using var strBuilder = new SpannableStringBuilder(sequence);

            // Make clickable links
            if (!this.Element.GestureRecognizers.Any())
            {
                control.MovementMethod = LinkMovementMethod.Instance;
                URLSpan[] urls = strBuilder
                    .GetSpans(0, sequence.Length(), Class.FromType(typeof(URLSpan)))
                    .Cast<URLSpan>()
                    .ToArray();
                foreach (URLSpan span in urls)
                {
                    this.MakeLinkClickable(strBuilder, span);
                }
            }

            // Android adds an unnecessary "\n" that must be removed
            using ISpanned value = RemoveTrailingNewLines(strBuilder);

            // Finally sets the value of the TextView
            control.SetText(value, TextView.BufferType.Spannable);
        }

        private void MakeLinkClickable(ISpannable strBuilder, URLSpan span)
        {
            var start = strBuilder.GetSpanStart(span);
            var end = strBuilder.GetSpanEnd(span);
            SpanTypes flags = strBuilder.GetSpanFlags(span);
            var clickable = new HtmlLabelClickableSpan((HtmlLabel)this.Element, span);
            strBuilder.SetSpan(clickable, start, end, flags);
            strBuilder.RemoveSpan(span);
        }

        private class HtmlLabelClickableSpan : ClickableSpan
        {
            private readonly HtmlLabel label;
            private readonly URLSpan span;

            public HtmlLabelClickableSpan(HtmlLabel label, URLSpan span)
            {
                this.label = label;
                this.span = span;
            }

            public override void UpdateDrawState(TextPaint ds)
            {
                base.UpdateDrawState(ds);
                ds.UnderlineText = this.label.UnderlineText;
            }

            public override void OnClick(Android.Views.View widget)
            {
                RendererHelper.HandleUriClick(this.label, this.span.URL);
            }
        }
    }
}
