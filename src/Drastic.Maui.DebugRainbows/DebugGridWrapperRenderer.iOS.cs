// <copyright file="DebugGridWrapperRenderer.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#nullable disable

using System.ComponentModel;
using CoreAnimation;
using CoreGraphics;
using Drastic.Maui.DebugRainbows;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using UIKit;

[assembly: ExportRenderer(typeof(DebugGridWrapper), typeof(DebugGridWrapperRendereriOS))]

namespace Drastic.Maui.DebugRainbows
{
    public class DebugGridWrapperRendereriOS : ViewRenderer<DebugGridWrapper, UIView>
    {
        private UIView contentView;

        protected override void OnElementChanged(ElementChangedEventArgs<DebugGridWrapper> e)
        {
            if (e.NewElement != null)
            {
                if (this.Control == null)
                {
                    var grid = e.NewElement as DebugGridWrapper;

                    this.SetNativeControl(new DebugGridViewiOS()
                    {
                        HorizontalItemSize = grid.HorizontalItemSize,
                        VerticalItemSize = grid.VerticalItemSize,
                        MajorGridLineInterval = grid.MajorGridLineInterval,
                        MajorGridLineColor = grid.MajorGridLineColor,
                        GridLineColor = grid.GridLineColor,
                        MajorGridLineOpacity = grid.MajorGridLineOpacity,
                        GridLineOpacity = grid.GridLineOpacity,
                        MajorGridLineThickness = grid.MajorGridLineWidth,
                        GridLineThickness = grid.GridLineWidth,
                        MakeGridRainbows = grid.MakeGridRainbows,
                        Inverse = grid.Inverse,
                        GridOrigin = grid.GridOrigin,
                    });
                }
            }

            base.OnElementChanged(e);
        }
    }

    public class DebugGridViewiOS : UIView
    {
        private CALayer gridLayer;
        private CALayer majorGridLayer;

        private CGColor[] rainbowColors =
        {
                        Color.FromHex("#f3855b").ToCGColor(),
                        Color.FromHex("#fbcf93").ToCGColor(),
                        Color.FromHex("#fbe960").ToCGColor(),
                        Color.FromHex("#a0e67a").ToCGColor(),
                        Color.FromHex("#33c6ee").ToCGColor(),
                        Color.FromHex("#c652ba").ToCGColor(),
                        Color.FromHex("#ef53b2").ToCGColor(),
                    };

        public DebugGridViewiOS()
        {
            this.BackgroundColor = UIColor.Clear;
            this.ContentMode = UIViewContentMode.Redraw;
        }

        public double HorizontalItemSize { get; set; }

        public double VerticalItemSize { get; set; }

        public int MajorGridLineInterval { get; set; }

        public Color MajorGridLineColor { get; set; }

        public Color GridLineColor { get; set; }

        public double MajorGridLineOpacity { get; set; }

        public double GridLineOpacity { get; set; }

        public double MajorGridLineThickness { get; set; }

        public double GridLineThickness { get; set; }

        public bool MakeGridRainbows { get; set; }

        public bool Inverse { get; set; }

        public DebugGridOrigin GridOrigin { get; set; }

        public override void Draw(CGRect rect)
        {
            this.DrawGrid(rect);
        }

        private void DrawGrid(CGRect rect)
        {
            if (this.Inverse)
            {
                this.DrawInverseGridLayer(rect);
            }
            else
            {
                this.DrawNormalGridLayer(this.gridLayer, false);
                this.DrawNormalGridLayer(this.majorGridLayer, true);
            }
        }

