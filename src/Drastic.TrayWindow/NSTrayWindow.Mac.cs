// <copyright file="NSTrayWindow.Mac.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.Tray;

namespace Drastic.TrayWindow
{
    /// <summary>
    /// NSTrayWindow.
    /// The Tray Window for macOS apps.
    /// </summary>
    public class NSTrayWindow
    {
        private TrayIcon icon;
        private TrayWindowOptions options;
        private NSPopover popover;

        /// <summary>
        /// Initializes a new instance of the <see cref="NSTrayWindow"/> class.
        /// </summary>
        /// <param name="icon"><see cref="TrayIcon"/>.</param>
        /// <param name="options"><see cref="TrayWindowOptions"/>.</param>
        /// <param name="contentViewController">The NSViewController used in the hosted TrayWindow.</param>
        public NSTrayWindow(TrayIcon icon, TrayWindowOptions options, NSViewController? contentViewController = default)
        {
            this.icon = icon;
            this.options = options;

            this.popover = new NSPopover()
            {
                ContentViewController = new NSViewController(),
                Behavior = NSPopoverBehavior.ApplicationDefined,
                Delegate = new NSPopoverDelegate(),
            };

            if (contentViewController is not null)
            {
                this.SetContent(contentViewController);
            }
        }

        /// <summary>
        /// Toggle Visibility of the TrayWindow.
        /// </summary>
        public void ToggleVisibility()
        {
            if (this.popover.Shown)
            {
                this.popover.Close();
            }
            else
            {
                this.popover.ContentViewController.View.Frame = new CoreGraphics.CGRect(0, 0, this.options.WindowWidth, this.options.WindowHeight);
                this.popover.Show(this.icon.StatusBarItem.Button.Bounds, this.icon.StatusBarItem.Button, NSRectEdge.MaxYEdge);
            }
        }

        /// <summary>
        /// Set the internal content view of the TrayWindow.
        /// </summary>
        /// <param name="contentViewController">The NSViewController to use in the hosted frame.</param>
        public void SetContent(NSViewController contentViewController)
        {
            this.popover.ContentViewController = contentViewController;
        }
    }
}