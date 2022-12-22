// <copyright file="TrayWindowSceneDelegate.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.Tray;

namespace Drastic.TrayWindow
{
    /// <summary>
    /// Tray Window Scene Delegate.
    /// Handles creating the UITrayWindow for Mac Catalyst.
    /// </summary>
    [Register("TraySceneDelegate")]
    internal class TrayWindowSceneDelegate : UIResponder, IUIWindowSceneDelegate
    {
        private UITrayWindow? trayWindow;

        /// <summary>
        /// Gets the Window.
        /// </summary>
        [Export("window")]
        public UIWindow? Window { get; set; }

        /// <summary>
        /// Run when SceneDelegate willConnectToSession is run.
        /// </summary>
        /// <param name="scene"><see cref="UIScene"/>.</param>
        /// <param name="session"><see cref="UISceneSession"/>.</param>
        /// <param name="connectionOptions"><see cref="UISceneConnectionOptions"/>.</param>
        [Export("scene:willConnectToSession:options:")]
        public virtual void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
        {
            if (scene is not UIWindowScene windowScene)
            {
                return;
            }

            // if this is being restored by debugger launch, get rid of it
            if (connectionOptions.UserActivities.AnyObject == null)
            {
                UIApplication.SharedApplication.RequestSceneSessionDestruction(session, new UISceneDestructionRequestOptions(), null);
                return;
            }

            if (this.Window is not null)
            {
                return;
            }

            this.Window = this.trayWindow = new UITrayWindow(windowScene, TrayAppDelegate.Icon!, TrayAppDelegate.Options!, TrayAppDelegate.Controller!);
            this.Window.MakeKeyAndVisible();

            if (TrayAppDelegate.HandleWindowOpenOnRightClick)
            {
                TrayAppDelegate.Icon!.RightClicked += (object? sender, TrayClickedEventArgs e) => this.trayWindow.ToggleVisibility();
            }
            else
            {
                TrayAppDelegate.Icon!.LeftClicked += (object? sender, TrayClickedEventArgs e) => this.trayWindow.ToggleVisibility();
            }
        }
    }
}