        private void DrawInverseGridLayer(CGRect rect)
        {
            var context = UIGraphics.GetCurrentContext();

            context.SetFillColor(this.GridLineColor.ToCGColor());
            context.SetAlpha((nfloat)this.GridLineOpacity);

            if (this.GridOrigin == DebugGridOrigin.TopLeft)
            {
                var horizontalTotal = 0;

                for (int i = 1; horizontalTotal < this.Bounds.Size.Width; i++)
                {
                    var verticalTotal = 0;
                    var horizontalSpacerSize = this.MajorGridLineInterval > 0 && i % this.MajorGridLineInterval == 0 ? this.MajorGridLineThickness : this.GridLineThickness;

                    for (int j = 1; verticalTotal < this.Bounds.Size.Height; j++)
                    {
                        var verticalSpacerSize = this.MajorGridLineInterval > 0 && j % this.MajorGridLineInterval == 0 ? this.MajorGridLineThickness : this.GridLineThickness;
                        var rectangle = new CGRect(horizontalTotal, verticalTotal, this.HorizontalItemSize, this.VerticalItemSize);

                        if (this.MakeGridRainbows)
                        {
                            var color = this.rainbowColors[(i + j) % this.rainbowColors.Length];
                            context.SetFillColor(color);
                        }

                        context.FillRect(rectangle);

                        verticalTotal += (int)(this.VerticalItemSize + verticalSpacerSize);
                    }

                    horizontalTotal += (int)(this.HorizontalItemSize + horizontalSpacerSize);
                }
            }
            else if (this.GridOrigin == DebugGridOrigin.Center)
            {
                var horizontalRightTotal = (this.Bounds.Size.Width / 2) + ((this.MajorGridLineInterval > 0 ? this.MajorGridLineThickness : this.GridLineThickness) / 2);
                var horizontalLeftTotal = (this.Bounds.Size.Width / 2) - (int)(this.HorizontalItemSize + ((this.MajorGridLineInterval > 0 ? this.MajorGridLineThickness : this.GridLineThickness) / 2));

                for (int i = 1; horizontalRightTotal < this.Bounds.Size.Width; i++)
                {
                    var horizontalSpacerSize = this.MajorGridLineInterval > 0 && i % this.MajorGridLineInterval == 0 ? this.MajorGridLineThickness : this.GridLineThickness;
                    var verticalBottomTotal = (this.Bounds.Size.Height / 2) + ((this.MajorGridLineInterval > 0 ? this.MajorGridLineThickness : this.GridLineThickness) / 2);
                    var verticalTopTotal = (this.Bounds.Size.Height / 2) - (int)(this.VerticalItemSize + ((this.MajorGridLineInterval > 0 ? this.MajorGridLineThickness : this.GridLineThickness) / 2));

                    for (int j = 1; verticalBottomTotal < this.Bounds.Size.Height; j++)
                    {
                        if (this.MakeGridRainbows)
                        {
                            var color = this.rainbowColors[(i + j) % this.rainbowColors.Length];
                            context.SetFillColor(color);
                        }

                        var verticalSpacerSize = this.MajorGridLineInterval > 0 && j % this.MajorGridLineInterval == 0 ? this.MajorGridLineThickness : this.GridLineThickness;

                        var rectangle = new CGRect(horizontalRightTotal, verticalBottomTotal, this.HorizontalItemSize, this.VerticalItemSize);
                        context.FillRect(rectangle);

                        var rectangle2 = new CGRect(horizontalLeftTotal, verticalTopTotal, this.HorizontalItemSize, this.VerticalItemSize);
                        context.FillRect(rectangle2);

                        var rectangle3 = new CGRect(horizontalRightTotal, verticalTopTotal, this.HorizontalItemSize, this.VerticalItemSize);
                        context.FillRect(rectangle3);

                        var rectangle4 = new CGRect(horizontalLeftTotal, verticalBottomTotal, this.HorizontalItemSize, this.VerticalItemSize);
                        context.FillRect(rectangle4);

                        verticalTopTotal -= (int)(this.VerticalItemSize + verticalSpacerSize);
                        verticalBottomTotal += (int)(this.VerticalItemSize + verticalSpacerSize);
                    }

                    horizontalRightTotal += (int)(this.HorizontalItemSize + horizontalSpacerSize);
                    horizontalLeftTotal -= (int)(this.HorizontalItemSize + horizontalSpacerSize);
                }
            }
        }

