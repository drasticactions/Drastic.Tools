// <copyright file="PlatformExtensions.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.UI;
using Microsoft.UI.Windowing;

namespace Drastic.TrayWindow
{
    /// <summary>
    /// Platform extensions for WinUI.
    /// </summary>
    internal static class PlatformExtensions
    {
        public static AppWindow GetAppWindowForWinUI(this Microsoft.UI.Xaml.Window window)
        {
            var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);

            return GetAppWindowFromWindowHandle(windowHandle);
        }

        private static AppWindow GetAppWindowFromWindowHandle(IntPtr windowHandle)
        {
            var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            return AppWindow.GetFromWindowId(windowId);
        }
    }
}
