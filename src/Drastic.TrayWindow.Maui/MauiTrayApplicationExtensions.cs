// <copyright file="MauiTrayApplicationExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Microsoft.Maui.Controls;

namespace Microsoft.Maui
{
    /// <summary>
    /// Maui Tray Application Extensions.
    /// </summary>
    public static class MauiTrayApplicationExtensions
    {
        /// <summary>
        /// Adds support for Tray Windows in MAUI.
        /// </summary>
        /// <param name="builder"><see cref="MauiAppBuilder"/>.</param>
        /// <returns>MauiAppBuilder.</returns>
        public static MauiAppBuilder AddTrayWindowSupport(this MauiAppBuilder builder)
        {
#if MACCATALYST
            builder.Services.AddTransient(typeof(UIKit.UIWindow));
#endif

            return builder;
        }
    }
}