        private void DrawNormalGridLayer(CALayer layer, bool isMajor)
        {
            if (isMajor && this.MajorGridLineInterval == 0)
            {
                return;
            }

            using (var path = this.CreatePath(isMajor ? this.MajorGridLineInterval : 0))
            {
                layer = new CAShapeLayer
                {
                    LineWidth = isMajor ? (nfloat)this.MajorGridLineThickness : (nfloat)this.GridLineThickness,
                    Path = path.CGPath,
                    StrokeColor = isMajor ? this.MajorGridLineColor.ToCGColor() : this.GridLineColor.ToCGColor(),
                    Opacity = isMajor ? (float)this.MajorGridLineOpacity : (float)this.GridLineOpacity,
                    Frame = new CGRect(0, 0, this.Bounds.Size.Width, this.Bounds.Size.Height),
                };

                if (!this.MakeGridRainbows)
                {
                    this.Layer.AddSublayer(layer);
                }
                else
                {
                    var gradientLayer = new CAGradientLayer
                    {
                        StartPoint = new CGPoint(0.5, 0.0),
                        EndPoint = new CGPoint(0.5, 1.0),
                        Frame = new CGRect(0, 0, this.Bounds.Size.Width, this.Bounds.Size.Height),
                        Colors = this.rainbowColors,
                        Mask = layer,
                    };

                    this.Layer.AddSublayer(gradientLayer);
                }
            }
        }

        private UIBezierPath CreatePath(int interval = 0)
        {
            var path = new UIBezierPath();
            var gridLinesHorizontal = this.Bounds.Width / this.HorizontalItemSize;
            var gridLinesVertical = this.Bounds.Height / this.VerticalItemSize;

            if (this.GridOrigin == DebugGridOrigin.TopLeft)
            {
                for (int i = 0; i < gridLinesHorizontal; i++)
                {
                    if (interval == 0 || i % interval == 0)
                    {
                        var start = new CGPoint(x: (nfloat)i * this.HorizontalItemSize, y: 0);
                        var end = new CGPoint(x: (nfloat)i * this.HorizontalItemSize, y: this.Bounds.Height);
                        path.MoveTo(start);
                        path.AddLineTo(end);
                    }
                }

                for (int i = 0; i < gridLinesVertical; i++)
                {
                    if (interval == 0 || i % interval == 0)
                    {
                        var start = new CGPoint(x: 0, y: (nfloat)i * this.VerticalItemSize);
                        var end = new CGPoint(x: this.Bounds.Width, y: (nfloat)i * this.VerticalItemSize);
                        path.MoveTo(start);
                        path.AddLineTo(end);
                    }
                }

                path.ClosePath();
            }
            else if (this.GridOrigin == DebugGridOrigin.Center)
            {
                var gridLinesHorizontalCenter = this.Bounds.Width / 2;
                var gridLinesVerticalCenter = this.Bounds.Height / 2;

                for (int i = 0; i < (gridLinesHorizontal / 2); i++)
                {
                    if (interval == 0 || i % interval == 0)
                    {
                        var startRight = new CGPoint(x: gridLinesHorizontalCenter + ((nfloat)i * this.HorizontalItemSize), y: 0);
                        var endRight = new CGPoint(x: gridLinesHorizontalCenter + ((nfloat)i * this.HorizontalItemSize), y: this.Bounds.Height);
                        path.MoveTo(startRight);
                        path.AddLineTo(endRight);

                        var startLeft = new CGPoint(x: gridLinesHorizontalCenter - ((nfloat)i * this.HorizontalItemSize), y: 0);
                        var endLeft = new CGPoint(x: gridLinesHorizontalCenter - ((nfloat)i * this.HorizontalItemSize), y: this.Bounds.Height);
                        path.MoveTo(startLeft);
                        path.AddLineTo(endLeft);
                    }
                }

                for (int i = 0; i < (gridLinesVertical / 2); i++)
                {
                    if (interval == 0 || i % interval == 0)
                    {
                        var startBottom = new CGPoint(x: 0, y: gridLinesVerticalCenter + ((nfloat)i * this.VerticalItemSize));
                        var endBottom = new CGPoint(x: this.Bounds.Width, y: gridLinesVerticalCenter + ((nfloat)i * this.VerticalItemSize));
                        path.MoveTo(startBottom);
                        path.AddLineTo(endBottom);

                        var startTop = new CGPoint(x: 0, y: gridLinesVerticalCenter - ((nfloat)i * this.VerticalItemSize));
                        var endTop = new CGPoint(x: this.Bounds.Width, y: gridLinesVerticalCenter - ((nfloat)i * this.VerticalItemSize));
                        path.MoveTo(startTop);
                        path.AddLineTo(endTop);
                    }
                }
            }

            return path;
        }
    }
}
