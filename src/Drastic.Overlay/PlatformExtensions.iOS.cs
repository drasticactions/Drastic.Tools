// <copyright file="PlatformExtensions.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#pragma warning disable SA1210 // Using directives need to be in a specific order for MAUI
using System.Numerics;
using System.Runtime.InteropServices;
using Microsoft.Maui.Platform;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using System;
#pragma warning restore SA1210 // Using directives can't be ordered alphabetically by namespace

namespace Drastic.Overlay
{
    /// <summary>
    /// iOS Platform Extensions.
    /// </summary>
    public static class PlatformExtensions
    {
        /// <summary>
        /// Get View Transform.
        /// </summary>
        /// <param name="view">IView.</param>
        /// <returns>Matrix4x4.</returns>
        public static System.Numerics.Matrix4x4 GetViewTransform(this IView view, IMauiContext context)
        {
            var nativeView = view?.ToPlatform(context);
            if (nativeView == null)
            {
                return default(System.Numerics.Matrix4x4);
            }

            return nativeView.Layer.GetViewTransform();
        }

        /// <summary>
        /// Get View Transform.
        /// </summary>
        /// <param name="view">Native View.</param>
        /// <returns>Matrix4x4.</returns>
        public static System.Numerics.Matrix4x4 GetViewTransform(this UIView view)
            => view.Layer.GetViewTransform();

        /// <summary>
        /// Get Bounding Box.
        /// </summary>
        /// <param name="view">IView.</param>
        /// <returns>Rect.</returns>
        public static Microsoft.Maui.Graphics.Rect GetBoundingBox(this IView view, IMauiContext context)
            => view.ToPlatform(context).GetBoundingBox();

        /// <summary>
        /// Get Bounding Box.
        /// </summary>
        /// <param name="nativeView">Native View.</param>
        /// <returns>Rect.</returns>
        public static Microsoft.Maui.Graphics.Rect GetBoundingBox(this UIView? nativeView)
        {
            if (nativeView == null)
            {
                return default(Rect);
            }

            var nvb = nativeView.GetNativeViewBounds();
            var transform = nativeView.GetViewTransform();
            var radians = transform.ExtractAngleInRadians();
            var rotation = CoreGraphics.CGAffineTransform.MakeRotation((NFloat)radians);
            CGAffineTransform.CGRectApplyAffineTransform(nvb, rotation);
            return new Rect(nvb.X, nvb.Y, nvb.Width, nvb.Height);
        }

        /// <summary>
        /// Extract angle in radians.
        /// </summary>
        /// <param name="matrix">Matrix4x4.</param>
        /// <returns>Angle as double.</returns>
        public static double ExtractAngleInRadians(this System.Numerics.Matrix4x4 matrix)
            => Math.Atan2(matrix.M21, matrix.M11);

        /// <summary>
        /// Get Native View Bounds.
        /// </summary>
        /// <param name="view">IView.</param>
        /// <returns>Rect.</returns>
        public static Rect GetNativeViewBounds(this IView view, IMauiContext context)
        {
            var nativeView = view?.ToPlatform(context);
            if (nativeView == null)
            {
                return default(Rect);
            }

            return nativeView.GetNativeViewBounds();
        }

        /// <summary>
        /// Get Native View Bounds.
        /// </summary>
        /// <param name="nativeView">Native View.</param>
        /// <returns>Rect.</returns>
        public static Rect GetNativeViewBounds(this UIView nativeView)
        {
            if (nativeView == null)
            {
                return default(Rect);
            }

            var superview = nativeView;
            while (superview.Superview is not null)
            {
                superview = superview.Superview;
            }

            var convertPoint = nativeView.ConvertRectToView(nativeView.Bounds, superview);

            var x = convertPoint.X;
            var y = convertPoint.Y;
            var width = convertPoint.Width;
            var height = convertPoint.Height;

            return new Rect(x, y, width, height);
        }

