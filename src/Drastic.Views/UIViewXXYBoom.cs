// <copyright file="UIViewXXYBoom.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using CoreAnimation;
using ObjCRuntime;

#nullable disable

namespace Drastic.Views
{
    public class UIViewXXYBoom : UIView
    {
        private Random random = new Random();

        public UIViewXXYBoom()
        {
        }

        private void ScaleOpacityAnimations()
        {
            var scaleAnimation = CABasicAnimation.FromKeyPath("transform.scale");
            scaleAnimation.To = new NSNumber(0.01);
            scaleAnimation.Duration = 0.15;
            scaleAnimation.FillMode = CAFillMode.Forwards;

            var opacityAnimation = CABasicAnimation.FromKeyPath("opacity");
            opacityAnimation.From = new NSNumber(1);
            opacityAnimation.To = new NSNumber(0);
            opacityAnimation.Duration = 0.15;
            opacityAnimation.FillMode = CAFillMode.Forwards;

            Layer.AddAnimation(scaleAnimation, "lscale");
            Layer.AddAnimation(opacityAnimation, "lopacity");
            Layer.Opacity = 0;
        }

        private static class AssociatedKeys
        {
            public static IntPtr BoomCellsName = new IntPtr(1);
            public static IntPtr ScaleSnapshotName = new IntPtr(2);
        }

        private List<CALayer> BoomCells { get; set; }

        private UIImage ScaleSnapshot { get; set; }

        private nfloat MakeShakeValue(nfloat p)
        {
            var basicOrigin = -10.0f;
            var maxOffset = -2 * basicOrigin;
            return (nfloat)(basicOrigin + maxOffset * ((nfloat)(random.Next() % 101) / (nfloat)100) + p);
        }

        private nfloat MakeScaleValue()
        {
            return 1 - 0.7f * ((nfloat)(random.Next() % 61) / (nfloat)50);
        }

        private UIBezierPath MakeRandomPath(CALayer aLayer)
        {
            var particlePath = new UIBezierPath();
            particlePath.MoveTo(Layer.Position);

            var basicLeft = -1.3f * (nfloat)Layer.Frame.Size.Width;
            var maxOffset = 2 * Math.Abs(basicLeft);

            var randomNumber = (uint)(random.Next() % 101);
            var endPointX = basicLeft + maxOffset * ((nfloat)randomNumber / (nfloat)100) + aLayer.Position.X;

            var controlPointOffSetX = (endPointX - aLayer.Position.X) / 2 + aLayer.Position.X;
            var controlPointOffSetY = Layer.Position.Y - 0.2f * (nfloat)Layer.Frame.Size.Height - (nfloat)(random.Next() % (uint)(1.2f * Layer.Frame.Size.Height));
            var endPointY = Layer.Position.Y + Layer.Frame.Size.Height / 2 + (nfloat)(random.Next() % (uint)(Layer.Frame.Size.Height / 2));

            particlePath.AddQuadCurveToPoint(
                new CGPoint(endPointX, endPointY),
                new CGPoint(controlPointOffSetX, controlPointOffSetY)
            );

            return particlePath;
        }

        private void CellAnimations()
        {
            foreach (CALayer shape in BoomCells)
            {
                shape.Position = Center;
                shape.Opacity = 1;

                var moveAnimation = CAKeyFrameAnimation.FromKeyPath("position");
                moveAnimation.Path = this.MakeRandomPath(shape).CGPath;
                moveAnimation.RemovedOnCompletion = false;
                moveAnimation.FillMode = CAFillMode.Forwards;
                moveAnimation.TimingFunction = CAMediaTimingFunction.FromControlPoints(0.240000f, 0.590000f, 0.506667f, 0.026667f);
                moveAnimation.Duration = (double)(new Random().Next() % 10) * 0.05 + 0.3;

                var scaleAnimation = CABasicAnimation.FromKeyPath("transform.scale");
                scaleAnimation.To = new NSNumber(this.MakeScaleValue());
                scaleAnimation.Duration = moveAnimation.Duration;
                scaleAnimation.RemovedOnCompletion = false;
                scaleAnimation.FillMode = CAFillMode.Forwards;

                var opacityAnimation = CABasicAnimation.FromKeyPath("opacity");
                opacityAnimation.From = new NSNumber(1);
                opacityAnimation.To = new NSNumber(0);
                opacityAnimation.Duration = moveAnimation.Duration;
                opacityAnimation.RemovedOnCompletion = true;
                opacityAnimation.FillMode = CAFillMode.Forwards;
                opacityAnimation.TimingFunction = CAMediaTimingFunction.FromControlPoints(0.380000f, 0.033333f, 0.963333f, 0.260000f);

                shape.Opacity = 0;
                shape.AddAnimation(scaleAnimation, "scaleAnimation");
                shape.AddAnimation(moveAnimation, "moveAnimation");
                shape.AddAnimation(opacityAnimation, "opacityAnimation");
            }
        }

