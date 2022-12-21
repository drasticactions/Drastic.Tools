// <copyright file="TrayMenuItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.Tray
{
    /// <summary>
    /// Drastic Tray Menu Item.
    /// </summary>
    public class TrayMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrayMenuItem"/> class.
        /// </summary>
        /// <param name="text">Menu Text.</param>
        /// <param name="icon">Icon.</param>
        /// <param name="action">Action to perform when clicked.</param>
        public TrayMenuItem(string text, TrayImage? icon = null, Func<Task>? action = null)
        {
            this.Text = text;
            this.Icon = icon;
            this.Action = action;
        }

#if MACOS || MACCATALYST

        /// <summary>
        /// Initializes a new instance of the <see cref="TrayMenuItem"/> class.
        /// </summary>
        /// <param name="text">Menu Text.</param>
        /// <param name="icon">Icon.</param>
        /// <param name="action">Action to perform when clicked.</param>
        /// <param name="keyEquivalent">Keyboard Shortcut key.</param>
        /// <param name="keyEquivalentModifierMask">Key.</param>
        public TrayMenuItem(string text, TrayImage? icon = null, Func<Task>? action = null, string? keyEquivalent = default, NSEventModifierMask? keyEquivalentModifierMask = default)
        {
            this.Text = text;
            this.Icon = icon;
            this.Action = action;
            this.KeyEquivalent = keyEquivalent;
            this.KeyEquivalentModifierMask = keyEquivalentModifierMask;
        }

#endif

        /// <summary>
        /// Gets the text for the menu item.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the icon for the menu item.
        /// Optional.
        /// </summary>
        public TrayImage? Icon { get; }

        /// <summary>
        /// Gets the action to be performed when the item is clicked.
        /// Optional.
        /// </summary>
        public Func<Task>? Action { get; }

#if MACOS || MACCATALYST

        /// <summary>
        /// Gets the Key Equivalent shortcut.
        /// Optional.
        /// </summary>
        public string? KeyEquivalent { get; }

        /// <summary>
        /// Gets the Key Equivalent Modifer Mask shortcut.
        /// Optional.
        /// </summary>
        public NSEventModifierMask? KeyEquivalentModifierMask { get; }

#endif
    }
}
