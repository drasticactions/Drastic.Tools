// <copyright file="TrayImage.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using AppKit;

namespace Drastic.Tray
{
    public class TrayImage
    {
        public TrayImage(NSImage image)
        {
            this.Image = image;
        }

        public NSImage Image { get; }
    }
}
