// <copyright file="ViewExtensions.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Drawing;
using System.Numerics;
using CoreAnimation;

namespace Drastic.Diagnostics
{
    /// <summary>
    /// View Extensions (iOS)
    /// </summary>
    public static partial class ViewExtensions
    {
        /// <summary>
        /// Get View Transformation.
        /// </summary>
        /// <param name="view">UIView.</param>
        /// <returns><see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 GetViewTransform(this UIView view)
            => view.Layer.GetViewTransform();

        /// <summary>
        /// Get the location of the UIView on screen.
        /// </summary>
        /// <param name="view">UIView.</param>
        /// <returns><see cref="Point"/>.</returns>
        public static Point GetLocationOnScreen(this UIView view) =>
            view.GetPlatformViewBounds().Location;

        /// <summary>
        /// Capture the UIWindow as an image.
        /// </summary>
        /// <param name="window">UIWindow.</param>
        /// <returns>UIImage</returns>
        /// <exception cref="ArgumentNullException">Thrown if window is null.</exception>
        public static Task<UIImage> CaptureAsync(this UIWindow window)
        {
            _ = window ?? throw new ArgumentNullException(nameof(window));

            // NOTE: We rely on the window frame having been set to the correct size when this method is invoked.
            UIGraphics.BeginImageContextWithOptions(window.Bounds.Size, false, window.Screen.Scale);
            var ctx = UIGraphics.GetCurrentContext();

            // ctx will be null if the width/height of the view is zero
            if (ctx is not null && !TryRender(window, out _))
            {
                // TODO: test/handle this case
            }

            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return Task.FromResult<UIImage>(image);
        }
        
        /// <summary>
        /// Capture the UIView as a UIImage.
        /// </summary>
        /// <param name="view">UIView.</param>
        /// <returns>UIImage.</returns>
        /// <exception cref="ArgumentNullException">Thrown if UIView is null.</exception>
        public static Task<UIImage?> CaptureAsync(this UIView view)
        {
            _ = view ?? throw new ArgumentNullException(nameof(view));

            // NOTE: We rely on the view frame having been set to the correct size when this method is invoked.
            UIGraphics.BeginImageContextWithOptions(view.Bounds.Size, false, view.Window.Screen.Scale);
            var ctx = UIGraphics.GetCurrentContext();

            // ctx will be null if the width/height of the view is zero
            if (ctx is not null && !TryRender(view, out _))
            {
                // TODO: test/handle this case
            }

            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return Task.FromResult<UIImage?>(image);
        }
        
        /// <summary>
        /// Capture a CALayer as an image.
        /// </summary>
        /// <param name="layer">CALayer.</param>
        /// <param name="skipChildren">Skip underlying children.</param>
        /// <returns>UIImage.</returns>
        /// <exception cref="ArgumentNullException">Thrown if layer is null.</exception>
        public static Task<UIImage?> CaptureAsync(this CALayer layer, bool skipChildren)
        {
            _ = layer ?? throw new ArgumentNullException(nameof(layer));

            // NOTE: We rely on the layer frame having been set to the correct size when this method is invoked.
            UIGraphics.BeginImageContextWithOptions(layer.Bounds.Size, false, layer.RasterizationScale);
            var ctx = UIGraphics.GetCurrentContext();

            // ctx will be null if the width/height of the view is zero
            if (ctx is not null && !TryRender(layer, ctx, skipChildren, out _))
            {
                // TODO: test/handle this case
            }

            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            
            return Task.FromResult<UIImage?>(image);
        }

        /// <summary>
        /// Get the platform view bounds as a Rectangle.
        /// </summary>
        /// <param name="platformView">UIView.</param>
        /// <returns><see cref="Rectangle"/>.</returns>
        public static Rectangle GetPlatformViewBounds(this UIView platformView)
        {
            var superview = platformView;
            while (superview?.Superview is not null)
            {
                superview = superview.Superview;
            }

            var convertPoint = platformView.ConvertRectToView(platformView.Bounds, superview);

            var x = convertPoint.X;
            var y = convertPoint.Y;
            var width = convertPoint.Width;
            var height = convertPoint.Height;

            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        /// <summary>
        /// Get the binding box for a given UIView.
        /// </summary>
        /// <param name="platformView">UIView.</param>
        /// <returns><see cref="Rectangle"/>.</returns>
        public static Rectangle GetBoundingBox(this UIView? platformView)
        {
            if (platformView == null)
            {
                return default(Rectangle);
            }

            var nvb = platformView.GetPlatformViewBounds();
            var transform = platformView.GetViewTransform();
            var radians = transform.ExtractAngleInRadians();
            var rotation = CoreGraphics.CGAffineTransform.MakeRotation((nfloat)radians);
            CGAffineTransform.CGRectApplyAffineTransform(nvb, rotation);
            return new Rectangle(nvb.X, nvb.Y, nvb.Width, nvb.Height);
        }
        
        private static bool TryRender(UIView view, out Exception? error)
        {
            try
            {
                view.DrawViewHierarchy(view.Bounds, afterScreenUpdates: true);

                error = null;
                return true;
            }
            catch (Exception e)
            {
                error = e;
                return false;
            }
        }

        private static bool TryRender(CALayer layer, CGContext ctx, bool skipChildren, out Exception? error)
        {
            var visibilitySnapshot = new Dictionary<CALayer, bool>();

            try
            {
                if (skipChildren)
                    HideSublayers(layer, visibilitySnapshot);

                layer.RenderInContext(ctx);

                error = null;
                return true;
            }
            catch (Exception e)
            {
                error = e;
                return false;
            }
            finally
            {
                if (skipChildren)
                    RestoreSublayers(layer, visibilitySnapshot);
            }
        }

        private static void HideSublayers(CALayer layer, Dictionary<CALayer, bool> visibilitySnapshot)
        {
            if (layer.Sublayers == null)
                return;

            foreach (var sublayer in layer.Sublayers)
            {
                HideSublayers(sublayer, visibilitySnapshot);

                visibilitySnapshot.Add(sublayer, sublayer.Hidden);
                sublayer.Hidden = true;
            }
        }

        private static void RestoreSublayers(CALayer layer, Dictionary<CALayer, bool> visibilitySnapshot)
        {
            if (layer.Sublayers == null)
                return;

            foreach (var sublayer in visibilitySnapshot)
            {
                sublayer.Key.Hidden = sublayer.Value;
            }
        }
    }
}
