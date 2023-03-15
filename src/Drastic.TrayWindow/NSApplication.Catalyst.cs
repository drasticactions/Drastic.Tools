// <copyright file="NSApplication.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using ObjCRuntime;

namespace Drastic.TrayWindow
{
    /// <summary>
    /// NSApplication Helpers.
    /// </summary>
    public static class NSApplication
    {
        /// <summary>
        /// Gets the shared NSApplication instance.
        /// </summary>
        /// <returns>NSObject.</returns>
        internal static NSObject GetSharedApplication()
        {
            var nsApplication = Runtime.GetNSObject(Class.GetHandle("NSApplication"))!;
            var sharedApplication = nsApplication.PerformSelector(new Selector("sharedApplication"))!;
            return sharedApplication;
        }

        /// <summary>
        /// Hides all the windows in a given application.
        /// </summary>
        /// <param name="window">The sender UIWindow to invoke the command.</param>
        public static async void Hide(UIWindow window)
        {
            var sharedApplication = GetSharedApplication();
            var nsWindow = await window.GetNSWindowFromUIWindowAsync();
            NativeHandle nonNullHandle = nsWindow!.GetNonNullHandle("sender");
            Drastic.Interop.ObjC.Call(sharedApplication.Handle, "hide:", nonNullHandle);
        }

        /// <summary>
        /// Close window in a given application.
        /// </summary>
        /// <param name="window">The sender UIWindow to invoke the command.</param>
        public static async Task CloseWindow(UIWindow window)
        {
            var sharedApplication = GetSharedApplication();
            var nsWindow = await window.GetNSWindowFromUIWindowAsync();
            NativeHandle nonNullHandle = nsWindow!.GetNonNullHandle("sender");
            Drastic.Interop.ObjC.Call(nsWindow!.Handle, "performClose:", nonNullHandle);
        }

        /// <summary>
        /// Sets the activaction policy on a Mac Catalyst app.
        /// </summary>
        /// <param name="policy">The sender UIWindow to invoke the command.</param>
        public static void SetActivationPolicy(NSApplicationActivationPolicy policy)
        {
            var sharedApplication = GetSharedApplication();
            Drastic.Interop.ObjC.Call(sharedApplication.Handle, "setActivationPolicy:", (nint)policy);
        }

        /// <summary>
        /// Terminates a Catalyst application.
        /// </summary>
        public static void Terminate()
        {
            var sharedApplication = GetSharedApplication();
            NativeHandle nonNullHandle = sharedApplication!.GetNonNullHandle("sender");
            Drastic.Interop.ObjC.Call(sharedApplication.Handle, "terminate:", nonNullHandle);
        }
    }
}