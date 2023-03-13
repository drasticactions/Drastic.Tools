// <copyright file="ViewExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Numerics;

namespace Drastic.Diagnostics;

/// <summary>
/// View Extensions (Generic).
/// </summary>
public static partial class ViewExtensions
{
    /// <summary>
    /// Extract position from Matrix4x4.
    /// </summary>
    /// <param name="matrix">Matrix4x4.</param>
    /// <returns>Vector3.</returns>
    public static Vector3 ExtractPosition(this Matrix4x4 matrix) => matrix.Translation;

    /// <summary>
    /// Extract scale from Matrix4x4.
    /// </summary>
    /// <param name="matrix">Matrix4x4.</param>
    /// <returns>Vector3.</returns>
    public static Vector3 ExtractScale(this Matrix4x4 matrix) => new Vector3(matrix.M11, matrix.M22, matrix.M33);

    /// <summary>
    /// Extract angle in radians.
    /// </summary>
    /// <param name="matrix">Matrix4x4.</param>
    /// <returns>Angle as double.</returns>
    public static double ExtractAngleInRadians(this Matrix4x4 matrix) => Math.Atan2(matrix.M21, matrix.M11);

    /// <summary>
    /// Extract angle in degrees.
    /// </summary>
    /// <param name="matrix">Matrix4x4.</param>
    /// <returns>Degrees as double.</returns>
    public static double ExtractAngleInDegrees(this Matrix4x4 matrix) => ExtractAngleInRadians(matrix) * 180 / Math.PI;
}