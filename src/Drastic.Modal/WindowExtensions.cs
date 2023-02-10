// <copyright file="WindowExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.UI.Xaml;
using Windows.Graphics;

namespace Drastic.Modal;

/// <summary>
/// Window Extensions.
/// </summary>
public static class WindowExtensions
{
    /// <summary>
    /// Get Position of Window.
    /// </summary>
    /// <param name="window">Window.</param>
    /// <returns>Position as <see cref="PointInt32"/>.</returns>
    public static PointInt32 GetPosition(this Window window)
        => window.GetAppWindow().Position;

    /// <summary>
    /// Gets Size of Window.
    /// </summary>
    /// <param name="window">Window.</param>
    /// <returns>Size as <see cref="SizeInt32"/>.</returns>
    public static SizeInt32 GetSize(this Window window)
    => window.GetAppWindow().Size;

    /// <summary>
    /// Get position of where to set a modal window so it's centered.
    /// </summary>
    /// <param name="window">Parent Window.</param>
    /// <param name="modalSize">Modal Size.</param>
    /// <returns>Position as <see cref="PointInt32"/>.</returns>
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

    /// <summary>
    /// Extends Content Into App Title Bar.
    /// </summary>
    /// <param name="window">Base Window.</param>
    /// <param name="value">Set to extend into title bar.</param>
    public static void ExtendsContentIntoAppTitleBar(this Window window, bool value)
    {
        var appWindowTitleBar = window.GetAppWindow().TitleBar;
        appWindowTitleBar.ExtendsContentIntoTitleBar = value;
    }
}
