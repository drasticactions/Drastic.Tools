// <copyright file="HtmlLabelApplicationExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui.Controls.Compatibility.Hosting;

namespace Drastic.HtmlLabel.Maui
{

    /// <summary>
    /// Html Label Application Extensions.
    /// </summary>
    public static class HtmlLabelApplicationExtensions
    {
        /// <summary>
        /// Adds support for Debug Rainbows in MAUI.
        /// </summary>
        /// <param name="builder"><see cref="MauiAppBuilder"/>.</param>
        /// <returns>MauiAppBuilder.</returns>
        public static MauiAppBuilder AddHtmlLabelSupport(this MauiAppBuilder builder)
        {

            builder.UseMauiCompatibility();

            builder.ConfigureMauiHandlers((handlers) => {
#if ANDROID
                HtmlLabelRenderer.Initialize();
                handlers.AddCompatibilityRenderer(typeof(HtmlLabelRenderer), typeof(HtmlLabelRenderer));
#endif

#if IOS || MACCATALYST
                HtmlLabelRenderer.Initialize();
                handlers.AddCompatibilityRenderer(typeof(HtmlLabelRenderer), typeof(HtmlLabelRenderer));
#endif
            });

            return builder;
        }
    }
}