        /// <summary>
        /// CATransform3D to view transform.
        /// </summary>
        /// <param name="transform">CATransform3D.</param>
        /// <returns>Matrix4x4.</returns>
        public static Matrix4x4 ToViewTransform(this CATransform3D transform) =>
           new Matrix4x4
           {
               M11 = (float)transform.M11,
               M12 = (float)transform.M12,
               M13 = (float)transform.M13,
               M14 = (float)transform.M14,
               M21 = (float)transform.M21,
               M22 = (float)transform.M22,
               M23 = (float)transform.M23,
               M24 = (float)transform.M24,
               M31 = (float)transform.M31,
               M32 = (float)transform.M32,
               M33 = (float)transform.M33,
               M34 = (float)transform.M34,
               Translation = new Vector3((float)transform.M41, (float)transform.M42, (float)transform.M43),
               M44 = (float)transform.M44,
           };

        /// <summary>
        /// Get View Transform.
        /// </summary>
        /// <param name="layer">Layer.</param>
        /// <returns>Matrix4x4.</returns>
        public static Matrix4x4 GetViewTransform(this CALayer layer)
        {
            if (layer == null)
            {
                return default(Matrix4x4);
            }

            var superLayer = layer.SuperLayer;
            if (layer.Transform.IsIdentity && (superLayer == null || superLayer.Transform.IsIdentity))
            {
                return default(Matrix4x4);
            }

            var superTransform = layer.SuperLayer?.GetChildTransform() ?? CATransform3D.Identity;

            return layer.GetLocalTransform()
                .Concat(superTransform)
                    .ToViewTransform();
        }

        /// <summary>
        /// Prepend.
        /// </summary>
        /// <param name="a">CATransform3D a.</param>
        /// <param name="b">CATransform3D b.</param>
        /// <returns>CATransform3D.</returns>
        public static CATransform3D Prepend(this CATransform3D a, CATransform3D b) =>
            b.Concat(a);

        /// <summary>
        /// Get Local Transform.
        /// </summary>
        /// <param name="layer">Layer.</param>
        /// <returns>CATransform3D.</returns>
        public static CATransform3D GetLocalTransform(this CALayer layer)
        {
            return CATransform3D.Identity
                .Translate(
                    layer.Position.X,
                    layer.Position.Y,
                    layer.ZPosition)
                .Prepend(layer.Transform)
                .Translate(
                    -layer.AnchorPoint.X * layer.Bounds.Width,
                    -layer.AnchorPoint.Y * layer.Bounds.Height,
                    -layer.AnchorPointZ);
        }

        /// <summary>
        /// Get Child Transform.
        /// </summary>
        /// <param name="layer">Layer.</param>
        /// <returns>CATransform3D.</returns>
        public static CATransform3D GetChildTransform(this CALayer layer)
        {
            var childTransform = layer.SublayerTransform;

            if (childTransform.IsIdentity)
            {
                return childTransform;
            }

            return CATransform3D.Identity
                .Translate(
                    layer.AnchorPoint.X * layer.Bounds.Width,
                    layer.AnchorPoint.Y * layer.Bounds.Height,
                    layer.AnchorPointZ)
                .Prepend(childTransform)
                .Translate(
                    -layer.AnchorPoint.X * layer.Bounds.Width,
                    -layer.AnchorPoint.Y * layer.Bounds.Height,
                    -layer.AnchorPointZ);
        }

        /// <summary>
        /// Transform to Ancestor.
        /// </summary>
        /// <param name="fromLayer">From Layer.</param>
        /// <param name="toLayer">To Layer.</param>
        /// <returns>CATransform3D.</returns>
        public static CATransform3D TransformToAncestor(this CALayer fromLayer, CALayer toLayer)
        {
            var transform = CATransform3D.Identity;

            CALayer? current = fromLayer;
            while (current != toLayer)
            {
                transform = transform.Concat(current.GetLocalTransform());

                current = current.SuperLayer;
                if (current == null)
                {
                    break;
                }

                transform = transform.Concat(current.GetChildTransform());
            }

            return transform;
        }
    }
}