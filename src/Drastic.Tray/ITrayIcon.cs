// <copyright file="ITrayIcon.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.Tray
{
    /// <summary>
    /// Tray Icon.
    /// </summary>
    public interface ITrayIcon
    {
        /// <summary>
        /// Replace an existing tray icon's menu with new items.
        /// </summary>
        /// <param name="items">Items to update.</param>
        void UpdateMenu(IEnumerable<TrayMenuItem> items);

        /// <summary>
        /// Update the tray icon image.
        /// </summary>
        /// <param name="image">Image to update.</param>
        void UpdateImage(TrayImage image);

        /// <summary>
        /// Update the icons name.
        /// </summary>
        /// <param name="name">Name to update to.</param>
        void UpdateName(string name);
    }
}