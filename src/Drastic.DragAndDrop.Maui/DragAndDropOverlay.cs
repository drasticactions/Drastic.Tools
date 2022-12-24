// <copyright file="DragAndDropOverlay.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.DragAndDrop.Maui
{
    /// <summary>
    /// Drag and Drop Overlay.
    /// </summary>
    public partial class DragAndDropOverlay : WindowOverlay
    {
        private DragAndDrop dragAndDrop;
        private IDragWindowOverlayElement? dropElementOverlay;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragAndDropOverlay"/> class.
        /// </summary>
        /// <param name="window"><see cref="IWindow"/>.</param>
        /// <param name="overlayElement">Element to draw when the drag event is fired. Optional.</param>
        public DragAndDropOverlay(IWindow window, IDragWindowOverlayElement? overlayElement = default)
            : base(window)
        {
            this.dropElementOverlay = overlayElement;
#if WINDOWS
            var handler = window.Handler as Microsoft.Maui.Handlers.WindowHandler;
            this.dragAndDrop = new DragAndDrop(handler!.PlatformView);
#elif IOS || MACCATALYST
            var handler = window.Handler as Microsoft.Maui.Handlers.WindowHandler;
            this.dragAndDrop = new DragAndDrop(handler!.PlatformView);
#else
            this.dragAndDrop = new DragAndDrop();
#endif
            this.dragAndDrop.PropertyChanged += this.DragAndDrop_PropertyChanged;
            if (this.dropElementOverlay is not null)
            {
                this.AddWindowElement(this.dropElementOverlay);
            }

            this.dragAndDrop.Drop += (sender, e) => this.Drop?.Invoke(this, e);
        }

        /// <summary>
        /// Fired when files are dropped on the overlay.
        /// </summary>
        public event EventHandler<DragAndDropOverlayTappedEventArgs>? Drop;

        /// <summary>
        /// Gets the DragAndDrop element. Use this to attach to the underlying events from the overlay.
        /// </summary>
        public DragAndDrop DragAndDrop => this.dragAndDrop;

        private void DragAndDrop_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.DragAndDrop.IsDragging))
            {
                if (this.dropElementOverlay is not null)
                {
                    this.dropElementOverlay.IsDragging = this.dragAndDrop?.IsDragging ?? false;
                }

                this.Invalidate();
            }
        }
    }
}