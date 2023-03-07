// <copyright file="MauiRainbowsApplicationExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui.Controls.Compatibility.Hosting;

namespace Drastic.Maui.DebugRainbows {

    /// <summary>
    /// Maui Rainbows Application Extensions.
    /// </summary>
    public static class MauiRainbowsApplicationExtensions {
        /// <summary>
        /// Adds support for Debug Rainbows in MAUI.
        /// </summary>
        /// <param name="builder"><see cref="MauiAppBuilder"/>.</param>
        /// <returns>MauiAppBuilder.</returns>
        public static MauiAppBuilder AddDebugRainbowsSupport(this MauiAppBuilder builder) {

            builder.UseMauiCompatibility();

            builder.ConfigureMauiHandlers((handlers) => {
#if ANDROID
                handlers.AddCompatibilityRenderer(typeof(DebugGridWrapper), typeof(Drastic.Maui.DebugRainbows.DebugGridWrapperRendererDroid));
#endif

#if IOS || MACCATALYST
                handlers.AddCompatibilityRenderer(typeof(DebugGridWrapper), typeof(Drastic.Maui.DebugRainbows.DebugGridWrapperRendereriOS));
#endif
            });

            return builder;
        }
    }
}
