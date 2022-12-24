// <copyright file="PageOverlay.Android.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Android.App;
using Android.Views;
using AndroidX.CoordinatorLayout.Widget;
using Drastic.Overlay;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Drastic.PageOverlay
{
    /// <summary>
    /// Page Overlay.
    /// </summary>
    public partial class PageOverlay
    {
        private Activity? nativeActivity;
        private ViewGroup? nativeLayer;

        /// <inheritdoc/>
        public override bool Initialize()
        {
            if (this.pageOverlayNativeElementsInitialized)
            {
                return true;
            }

            if (this.Window == null)
            {
                return false;
            }

            if (this.Window?.Handler?.MauiContext is null)
            {
                return false;
            }

            var nativeWindow = this.Window?.Content?.ToPlatform(this.Window.Handler.MauiContext);
            if (nativeWindow == null)
            {
                return false;
            }

            var handler = this.Window?.Handler as WindowHandler;
            if (handler?.MauiContext == null)
            {
                return false;
            }

            this.context = handler.MauiContext;

            var rootManager = handler.MauiContext.GetNavigationRootManager();
            if (rootManager == null)
            {
                return false;
            }

            if (handler.PlatformView is not Activity activity)
            {
                return false;
            }

            this.nativeActivity = activity;
            this.nativeLayer = rootManager.RootView as ViewGroup;

            if (this.nativeLayer?.Context == null)
            {
                return false;
            }

            if (this.nativeActivity?.WindowManager?.DefaultDisplay == null)
            {
                return false;
            }

            return this.pageOverlayNativeElementsInitialized = true;
        }

        /// <inheritdoc/>
        public override bool Deinitialize()
        {
            this.RemoveViews();
            return base.Deinitialize();
        }

        /// <summary>
        /// Add Native Elements.
        /// </summary>
        /// <param name="view">View.</param>
        internal void AddNativeElements(Page view)
        {
            if (this.nativeLayer == null || this.context == null)
            {
                return;
            }

            var pageHandler = view.ToHandler(this.context);
            var element = pageHandler?.PlatformView;
            if (element is Android.Views.View aView)
            {
                aView.Touch += this.Element_Touch;
                var layerCount = this.nativeLayer.ChildCount;
                var childView = this.nativeLayer.GetChildAt(1);
                this.nativeLayer.AddView(aView, layerCount, new CoordinatorLayout.LayoutParams(CoordinatorLayout.LayoutParams.MatchParent, CoordinatorLayout.LayoutParams.MatchParent));
                aView.BringToFront();
            }
        }

        /// <summary>
        /// Remove Native Elements.
        /// </summary>
        /// <param name="view">Views.</param>
        internal void RemoveNativeElements(Page view)
        {
            if (this.nativeLayer == null || this.context is null)
            {
                return;
            }

            var pageHandler = view.ToHandler(this.context);
            var element = pageHandler?.PlatformView;
            if (element is Android.Views.View aView)
            {
                aView.Touch -= this.Element_Touch;
                this.nativeLayer.RemoveView(aView);
            }
        }

        private void Element_Touch(object? sender, Android.Views.View.TouchEventArgs e)
        {
            if (this.context is null)
            {
                return;
            }

            if (e?.Event == null)
            {
                return;
            }

            if (e.Event.Action != MotionEventActions.Down && e.Event.ButtonState != MotionEventButtonState.Primary)
            {
                return;
            }

            var point = new Point(e.Event.RawX, e.Event.RawY);
            e.Handled = this.HitTestElements.Any(n => n.GetBoundingBox(this.context).Contains(point));
        }
    }
}

