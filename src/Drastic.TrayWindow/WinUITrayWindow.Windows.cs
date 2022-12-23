// <copyright file="WinUITrayWindow.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Windows.Forms;
using Drastic.Tray;
using Microsoft.UI.Windowing;

namespace Drastic.TrayWindow
{
    /// <summary>
    /// WinUI Tray Window.
    /// </summary>
    public class WinUITrayWindow : Microsoft.UI.Xaml.Window
    {
        private Microsoft.UI.Windowing.AppWindow appWindow;
        private bool appLaunched;
        private TrayIcon icon;
        private TrayWindowOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUITrayWindow"/> class.
        /// </summary>
        /// <param name="icon">Tray Icon.</param>
        /// <param name="options">Tray Icon Options.</param>
        public WinUITrayWindow(TrayIcon icon, TrayWindowOptions options)
        {
            this.icon = icon;
            this.options = options;
            this.appWindow = this.GetAppWindowForWinUI();
            if (this.appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = false;
                presenter.IsAlwaysOnTop = true;
                presenter.IsMaximizable = false;
                presenter.IsMinimizable = false;
                presenter.SetBorderAndTitleBar(false, false);
            }

            this.appWindow.Hide();
        }

        /// <summary>
        /// Toggle visibility of the window.
        /// </summary>
        public void ToggleVisibility()
        {
            if (this.appWindow.IsVisible)
            {
                this.HideWindow();
            }
            else
            {
                this.ShowWindow();
            }
        }

        private void ShowWindow()
        {
            // The cursor is, most likely, positioned next to the tray icon.
            var point = Cursor.Position;
            var x = point.X - (this.options.WindowWidth / 2);
            var y = point.Y - (this.options.WindowHeight + 50);
            var width = this.options.WindowWidth;
            var height = this.options.WindowHeight;

            this.appWindow.MoveAndResize(new Windows.Graphics.RectInt32(x, y, width, height));
            this.appWindow.Show();
        }

        private void HideWindow()
        {
            this.appWindow.Hide();
        }
    }
}
