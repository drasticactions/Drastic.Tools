// <copyright file="FPSViewer.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using CoreFoundation;
using Drastic.Interop;
using Microsoft.Extensions.Logging;
using ObjCRuntime;
using CFTimeInterval = System.Double;

namespace Drastic.UITools;

public class FPSViewer
{
    private const double KBestWatchdogFramerate = 60.0;
    private static UIWindow? kInspectorWindow = null;
    private static double updateWatchdogInterval = 2.0;
    private static double watchdogMaximumStallingTimeInterval = 3.0;
    private static bool enableWatchdogStallingException = true;
    private static int numberOfFrames = 0;
    private static double lastMainThreadEntryTime = 0;
    private static DispatchSource.Timer? watchdogTimer;
    private static DispatchSource.Timer? mainthreadTimer;
    private static IntPtr kObserverRef = IntPtr.Zero;
    private static double nSECPERSEC = 1000000000;
    private static ILogger? logger;

    public static bool IsRunning => watchdogTimer is not null;

    public static void Start(ILogger? logger = null)
    {
        FPSViewer.logger = logger;
        FPSViewer.logger?.LogInformation("Start WatchdogInspector");

        AddRunLoopObserver();
        AddWatchdogTimer();
        AddMainThreadWatchdogCounter();
        if (kInspectorWindow == null)
        {
            SetupStatusView();
        }
    }

    public static void Stop()
    {
        FPSViewer.logger?.LogInformation("Stop WatchdogInspector");

        if (watchdogTimer is not null)
        {
            watchdogTimer.Cancel();
            watchdogTimer = null;
        }

        if (mainthreadTimer is not null)
        {
            mainthreadTimer.Cancel();
            mainthreadTimer = null;
        }

        if (kObserverRef != IntPtr.Zero)
        {
            CFInterop.CFRunLoopRemoveObserver(CFInterop.CFRunLoopGetMain(), kObserverRef,
                CFInterop.KCFRunLoopCommonModes);
            kObserverRef = IntPtr.Zero;
        }

        ResetCountValues();
        if (kInspectorWindow is not null)
        {
            kInspectorWindow.Hidden = true;
        }

        kInspectorWindow = null;
    }

    public static void SetStallingThreshhold(double time)
    {
        watchdogMaximumStallingTimeInterval = time;
    }

    public static void SetEnableMainthreadStallingException(bool enable)
    {
        enableWatchdogStallingException = enable;
    }

    public static void SetUpdateWatchdogInterval(double time)
    {
        updateWatchdogInterval = time;
    }

    private static void AddMainThreadWatchdogCounter()
    {
        mainthreadTimer = new DispatchSource.Timer(DispatchQueue.MainQueue);
        var updateWatchdog = (1.0 / KBestWatchdogFramerate) * nSECPERSEC;

        mainthreadTimer.SetTimer(DispatchTime.Now, (long)updateWatchdog, 0);
        mainthreadTimer.SetEventHandler(() => { numberOfFrames++; });
        mainthreadTimer.Resume();
    }

    private static void AddWatchdogTimer()
    {
        watchdogTimer = new DispatchSource.Timer(DispatchQueue.MainQueue);
        watchdogTimer.SetTimer(DispatchTime.Now, (long)(updateWatchdogInterval * nSECPERSEC),
            (long)(updateWatchdogInterval * nSECPERSEC) / 10);
        watchdogTimer.SetEventHandler(() =>
        {
            double fps = numberOfFrames / updateWatchdogInterval;
            numberOfFrames = 0;
            FPSViewer.logger?.LogInformation("fps {0:F2}", fps);

            ThrowExceptionForStallingIfNeeded();
            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                if (kInspectorWindow is not null)
                {
                    ((FPSViewerViewController)kInspectorWindow.RootViewController!).UpdateFPS(fps);
                }
            });
        });
        watchdogTimer.Resume();
    }

    private static void AddRunLoopObserver()
    {
        kObserverRef = CFInterop.CFRunLoopObserverCreate(
            IntPtr.Zero,
            CFInterop.CFOptionFlags.KCFRunLoopAfterWaiting | CFInterop.CFOptionFlags.KCFRunLoopBeforeSources |
            CFInterop.CFOptionFlags.KCFRunLoopBeforeWaiting,
            true, 0, ObserverCallback, IntPtr.Zero);
        var tun = CFInterop.CFRunLoopGetMain();
        CFInterop.CFRunLoopAddObserver(tun, kObserverRef, CFInterop.KCFRunLoopCommonModes);
    }

    [MonoPInvokeCallback(typeof(CFInterop.CFRunLoopObserverCallback))]
    private static void ObserverCallback(IntPtr observer, CFInterop.CFOptionFlags activity, IntPtr info)
    {
        switch (activity)
        {
            case CFInterop.CFOptionFlags.KCFRunLoopAfterWaiting:
                lastMainThreadEntryTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                break;
            case CFInterop.CFOptionFlags.KCFRunLoopBeforeSources:
                ThrowExceptionForStallingIfNeeded();
                break;
            case CFInterop.CFOptionFlags.KCFRunLoopBeforeWaiting:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(activity), activity, null);
        }
    }

    private static void ThrowExceptionForStallingIfNeeded()
    {
        if (!enableWatchdogStallingException)
        {
            return;
        }

        double time = DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastMainThreadEntryTime;
        if (time > watchdogMaximumStallingTimeInterval && lastMainThreadEntryTime > 0)
        {
            throw new FPSViewerTimeoutException($"Watchdog timeout: Mainthread stalled for {time:F2} seconds");
        }
    }

    private static void ResetCountValues()
    {
        lastMainThreadEntryTime = 0;
        numberOfFrames = 0;
    }

    private static void SetupStatusView()
    {
        // TODO: Generate views for tvOS and Catalyst.
#if IOS
        CGRect statusBarFrame = UIApplication.SharedApplication.StatusBarFrame;
        CGSize size = statusBarFrame.Size;
        CGRect frame = new CGRect(0, 0, size.Width, size.Height);
        UIWindow window = new UIWindow(frame)
        {
            RootViewController = new FPSViewerViewController(),
            Hidden = false,
        };
        window.WindowLevel = UIWindowLevel.StatusBar + 50;
        kInspectorWindow = window;
#endif
    }
}