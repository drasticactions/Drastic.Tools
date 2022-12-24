// <copyright file="DragElementOverlay.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.DragAndDrop.Maui
{
    /// <summary>
    /// Drop Element Overlay.
    /// Drawn when the dragged event is fired.
    /// </summary>
    public class DragElementOverlay : IDragWindowOverlayElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DragElementOverlay"/> class.
        /// </summary>
        /// <param name="color">Color to use when the drag event is invoked. Defaults to Transparent.</param>
        /// <param name="overlayElement">Optional overlay element to draw instead of a color.</param>
        public DragElementOverlay(Color? color = default, IWindowOverlayElement? overlayElement = default)
        {
            this.OverlayElement = overlayElement;
            this.Color = color ?? Colors.Transparent;
        }

        /// <summary>
        /// Gets or sets a different overlay element instead of the given color.
        /// </summary>
        public IWindowOverlayElement? OverlayElement { get; set; }

        /// <inheritdoc/>
        public bool IsDragging { get; set; }

        /// <summary>
        /// Gets or sets the color to draw.
        /// </summary>
        public Microsoft.Maui.Graphics.Color Color { get; set; }

        // We are not going to use Contains for this.
        // We're gonna set if it's invoked externally.
        public bool Contains(Microsoft.Maui.Graphics.Point point) => false;

        public void Draw(ICanvas canvas, Microsoft.Maui.Graphics.RectF dirtyRect)
        {
            if (!this.IsDragging)
            {
                return;
            }

            if (this.OverlayElement is not null)
            {
                this.OverlayElement.Draw(canvas, dirtyRect);
                return;
            }

            canvas.FillColor = this.Color;
            canvas.FillRectangle(dirtyRect);
        }
    }
}