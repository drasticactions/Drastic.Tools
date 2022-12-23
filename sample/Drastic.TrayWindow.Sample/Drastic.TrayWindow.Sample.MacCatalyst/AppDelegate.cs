// <copyright file="AppDelegate.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Reflection;
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
        var trayImage = new TrayImage(GetResourceFileContent("favicon.png")!);
        var icon = new Tray.TrayIcon("Tray Window Sample", trayImage);
        this.CreateTrayWindow(icon, new TrayWindowOptions(), new SampleViewController("Welcome to the tray!"));

        return true;
    }

    /// <summary>
    /// Get Resource File Content via FileName.
    /// </summary>
    /// <param name="fileName">Filename.</param>
    /// <returns>Stream.</returns>
    public static Stream? GetResourceFileContent(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Drastic.TrayWindow.Sample.MacCatalyst." + fileName;
        if (assembly is null)
        {
            return null;
        }

        return assembly.GetManifestResourceStream(resourceName);
    }
}