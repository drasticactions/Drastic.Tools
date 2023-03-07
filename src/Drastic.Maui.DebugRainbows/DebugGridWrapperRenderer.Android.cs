// <copyright file="DebugGridWrapperRenderer.Android.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#nullable disable

using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Drastic.Maui.DebugRainbows;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using AView = Android.Views.View;
using Color = Microsoft.Maui.Graphics.Color;

[assembly: ExportRenderer(typeof(DebugGridWrapper), typeof(DebugGridWrapperRendererDroid))]

namespace Drastic.Maui.DebugRainbows
{
    public class DebugGridWrapperRendererDroid : ViewRenderer<DebugGridWrapper, AView>
    {
        public DebugGridWrapperRendererDroid(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DebugGridWrapper> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var grid = e.NewElement as DebugGridWrapper;

                this.SetNativeControl(new DebugGridViewDroid(this.Context)
                {
                    HorizontalItemSize = (float)grid.HorizontalItemSize,
                    VerticalItemSize = (float)grid.VerticalItemSize,
                    MajorGridLineInterval = grid.MajorGridLineInterval,
                    MajorGridLineColor = grid.MajorGridLineColor,
                    GridLineColor = grid.GridLineColor,
                    MajorGridLineOpacity = (float)grid.MajorGridLineOpacity,
                    GridLineOpacity = (float)grid.GridLineOpacity,
                    MajorGridLineThickness = (float)grid.MajorGridLineWidth,
                    GridLineThickness = (float)grid.GridLineWidth,
                    MakeGridRainbows = grid.MakeGridRainbows,
                    Inverse = grid.Inverse,
                    GridOrigin = grid.GridOrigin,
                });
            }
        }
    }

    public class DebugGridViewDroid : AView
    {
        private int screenWidth;
        private int screenHeight;

        public DebugGridViewDroid(Context context)
            : base(context)
        {
            this.Init();
        }

        public float HorizontalItemSize { get; set; }

        public float VerticalItemSize { get; set; }

        public int MajorGridLineInterval { get; set; }

        public Color MajorGridLineColor { get; set; }

        public Color GridLineColor { get; set; }

        public float MajorGridLineOpacity { get; set; }

        public float GridLineOpacity { get; set; }

        public float MajorGridLineThickness { get; set; }

        public float GridLineThickness { get; set; }

        public bool MakeGridRainbows { get; set; }

        public bool Inverse { get; set; }

        public DebugGridOrigin GridOrigin { get; set; }

        public static float ConvertDpToPixel(float dp, Context context)
        {
            return dp * ((float)context.Resources.DisplayMetrics.DensityDpi / (int)DisplayMetricsDensity.Default);
        }

        public void Init()
        {
            this.GetScreenDimensions();
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);
            this.GetScreenDimensions();
        }

        private void GetScreenDimensions()
        {
            DisplayMetrics displayMetrics = new DisplayMetrics();
            ((Activity)this.Context).WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            this.screenWidth = displayMetrics.WidthPixels;
            this.screenHeight = displayMetrics.HeightPixels;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            var majorPaint = new Android.Graphics.Paint();
            var minorPaint = new Android.Graphics.Paint();

            var colors = new[]
            {
                Color.FromHex("#f3855b").ToAndroid(),
                Color.FromHex("#fbcf93").ToAndroid(),
                Color.FromHex("#fbe960").ToAndroid(),
                Color.FromHex("#a0e67a").ToAndroid(),
                Color.FromHex("#33c6ee").ToAndroid(),
                Color.FromHex("#c652ba").ToAndroid(),
            };

            // Make these into true pixels from DP.
            this.HorizontalItemSize = ConvertDpToPixel(this.HorizontalItemSize, this.Context);
            this.VerticalItemSize = ConvertDpToPixel(this.VerticalItemSize, this.Context);
            this.MajorGridLineThickness = ConvertDpToPixel(this.MajorGridLineThickness, this.Context);
            this.GridLineThickness = ConvertDpToPixel(this.GridLineThickness, this.Context);

            if (this.Inverse)
            {
                this.DrawInverse(canvas, majorPaint, colors);
            }
            else
            {
                if (this.MakeGridRainbows)
                {
                    var a = canvas.Width * Math.Pow(Math.Sin(2 * Math.PI * ((90 + 0.75) / 2)), 2);
                    var b = canvas.Height * Math.Pow(Math.Sin(2 * Math.PI * ((90 + 0.0) / 2)), 2);
                    var c = canvas.Width * Math.Pow(Math.Sin(2 * Math.PI * ((90 + 0.25) / 2)), 2);
                    var d = canvas.Height * Math.Pow(Math.Sin(2 * Math.PI * ((90 + 0.5) / 2)), 2);

                    var locations = new float[] { 0, 0.2f, 0.4f, 0.6f, 0.8f, 1 };
                    var shader = new LinearGradient(canvas.Width - (float)a, (float)b, canvas.Width - (float)c, (float)d, colors.Select(x => (int)x.ToArgb()).ToArray(), locations, Shader.TileMode.Clamp);

                    minorPaint.SetShader(shader);
                    majorPaint.SetShader(shader);
                }

                this.DrawNormal(canvas, majorPaint, minorPaint);
            }
        }

        private void DrawNormal(Canvas canvas, Android.Graphics.Paint majorPaint, Android.Graphics.Paint minorPaint)
        {
            majorPaint.StrokeWidth = this.MajorGridLineThickness;
            majorPaint.Color = this.MajorGridLineColor.ToAndroid();
            majorPaint.Alpha = (int)(255 * this.MajorGridLineOpacity);

            minorPaint.StrokeWidth = this.GridLineThickness;
            minorPaint.Color = this.GridLineColor.ToAndroid();
            minorPaint.Alpha = (int)(255 * this.GridLineOpacity);

            if (this.GridOrigin == DebugGridOrigin.TopLeft)
            {
                float verticalPosition = 0;
                int i = 0;
                while (verticalPosition <= this.screenHeight)
                {
                    canvas.DrawLine(0, verticalPosition, this.screenWidth, verticalPosition, this.MajorGridLineInterval > 0 && i % this.MajorGridLineInterval == 0 ? majorPaint : minorPaint);
                    verticalPosition += this.VerticalItemSize;
                    i++;
                }

                float horizontalPosition = 0;
                i = 0;
                while (horizontalPosition <= this.screenWidth)
                {
                    canvas.DrawLine(horizontalPosition, 0, horizontalPosition, this.screenHeight, this.MajorGridLineInterval > 0 && i % this.MajorGridLineInterval == 0 ? majorPaint : minorPaint);
                    horizontalPosition += this.HorizontalItemSize;
                    i++;
                }
            }
            else if (this.GridOrigin == DebugGridOrigin.Center)
            {
                var gridLinesHorizontalCenter = this.screenWidth / 2;
                var gridLinesVerticalCenter = this.screenHeight / 2;
                var amountOfVerticalLines = this.screenWidth / this.HorizontalItemSize;
                var amountOfHorizontalLines = this.screenHeight / this.VerticalItemSize;

                // Draw the horizontal lines.
                for (int i = 0; i < (amountOfHorizontalLines / 2); i++)
                {
                    canvas.DrawLine(
                        startX: 0,
                        startY: gridLinesVerticalCenter + (i * this.VerticalItemSize),
                        stopX: this.screenWidth,
                        stopY: gridLinesVerticalCenter + (i * this.VerticalItemSize),
                        paint: this.MajorGridLineInterval > 0 && i % this.MajorGridLineInterval == 0 ? majorPaint : minorPaint);

                    canvas.DrawLine(
                        startX: 0,
                        startY: gridLinesVerticalCenter - (i * this.VerticalItemSize),
                        stopX: this.screenWidth,
                        stopY: gridLinesVerticalCenter - (i * this.VerticalItemSize),
                        paint: this.MajorGridLineInterval > 0 && i % this.MajorGridLineInterval == 0 ? majorPaint : minorPaint);
                }

                // Draw vertical lines.
                for (int i = 0; i < (amountOfVerticalLines / 2); i++)
                {
                    canvas.DrawLine(
                        startX: gridLinesHorizontalCenter + (i * this.HorizontalItemSize),
                        startY: 0,
                        stopX: gridLinesHorizontalCenter + (i * this.HorizontalItemSize),
                        stopY: this.screenHeight,
                        paint: this.MajorGridLineInterval > 0 && i % this.MajorGridLineInterval == 0 ? majorPaint : minorPaint);

                    canvas.DrawLine(
                        startX: gridLinesHorizontalCenter - (i * this.HorizontalItemSize),
                        startY: 0,
                        stopX: gridLinesHorizontalCenter - (i * this.HorizontalItemSize),
                        stopY: this.screenHeight,
                        paint: this.MajorGridLineInterval > 0 && i % this.MajorGridLineInterval == 0 ? majorPaint : minorPaint);
                }
            }
        }

        private void DrawInverse(Canvas canvas, Android.Graphics.Paint majorPaint, global::Android.Graphics.Color[] colors)
        {
            majorPaint.StrokeWidth = 0;
            majorPaint.Color = this.GridLineColor.ToAndroid();
            majorPaint.Alpha = (int)(255 * this.GridLineOpacity);

            if (this.GridOrigin == DebugGridOrigin.TopLeft)
            {
                var horizontalTotal = 0;
                for (int i = 1; horizontalTotal < this.screenWidth; i++)
                {
                    var verticalTotal = 0;
                    var horizontalSpacerSize = this.MajorGridLineInterval > 0 && i % this.MajorGridLineInterval == 0 ? this.MajorGridLineThickness : this.GridLineThickness;

                    for (int j = 1; verticalTotal < this.screenHeight; j++)
                    {
                        var verticalSpacerSize = this.MajorGridLineInterval > 0 && j % this.MajorGridLineInterval == 0 ? this.MajorGridLineThickness : this.GridLineThickness;

                        var rectangle = new Android.Graphics.Rect(
                            (int)horizontalTotal,
                            (int)verticalTotal,
                            (int)(horizontalTotal + this.HorizontalItemSize),
                            (int)(verticalTotal + this.VerticalItemSize));

                        if (this.MakeGridRainbows)
                        {
                            var color = colors[(i + j) % colors.Length];
                            majorPaint.Color = color;
                        }

                        canvas.DrawRect(rectangle, majorPaint);

                        verticalTotal += (int)(this.VerticalItemSize + verticalSpacerSize);
                    }

                    horizontalTotal += (int)(this.HorizontalItemSize + horizontalSpacerSize);
                }
            }
            else if (this.GridOrigin == DebugGridOrigin.Center)
            {
                var horizontalRightTotal = (this.screenWidth / 2) + (int)((this.MajorGridLineInterval > 0 ? this.MajorGridLineThickness : this.GridLineThickness) / 2);
                var horizontalLeftTotal = (this.screenWidth / 2) - (int)(this.HorizontalItemSize + ((this.MajorGridLineInterval > 0 ? this.MajorGridLineThickness : this.GridLineThickness) / 2));

                for (int i = 1; horizontalRightTotal < this.screenWidth; i++)
                {
                    var horizontalSpacerSize = this.MajorGridLineInterval > 0 && i % this.MajorGridLineInterval == 0 ? this.MajorGridLineThickness : this.GridLineThickness;
                    var verticalBottomTotal = (this.screenHeight / 2) + (int)((this.MajorGridLineInterval > 0 ? this.MajorGridLineThickness : this.GridLineThickness) / 2);
                    var verticalTopTotal = (this.screenHeight / 2) - (int)(this.VerticalItemSize + ((this.MajorGridLineInterval > 0 ? this.MajorGridLineThickness : this.GridLineThickness) / 2));

                    for (int j = 1; verticalBottomTotal < this.screenHeight; j++)
                    {
                        if (this.MakeGridRainbows)
                        {
                            var color = colors[(i + j) % colors.Length];
                            majorPaint.Color = color;
                        }

                        var verticalSpacerSize = this.MajorGridLineInterval > 0 && j % this.MajorGridLineInterval == 0 ? this.MajorGridLineThickness : this.GridLineThickness;

                        var rectangle = new Android.Graphics.Rect(horizontalRightTotal, verticalBottomTotal, (int)(horizontalRightTotal + this.HorizontalItemSize), (int)(verticalBottomTotal + this.VerticalItemSize));
                        canvas.DrawRect(rectangle, majorPaint);

                        var rectangle2 = new Android.Graphics.Rect(horizontalLeftTotal, verticalTopTotal, (int)(horizontalLeftTotal + this.HorizontalItemSize), (int)(verticalTopTotal + this.VerticalItemSize));
                        canvas.DrawRect(rectangle2, majorPaint);

                        var rectangle3 = new Android.Graphics.Rect(horizontalRightTotal, verticalTopTotal, (int)(horizontalRightTotal + this.HorizontalItemSize), (int)(verticalTopTotal + this.VerticalItemSize));
                        canvas.DrawRect(rectangle3, majorPaint);

                        var rectangle4 = new Android.Graphics.Rect(horizontalLeftTotal, verticalBottomTotal, (int)(horizontalLeftTotal + this.HorizontalItemSize), (int)(verticalBottomTotal + this.VerticalItemSize));
                        canvas.DrawRect(rectangle4, majorPaint);

                        verticalTopTotal -= (int)(this.VerticalItemSize + verticalSpacerSize);
                        verticalBottomTotal += (int)(this.VerticalItemSize + verticalSpacerSize);
                    }

                    horizontalRightTotal += (int)(this.HorizontalItemSize + horizontalSpacerSize);
                    horizontalLeftTotal -= (int)(this.HorizontalItemSize + horizontalSpacerSize);
                }
            }
        }
    }
}
