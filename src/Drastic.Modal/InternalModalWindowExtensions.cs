// <copyright file="InternalModalWindowExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace Drastic.Modal;

/// <summary>
/// Internal Modal Window Extensions.
/// </summary>
internal static class InternalModalWindowExtensions
{
    private const string User32DllName = "user32.dll";

    /// <summary>
    /// Enable Window.
    /// </summary>
    /// <param name="hWnd">Window Pointer.</param>
    /// <param name="enabled">Sets enable window.</param>
    /// <returns>Bool.</returns>
    [DllImport(User32DllName, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool EnableWindow(IntPtr hWnd, bool enabled);

    /// <summary>
    /// Set Window Long.
    /// </summary>
    /// <param name="hWnd">Pointer.</param>
    /// <param name="nIndex">Index.</param>
    /// <param name="dwNewLong">New Long.</param>
    /// <returns>int.</returns>
    [DllImport(User32DllName, SetLastError = true)]
    public static extern int SetWindowLong(IntPtr hWnd, WindowLongIndexFlags nIndex, SetWindowLongFlags dwNewLong);

    /// <summary>
    /// Set Window Long.
    /// </summary>
    /// <param name="window">Window.</param>
    /// <param name="nIndex">Index.</param>
    /// <param name="dwNewLong">New Long.</param>
    /// <returns>int.</returns>
    public static int SetWindowLong(this Window window, WindowLongIndexFlags nIndex, SetWindowLongFlags dwNewLong)
        => SetWindowLong(window.GetWindowHandle(), nIndex, dwNewLong);

    /// <summary>
    /// Get App Window.
    /// </summary>
    /// <param name="window">Window.</param>
    /// <returns>App Window.</returns>
    public static AppWindow GetAppWindow(this Window window)
        => Microsoft.UI.Windowing.AppWindow.GetFromWindowId(Microsoft.UI.Win32Interop.GetWindowIdFromWindow(window.GetWindowHandle()));

    /// <summary>
    /// Set Enable Window.
    /// </summary>
    /// <param name="applicationWindow">App Window.</param>
    /// <param name="enable">Enable.</param>
    public static void SetEnableWindow(Window applicationWindow, bool enable)
        => SetEnableWindows(new List<Window>() { applicationWindow }, enable);

    /// <summary>
    /// Set Enable Windows.
    /// </summary>
    /// <param name="applicationWindows">App Windows.</param>
    /// <param name="enable">Enable.</param>
    public static void SetEnableWindows(this List<Window> applicationWindows, bool enable)
    {
        foreach (var item in applicationWindows)
        {
            item.EnableWindow(enable);
            if (item is IModalWindowEventHandler handler)
            {
                if (enable)
                {
                    handler.Enabled();
                }
                else
                {
                    handler.Disabled();
                }
            }
        }
    }

    /// <summary>
    /// Get Window Handle.
    /// </summary>
    /// <param name="window">Window.</param>
    /// <returns>Pointer.</returns>
    internal static IntPtr GetWindowHandle(this Window window)
        => WinRT.Interop.WindowNative
            .GetWindowHandle(window);
}
