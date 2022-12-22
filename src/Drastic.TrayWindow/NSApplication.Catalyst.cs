// <copyright file="NSApplication.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using ObjCRuntime;

namespace Drastic.TrayWindow
{
    public static class NSApplication
    {
        public static NSObject GetSharedApplication()
        {
            var nsApplication = Runtime.GetNSObject(Class.GetHandle("NSApplication"))!;
            var sharedApplication = nsApplication.PerformSelector(new Selector("sharedApplication"))!;
            return sharedApplication;
        }

        public static async void Hide(UIWindow window)
        {
            var sharedApplication = GetSharedApplication();
            var nsWindow = await window.GetNSWindowFromUIWindowAsync();
            NativeHandle nonNullHandle = nsWindow!.GetNonNullHandle("sender");
            Drastic.Interop.ObjC.Call(sharedApplication.Handle, "hide:", nonNullHandle);
        }
    }
}