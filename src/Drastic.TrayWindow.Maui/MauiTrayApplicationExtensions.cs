using System;
using Microsoft.Maui.Controls;

namespace Microsoft.Maui
{
    public static class MauiTrayApplicationExtensions
    {
        public static MauiAppBuilder AddTrayWindowSupport(this MauiAppBuilder builder)
        {
#if MACCATALYST
            builder.Services.AddTransient(typeof(UIKit.UIWindow));
#endif

            return builder;
        }
    }
}