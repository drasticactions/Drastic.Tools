// <copyright file="WinUITrayMicaWindow.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.Tray;
using Microsoft.UI.Composition.SystemBackdrops;

namespace Drastic.TrayWindow
{
    /// <summary>
    /// Mica Window.
    /// </summary>
    public class WinUITrayMicaWindow : WinUITrayWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinUITrayMicaWindow"/> class.
        /// </summary>
        /// <param name="config"><see cref="SystemBackdropConfiguration"/>, Optional.</param>
        public WinUITrayMicaWindow(TrayIcon icon, TrayWindowOptions options, bool hideWindowFromTaskbar = true, SystemBackdropConfiguration? config = null)
            : base(icon, options, hideWindowFromTaskbar, new MicaController(), config)
        {
        }
    }
}
