// <copyright file="TrayWindowOptions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.TrayWindow
{
    public class TrayWindowOptions
    {
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
    }
}