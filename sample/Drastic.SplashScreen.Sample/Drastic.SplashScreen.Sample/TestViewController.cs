// <copyright file="TestViewController.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.SplashScreen.Sample
{
    public class TestViewController : UIViewController
    {
        private Drastic.SplashScreen.SplashScreen splashScreen;
        private UILabel label;

        public TestViewController(CGRect frame)
        {
            this.splashScreen = new Drastic.SplashScreen.SplashScreen(this.TwitterPath(), UIColor.Blue, UIColor.White);
            this.splashScreen.DurationAnimation = 5f;
            this.label = new UILabel(frame)
            {
#if !TVOS
                BackgroundColor = UIColor.SystemBackground,
#endif
                TextAlignment = UITextAlignment.Center,
                Text = "Hello, Splash Screen!",
                AutoresizingMask = UIViewAutoresizing.All,
            };

            this.View!.AddSubview(this.label);
            this.View!.AddSubview(this.splashScreen);
        }

        public override void ViewDidAppear(bool animated)
        {
            this.splashScreen.StartAnimation();
            base.ViewDidAppear(animated);
        }

        private UIBezierPath Plus()
        {
            var shape = new UIBezierPath();
            shape.MoveTo(new CGPoint(10, 6));
            shape.AddLineTo(new CGPoint(10, 0));
            shape.AddLineTo(new CGPoint(6, 0));
            shape.AddLineTo(new CGPoint(6, 6));
            shape.AddLineTo(new CGPoint(0, 6));
            shape.AddLineTo(new CGPoint(0, 10));
            shape.AddLineTo(new CGPoint(6, 10));
            shape.AddLineTo(new CGPoint(6, 16));
            shape.AddLineTo(new CGPoint(10, 16));
            shape.AddLineTo(new CGPoint(10, 10));
            shape.AddLineTo(new CGPoint(16, 10));
            shape.AddLineTo(new CGPoint(16, 6));
            shape.AddLineTo(new CGPoint(10, 6));
            shape.ClosePath();
            return shape;
        }

        private UIBezierPath TwitterPath()
        {
            var bezierPath = new UIBezierPath();
            bezierPath.MoveTo(new CGPoint(22.05f, 56.63f));
            bezierPath.AddCurveToPoint(new CGPoint(34.23f, 54.93f), new CGPoint(26.32f, 56.63f), new CGPoint(30.38f, 56.06f));
            bezierPath.AddCurveToPoint(new CGPoint(44.42f, 50.27f), new CGPoint(38.08f, 53.8f), new CGPoint(41.48f, 52.25f));
            bezierPath.AddCurveToPoint(new CGPoint(52.37f, 43.37f), new CGPoint(47.36f, 48.29f), new CGPoint(50.01f, 45.99f));
            bezierPath.AddCurveToPoint(new CGPoint(58.2f, 34.89f), new CGPoint(54.74f, 40.75f), new CGPoint(56.68f, 37.92f));
            bezierPath.AddCurveToPoint(new CGPoint(61.69f, 25.53f), new CGPoint(59.72f, 31.86f), new CGPoint(60.89f, 28.74f));
            bezierPath.AddCurveToPoint(new CGPoint(62.9f, 15.97f), new CGPoint(62.5f, 22.32f), new CGPoint(62.9f, 19.14f));
            bezierPath.AddLineTo(new CGPoint(62.9f, 14.11f));
            bezierPath.AddCurveToPoint(new CGPoint(70.03f, 6.68f), new CGPoint(65.7f, 12.07f), new CGPoint(68.07f, 9.6f));
            bezierPath.AddCurveToPoint(new CGPoint(61.78f, 8.95f), new CGPoint(67.37f, 7.86f), new CGPoint(64.62f, 8.61f));
            bezierPath.AddCurveToPoint(new CGPoint(68.08f, 1.05f), new CGPoint(64.87f, 7.12f), new CGPoint(66.97f, 4.49f));
            bezierPath.AddCurveToPoint(new CGPoint(59.02f, 4.51f), new CGPoint(65.17f, 2.72f), new CGPoint(62.15f, 3.88f));
            bezierPath.AddCurveToPoint(new CGPoint(48.49f, 0), new CGPoint(56.07f, 1.5f), new CGPoint(52.56f, 0));
            bezierPath.AddCurveToPoint(new CGPoint(38.34f, 4.17f), new CGPoint(44.51f, 0), new CGPoint(41.13f, 1.39f));
            bezierPath.AddCurveToPoint(new CGPoint(34.14f, 14.28f), new CGPoint(35.54f, 6.95f), new CGPoint(34.14f, 10.32f));
            bezierPath.AddCurveToPoint(new CGPoint(34.45f, 17.57f), new CGPoint(34.14f, 15.56f), new CGPoint(34.25f, 16.66f));
            bezierPath.AddCurveToPoint(new CGPoint(17.92f, 13.26f), new CGPoint(28.54f, 17.25f), new CGPoint(23.04f, 15.81f));
            bezierPath.AddCurveToPoint(new CGPoint(4.87f, 2.92f), new CGPoint(12.81f, 10.7f), new CGPoint(8.46f, 7.26f));
            bezierPath.AddCurveToPoint(new CGPoint(2.93f, 9.83f), new CGPoint(3.58f, 5.15f), new CGPoint(2.93f, 7.46f));
            bezierPath.AddCurveToPoint(new CGPoint(4.65f, 16.6f), new CGPoint(2.93f, 12.25f), new CGPoint(3.5f, 14.51f));
            bezierPath.AddCurveToPoint(new CGPoint(9.34f, 21.7f), new CGPoint(5.8f, 18.69f), new CGPoint(7.36f, 20.39f));
            bezierPath.AddCurveToPoint(new CGPoint(2.86f, 19.9f), new CGPoint(7.04f, 21.61f), new CGPoint(4.88f, 21.01f));
            bezierPath.AddLineTo(new CGPoint(2.86f, 20.07f));
            bezierPath.AddCurveToPoint(new CGPoint(6.13f, 29.2f), new CGPoint(2.86f, 23.53f), new CGPoint(3.95f, 26.57f));
            bezierPath.AddCurveToPoint(new CGPoint(14.35f, 34.11f), new CGPoint(8.31f, 31.82f), new CGPoint(11.05f, 33.46f));
            bezierPath.AddCurveToPoint(new CGPoint(10.56f, 34.59f), new CGPoint(13.12f, 34.43f), new CGPoint(11.86f, 34.59f));
            bezierPath.AddCurveToPoint(new CGPoint(7.87f, 34.35f), new CGPoint(9.59f, 34.59f), new CGPoint(8.69f, 34.51f));
            bezierPath.AddCurveToPoint(new CGPoint(12.93f, 41.44f), new CGPoint(8.78f, 37.22f), new CGPoint(10.47f, 39.58f));
            bezierPath.AddCurveToPoint(new CGPoint(21.26f, 44.29f), new CGPoint(15.4f, 43.29f), new CGPoint(18.17f, 44.24f));
            bezierPath.AddCurveToPoint(new CGPoint(3.41f, 51.03f), new CGPoint(15.58f, 48.78f), new CGPoint(9.63f, 51.03f));
            bezierPath.AddCurveToPoint(new CGPoint(0f, 50.8f), new CGPoint(2.23f, 51.03f), new CGPoint(1.09f, 50.95f));
            bezierPath.AddCurveToPoint(new CGPoint(22.05f, 56.63f), new CGPoint(6.11f, 54.68f), new CGPoint(13.46f, 56.63f));
            bezierPath.AddLineTo(new CGPoint(22.05f, 56.63f));
            bezierPath.ClosePath();
            return bezierPath;
        }
    }
}