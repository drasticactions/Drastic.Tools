// <copyright file="Dispatch.Apple.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Drastic.Interop;

namespace Drastic.Interop
{
    internal static class Dispatch
    {
        private const string DispatchLib = "/usr/lib/system/libdispatch.dylib";
        private const string SystemLib = "/usr/lib/libSystem.dylib";

        private static readonly IntPtr MainQueueHandle = IntPtr.Zero;
        private static readonly IntPtr DispatchLibHandle = IntPtr.Zero;

        static Dispatch()
        {
            // dispatch_get_main_queue is not exposed (macro?) so we need to access the main queue field instead
            DispatchLibHandle = LoadLibrary(DispatchLib, 0);
            MainQueueHandle = GetSymbol(DispatchLibHandle, "_dispatch_main_q");
        }

        public static IntPtr MainQueue
        {
            get { return MainQueueHandle; }
        }

        [DllImport(DispatchLib, EntryPoint = "dispatch_sync_f")]
        public static extern void SyncFunction(IntPtr queue, IntPtr context, DispatchDelegate work);

        [DllImport(DispatchLib, EntryPoint = "dispatch_async_f")]
        public static extern void AsyncFunction(IntPtr queue, IntPtr context, DispatchDelegate work);

        [DllImport(SystemLib, EntryPoint = "dlopen", CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary(string path, int mode);

        [DllImport(SystemLib, EntryPoint = "dlclose")]
        private static extern int ReleaseLibrary(IntPtr handle);

        [DllImport(SystemLib, EntryPoint = "dlsym", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetSymbol(IntPtr handle, string symbol);
    }
}