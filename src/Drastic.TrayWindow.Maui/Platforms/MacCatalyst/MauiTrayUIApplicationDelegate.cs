// <copyright file="MauiTrayUIApplicationDelegate.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Foundation;
using UIKit;

namespace Drastic.TrayWindow
{
    public class MauiTrayUIApplicationDelegate : MauiUIApplicationDelegate
    {
        /// <summary>
        /// Gets the internal TrayIcon. Used for handling setting up the tray window via the scene delegate.
        /// </summary>
        internal static Drastic.Tray.TrayIcon? Icon { get; private set; }

        /// <summary>
        /// Gets the TrayWindowOptions used when creating the TrayWindow.
        /// </summary>
        internal static Drastic.TrayWindow.TrayWindowOptions? Options { get; private set; }

        /// <summary>
        /// Gets the UIViewController used for the TrayWindow.
        /// </summary>
        internal static UIViewController? Controller { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to use right click for showing the TrayWindow.
        /// </summary>
        internal static bool HandleWindowOpenOnRightClick { get; private set; }

        /// <summary>
        /// Create the TrayWindow for the application.
        /// When applied, the TrayIcon will auto-implement either the left or right clicked events
        /// to show the window.
        /// </summary>
        /// <param name="icon">Icon to attach the TrayWindow to, <see cref="Tray.TrayIcon"/>.</param>
        /// <param name="options"><see cref="TrayWindowOptions"/>.</param>
        /// <param name="controller">UIViewController to host inside of the TrayWindow.</param>
        /// <param name="handleOnRightClick">Handle showing the window on the right click.</param>
        /// <exception cref="ArgumentException">Thrown if the TrayIcon/TrayWindow are already set. You can only run this method once.</exception>
        public void CreateTrayWindow(Drastic.Tray.TrayIcon icon, TrayWindowOptions options, UIViewController controller, bool handleOnRightClick = false)
        {
            Icon = icon;
            Options = options;
            Controller = controller;
            HandleWindowOpenOnRightClick = handleOnRightClick;
            UIApplication.SharedApplication.RequestSceneSessionActivation(null, new NSUserActivity("TraySceneDelegate"), null, null);
        }

        public override UISceneConfiguration GetConfiguration(UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
        {
            var name = options.UserActivities.AnyObject?.ActivityType ?? string.Empty;

            if (name is "TraySceneDelegate")
            {
                var test = new UISceneConfiguration("MauiTraySceneDelegate", connectingSceneSession.Role);
                test.DelegateType = typeof(MauiTrayWindowSceneDelegate);
                return test;
            }

            return base.GetConfiguration(application, connectingSceneSession, options);
        }

        protected override MauiApp CreateMauiApp()
        {
            throw new NotImplementedException();
        }
    }
}