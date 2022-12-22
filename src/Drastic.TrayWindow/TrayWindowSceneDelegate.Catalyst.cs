// <copyright file="TrayWindowSceneDelegate.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.Tray;

namespace Drastic.TrayWindow
{
    [Register("TraySceneDelegate")]
    public class TrayWindowSceneDelegate : UIResponder, IUIWindowSceneDelegate
    {
        private UITrayWindow? trayWindow;

        [Export("window")]
        public UIWindow? Window { get; set; }

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
                TrayAppDelegate.Icon!.RightClicked += (object? sender, TrayClickedEventArgs e) => this.trayWindow.ToggleVisibilityAsync();
            }
            else
            {
                TrayAppDelegate.Icon!.LeftClicked += (object? sender, TrayClickedEventArgs e) => this.trayWindow.ToggleVisibilityAsync();
            }
        }

        [Export("sceneDidDisconnect:")]
        public virtual void DidDisconnect(UIScene scene)
        {
            // Called as the scene is being released by the system.
            // This occurs shortly after the scene enters the background, or when its session is discarded.
            // Release any resources associated with this scene that can be re-created the next time the scene connects.
            // The scene may re-connect later, as its session was not neccessarily discarded (see UIApplicationDelegate `DidDiscardSceneSessions` instead).
        }

        [Export("sceneDidBecomeActive:")]
        public virtual void DidBecomeActive(UIScene scene)
        {
            // Called when the scene has moved from an inactive state to an active state.
            // Use this method to restart any tasks that were paused (or not yet started) when the scene was inactive.
        }

        [Export("sceneWillResignActive:")]
        public virtual void WillResignActive(UIScene scene)
        {
            // Called when the scene will move from an active state to an inactive state.
            // This may occur due to temporary interruptions (ex. an incoming phone call).
        }

        [Export("sceneWillEnterForeground:")]
        public virtual void WillEnterForeground(UIScene scene)
        {
            // Called as the scene transitions from the background to the foreground.
            // Use this method to undo the changes made on entering the background.
        }

        [Export("sceneDidEnterBackground:")]
        public virtual void DidEnterBackground(UIScene scene)
        {
            // Called as the scene transitions from the foreground to the background.
            // Use this method to save data, release shared resources, and store enough scene-specific state information
            // to restore the scene back to its current state.
        }
    }
}