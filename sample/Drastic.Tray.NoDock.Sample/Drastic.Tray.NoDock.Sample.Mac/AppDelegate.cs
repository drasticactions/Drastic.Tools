// <copyright file="AppDelegate.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.PureLayout;
using Drastic.TrayWindow;

namespace Drastic.Tray.NoDock.Sample.Mac;

[Register("AppDelegate")]
public class AppDelegate : NSApplicationDelegate
{
    private TrayIcon? trayIcon;
    private NSTrayWindow? trayWindow;

    public override void DidFinishLaunching(NSNotification notification)
    {
        var menuItems = new List<TrayMenuItem>();
        var trayImage = new TrayImage(NSImage.ImageNamed("TrayIcon.ico")!);
        menuItems.Add(new TrayMenuItem("Hello!", trayImage, async () => { }, "h"));
        menuItems.Add(new TrayMenuItem("From!", trayImage, async () => { }, "f"));
        menuItems.Add(new TrayMenuItem("MacOS!", trayImage, async () => { }, "m", NSEventModifierMask.ControlKeyMask | NSEventModifierMask.CommandKeyMask));
        menuItems.Add(new TrayMenuItem("Quit!", trayImage, async () => { NSApplication.SharedApplication.Terminate(this); }, "q"));
        this.trayIcon = new Drastic.Tray.TrayIcon("Tray Icon", trayImage, menuItems, false);
        this.trayWindow = new NSTrayWindow(this.trayIcon, new TrayWindowOptions(), new SampleViewController());
        this.trayIcon.LeftClicked += (object? sender, TrayClickedEventArgs e) => this.trayWindow.ToggleVisibility();
        this.trayIcon.RightClicked += (object? sender, TrayClickedEventArgs e) => this.trayIcon.OpenMenu();
    }

    public override void WillTerminate(NSNotification notification)
    {
        // Insert code here to tear down your application
    }
}

public class SampleViewController : NSViewController
{
    private NSButton button = new NSButton() { Title = "Greetings from MacOS!", BezelStyle = NSBezelStyle.Rounded, };

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        this.View!.AddSubview(this.button);

        this.button.AutoCenterInSuperview();
    }

    public override void LoadView()
    {
        // This is how you create NSViewControllers without XIBs
        // Without it, the view will explode on loading.
        // <3 Apple.
        this.View = new NSView();
    }
}
