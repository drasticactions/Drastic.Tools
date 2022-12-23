// <copyright file="TrayImage.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

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
        /// <param name="image">UIImage to use for the tray icon.</param>
        public TrayImage(UIImage image)
        {
            var imageStream = Foundation.NSData.FromStream(image.AsPNG().AsStream())!;
            this.Image = Runtime.GetNSObject<AppKit.NSImage>(IntPtr_objc_msgSend(ObjCRuntime.Class.GetHandle("NSImage"), Selector.GetHandle("alloc")))!;
            IntPtr_objc_msgSend_IntPtr(this.Image.Handle, Selector.GetHandle("initWithData:"), imageStream.Handle);
        }

        public AppKit.NSImage Image { get; }

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
#pragma warning disable SA1600 // Elements should be documented
        internal static extern IntPtr IntPtr_objc_msgSend(IntPtr receiver, IntPtr selector);
#pragma warning restore SA1600 // Elements should be documented

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
#pragma warning disable SA1600 // Elements should be documented
        internal static extern IntPtr IntPtr_objc_msgSend_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1);
#pragma warning restore SA1600 // Elements should be documented
    }
}