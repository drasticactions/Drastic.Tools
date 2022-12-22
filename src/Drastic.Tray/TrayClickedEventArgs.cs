// <copyright file="TrayClickedEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.Tray
{
    /// <summary>
    /// Tray Clicked Event Arguments.
    /// </summary>
    public class TrayClickedEventArgs : EventArgs
    {
        /// <summary>
        /// Empty Event Arguments.
        /// </summary>
        public static new readonly TrayClickedEventArgs Empty = new TrayClickedEventArgs();
    }
}