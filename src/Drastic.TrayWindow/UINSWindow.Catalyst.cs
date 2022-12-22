// <copyright file="UINSWindow.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.Interop;

namespace Drastic.TrayWindow
{
    /// <summary>
    /// UINSWindow is the underlying NSWindow contained within a UIWindow.
    /// This is used to poke at the underlying Window element to get access to additional features.
    /// </summary>
    internal class UINSWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UINSWindow"/> class.
        /// </summary>
        /// <param name="nsWindow">The NSWindow instance.</param>
        /// <param name="uiWindow">The UIWindow.</param>
        public UINSWindow(NSObject nsWindow, UIWindow uiWindow)
        {
            this.NSWindow = nsWindow;
            this.UIWindow = uiWindow;
        }

        /// <summary>
        /// Gets the NSWindow of the given UIWindow.
        /// </summary>
        public NSObject NSWindow { get; }

        /// <summary>
        /// Gets the UIWindow.
        /// </summary>
        public UIWindow UIWindow { get; }

        /// <summary>
        /// Gets the internal NSWindow Frame.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Level of the given window.
        /// <see cref="NSWindowLevel"/>.
        /// </summary>
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

        /// <summary>
        /// Sets the given NSWindow Frame.
        /// </summary>
        /// <param name="rect">The size of the new frame.</param>
        /// <param name="display">Should display the frame.</param>
        /// <param name="animate">Should animate the change.</param>
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
    }
}