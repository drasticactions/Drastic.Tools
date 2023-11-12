// <copyright file="FPSViewer.Android.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Android.OS;
using AndroidX.Core.App;
using Microsoft.Extensions.Logging;

// Taken from https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#improve-layout-performance-of-label-on-android.
namespace Drastic.UITools;

/// <summary>
/// FPS Viewer.
/// </summary>
public class FPSViewer
{
    const int Duration = 1000;
    private FrameMetricsAggregator aggregator;
    private Handler handler;
    private ILogger? logger;
    private bool isRunning = false;

    /// <summary>
    /// Gets the current instance of the FPSViewer.
    /// </summary>
    public static FPSViewer? Instance { get; private set; }

    public FPSViewer(ILogger? logger)
    {
        this.logger = logger;
        var looper = Looper.MainLooper ?? throw new NullReferenceException("MainLooper is null");
        this.aggregator = new FrameMetricsAggregator();
        this.handler = new Handler(looper);
    }

    /// <summary>
    /// Gets the current instance of the FPSViewer.
    /// </summary>
    /// <param name="logger">Logger to set.</param>
    /// <returns>FPSViewer.</returns>
    public static FPSViewer GetInstance(ILogger? logger = default)
    {
        return FPSViewer.Instance ??= new FPSViewer(logger);
    }

    /// <summary>
    /// Start the FPSViewer.
    /// </summary>
    public void Start(Activity activity)
    {
        this.aggregator.Add(activity);
        this.isRunning = true;
        this.handler.PostDelayed(this.OnFrame, Duration);
    }

    /// <summary>
    /// Stop the FPSViewer.
    /// </summary>
    public void Stop(Activity activity)
    {
        this.aggregator.Remove(activity);
        this.isRunning = false;
    }

    private void OnFrame()
    {
        if (!this.isRunning)
        {
            return;
        }

        var metrics = this.aggregator.GetMetrics()?[FrameMetricsAggregator.TotalIndex];
        if (metrics is null)
        {
            this.logger?.LogDebug("No metrics available");
            this.handler.PostDelayed(this.OnFrame, Duration);
            return;
        }

        this.logger?.LogInformation($"----- {DateTime.Now} -----");
        int size = metrics.Size();
        double sum = 0, count = 0, slow = 0;
        for (int i = 0; i < size; i++)
        {
            int value = metrics.Get(i);
            if (value != 0)
            {
                count += value;
                sum += i * value;
                if (i > 16)
                {
                    slow += value;
                }

                this.logger?.LogTrace($"Frame(s) that took ~{i}ms, count: {value}");
            }
        }

        if (sum > 0)
        {
            this.logger?.LogInformation($"Average frame time: {sum / count:0.00}ms");
            this.logger?.LogTrace($"No. of slow frames: {slow}");
        }

        this.logger?.LogInformation("-----");
        this.handler.PostDelayed(this.OnFrame, Duration);
    }
}