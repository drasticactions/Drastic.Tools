// <copyright file="DragAndDropOverlay.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.Overlay;

namespace Drastic.DragAndDrop.Maui
{
    /// <summary>
    /// Drag and Drop Overlay.
    /// </summary>
    public partial class DragAndDropOverlay : BaseOverlay
    {
        public DragAndDropOverlay(IWindow window)
            : base(window)
        {
        }

        private class DropElementOverlay : IWindowOverlayElement
        {
            public IWindowOverlayElement? OverlayElement { get; set; }

            public bool IsDragging { get; set; }

            public Microsoft.Maui.Graphics.Color Color { get; set; } = Microsoft.Maui.Graphics.Colors.Transparent;

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
}