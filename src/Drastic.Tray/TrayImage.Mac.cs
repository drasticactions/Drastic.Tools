// <copyright file="TrayImage.Mac.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using AppKit;

namespace Drastic.Tray
{
    /// <summary>
    /// Tray Image.
    /// </summary>
    public partial class TrayImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrayImage"/> class.
        /// </summary>
        /// <param name="image">NSImage.</param>
        public TrayImage(NSImage image)
        {
            this.Image = image;
        }

        /// <summary>
        /// Gets the underlying NSImage for the tray image.
        /// </summary>
        public NSImage Image { get; }
    }
}
