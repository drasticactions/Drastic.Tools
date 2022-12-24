// <copyright file="DragAndDropWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.Overlay;

namespace Drastic.DragAndDrop.Maui
{
    /// <summary>
    /// Drag And Drop Window.
    /// </summary>
    public class DragAndDropWindow : OverlayWindow
    {
        private DragAndDropOverlay? overlay;
        private IDragWindowOverlayElement? overlayElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragAndDropWindow"/> class.
        /// </summary>
        /// <param name="element">Optional Drag Element to show when the Drag event is fired.</param>
        public DragAndDropWindow(IDragWindowOverlayElement? element = default)
        {
            this.overlayElement = element;
        }

        /// <summary>
        /// Fired when files are dropped on the overlay.
        /// </summary>
        public event EventHandler<DragAndDropOverlayTappedEventArgs>? Drop;

        /// <inheritdoc/>
        public override Task AddOverlaysAsync()
        {
            this.overlay = new DragAndDropOverlay(this, this.overlayElement);
            this.overlay.Drop += (sender, e) => this.Drop?.Invoke(this, e);

            this.AddOverlay(this.overlay);

            return base.AddOverlaysAsync();
        }
    }
}
