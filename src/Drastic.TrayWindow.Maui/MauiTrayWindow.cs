// <copyright file="MauiTrayWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using Drastic.Tray;
using Microsoft.Maui.Platform;

namespace Drastic.TrayWindow.Maui
{
    /// <summary>
    /// Tray Window.
    /// </summary>
    public static class MauiTrayWindow
    {
        /// <summary>
        /// Generate a Tray Window and Icon using a MAUI Page.
        /// </summary>
        /// <param name="name">Name of the tray icon.</param>
        /// <param name="image">Image Stream.</param>
        /// <param name="options">Tray Window Options.</param>
        /// <param name="page">MAUI Page to render.</param>
        /// <param name="handleOnRightClick">Enable if you want to launch the window with a right click.</param>
        /// <param name="menuItems">List of optional menu items for the tray icon.</param>
        /// <exception cref="ArgumentException">Thrown if, on Mac Catalyst, you have not set your AppDelegate to use MauiTrayUIApplicationDelegate.</exception>
        /// <returns>TrayIcon with attached window event.</returns>
        public static TrayIcon? Generate(string name, Stream image, TrayWindowOptions options, Page page, bool handleOnRightClick = false, List<TrayMenuItem>? menuItems = default)
        {
#if MACCATALYST

            var uiImage = UIKit.UIImage.LoadFromData(Foundation.NSData.FromStream(image)!)!;
            var trayIcon = new TrayIcon(name, new TrayImage(UIKit.UIImage.GetSystemImage("circle")!), menuItems);
            if (UIKit.UIApplication.SharedApplication.Delegate is MauiTrayUIApplicationDelegate trayDelegate)
            {
                var control = page.ToUIViewController(Microsoft.Maui.Controls.Application.Current!.Handler.MauiContext!);
                trayDelegate.CreateTrayWindow(trayIcon, options, control, handleOnRightClick);
            }
            else
            {
                throw new ArgumentException("You must set your AppDelegate to use Drastic.TrayWindow.TrayAppDelegate");
            }
            return trayIcon;

#elif WINDOWS

            var bitmap = System.Drawing.Image.FromStream(image);
            var trayIcon = new TrayIcon(name, new TrayImage(bitmap!), menuItems);
            var element = page.ToPlatform(Microsoft.Maui.Controls.Application.Current!.Handler.MauiContext!);
            var window = new WinUITrayWindow(trayIcon, options) { Content = element };
            if (handleOnRightClick)
            {
                trayIcon.RightClicked += (object? sender, TrayClickedEventArgs e) => window.ToggleVisibility();
            }
            else
            {
                trayIcon.LeftClicked += (object? sender, TrayClickedEventArgs e) => window.ToggleVisibility();
            }

            return trayIcon;
#else
            return null;
#endif
        }
    }
}