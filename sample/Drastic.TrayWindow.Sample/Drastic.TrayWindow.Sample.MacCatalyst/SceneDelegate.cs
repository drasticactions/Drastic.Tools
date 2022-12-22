// <copyright file="SceneDelegate.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.TrayWindow.Sample.MacCatalyst;

/// <summary>
/// Default Scene Delegate.
/// </summary>
[Register("SceneDelegate")]
public class SceneDelegate : UIResponder, IUIWindowSceneDelegate
{
    /// <summary>
    /// Gets or sets the Window.
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
    public void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
    {
        // This is a cheap way to create a new window for the sample.
        // Normally, you would handle the connectionOptions and UserActivies to create the correct
        // View Controller for your window.
        // This is cheating...
        this.Window = new UIWindow((UIWindowScene)scene);

        this.Window.RootViewController = new SampleViewController("Check out the icon in the menu bar!");

        this.Window.MakeKeyAndVisible();
    }
}
