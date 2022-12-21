// <copyright file="TrayImage.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

namespace Drastic.Tray
{
    public class TrayImage
    {
        public TrayImage(UIImage image)
        {
            var imageStream = Foundation.NSData.FromStream(image.AsPNG().AsStream())!;
            this.Image = Runtime.GetNSObject<AppKit.NSImage>(IntPtr_objc_msgSend(ObjCRuntime.Class.GetHandle("NSImage"), Selector.GetHandle("alloc")))!;
            IntPtr_objc_msgSend_IntPtr(this.Image.Handle, Selector.GetHandle("initWithData:"), imageStream.Handle);
        }

        public AppKit.NSImage Image { get; }

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        internal static extern IntPtr IntPtr_objc_msgSend(IntPtr receiver, IntPtr selector);

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        internal static extern IntPtr IntPtr_objc_msgSend_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1);
    }
}