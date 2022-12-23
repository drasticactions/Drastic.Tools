// <copyright file="AppDelegate.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.PureLayout;
using Drastic.Tray;

namespace Drastic.TrayWindow.Sample.MacCatalyst;

[Register("AppDelegate")]
public class AppDelegate : TrayAppDelegate
{
    public override UIWindow? Window
    {
        get;
        set;
    }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        var image = UIImage.GetSystemImage("trophy.circle");
        var trayImage = new TrayImage(image!);
        var icon = new Tray.TrayIcon("Tray Window Sample", trayImage);
        this.CreateTrayWindow(icon, new TrayWindowOptions(), new SampleViewController("Welcome to the tray!"));

        return true;
    }
}