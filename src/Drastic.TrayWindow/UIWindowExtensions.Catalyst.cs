// <copyright file="UIWindowExtensions.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.Interop;
using ObjCRuntime;

namespace Drastic.TrayWindow
{
    internal static class UIWindowExtensions
    {
        public static async Task Hide(this UIWindow window)
        {
            var uinswindow = await window.ToUINSWindowAsync()!;
        }

        internal static async Task ToggleTitleBarButtons(this UIWindow window, bool hideButtons)
        {
            var uinswindow = await window.ToUINSWindowAsync();
            if (uinswindow is null)
            {
                return;
            }

            var closeButton = Runtime.GetNSObject(Drastic.Interop.ObjC.Call(uinswindow.NSWindow.Handle, "standardWindowButton:", 0));

            if (closeButton is null)
            {
                return;
            }

            var miniaturizeButton = Runtime.GetNSObject(Drastic.Interop.ObjC.Call(uinswindow.NSWindow.Handle, "standardWindowButton:", 1));
            if (miniaturizeButton is null)
            {
                return;
            }

            var zoomButton = Runtime.GetNSObject(Drastic.Interop.ObjC.Call(uinswindow.NSWindow.Handle, "standardWindowButton:", 2));

            if (zoomButton is null)
            {
                return;
            }

            Drastic.Interop.ObjC.Call(closeButton.Handle, "isHidden", hideButtons);
            Drastic.Interop.ObjC.Call(miniaturizeButton.Handle, "isHidden", hideButtons);
            Drastic.Interop.ObjC.Call(zoomButton.Handle, "isHidden", hideButtons);
        }

        internal static void SetFrameForUIWindow(this UINSWindow uinswindow, CGRect rect)
        {
            var attachedWindow = uinswindow.NSWindow.ValueForKey(new Foundation.NSString("attachedWindow"));

            if (attachedWindow is null)
            {
                return;
            }

            var windowFrame = (NSValue)attachedWindow.ValueForKey(new Foundation.NSString("frame"));

            var originalOne = windowFrame.CGRectValue;

            var test = new CGRect(rect.X + (rect.Width / 2), rect.Y, originalOne.Width, originalOne.Height);

            Drastic.Interop.ObjC.Call(attachedWindow.Handle, "setFrame:display:animate:", test, true, true);

            windowFrame = (NSValue)attachedWindow.ValueForKey(new Foundation.NSString("frame"));
        }

        /// <summary>
        /// Get NSWindow from UIWindow.
        /// </summary>
        /// <param name="window">UIWindow.</param>
        /// <returns><see cref="UINSWindow"/>.</returns>
        internal static async Task<UINSWindow?> ToUINSWindowAsync(this UIWindow window)
        {
            var nsWindow = await window.GetNSWindowFromUIWindowAsync();
            return nsWindow is null ? null : new UINSWindow(nsWindow, window);
        }

        /// <summary>
        /// Get NSWindow from UIWindow.
        /// </summary>
        /// <param name="window">UIWindow.</param>
        /// <returns>NSWindow as NSObject.</returns>
        internal static async Task<NSObject?> GetNSWindowFromUIWindowAsync(this UIWindow window)
        {
            if (window is null)
            {
                return null;
            }

            var sharedApplication = NSApplication.GetSharedApplication();

            var applicationDelegate = sharedApplication.PerformSelector(new Selector("delegate"));
            if (applicationDelegate is null)
            {
                return null;
            }

            return await GetNSWindowAsync(window, applicationDelegate);
        }

        internal static async Task<NSObject?> GetNSWindowAsync(UIWindow window, NSObject applicationDelegate)
        {
            var nsWindowHandle = ObjC.Call(applicationDelegate.Handle, "hostWindowForUIWindow:", window.Handle);
            var nsWindow = Runtime.GetNSObject<NSObject>(nsWindowHandle);
            if (nsWindow is null)
            {
                await Task.Delay(500);
                return await GetNSWindowAsync(window, applicationDelegate);
            }

            return nsWindow;
        }
    }
}