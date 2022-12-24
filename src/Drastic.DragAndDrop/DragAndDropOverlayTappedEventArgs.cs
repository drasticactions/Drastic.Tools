// <copyright file="DragAndDropOverlayTappedEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.DragAndDrop
{
    /// <summary>
    /// Drag and Drop Overlay Tapped Event Args.
    /// </summary>
    public class DragAndDropOverlayTappedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DragAndDropOverlayTappedEventArgs"/> class.
        /// </summary>
        /// <param name="paths">Paths to files that were dropped.</param>
        public DragAndDropOverlayTappedEventArgs(List<string> paths)
        {
            this.Paths = paths;
        }

        /// <summary>
        /// Gets the paths to files that were dropped.
        /// </summary>
        public List<string> Paths { get; private set; }
    }
}
