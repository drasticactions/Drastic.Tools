// <copyright file="UIToolsExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

namespace Microsoft.Maui.Controls;

/// <summary>
/// UITools Extensions.
/// </summary>
public static class UIToolsExtensions
{
    /// <summary>
    /// Enable FPSViewer.
    /// </summary>
    /// <param name="app">MauiApp.</param>
    /// <param name="logger">Logger.</param>
    public static void EnableFPSViewer(this Window app, ILogger? logger = default)
    {
        ArgumentNullException.ThrowIfNull(app.Handler);
#if IOS || MACCATALYST
        Drastic.UITools.FPSViewer.Start(logger);
#elif ANDROID
        Drastic.UITools.FPSViewer.GetInstance(logger).Start(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity!);
#endif
    }

    /// <summary>
    /// Disable FPSViewer.
    /// </summary>
    /// <param name="app">MauiApp.</param>
    public static void DisableFPSViewer(this Window app)
    {
        ArgumentNullException.ThrowIfNull(app.Handler);
#if IOS || MACCATALYST
        Drastic.UITools.FPSViewer.Stop();
#elif ANDROID
        Drastic.UITools.FPSViewer.GetInstance().Stop(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity!);
#endif
    }
}