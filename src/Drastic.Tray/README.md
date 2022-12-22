[![NuGet Version](https://img.shields.io/nuget/v/Drastic.Tray.svg)](https://www.nuget.org/packages/Drastic.Tray/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

# Drastic.Tray

Drastic.Tray is a straightforward API for creating tray icons, designed for dotnet Mac Catalyst and macOS applications.

![animation](https://user-images.githubusercontent.com/898335/209154534-9d4361f9-f970-4bb9-b107-03fdc93f4941.gif)

## **IMPORTANT**

For this to work on Mac Catalyst, we are using Objective-C Selectors for directly accessing AppKit APIs from UIKit. These are private, and are frowd upon by Apple. You may have issues publishing apps using this library within the Mac App Store. While it should "work" it is best seen as experimental.

## How To Use

Usage between macOS and Catalyst is similar.


```c#
var menuItems = new List<TrayMenuItem>();
var trayImage = new TrayImage(NSImage.ImageNamed("TrayIcon.ico")!);
menuItems.Add(new TrayMenuItem("Hello!", trayImage, async () => { }, "h"));
menuItems.Add(new TrayMenuItem("From!", trayImage, async () => { }, "f"));
menuItems.Add(new TrayMenuItem("MacOS!", trayImage, async () => { }, "m", NSEventModifierMask.ControlKeyMask | NSEventModifierMask.CommandKeyMask));
var trayIcon = new Drastic.Tray.TrayIcon(trayImage, menuItems);
trayIcon.LeftClicked += (object? sender, TrayClickedEventArgs e) => trayIcon.OpenMenu();
trayIcon.RightClicked += (object? sender, TrayClickedEventArgs e) => { };
```

Create a `TrayImage`. This requires either an `NSImage` or `UIImage` (Which gets turned back into an `NSImage`). If you wish, create a list of `TrayMenuItem`. These can then be invoked in the tray icon with `trayIcon.OpenMenu()`.

When you create the `TrayIcon`, it should automatically appear in the Mac Tray.

After you create the icon, you can attach to `LeftClicked` and `RightClicked` to handle accessing the respected events on the button.

You can have as many TrayIcon's as allowed by the system. However, you should probably limit yourself to one, unless you hate your users. But hey, you do you, mythical developer.