// <copyright file="TrayIcon.Mac.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using AppKit;

namespace Drastic.Tray
{
    /// <summary>
    /// Tray Icon.
    /// </summary>
    public partial class TrayIcon
    {
        private TrayImage? iconImage;
        private NSStatusItem statusBarItem;
        private NSMenu menu = new NSMenu();

        /// <summary>
        /// Initializes a new instance of the <see cref="TrayIcon"/> class.
        /// </summary>
        /// <param name="name">Icon Title.</param>
        /// <param name="image">Icon Image Stream.</param>
        /// <param name="menuItems">Items to populate context menu. Optional.</param>
        /// <param name="setToSystemTheme">Sets the icon to match the system theme.</param>
        public TrayIcon(string name, TrayImage image, List<TrayMenuItem>? menuItems = null, bool setToSystemTheme = true)
        {
            this.menuItems = menuItems ?? new List<TrayMenuItem>();
            this.iconImage = image;

            // Create the status bar item
            NSStatusBar statusBar = NSStatusBar.SystemStatusBar;
            this.statusBarItem = statusBar.CreateStatusItem(NSStatusItemLength.Variable);

            this.statusBarItem!.Button.ToolTip = name ?? string.Empty;
            this.statusBarItem!.Button.Image = image?.Image!;
            this.statusBarItem!.Button.Image.Size = new CGSize(20, 20);
            this.statusBarItem!.Button.Frame = new CGRect(0, 0, 40, 24);
            this.statusBarItem!.Button.Image.Template = setToSystemTheme;

            // Listen to touches on the status bar item
            this.statusBarItem.Button.SendActionOn(NSEventType.OtherMouseUp);
            this.statusBarItem.Button.Activated += this.StatusItemActivated;

            if (menuItems is not null)
            {
                this.SetupStatusBarMenu(menuItems);
            }
        }

        /// <summary>
        /// Gets the Status Bar.
        /// </summary>
        public NSStatusItem StatusBarItem => this.statusBarItem;

        /// <summary>
        /// Opens the menu.
        /// </summary>
        public void OpenMenu()
        {
#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1422 // Validate platform compatibility
            this.statusBarItem!.PopUpStatusItemMenu(this.menu);
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning disable CA1422 // Validate platform compatibility
        }

        /// <summary>
        /// Set up the list of status bar menu items.
        /// Clears out the existing list of items and replaces them.
        /// </summary>
        /// <param name="menuItems">List of TrayMenuItems.</param>
        public void SetupStatusBarMenu(List<TrayMenuItem> menuItems)
        {
            this.menu.RemoveAllItems();

            foreach (var item in this.menuItems)
            {
                var menuItem = new NSMenuItem(item.Text);
                var icon = item.Icon;
                if (icon?.Image is not null)
                {
                    menuItem.Image = icon.Image;
                }

                menuItem.KeyEquivalent = item.KeyEquivalent ?? string.Empty;
                if (item.KeyEquivalentModifierMask is not null)
                {
                    menuItem.KeyEquivalentModifierMask = (NSEventModifierMask)item.KeyEquivalentModifierMask;
                }

                menuItem.Activated += (object? sender, EventArgs e) => { item.Action?.Invoke(); };
                this.menu.AddItem(menuItem);
            }
        }

        private void StatusItemActivated(object? sender, EventArgs e)
        {
            var currentEvent = NSApplication.SharedApplication.CurrentEvent;
            switch (currentEvent.Type)
            {
                case NSEventType.LeftMouseDown:
                    this.LeftClicked?.Invoke(this, TrayClickedEventArgs.Empty);
                    break;
                case NSEventType.RightMouseDown:
                    this.RightClicked?.Invoke(this, TrayClickedEventArgs.Empty);
                    break;
            }
        }

        private void NativeElementDispose()
        {
            this.statusBarItem?.Dispose();
        }
    }
}
