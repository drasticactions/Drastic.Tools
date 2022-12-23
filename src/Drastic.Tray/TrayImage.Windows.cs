// <copyright file="TrayImage.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.Tray
{
    /// <summary>
    /// Tray Image.
    /// </summary>
    public class TrayImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrayImage"/> class.
        /// </summary>
        /// <param name="image">NSImage.</param>
        public TrayImage(Stream image)
        {
            this.Image = image;
        }

        public Stream Image { get; }
    }
}
