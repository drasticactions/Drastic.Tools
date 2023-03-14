// <copyright file="ViewExtensions.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using ObjCRuntime;

namespace Drastic.DebugRainbows
{
    public static class ViewExtensions
    {
        private static readonly Random randomGen = new Random();

        private static readonly UIColor color = new UIColor(randomGen.Next(0, 255), randomGen.Next(0, 255), randomGen.Next(0, 255), 255);

        public static void AddDebugRainbows(this UIView view)
        {
            foreach (var subview in view.Subviews)
            {
                subview.BackgroundColor = color;
                subview.AddDebugRainbows();
            }
        }

        public static void AddDebugRainbows(this UIViewController viewController)
        => viewController.View?.AddDebugRainbows();
    }
}
