using System.Drawing;
using Point = System.Drawing.Point;

namespace Drastic.Diagnostics;

/// <summary>
/// Core Graphics Extensions.
/// </summary>
public static class CoreGraphicsExtensions
{
    /// <summary>
    /// CGPoint to Point.
    /// </summary>
    /// <param name="size">CSRect.</param>
    /// <returns>Point.</returns>
    public static Point ToPoint(this CGPoint size)
    {
        return new Point((int)size.X, (int)size.Y);
    }

    /// <summary>
    /// CGSize to Size.
    /// </summary>
    /// <param name="size">CGSize.</param>
    /// <returns>Size.</returns>
    public static Size ToSize(this CGSize size)
    {
        return new Size((int)size.Width, (int)size.Height);
    }

    /// <summary>
    /// Size to CGSize.
    /// </summary>
    /// <param name="size">Size.</param>
    /// <returns>CGSize.</returns>
    public static CGSize ToCGSize(this Size size)
    {
        return new CGSize(size.Width, size.Height);
    }

    /// <summary>
    /// CGRect to Rectangle.
    /// </summary>
    /// <param name="rect">CGRect</param>
    /// <returns>Rectangle.</returns>
    public static Rectangle ToRectangle(this CGRect rect)
    {
        return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
    }

    /// <summary>
    /// Rectangle to CGRect.
    /// </summary>
    /// <param name="rect">Rectangle.</param>
    /// <returns>CGRect.</returns>
    public static CGRect ToCGRect(this Rectangle rect)
    {
        return new CGRect(rect.X, rect.Y, rect.Width, rect.Height);
    }

    /// <summary>
    /// Detect if one object is next to another.
    /// </summary>
    /// <param name="size0">CGSize 1.</param>
    /// <param name="size1">CGSize 2.</param>
    /// <param name="tolerance">Tolerance for how close the objects need to be.</param>
    /// <returns>Bool.</returns>
    public static bool IsCloseTo(this CGSize size0, CGSize size1, nfloat tolerance)
    {
        var diff = size0 - size1;
        return Math.Abs(diff.Width) < tolerance && Math.Abs(diff.Height) < tolerance;
    }
}