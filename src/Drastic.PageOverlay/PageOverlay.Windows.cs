// <copyright file="PageOverlay.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.Overlay;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Drastic.PageOverlay
{
    /// <summary>
    /// Page Overlay.
    /// </summary>
    public partial class PageOverlay
    {
        private Microsoft.UI.Xaml.Controls.Panel? panel;
        private FrameworkElement? element;

        /// <inheritdoc/>
        public override bool Initialize()
        {
            if (this.pageOverlayNativeElementsInitialized)
            {
                return true;
            }

            var handler = this.Window.Handler as Microsoft.Maui.Handlers.WindowHandler;
            if (handler?.PlatformView is not Microsoft.UI.Xaml.Window window)
            {
                return false;
            }

            if (handler.MauiContext is null)
            {
                return false;
            }

            this.context = handler.MauiContext;

            var nativeElement = this.Window.Content.ToPlatform(this.context);
            if (nativeElement is null)
            {
                return false;
            }

            this.panel = window.Content as Microsoft.UI.Xaml.Controls.Panel;
            if (this.panel is null)
            {
                return false;
            }

            this.panel.PointerMoved += this.Panel_PointerMoved;
            return this.pageOverlayNativeElementsInitialized = true;
        }

        /// <inheritdoc/>
        public override bool Deinitialize()
        {
            this.RemoveViews();
            if (this.panel is not null)
            {
                this.panel.PointerMoved -= this.Panel_PointerMoved;
            }

            return base.Deinitialize();
        }

        /// <summary>
        /// Add Native Elements.
        /// </summary>
        /// <param name="page">View.</param>
        internal void AddNativeElements(Microsoft.Maui.Controls.Page page)
        {
            if (this.panel == null)
            {
                return;
            }

            if (this.context == null)
            {
                return;
            }

            var element = page.ToPlatform(this.context);

            var zindex = 100 + this.Views.Count();
            if (element is null)
            {
                return;
            }

            element.SetValue(Canvas.ZIndexProperty, zindex);
            this.panel.Children.Add(element);
        }

        /// <summary>
        /// Remove Native Elements.
        /// </summary>
        /// <param name="page">Views.</param>
        internal void RemoveNativeElements(Microsoft.Maui.Controls.Page page)
        {
            if (this.panel == null)
            {
                return;
            }

            if (this.context == null)
            {
                return;
            }

            var element = page.ToPlatform(this.context);
            if (element is null)
            {
                return;
            }

            this.panel.Children.Remove(element);
        }

        private void Panel_PointerMoved(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (this.context is null)
            {
                return;
            }

            if (!this.HitTestElements.Any() || this.context is null)
            {
                return;
            }

            var pointerPoint = e.GetCurrentPoint(this.element);
            if (pointerPoint == null)
            {
                return;
            }

            foreach (var view in this.Views)
            {
                var nativeView = (view as Microsoft.Maui.Controls.Page)?.ToPlatform(this.context);
                var hitTests = view.HitTestViews.Any(n => n.GetBoundingBox(this.context).Contains(new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y)));
                if (nativeView is not null)
                {
                    nativeView.IsHitTestVisible = hitTests;
                }
            }
        }
    }
}