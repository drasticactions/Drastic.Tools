[![NuGet Version](https://img.shields.io/nuget/v/Drastic.TrayWindow.svg)](https://www.nuget.org/packages/Drastic.TrayWindow/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

# Drastic.TrayWindow

Drastic.TrayWindow is a API for creating tray icons that contain popup windows, designed for dotnet Mac Catalyst and macOS applications.

![animation](https://user-images.githubusercontent.com/898335/209158995-b987ef15-351c-4c1e-a7b7-eaceb1d9d12b.gif)

## **IMPORTANT**

For this to work on Mac Catalyst, we are using Objective-C Selectors for directly accessing AppKit APIs from UIKit. These are private, and are frowd upon by Apple. You may have issues publishing apps using this library within the Mac App Store. While it should "work" it is best seen as experimental.

## How To Use

You can check in this repos `samples` directory for a complete sample of how to use this library. For macOS it should be straight forward, but Mac Catalyst requires some additional setup.

For both Mac Catalyst and macOS, you need to create a `TrayIcon`. You can read how to make one in the `Drastic.Tray` README.

For macOS, 

```c#
var menuItems = new List<TrayMenuItem>();
var trayImage = new TrayImage(NSImage.ImageNamed("TrayIcon.ico")!);
menuItems.Add(new TrayMenuItem("Hello!", trayImage, async () => { }, "h"));
menuItems.Add(new TrayMenuItem("From!", trayImage, async () => { }, "f"));
menuItems.Add(new TrayMenuItem("MacOS!", trayImage, async () => { }, "m", NSEventModifierMask.ControlKeyMask | NSEventModifierMask.CommandKeyMask));
var trayIcon = new Drastic.Tray.TrayIcon(trayImage, menuItems);
var trayWindow = new NSTrayWindow(trayIcon, new TrayWindowOptions(), new SampleViewController());
trayIcon.LeftClicked += (object? sender, TrayClickedEventArgs e) => trayWindow.ToggleVisibility();
trayIcon.RightClicked += (object? sender, TrayClickedEventArgs e) => trayIcon.OpenMenu();
```

Create the `TrayIcon`, then create the `TrayWindow`, using the `TrayIcon` you created. It will now be "attached" to that `TrayIcon`. Once you run `ToggleVisibility` you should the see the window open.

For Mac Catalyst, you need to add some additional items. 

First, in your `Info.plist`, you must add support for multiple windows. The documentation for handling this with iOS (https://learn.microsoft.com/en-us/xamarin/ios/platform/ios13/multi-window-ipad) applies here too.

```
    <key>UIApplicationSceneManifest</key>
    <dict>
        <key>UIApplicationSupportsMultipleScenes</key>
        <true/>
        <key>UISceneConfigurations</key>
        <dict>
            <key>UIWindowSceneSessionRoleApplication</key>
            <array>
                <dict>
                    <key>UISceneConfigurationName</key>
                    <string>Default Configuration</string>
                    <key>UISceneDelegateClassName</key>
                    <string>SceneDelegate</string>
                </dict>
            </array>
        </dict>
    </dict>
```

Next, change your `AppDelegate` to use `TrayAppDelegate`

```c#
using Drastic.Tray;

namespace Drastic.TrayWindow.Sample.MacCatalyst;

[Register("AppDelegate")]
public class AppDelegate : TrayAppDelegate
{
```

Unlike macOS, you can only have one `TrayIcon` with one `TrayWindow` in your application. This is because `TrayAppDelegate` sets up the infrastructure for handling these windows and, trust me, it's for the best.

From your `TrayAppDelegate` instance, you can now add your `TrayIcon` and setup the window.

```c#
var image = UIImage.GetSystemImage("trophy.circle");
var trayImage = new TrayImage(image!);
var icon = new Tray.TrayIcon(trayImage);
this.CreateTrayWindow(icon, new TrayWindowOptions(), new SampleViewController("Welcome to the tray!"));
```

Now it should appear on your systems menu bar.

## WinUI

WinUI is similar to macOS. You can create any number of `WinUITrayWindow`s and apply them to `TrayIcon`. 

```c#
            var trayImage = new TrayImage(GetResourceFileContent("TrayIcon.ico")!);
            var menuItems = new List<TrayMenuItem>
            {
                new TrayMenuItem("Hello!", trayImage, async () => { }),
                new TrayMenuItem("From!", trayImage, async () => { }),
                new TrayMenuItem("Windows!", trayImage, async () => { }),
            };
            this.icon = new TrayIcon("Tray Icon", trayImage, menuItems);
            this.trayWindow = new SampleTrayWindow(this.icon, new TrayWindowOptions(500, 700));
            this.icon.RightClicked += (object? sender, TrayClickedEventArgs e) =>
            {
                System.Diagnostics.Debug.WriteLine("Right Click!");
            };
            this.icon.LeftClicked += (object? sender, TrayClickedEventArgs e) =>
            {
                this.trayWindow.ToggleVisibility();
            };
```

```c#
    public sealed partial class SampleTrayWindow : WinUITrayWindow
    {
        public SampleTrayWindow(TrayIcon icon, TrayWindowOptions options)
            : base(icon, options)
        {
            this.InitializeComponent();
        }
    ...
```

The `WinUITrayWindow` can be created from codebehind or XAML. It is the same as a regular `Window`.