        private UIColor ColorWithPoint(int x, int y, UIImage image)
        {
            var pixelData = image.CGImage.DataProvider.CopyData();
            var data = pixelData.ToArray();
            var pixelInfo = ((int)image.Size.Width * y + x) * 4;

            var a = (nfloat)data[pixelInfo] / 255.0f;
            var r = (nfloat)data[pixelInfo + 1] / 255.0f;
            var g = (nfloat)data[pixelInfo + 2] / 255.0f;
            var b = (nfloat)data[pixelInfo + 3] / 255.0f;

            return UIColor.FromRGBA(r, g, b, a);
        }

        private void RemoveBoomCells()
        {
            if (BoomCells == null)
            {
                return;
            }

            foreach (var item in BoomCells)
            {
                item.RemoveFromSuperLayer();
            }

            BoomCells = null;
        }

        private UIImage Snapshot()
        {
            UIGraphics.BeginImageContext(Layer.Frame.Size);
            Layer.RenderInContext(UIGraphics.GetCurrentContext());
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return image;
        }

        public void Boom()
        {
            var shakeXAnimation = new CAKeyFrameAnimation();
            shakeXAnimation.KeyPath = "position.x";
            shakeXAnimation.Duration = 0.2;

            shakeXAnimation.Values = new NSObject[] {
                 NSNumber.FromNFloat(MakeShakeValue(Layer.Position.X)),
                 NSNumber.FromNFloat(MakeShakeValue(Layer.Position.X)),
                 NSNumber.FromNFloat(MakeShakeValue(Layer.Position.X)),
                 NSNumber.FromNFloat(MakeShakeValue(Layer.Position.X)),
                 NSNumber.FromNFloat(MakeShakeValue(Layer.Position.X))
            };

            var shakeYAnimation = new CAKeyFrameAnimation();
            shakeYAnimation.KeyPath = "position.y";
            shakeYAnimation.Duration = shakeXAnimation.Duration;
            shakeYAnimation.Values = new NSObject[] {
                NSNumber.FromNFloat(MakeShakeValue(Layer.Position.Y)),
                NSNumber.FromNFloat(MakeShakeValue(Layer.Position.Y)),
                NSNumber.FromNFloat(MakeShakeValue(Layer.Position.Y)),
                NSNumber.FromNFloat(MakeShakeValue(Layer.Position.Y)),
                NSNumber.FromNFloat(MakeShakeValue(Layer.Position.Y))
            };

            Layer.AddAnimation(shakeXAnimation, "shakeXAnimation");
            Layer.AddAnimation(shakeYAnimation, "shakeYAnimation");

            NSTimer.CreateScheduledTimer(0.2, (obj) =>
            {
                ScaleOpacityAnimations();
            });

            if (BoomCells == null)
            {
                BoomCells = new List<CALayer>();
                for (int i = 0; i <= 16; i++)
                {
                    for (int j = 0; j <= 16; j++)
                    {
                        if (ScaleSnapshot == null)
                        {
                            ScaleSnapshot = Snapshot().Scale(new CGSize(34, 34));
                        }

                        var pWidth = (nfloat)Math.Min(Frame.Size.Width, Frame.Size.Height) / 17;
                        var color = ColorWithPoint(i * 2, j * 2, ScaleSnapshot);
                        var shape = new CALayer();
                        shape.BackgroundColor = color.CGColor;
                        shape.Opacity = 0;
                        shape.CornerRadius = pWidth / 2;
                        shape.Frame = new CGRect(i * pWidth, j * pWidth, pWidth, pWidth);
                        Layer.SuperLayer.AddSublayer(shape);
                        BoomCells.Add(shape);
                    }
                }
            }

            NSTimer.CreateScheduledTimer(0.35, (obj) =>
            {
                CellAnimations();
            });
        }

        public void Reset()
        {
            Layer.Opacity = 1;
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            this.RemoveBoomCells();
            base.WillMoveToSuperview(newsuper);
        }
    }
}