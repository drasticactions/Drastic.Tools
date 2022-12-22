// <copyright file="UINSWindow.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.Interop;

namespace Drastic.TrayWindow
{
    internal class UINSWindow
    {
        public UINSWindow(NSObject nsWindow, UIWindow uiWindow)
        {
            this.NSWindow = nsWindow;
            this.UIWindow = uiWindow;
        }

        public NSObject NSWindow { get; }

        public UIWindow UIWindow { get; }

        public CGRect? Frame
        {
            get
            {
                var attachedWindow = this.NSWindow.ValueForKey(new Foundation.NSString("attachedWindow"));

                if (attachedWindow is null)
                {
                    return null;
                }

                var windowFrame = (NSValue)attachedWindow.ValueForKey(new Foundation.NSString("frame"));

                return windowFrame.CGRectValue;
            }
        }

        public NSWindowLevel Level
        {
            get
            {
                return (NSWindowLevel)(nint)Drastic.Interop.ObjC.Call(this.NSWindow.Handle, "level");
            }

            set
            {
                Drastic.Interop.ObjC.Call(this.NSWindow.Handle, "setLevel:", (nint)value);
            }
        }

        public void SetFrame(CGRect rect, bool display = true, bool animate = true)
        {
            var attachedWindow = this.NSWindow.ValueForKey(new Foundation.NSString("attachedWindow"));

            if (attachedWindow is null)
            {
                return;
            }

            var windowFrame = (NSValue)attachedWindow.ValueForKey(new Foundation.NSString("frame"));

            ObjC.Call(attachedWindow.Handle, "setFrame:display:animate:", rect, display, animate);
        }

        public void Hide()
        {

        }
    }
}