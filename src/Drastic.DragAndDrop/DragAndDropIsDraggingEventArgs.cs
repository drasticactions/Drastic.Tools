// <copyright file="DragAndDropIsDraggingEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.DragAndDrop
{
    /// <summary>
    /// Drag And Drop Is Dragging Event Args.
    /// </summary>
    public class DragAndDropIsDraggingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DragAndDropIsDraggingEventArgs"/> class.
        /// </summary>
        /// <param name="isDragging">If the user is dragging.</param>
        public DragAndDropIsDraggingEventArgs(bool isDragging, List<string>? paths = default)
        {
            this.IsDragging = isDragging;
            this.Paths = paths ?? new List<string>();
        }

        /// <summary>
        /// Gets a value indicating whether a user has started or finished dragging.
        /// </summary>
        public bool IsDragging { get; }

        /// <summary>
        /// Gets the paths to files that were dropped.
        /// </summary>
        public List<string> Paths { get; private set; }
    }
}