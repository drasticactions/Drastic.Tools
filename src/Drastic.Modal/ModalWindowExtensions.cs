// <copyright file="ModalWindowExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.Graphics;

namespace Drastic.Modal;

internal static class InternalModalWindowExtensions
{
    const string User32DllName = "user32.dll";

    [DllImport(User32DllName, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool EnableWindow(IntPtr hWnd, bool enabled);

    [DllImport(User32DllName, SetLastError = true)]
    public static extern int SetWindowLong(IntPtr hWnd, WindowLongIndexFlags nIndex, SetWindowLongFlags dwNewLong);

    public static int SetWindowLong(this Window window, WindowLongIndexFlags nIndex, SetWindowLongFlags dwNewLong)
        => SetWindowLong(window.GetWindowHandle(), nIndex, dwNewLong);

    internal static IntPtr GetWindowHandle(this Window window)
        => WinRT.Interop.WindowNative
            .GetWindowHandle(window);

    public static AppWindow GetAppWindow(this Window window)
        => Microsoft.UI.Windowing.AppWindow.GetFromWindowId(Microsoft.UI.Win32Interop.GetWindowIdFromWindow(window.GetWindowHandle()));

    public static void SetEnableWindow(Window applicationWindow, bool enable)
        => SetEnableWindows(new List<Window>() { applicationWindow }, enable);

    public static void SetEnableWindows(this List<Window> applicationWindows, bool enable)
    {
        foreach (var item in applicationWindows)
        {
            item.EnableWindow(enable);
            if (item is IModalWindowEventHandler handler)
            {
                if (enable)
                    handler.Enabled();
                else
                    handler.Disabled();
            }
        }
    }
}

public static class ModalWindowExtensions
{
    public static bool EnableWindow(this Window window, bool enabled)
        => InternalModalWindowExtensions.EnableWindow(window.GetWindowHandle(), enabled);
}

public static class WindowExtensions
{
    public static PointInt32 GetPosition(this Window window)
        => window.GetAppWindow().Position;

    public static SizeInt32 GetSize(this Window window)
    => window.GetAppWindow().Size;

    public static PointInt32 PositionModalInCenter(this Window window, SizeInt32 modalSize)
    {
        var parentWindowPosition = window.GetPosition();
        var parentSize = window.GetSize();

        var centerX = parentWindowPosition.X + (parentSize.Width / 2);
        var centerY = parentWindowPosition.Y + (parentSize.Height / 2);

        var leftX = centerX - (modalSize.Width / 2);
        var leftY = centerY - (modalSize.Height / 2);
        return new PointInt32(leftX, leftY);
    }

    public static void ExtendsContentIntoAppTitleBar(this Window window, bool value)
    {
        var appWindowTitleBar = window.GetAppWindow().TitleBar;
        appWindowTitleBar.ExtendsContentIntoTitleBar = value;
    }
}

[Flags]
enum SetWindowLongFlags : uint
{
    WS_OVERLAPPED = 0,
    WS_POPUP = 0x80000000,
    WS_CHILD = 0x40000000,
    WS_MINIMIZE = 0x20000000,
    WS_VISIBLE = 0x10000000,
    WS_DISABLED = 0x8000000,
    WS_CLIPSIBLINGS = 0x4000000,
    WS_CLIPCHILDREN = 0x2000000,
    WS_MAXIMIZE = 0x1000000,
    WS_CAPTION = 0xC00000,
    WS_BORDER = 0x800000,
    WS_DLGFRAME = 0x400000,
    WS_VSCROLL = 0x200000,
    WS_HSCROLL = 0x100000,
    WS_SYSMENU = 0x80000,
    WS_THICKFRAME = 0x40000,
    WS_GROUP = 0x20000,
    WS_TABSTOP = 0x10000,
    WS_MINIMIZEBOX = 0x20000,
    WS_MAXIMIZEBOX = 0x10000,
    WS_TILED = WS_OVERLAPPED,
    WS_ICONIC = WS_MINIMIZE,
    WS_SIZEBOX = WS_THICKFRAME,
    WS_EX_DLGMODALFRAME = 0x0001,
    WS_EX_NOPARENTNOTIFY = 0x0004,
    WS_EX_TOPMOST = 0x0008,
    WS_EX_ACCEPTFILES = 0x0010,
    WS_EX_TRANSPARENT = 0x0020,
    WS_EX_MDICHILD = 0x0040,
    WS_EX_TOOLWINDOW = 0x0080,
    WS_EX_WINDOWEDGE = 0x0100,
    WS_EX_CLIENTEDGE = 0x0200,
    WS_EX_CONTEXTHELP = 0x0400,
    WS_EX_RIGHT = 0x1000,
    WS_EX_LEFT = 0x0000,
    WS_EX_RTLREADING = 0x2000,
    WS_EX_LTRREADING = 0x0000,
    WS_EX_LEFTSCROLLBAR = 0x4000,
    WS_EX_RIGHTSCROLLBAR = 0x0000,
    WS_EX_CONTROLPARENT = 0x10000,
    WS_EX_STATICEDGE = 0x20000,
    WS_EX_APPWINDOW = 0x40000,
    WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
    WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
    WS_EX_LAYERED = 0x00080000,
    WS_EX_NOINHERITLAYOUT = 0x00100000,
    WS_EX_LAYOUTRTL = 0x00400000,
    WS_EX_COMPOSITED = 0x02000000,
    WS_EX_NOACTIVATE = 0x08000000,
}

[Flags]
enum WindowLongIndexFlags : int
{
    GWL_EXSTYLE = -20,
    GWLP_HINSTANCE = -6,
    GWLP_HWNDPARENT = -8,
    GWL_ID = -12,
    GWLP_ID = GWL_ID,
    GWL_STYLE = -16,
    GWL_USERDATA = -21,
    GWLP_USERDATA = GWL_USERDATA,
    GWL_WNDPROC = -4,
    GWLP_WNDPROC = GWL_WNDPROC,
    DWLP_USER = 0x8,
    DWLP_MSGRESULT = 0x0,
    DWLP_DLGPROC = 0x4,
}