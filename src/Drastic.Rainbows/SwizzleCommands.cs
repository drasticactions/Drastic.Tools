// <copyright file="SwizzleCommands.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#if MACOS
using DAView = AppKit.NSView;
using DAViewController = AppKit.NSViewController;
using DAColor = AppKit.NSColor;
using CoreGraphics;
#else
using DAView = UIKit.UIView;
using DAViewController = UIKit.UIViewController;
using DAColor = UIKit.UIColor;
#endif

namespace Drastic.Rainbows
{
    public delegate void ViewDidAppearAnimatedEventHandler(IntPtr a, IntPtr b);

    public delegate void ViewDidAppearEventHandler(IntPtr a);

    public delegate void ViewDidMoveToSuperViewEventHandler(IntPtr a);

    public static class SwizzleCommands
    {
        private static DAColor GetRandomColor()
        {
            if (random is null)
            {
                random = new Random();
            }

            var red = (float)random.NextDouble();
            var green = (float)random.NextDouble();
            var blue = (float)random.NextDouble();
            var alpha = 1.0f;
#if MACOS
            return DAColor.FromRgba(red, green, blue, alpha);
#else
            return DAColor.FromRGBA(red, green, blue, alpha);
#endif
        }

        private static string logTag = string.Empty;

        static Random? random;

        static Swizzle<ViewDidAppearEventHandler>? viewDidAppear;

        static Swizzle<ViewDidAppearAnimatedEventHandler>? viewDidAppearAnimated;

        static Swizzle<ViewDidMoveToSuperViewEventHandler>? didMoveToSuperview;

        public static void Start(string tag = "")
        {
            logTag = tag;
#if MACOS
            viewDidAppear = new Swizzle<ViewDidAppearEventHandler>(typeof(DAViewController), "viewDidAppear", HijackedViewDidAppear);
            didMoveToSuperview = new Swizzle<ViewDidMoveToSuperViewEventHandler>(typeof(DAView), "viewDidMoveToSuperview", HijackedViewDidMoveToSuperview);
#else
            viewDidAppearAnimated = new Swizzle<ViewDidAppearAnimatedEventHandler>(typeof(DAViewController), "viewDidAppear:", HijackedViewDidAppearAnimated);
            didMoveToSuperview = new Swizzle<ViewDidMoveToSuperViewEventHandler>(typeof(DAView), "didMoveToSuperview", HijackedViewDidMoveToSuperview);
#endif
        }

        static void HijackedViewDidAppear(IntPtr @this)
        {
            DAViewController nsObject = (DAViewController)ObjCRuntime.Runtime.GetINativeObject(@this, true, typeof(DAViewController))!;
            nsObject.PrintPath();

            using (var orig = viewDidAppear?.Restore())
            {
                orig?.Delegate(@this);
            }
        }

        static void HijackedViewDidAppearAnimated(IntPtr @this, IntPtr animated)
        {
            DAViewController nsObject = (DAViewController)ObjCRuntime.Runtime.GetINativeObject(@this, true, typeof(DAViewController))!;
            nsObject.PrintPath();

            using (var orig = viewDidAppearAnimated?.Restore())
            {
                orig?.Delegate(@this, animated);
            }
        }

        static void HijackedViewDidMoveToSuperview(IntPtr @this)
        {
            DAView nsObject = (DAView)ObjCRuntime.Runtime.GetINativeObject(@this, true, typeof(DAView))!;
#if !MACOS
            nsObject.BackgroundColor = GetRandomColor();
#else
            var color = GetRandomColor();
            if (nsObject.Layer is null)
            {
                nsObject.Layer = new CoreAnimation.CALayer();
            }

            nsObject.Layer!.BackgroundColor = color!.ToCGColor();
#endif

            using (var orig = didMoveToSuperview?.Restore())
            {
                orig?.Delegate(@this);
            }
        }

        private static void LogWithLevel(DAViewController self, nuint level)
        {
            var paddingItems = string.Empty;

            for (nuint i = 0; i <= level; i++)
            {
                paddingItems = paddingItems.Insert(paddingItems.Length, "--");
            }

            Console.WriteLine($"{logTag}{paddingItems}-> {self.GetType().FullName}");
        }

        private static void PrintPath(this DAViewController self)
        {
            if (self.ParentViewController == null)
            {
                LogWithLevel(self, 0);
            }
#if !MACOS
            else if (self.ParentViewController is UINavigationController nav)
            {
                var integer = -1;

                if (nav.ViewControllers is not null)
                {
                    integer = Array.IndexOf(nav.ViewControllers, self);
                }

                if (integer >= 0)
                {
                    LogWithLevel(self, (nuint)integer);
                }
            }
            else if (self.ParentViewController is UITabBarController)
            {
                LogWithLevel(self, 1);
            }
#endif
        }
    }

#if MACOS
    public static class NSColorExtensions
    {
        public static CGColor ToCGColor(this NSColor color)
        {
            var colorSpace = CGColorSpace.CreateDeviceRGB();

            var selfCopy = color.UsingColorSpace(NSColorSpace.DeviceRGBColorSpace);

            var colorValues = new nfloat[4];
            selfCopy.GetRgba(out colorValues[0], out colorValues[1], out colorValues[2], out colorValues[3]);

            var cgColor = new CGColor(colorSpace, colorValues);

            colorSpace.Dispose();

            return cgColor;
        }
    }
#endif
}