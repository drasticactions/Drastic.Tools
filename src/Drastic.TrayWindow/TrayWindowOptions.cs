// <copyright file="TrayWindowOptions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.TrayWindow
{
    /// <summary>
    /// Tray Window Options.
    /// </summary>
    public class TrayWindowOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrayWindowOptions"/> class.
        /// </summary>
        /// <param name="width">The width of the window.</param>
        /// <param name="height">The height of the window.</param>
        public TrayWindowOptions(int width = 400, int height = 480)
        {
            this.WindowWidth = width;
            this.WindowHeight = height;
        }

        /// <summary>
        /// Gets the window width.
        /// </summary>
        public int WindowWidth { get; }

        /// <summary>
        /// Gets the window height.
        /// </summary>
        public int WindowHeight { get; }

        /// <summary>
        /// Gets a value indicating whether to set the icon to match the system theme.
        /// </summary>
        public bool SetToSystemTheme { get; }
    }
}