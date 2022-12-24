using System;
using System.Collections.Generic;
using Microsoft.Maui.Platform;

namespace Drastic.Overlay
{
    /// <summary>
    /// Android Platform Extensions.
    /// </summary>
    public static class PlatformExtensions
    {
        /// <summary>
        /// Gets Navigation Root Manager.
        /// </summary>
        /// <param name="mauiContext">MAUI Context.</param>
        /// <returns>NavigationRootManager.</returns>
        public static NavigationRootManager GetNavigationRootManager(this IMauiContext mauiContext) =>
            mauiContext.Services.GetRequiredService<NavigationRootManager>();

        /// <summary>
        /// Get Children View.
        /// </summary>
        /// <typeparam name="T">Base View Type.</typeparam>
        /// <param name="view">View.</param>
        /// <returns>List of T.</returns>
        public static List<T> GetChildView<T>(this Android.Views.ViewGroup view)
        {
            var childCount = view.ChildCount;
            var list = new List<T>();
            for (var i = 0; i < childCount; i++)
            {
                var child = view.GetChildAt(i);
                if (child is T tChild)
                {
                    list.Add(tChild);
                }
            }

            return list;
        }

        /// <summary>
        /// Get the bounding box for an Android View.
        /// </summary>
        /// <param name="view">MAUI IView.</param>
        /// <returns>Rectangle.</returns>
        public static Microsoft.Maui.Graphics.Rect GetBoundingBox(this IView view, IMauiContext context)
            => view.ToPlatform(context).GetBoundingBox();

        /// <summary>
        /// Get the bounding box for an Android View.
        /// </summary>
        /// <param name="nativeView">Android View.</param>
        /// <returns>Rectangle.</returns>
        public static Microsoft.Maui.Graphics.Rect GetBoundingBox(this Android.Views.View? nativeView)
        {
            if (nativeView == null)
            {
                return default(Rect);
            }

            var rect = new Android.Graphics.Rect();
            nativeView.GetGlobalVisibleRect(rect);
            return new Rect(rect.ExactCenterX() - (rect.Width() / 2), rect.ExactCenterY() - (rect.Height() / 2), (float)rect.Width(), (float)rect.Height());
        }
    }
}