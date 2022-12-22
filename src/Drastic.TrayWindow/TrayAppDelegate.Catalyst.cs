// <copyright file="TrayAppDelegate.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.TrayWindow
{
    public class TrayAppDelegate : UIApplicationDelegate
    {
        internal static Drastic.Tray.TrayIcon? Icon { get; private set; }

        internal static Drastic.TrayWindow.TrayWindowOptions? Options { get; private set; }

        internal static Drastic.TrayWindow.UITrayWindow? Window { get; private set; }

        internal static UIViewController? Controller { get; private set; }

        internal static bool HandleWindowOpenOnRightClick { get; private set; }

        public void CreateTrayWindow(Drastic.Tray.TrayIcon icon, TrayWindowOptions options, UIViewController controller, bool handleOnRightClick = false)
        {
            if (Icon is not null)
            {
                throw new ArgumentException("Can only have one Tray Icon Window per application.");
            }

            Icon = icon;
            Options = options;
            Controller = controller;
            HandleWindowOpenOnRightClick = handleOnRightClick;

            UIApplication.SharedApplication.RequestSceneSessionActivation(null, new NSUserActivity("TraySceneDelegate"), null, null);
        }

        public override UISceneConfiguration GetConfiguration(UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
        {
            var name = options.UserActivities.AnyObject.ActivityType;

            if (name is "TraySceneDelegate")
            {
                var test = new UISceneConfiguration("TraySceneDelegate", connectingSceneSession.Role);
                test.DelegateType = typeof(TrayWindowSceneDelegate);
                return test;
            }

            return base.GetConfiguration(application, connectingSceneSession, options);
        }
    }
}