// <copyright file="SplashScreen.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#nullable disable

using System;
using CoreAnimation;

namespace Drastic.SplashScreen
{
    /// <summary>
    /// Splash Screen.
    /// </summary>
    public class SplashScreen : UIView, ICAAnimationDelegate
    {
        private static readonly nfloat sizeScale = 0.2f;

        private nfloat durationAnimation;
        private Action animationCompletionHandler;
        private CAAnimation logoAnimation;
        private CALayer gradientLayer;

        public SplashScreen(UIBezierPath bezierPath, UIColor backgroundColor, UIColor logoColor)
            : this(bezierPath, backgroundColor, backgroundColor, logoColor)
        {
        }

        public SplashScreen(UIBezierPath bezierPath, UIColor topColor, UIColor bottomColor, UIColor logoColor)
            : base(UIScreen.MainScreen.Bounds)
        {
            if (topColor.Equals(bottomColor))
            {
                this.gradientLayer = this.ConfigureLayerWithLogoFromBezierPath(bezierPath, topColor, topColor);
            }
            else
            {
                this.gradientLayer = this.ConfigureLayerWithLogoFromBezierPath(bezierPath, topColor, bottomColor);
            }

            this.Layer.AddSublayer(this.gradientLayer!);
            this.BackgroundColor = logoColor;
        }

        public CGRect LayerSizeRect => this.Bounds.Inset(-this.Bounds.Width * sizeScale, -this.Bounds.Height * sizeScale);

        public CGPath MutableLogoPath
        {
            get
            {
                var logoPath = new CGPath();
                logoPath.AddRect(this.LayerSizeRect);

                return logoPath;
            }
        }

        private CAShapeLayer GetLayerFromBezierPath(UIBezierPath bezierPath)
        {
            var logoPath = this.MutableLogoPath;

            var logoLocation = new CGPoint(
                (this.Bounds.Width - bezierPath.Bounds.Width) / 2,
                (this.Bounds.Height - bezierPath.Bounds.Height) / 2);
            var logoTransform = CGAffineTransform.MakeTranslation(logoLocation.X, logoLocation.Y);
            logoPath.AddPath(logoTransform, bezierPath.CGPath);

            var shapeLayer = new CAShapeLayer
            {
                Bounds = this.LayerSizeRect,
                Path = logoPath,
                AnchorPoint = CGPoint.Empty,
            };
            return shapeLayer;
        }

        private CAGradientLayer CreateGradientLayerWithGradientFromTopColor(UIColor topColor, UIColor bottomColor)
        {
            var gradientLayer = new CAGradientLayer
            {
                Frame = LayerSizeRect,
                AnchorPoint = new CGPoint(0.5f, 0.5f),
                Colors = new[] { topColor.CGColor, bottomColor.CGColor },
            };
            return gradientLayer;
        }

        private CAGradientLayer ConfigureLayerWithLogoFromBezierPath(UIBezierPath bezierPath, UIColor topColor, UIColor bottomColor)
        {
            var gradientLayer = this.CreateGradientLayerWithGradientFromTopColor(topColor, bottomColor);
            var shapeLayer = this.GetLayerFromBezierPath(bezierPath);
            gradientLayer.Mask = shapeLayer;
            return gradientLayer;
        }

        public void StartAnimation()
        {
            this.StartAnimation(null);
        }

        public void StartAnimation(Action completionHandler)
        {
            this.animationCompletionHandler = completionHandler;
            this.LogoAnimation.Delegate = this;
            this.gradientLayer.AddAnimation(this.LogoAnimation, null);
            this.PerformSelector(new ObjCRuntime.Selector("SetBackgroundColor:"), UIColor.Clear, this.DurationAnimation * 0.45);
        }

        [Export("SetBackgroundColor:")]
        private void SetBackgroundColor(UIColor color)
        {
            this.BackgroundColor = color;
        }

        [Export("animationDidStop:finished:")]
        public void AnimationStopped(CAAnimation anim, bool finished)
        {
            if (this.animationCompletionHandler != null)
            {
                this.animationCompletionHandler();
            }

            this.RemoveFromSuperview();
        }

        public nfloat DurationAnimation
        {
            get
            {
                if (this.durationAnimation <= 0)
                {
                    this.durationAnimation = 1.0f;
                }

                return this.durationAnimation;
            }
            set => this.durationAnimation = value;
        }

        private CAAnimation LogoAnimation
        {
            get
            {
                if (this.logoAnimation == null)
                {
                    var keyFrameAnimation = CAKeyFrameAnimation.FromKeyPath("transform.scale");
                    keyFrameAnimation.Values = new NSObject[] { NSNumber.FromFloat(1f), NSNumber.FromFloat(0.9f), NSNumber.FromFloat(300f) };
                    keyFrameAnimation.KeyTimes = new NSNumber[] { NSNumber.FromFloat(0), NSNumber.FromFloat(0.4f), NSNumber.FromFloat(1) };
                    keyFrameAnimation.Duration = this.DurationAnimation;
                    keyFrameAnimation.RemovedOnCompletion = false;
                    keyFrameAnimation.FillMode = CAFillMode.Forwards;
                    keyFrameAnimation.TimingFunctions = new[]
                    {
                        CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut),
                        CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseIn),
                    };
                    this.logoAnimation = keyFrameAnimation;
                }

                return this.logoAnimation;
            }
        }
    }

    internal static class UIColorExtensions
    {
        public static CGColor GradientFromTopColor(this UIColor color, UIColor bottomColor)
        {
            var gradientLayer = new CAGradientLayer
            {
                Frame = new CGRect(0, 0, 1, 1),
                Colors = new[] { color.CGColor, bottomColor.CGColor },
                StartPoint = new CGPoint(0.5, 0),
                EndPoint = new CGPoint(0.5, 1),
            };

            UIGraphics.BeginImageContextWithOptions(gradientLayer.Frame.Size, false, 0);
            gradientLayer.RenderInContext(UIGraphics.GetCurrentContext());
            var gradientImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return new UIColor(patternImage: gradientImage).CGColor;
        }

        public static CGColor GradientFromBottomColor(this UIColor color, UIColor topColor)
        {
            var gradientLayer = new CAGradientLayer
            {
                Frame = new CGRect(0, 0, 1, 1),
                Colors = new[] { topColor.CGColor, color.CGColor },
                StartPoint = new CGPoint(0.5, 0),
                EndPoint = new CGPoint(0.5, 1),
            };

            UIGraphics.BeginImageContextWithOptions(gradientLayer.Frame.Size, false, 0);
            gradientLayer.RenderInContext(UIGraphics.GetCurrentContext());
            var gradientImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return new UIColor(patternImage: gradientImage).CGColor;
        }
    }
}