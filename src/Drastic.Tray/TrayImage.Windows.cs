// <copyright file="TrayImage.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Drawing.Imaging;

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
        /// <param name="image">System.Drawing.Icon.</param>
        public TrayImage(System.Drawing.Image image)
        {
            this.Image = image;
        }

        public System.Drawing.Image Image { get; }
    }
}
