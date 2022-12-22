// <copyright file="TrayAppDelegate.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.TrayWindow
{
    /// <summary>
    /// Tray App Delegate.
    /// Manages the Tray Icon and Window session, as well as the Scene Delegates for handling the window.
    /// In order to use TrayWindow with Catalyst, you must base your AppDelegate off of this delegate.
    /// </summary>
    public class TrayAppDelegate : UIApplicationDelegate
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

        /// <inheritdoc/>
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