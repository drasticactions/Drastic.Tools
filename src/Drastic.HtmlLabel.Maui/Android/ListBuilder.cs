// <copyright file="ListBuilder.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Android.Text;
using Android.Text.Style;
using Android.Widget;
using Drastic.HtmlLabel.Maui;
using Java.Lang;
using Microsoft.Maui.Controls.Compatibility;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]

namespace Drastic.HtmlLabel.Maui
{
    internal class ListBuilder
    {
        private readonly int gap = 0;

        private int listIndent = 20; // KWI-FIX : changed from constant to prop
        private readonly LiGap liGap;
        private readonly ListBuilder parent = null;

        private int liIndex = -1;
        private int liStart = -1;

        public ListBuilder(int listIndent) // KWI-FIX: added listIndent
        {
            this.listIndent = listIndent;
            this.parent = null;
            this.gap = 0;
            this.liGap = GetLiGap(null);
        }

        private ListBuilder(ListBuilder parent, bool ordered, int listIndent) // KWI-FIX: added listIndent
        {
            this.listIndent = listIndent;
            this.parent = parent;
            this.liGap = parent.liGap;
            this.gap = parent.gap + this.listIndent + this.liGap.GetGap(ordered);
            this.liIndex = ordered ? 0 : -1;
        }

        public ListBuilder StartList(bool ordered, IEditable output)
        {
            if (this.parent == null && output.Length() > 0)
            {
                _ = output.Append("\n ");
            }

            return new ListBuilder(this, ordered, this.listIndent); // KWI-FIX: pass thru listIndent
        }

        public void AddListItem(bool isOpening, IEditable output)
        {
            if (isOpening)
            {
                EnsureParagraphBoundary(output);
                this.liStart = output.Length();

                var lineStart = this.IsOrdered()
                    ? ++this.liIndex + ". "
                    : "•  ";
                _ = output.Append(lineStart);
            }
            else
            {
                if (this.liStart >= 0)
                {
                    EnsureParagraphBoundary(output);
                    using var leadingMarginSpan = new LeadingMarginSpanStandard(this.gap - this.liGap.GetGap(this.IsOrdered()), this.gap);
                    output.SetSpan(leadingMarginSpan, this.liStart, output.Length(), SpanTypes.ExclusiveExclusive);
                    this.liStart = -1;
                }
            }
        }

        public ListBuilder CloseList(IEditable output)
        {
            EnsureParagraphBoundary(output);
            ListBuilder result = this.parent;
            if (result == null)
            {
                result = this;
            }

            if (result.parent == null)
            {
                _ = output.Append('\n');
            }

            return result;
        }

        private static void EnsureParagraphBoundary(IEditable output)
        {
            if (output.Length() == 0)
            {
                return;
            }

            var lastChar = output.CharAt(output.Length() - 1);
            if (lastChar != '\n')
            {
                _ = output.Append('\n');
            }
        }

        private static LiGap GetLiGap(TextView tv)
        {
            var orderedGap = tv == null ? 40 : ComputeWidth(tv, true);
            var unorderedGap = tv == null ? 30 : ComputeWidth(tv, false);

            return new LiGap(orderedGap, unorderedGap);
        }

        private bool IsOrdered()
        {
            return this.liIndex >= 0;
        }

        private class LiGap
        {
            private readonly int orderedGap;
            private readonly int unorderedGap;

            internal LiGap(int orderedGap, int unorderedGap)
            {
                this.orderedGap = orderedGap;
                this.unorderedGap = unorderedGap;
            }

            public int GetGap(bool ordered)
            {
                return ordered ? this.orderedGap : this.unorderedGap;
            }
        }

        private static int ComputeWidth(TextView tv, bool isOrdered)
        {
            Android.Graphics.Paint paint = tv.Paint;
            using var bounds = new Android.Graphics.Rect();
            var startString = isOrdered ? "99. " : "• ";
            paint.GetTextBounds(startString, 0, startString.Length, bounds);
            var width = bounds.Width();
            var pt = Android.Util.TypedValue.ApplyDimension(Android.Util.ComplexUnitType.Pt, width, tv.Context.Resources.DisplayMetrics);
            return (int)pt;
        }
    }
}
