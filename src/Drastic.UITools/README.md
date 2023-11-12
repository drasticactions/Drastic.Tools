[![NuGet Version](https://img.shields.io/nuget/v/Drastic.UITools.svg)](https://www.nuget.org/packages/Drastic.UITools/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

# Drastic.UITools

Drastic.UITools is a set of utilities and helper functions for debugging UI in .NET Applications.

## FPSViewer

FPSViewer generates a frame rate for your application's UI in the debug output; This is done on iOS and Mac Catalyst with a reimplementation of [WatchdogInspector](https://github.com/tapwork/WatchdogInspector/), monitoring the UI Thread for activity.  For Android, we use an implementation of [FrameMetricsAggregator](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#improve-layout-performance-of-label-on-android).

### iOS/Catalyst

```csharp
// Generates UI in the status bar for iOS apps.
// For log output, add an ILogger implementation.
Drastic.UITools.FPSViewer.Start();
// For log output, add an ILogger implementation. For example...
Drastic.UITools.FPSViewer.Start(new DebugLoggerProvider().CreateLogger("UITools"));
// To stop.
Drastic.UITools.FPSViewer.Stop();
```

### Android

```csharp
// Requires the Android Activity you wish to trace. For example...
Drastic.UITools.FPSViewer.GetInstance(logger).Start(CurrentAndroidActivity);
// To stop
Drastic.UITools.FPSViewer.GetInstance(logger).Stop(CurrentAndroidActivity);
```
