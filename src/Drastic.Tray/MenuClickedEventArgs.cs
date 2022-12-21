// <copyright file="MenuClickedEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.Tray
{
    /// <summary>
    /// Drastic Tray Menu Clicked.
    /// </summary>
    public class MenuClickedEventArgs : EventArgs
    {
        private TrayMenuItem menuItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuClickedEventArgs"/> class.
        /// </summary>
        /// <param name="item">Position.</param>
        internal MenuClickedEventArgs(TrayMenuItem item)
        {
            this.menuItem = item;
        }

        /// <summary>
        /// Gets the DrasticTrayMenuItem.
        /// </summary>
        public TrayMenuItem MenuItem => this.menuItem;
    }
}
