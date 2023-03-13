// <copyright file="CoreAnimationExtensions.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Numerics;
using CoreAnimation;

namespace Drastic.Diagnostics;

public static class CoreAnimationExtensions
	{
		public static Matrix4x4 ToViewTransform(this CATransform3D transform) =>
			new Matrix4x4
			{
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
				M44 = (float)transform.M44
			};

		public static Matrix4x4 GetViewTransform(this CALayer layer)
		{
			if (layer == null)
				return new Matrix4x4();

			var superLayer = layer.SuperLayer;
			if (layer.Transform.IsIdentity && superLayer == null)
				return new Matrix4x4();

			var superTransform = layer.SuperLayer?.GetChildTransform() ?? CATransform3D.Identity;

			return layer.GetLocalTransform()
				.Concat(superTransform)
					.ToViewTransform();
		}

		public static CATransform3D Prepend(this CATransform3D a, CATransform3D b) =>
			b.Concat(a);

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

		public static CATransform3D GetChildTransform(this CALayer layer)
		{
			var childTransform = layer.SublayerTransform;

			if (childTransform.IsIdentity)
				return childTransform;

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

		public static CATransform3D TransformToAncestor(this CALayer fromLayer, CALayer toLayer)
		{
			var transform = CATransform3D.Identity;

			CALayer? current = fromLayer;
			while (current != toLayer)
			{
				transform = transform.Concat(current.GetLocalTransform());

				current = current.SuperLayer;
				if (current == null)
					break;

				transform = transform.Concat(current.GetChildTransform());
			}
			return transform;
		}
	}