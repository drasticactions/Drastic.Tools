// <copyright file="WinUITrayWindow.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using System.Windows.Forms;
using Drastic.Tray;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using WinRT.Interop;

namespace Drastic.TrayWindow
{
    /// <summary>
    /// WinUI Tray Window.
    /// </summary>
    public class WinUITrayWindow : BaseWindow
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
        /// <param name="hideWindowFromTaskbar">Hides the Tray Window from the Taskbar and Alt-Tab. Defaults to True.</param>
        public WinUITrayWindow(TrayIcon icon, TrayWindowOptions options, bool hideWindowFromTaskbar = true,
            ISystemBackdropControllerWithTargets? controller = default,
            SystemBackdropConfiguration? config = default)
            : base(controller, config)
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

            if (hideWindowFromTaskbar)
            {
                IntPtr hWnd = WindowNative.GetWindowHandle(this);
                int exStyle = (int)GetWindowLong(hWnd, (int)GetWindowLongFields.GWL_EXSTYLE);
                exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
                SetWindowLong(hWnd, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
            }
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

        // From https://stackoverflow.com/questions/74261765/remove-the-window-from-the-taskbar-in-winui-3
        #region Window styles
        [Flags]
        private enum ExtendedWindowStyles
        {
            // ...
            WS_EX_TOOLWINDOW = 0x00000080,
            // ...
        }

        private enum GetWindowLongFields
        {
            // ...
            GWL_EXSTYLE = (-20),
            // ...
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        private static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        private static extern void SetLastError(int dwErrorCode);
        #endregion
    }
}
