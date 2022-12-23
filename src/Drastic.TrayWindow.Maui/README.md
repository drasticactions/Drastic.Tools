[![NuGet Version](https://img.shields.io/nuget/v/Drastic.TrayWindow.Maui.svg)](https://www.nuget.org/packages/Drastic.Tray/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

# Drastic.TrayWindow.Maui

Drastic.TrayWindow.Maui is a helper library for Drastic.TrayWindow, letting you intergrate it into your existing MAUI application.

Drastic.TrayWindow.Maui supports Mac Catalyst and WinUI.

## Setup

First, for Mac Catalyst, you need to set it up to allow for multiple windows. You **MUST** follow the [documentation](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/windows?view=net-maui-7.0#ipados-and-macos-configuration), including setting up the info.plist and SceneDelegate for MAUI. If you don't do this, you won't get full multi-window support and this won't work. 

Once you do this, verify that multiple window support works for your project. Then, install this nuget.

In your `AppDelegate.cs`, switch it to use `MauiTrayUIApplicationDelegate`

```c#
namespace Drastic.TrayWindow.Sample.Maui;

[Register("AppDelegate")]
public class AppDelegate : MauiTrayUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
```

Next, in your `MauiProgram.cs`, add `AddTrayWindowSupport`

```c#
 var builder = MauiApp.CreateBuilder();
        builder
            .AddTrayWindowSupport()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
```

Now you can add a tray window. This can only happen after the Maui Context has been created, so only after your first Maui `Window` has been created. Once you verify that has occured, you can then invoke the following helper:

```c#

        var image = App.GetResourceFileContent("favicon.png");

        Drastic.TrayWindow.Maui.MauiTrayWindow.Generate("Test", image!, new TrayWindowOptions(), new SamplePage());

```

`Image` is an image `Stream`, this could come from an existing MAUI image from your resources, or as a standalone file on your system.

## Tips

- While you can use `Shell` inside of the `TrayWindow`, you lose most of the functionality for page navigation and deep linking.
- In general, use simple pages for best results. Trying to put your entire application inside of the tray is not ideal. Use discretion.