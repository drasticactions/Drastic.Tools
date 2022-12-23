// <copyright file="AppDelegate.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Reflection;
using Drastic.PureLayout;
using Drastic.Tray;
using Drastic.TrayWindow;

namespace Drastic.TrayWindow.Sample.MacOS;

[Register("AppDelegate")]
public class AppDelegate : NSApplicationDelegate
{
    private MainWindowController? mainWindowController;

    public override void DidFinishLaunching(NSNotification notification)
    {
        this.mainWindowController = new MainWindowController();
        this.mainWindowController.Window.MakeKeyAndOrderFront(this);
    }

    public override void WillTerminate(NSNotification notification)
    {
        // Insert code here to tear down your application
    }
}

public class MainWindow : NSWindow
{
    private NSButton button = new NSButton() { Title = "Add Tray Icon", BezelStyle = NSBezelStyle.Rounded, };

    public MainWindow(CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation)
        : base(contentRect, aStyle, bufferingType, deferCreation)
    {
        this.Title = "Drastic.Tray.Sample";

        // Create the content view for the window and make it fill the window
        this.ContentView = new NSView(this.Frame);

        this.ContentView.AddSubview(this.button);

        this.button.Activated += this.Button_Activated;

        this.button.AutoCenterInSuperview();
    }

    private void Button_Activated(object? sender, EventArgs e)
    {
        var menuItems = new List<TrayMenuItem>();
        var trayImage = new TrayImage(GetResourceFileContent("TrayIcon.ico")!);
        menuItems.Add(new TrayMenuItem("Hello!", trayImage, async () => { }, "h"));
        menuItems.Add(new TrayMenuItem("From!", trayImage, async () => { }, "f"));
        menuItems.Add(new TrayMenuItem("MacOS!", trayImage, async () => { }, "m", NSEventModifierMask.ControlKeyMask | NSEventModifierMask.CommandKeyMask));
        var trayIcon = new Drastic.Tray.TrayIcon("Tray Window Sample", trayImage, menuItems);
        var trayWindow = new NSTrayWindow(trayIcon, new TrayWindowOptions(), new SampleViewController());
        trayIcon.LeftClicked += (object? sender, TrayClickedEventArgs e) => trayWindow.ToggleVisibility();
        trayIcon.RightClicked += (object? sender, TrayClickedEventArgs e) => trayIcon.OpenMenu();
    }

    /// <summary>
    /// Get Resource File Content via FileName.
    /// </summary>
    /// <param name="fileName">Filename.</param>
    /// <returns>Stream.</returns>
    public static Stream? GetResourceFileContent(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Drastic.TrayWindow.Sample.MacOS." + fileName;
        if (assembly is null)
        {
            return null;
        }

        return assembly.GetManifestResourceStream(resourceName);
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

public class MainWindowController : NSWindowController
{
    public MainWindowController()
        : base()
    {
        // Construct the window from code here
        CGRect contentRect = new CGRect(0, 0, 1000, 500);
        base.Window = new MainWindow(contentRect, NSWindowStyle.Titled | NSWindowStyle.Closable | NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable, NSBackingStore.Buffered, false);

        // Simulate Awaking from Nib
        this.Window.AwakeFromNib();
    }

    public new MainWindow Window
    {
        get { return (MainWindow)base.Window; }
    }
}