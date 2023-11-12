[![NuGet Version](https://img.shields.io/nuget/v/Drastic.UITools.Maui.svg)](https://www.nuget.org/packages/Drastic.UITools.Maui/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

# Drastic.UITools.Maui

Drastic.UITools.Maui is a set of utilities and helper functions for debugging UI in .NET Maui Applications.

## FPSViewer

You can use the default implementation of Drastic.UITools.FPSViewer against the native platforms, or the included `UIToolsWindow` Helper.

```csharp
// app.xaml.cs
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new UIToolsWindow(new AppShell(), new DebugLoggerProvider().CreateLogger("UITools"));
    }
}
```

This will automatically enable FPSViewer when the MAUI `Window` is created.