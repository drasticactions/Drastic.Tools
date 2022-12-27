// <copyright file="WinUITrayAcrylicWindow.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.Tray;
using Microsoft.UI.Composition.SystemBackdrops;

namespace Drastic.TrayWindow
{
    /// <summary>
    /// Acrylic Window.
    /// </summary>
    public class WinUITrayAcrylicWindow : WinUITrayWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinUITrayAcrylicWindow"/> class.
        /// </summary>
        /// <param name="config"><see cref="SystemBackdropConfiguration"/>, Optional.</param>
        public WinUITrayAcrylicWindow(TrayIcon icon, TrayWindowOptions options, bool hideWindowFromTaskbar = true, SystemBackdropConfiguration? config = null)
            : base(icon, options, hideWindowFromTaskbar, new DesktopAcrylicController(), config)
        {
        }
    }
}
