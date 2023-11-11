// <copyright file="CFInterop.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using CoreFoundation;
using ObjCRuntime;

namespace Drastic.Interop;

internal class CFInterop
{
    internal const string CoreFoundationLibrary = "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";
    internal static NativeHandle KCFRunLoopCommonModes = CFString.CreateNative("kCFRunLoopCommonModes");

    [Flags]
    internal enum CFOptionFlags : ulong
    {
        KCFRunLoopBeforeSources = 1UL << 2,
        KCFRunLoopAfterWaiting = 1UL << 6,
        KCFRunLoopBeforeWaiting = 1UL << 5,
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate void CFRunLoopObserverCallback(IntPtr observer, CFOptionFlags activity, IntPtr info);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate void CFRunLoopTimerCallback(IntPtr timer, IntPtr info);

    [DllImport(CoreFoundationLibrary)]
    internal static extern IntPtr CFRunLoopGetMain();

    [DllImport(CoreFoundationLibrary)]
    internal static extern IntPtr CFRunLoopObserverCreate(IntPtr allocator, CFOptionFlags activities,
        bool repeats, int index, CFRunLoopObserverCallback callout, IntPtr context);

    [DllImport(CoreFoundationLibrary)]
    internal static extern IntPtr CFRunLoopAddObserver(IntPtr loop, IntPtr observer, IntPtr mode);

    [DllImport(CoreFoundationLibrary)]
    internal static extern IntPtr CFRunLoopRemoveObserver(IntPtr loop, IntPtr observer, IntPtr mode);

    [DllImport(CoreFoundationLibrary)]
    internal static extern IntPtr CFRunLoopTimerCreate(IntPtr allocator, double firstDate, double interval,
        CFOptionFlags flags, int order, CFRunLoopTimerCallback callout, IntPtr context);

    [DllImport(CoreFoundationLibrary)]
    internal static extern void CFRunLoopTimerSetTolerance(IntPtr timer, double tolerance);

    [DllImport(CoreFoundationLibrary)]
    internal static extern void CFRunLoopTimerSetNextFireDate(IntPtr timer, double fireDate);

    [DllImport(CoreFoundationLibrary)]
    internal static extern void CFRunLoopAddTimer(IntPtr loop, IntPtr timer, IntPtr mode);

    [DllImport(CoreFoundationLibrary)]
    internal static extern double CFAbsoluteTimeGetCurrent();
}