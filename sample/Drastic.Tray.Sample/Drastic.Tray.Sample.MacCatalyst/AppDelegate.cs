// <copyright file="AppDelegate.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AppKit;
using Drastic.PureLayout;

namespace Drastic.Tray.Sample.MacCatalyst;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    /// <inheritdoc/>
    public override UIWindow? Window
    {
        get;
        set;
    }

    /// <inheritdoc/>
    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // create a new window instance based on the screen size
        this.Window = new UIWindow(UIScreen.MainScreen.Bounds);

        this.Window.RootViewController = new SampleViewController(this.Window!);

        // make the window visible
        this.Window.MakeKeyAndVisible();

        return true;
    }
}

public class SampleViewController : UIViewController
{
    public UIButton TrayButton = new UIButton(UIButtonType.RoundedRect);
    private UIWindow window;

    private TrayIcon? trayIcon;

    public SampleViewController(UIWindow window)
    {
        this.window = window;
        this.SetupUI();
        this.SetupLayout();
    }

    private void SetupUI()
    {
        this.View!.AddSubview(this.TrayButton);
        this.TrayButton.SetTitle("Add Tray Icon", UIControlState.Normal);
        this.TrayButton.TouchUpInside += this.TrayButton_TouchUpInside;
    }

    private async void TrayButton_TouchUpInside(object? sender, EventArgs e)
    {
        var menuItems = new List<TrayMenuItem>();
        var image = UIImage.GetSystemImage("trophy.circle");
        var trayImage = new TrayImage(image!);
        menuItems.Add(new TrayMenuItem("Hello!", trayImage, async () => { }, "h"));
        menuItems.Add(new TrayMenuItem());
        menuItems.Add(new TrayMenuItem("From!", trayImage, async () => { }, "f"));
        menuItems.Add(new TrayMenuItem());
        menuItems.Add(new TrayMenuItem("Mac Catalyst!", trayImage, async () => { }, "m", NSEventModifierMask.ControlKeyMask | NSEventModifierMask.CommandKeyMask));
        this.trayIcon = new Drastic.Tray.TrayIcon("Tray Icon", trayImage, menuItems);
        this.trayIcon.RightClicked += (object? sender, TrayClickedEventArgs e) => { this.trayIcon.OpenMenu(); };
        this.trayIcon.LeftClicked += (object? sender, TrayClickedEventArgs e) =>
        {
            var okAlertController = UIAlertController.Create("Drastic.Tray.Sample", "Welcome!", UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            this.PresentViewController(okAlertController, true, null);
        };
    }

    private void SetupLayout()
    {
        this.TrayButton.AutoCenterInSuperview();
    }
}
