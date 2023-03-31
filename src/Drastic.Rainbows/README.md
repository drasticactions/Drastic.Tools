[![NuGet Version](https://img.shields.io/nuget/v/Drastic.Rainbows.svg)](https://www.nuget.org/packages/Drastic.Rainbows/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

# Drastic.Rainbows

Drastic.Rainbows is a drop in extension to add random colors to UI/NSViews for .NET iOS and Mac platforms. This can help when designing UIs and handling constraints.

## How To Use

Install the nuget and run 

```c#
    Drastic.Rainbows.SwizzleCommands.Start();
```

During your apps startup. All future UIView's should then show a random color after they appear on screen.