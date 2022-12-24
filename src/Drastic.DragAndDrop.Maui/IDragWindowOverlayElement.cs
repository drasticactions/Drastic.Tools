// <copyright file="IDragWindowOverlayElement.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.DragAndDrop.Maui
{
    /// <summary>
    /// Drag Window Overlay Element.
    /// </summary>
    public interface IDragWindowOverlayElement : IWindowOverlayElement
    {
        /// <summary>
        /// Gets or sets a value indicating whether something is being dragged over the element.
        /// </summary>
        public bool IsDragging { get; set; }
    }
}
